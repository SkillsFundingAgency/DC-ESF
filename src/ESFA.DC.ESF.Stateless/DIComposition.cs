using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Features.AttributeFilters;
using ESFA.DC.Auditing.Interface;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.ESF.DataStore;
using ESFA.DC.ESF.Helpers;
using ESFA.DC.ESF.Interfaces;
using ESFA.DC.ESF.Interfaces.Config;
using ESFA.DC.ESF.Interfaces.Controllers;
using ESFA.DC.ESF.Interfaces.DataStore;
using ESFA.DC.ESF.Interfaces.Helpers;
using ESFA.DC.ESF.Interfaces.Services;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models.Configuration;
using ESFA.DC.ESF.Service.Config;
using ESFA.DC.ESF.Service.Stateless.Handlers;
using ESFA.DC.ESF.Services;
using ESFA.DC.ESF.ValidationService;
using ESFA.DC.ESF.ValidationService.Commands;
using ESFA.DC.ESF.ValidationService.Commands.BusinessRules;
using ESFA.DC.ESF.ValidationService.Commands.CrossRecord;
using ESFA.DC.ESF.ValidationService.Commands.FieldDefinition;
using ESFA.DC.ESF.ValidationService.Commands.FileLevel;
using ESFA.DC.IO.AzureStorage;
using ESFA.DC.IO.AzureStorage.Config.Interfaces;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.JobContextManager;
using ESFA.DC.JobContextManager.Interface;
using ESFA.DC.JobContextManager.Model;
using ESFA.DC.JobStatus.Interface;
using ESFA.DC.Logging;
using ESFA.DC.Logging.Config;
using ESFA.DC.Logging.Config.Interfaces;
using ESFA.DC.Logging.Enums;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Mapping.Interface;
using ESFA.DC.Queueing;
using ESFA.DC.Queueing.Interface;
using ESFA.DC.Serialization.Interfaces;
using ESFA.DC.Serialization.Json;
using ESFA.DC.ServiceFabric.Helpers.Interfaces;
using ExecutionContext = ESFA.DC.Logging.ExecutionContext;

namespace ESFA.DC.ESF.Service.Stateless
{
    public class DIComposition
    {
        public static ContainerBuilder BuildContainer(IConfigurationHelper configHelper)
        {
            var container = new ContainerBuilder();

            RegisterPersistence(container, configHelper);
            RegisterServiceBusConfig(container, configHelper);
            RegisterJobContextManagementServices(container);
            RegisterLogger(container);
            RegisterSerializers(container);
            RegisterMessageHandler(container);

            RegisterControllers(container);

            RegisterCommands(container);

            RegisterStorage(container);

            RegisterHelpers(container);

            RegisterFileLevelValidators(container);
            RegisterCrossRecordValidators(container);
            RegisterBusinessRuleValidators(container);
            RegisterFieldDefinitionValidators(container);

            RegisterServices(container);
            
            return container;
        }

        private static void RegisterJobContextManagementServices(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<JobContextManager<JobContextMessage>>().As<IJobContextManager<JobContextMessage>>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultJobContextMessageMapper<JobContextMessage>>().As<IMapper<JobContextMessage, JobContextMessage>>();
            containerBuilder.RegisterType<DateTimeProvider.DateTimeProvider>().As<IDateTimeProvider>();
        }

        private static void RegisterPersistence(ContainerBuilder containerBuilder, IConfigurationHelper configHelper)
        {
            // register azure blob storage service
            var azureBlobStorageOptions = configHelper.GetSectionValues<AzureStorageOptions>("AzureStorageSection");
            containerBuilder.Register(c =>
                    new AzureStorageKeyValuePersistenceConfig(
                        azureBlobStorageOptions.AzureBlobConnectionString,
                        azureBlobStorageOptions.AzureBlobContainerName))
                .As<IAzureStorageKeyValuePersistenceServiceConfig>().SingleInstance();

            containerBuilder.RegisterType<AzureStorageKeyValuePersistenceService>()
                .Keyed<IKeyValuePersistenceService>(PersistenceStorageKeys.Blob)
                .As<IStreamableKeyValuePersistenceService>()
                .InstancePerLifetimeScope();
        }

        private static void RegisterServiceBusConfig(ContainerBuilder containerBuilder,
            IConfigurationHelper configHelper)
        {
            // get ServiceBus, Azurestorage config values and register container
            var serviceBusOptions =
                configHelper.GetSectionValues<ServiceBusOptions>("ServiceBusSettings");
            containerBuilder.RegisterInstance(serviceBusOptions).As<ServiceBusOptions>().SingleInstance();

            // register Jobcontext services
            var topicConfig = new ServiceBusTopicConfig(
                serviceBusOptions.ServiceBusConnectionString,
                serviceBusOptions.TopicName,
                serviceBusOptions.SubscriptionName,
                Environment.ProcessorCount);
            containerBuilder.Register(c =>
            {
                var topicSubscriptionSevice =
                    new TopicSubscriptionSevice<JobContextDto>(
                        topicConfig,
                        c.Resolve<IJsonSerializationService>(),
                        c.Resolve<ILogger>());
                return topicSubscriptionSevice;
            }).As<ITopicSubscriptionService<JobContextDto>>();

            containerBuilder.Register(c =>
            {
                var topicPublishSevice =
                    new TopicPublishService<JobContextDto>(
                        topicConfig,
                        c.Resolve<IJsonSerializationService>());
                return topicPublishSevice;
            }).As<ITopicPublishService<JobContextDto>>();

            containerBuilder.Register(c =>
            {
                var topicSubscriptionConfig = new TopicConfiguration(serviceBusOptions.ServiceBusConnectionString,
                    serviceBusOptions.TopicName, serviceBusOptions.SubscriptionName, 1,
                    maximumCallbackTimeSpan: TimeSpan.FromMinutes(40));

                return new TopicSubscriptionSevice<JobContextDto>(
                    topicSubscriptionConfig,
                    c.Resolve<IJsonSerializationService>(),
                    c.Resolve<ILogger>());
            }).As<ITopicSubscriptionService<JobContextDto>>();

            //containerBuilder.RegisterType<TopicPublishServiceStub<JobContextDto>>().As<ITopicPublishService<JobContextDto>>();

            containerBuilder.Register(c =>
            {
                var auditPublishConfig = new QueueConfiguration(serviceBusOptions.ServiceBusConnectionString,
                    serviceBusOptions.AuditQueueName, 1);

                return new QueuePublishService<AuditingDto>(
                    auditPublishConfig,
                    c.Resolve<IJsonSerializationService>());
            }).As<IQueuePublishService<AuditingDto>>();

            var jobStatusQueueOptions =
                configHelper.GetSectionValues<JobStatusQueueOptions>("JobStatusSection");
            containerBuilder.RegisterInstance(jobStatusQueueOptions).As<JobStatusQueueOptions>().SingleInstance();

            var jobStatusPublishConfig = new JobStatusQueueConfig(
                jobStatusQueueOptions.JobStatusConnectionString,
                jobStatusQueueOptions.JobStatusQueueName,
                Environment.ProcessorCount);

            containerBuilder.Register(c => new QueuePublishService<JobStatusDto>(
                    jobStatusPublishConfig,
                    c.Resolve<IJsonSerializationService>()))
                .As<IQueuePublishService<JobStatusDto>>();
        }

        private static void RegisterMessageHandler(ContainerBuilder containerBuilder)
        {
            // register MessageHandler
            containerBuilder.RegisterType<MessageHandler>().As<IMessageHandler<JobContextMessage>>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultJobContextMessageMapper<JobContextMessage>>().As<IMapper<JobContextMessage, JobContextMessage>>();

            // register EntryPoint
            containerBuilder.RegisterType<EntryPoint>()
                .WithAttributeFiltering()
                .InstancePerLifetimeScope();
        }

        private static void RegisterSerializers(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<JsonSerializationService>().As<IJsonSerializationService>();
        }

        private static void RegisterLogger(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterInstance(new LoggerOptions
            {
                LoggerConnectionString = @"Server=(local);Database=Logging;Trusted_Connection=True;"
            }).As<ILoggerOptions>().SingleInstance();

            containerBuilder.Register(c =>
            {
                var loggerOptions = c.Resolve<ILoggerOptions>();
                return new ApplicationLoggerSettings
                {
                    ApplicationLoggerOutputSettingsCollection = new List<IApplicationLoggerOutputSettings>
                    {
                        new MsSqlServerApplicationLoggerOutputSettings
                        {
                            MinimumLogLevel = LogLevel.Verbose,
                            ConnectionString = loggerOptions.LoggerConnectionString
                        },
                        new ConsoleApplicationLoggerOutputSettings
                        {
                            MinimumLogLevel = LogLevel.Verbose
                        }
                    }
                };
            }).As<IApplicationLoggerSettings>().SingleInstance();

            containerBuilder.RegisterType<ExecutionContext>().As<IExecutionContext>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<SerilogLoggerFactory>().As<ISerilogLoggerFactory>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<SeriLogger>().As<ILogger>().InstancePerLifetimeScope();
        }

        private static void RegisterServices(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ESFProviderService>().As<IESFProviderService>()
                .WithAttributeFiltering()
                .InstancePerLifetimeScope();
        }

        private static void RegisterControllers(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ValidationController>().As<IValidationController>();
            containerBuilder.RegisterType<StorageController>().As<IStorageController>();
        }

        private static void RegisterHelpers(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<FileHelper>().As<IFileHelper>();
            containerBuilder.RegisterType<TaskHelper>().As<ITaskHelper>();
        }

        private static void RegisterCommands(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<FileLevelCommands>().As<IValidatorCommand>();
            containerBuilder.RegisterType<BusinessRuleCommands>().As<IValidatorCommand>();
            containerBuilder.RegisterType<FieldDefinitionCommand>().As<IValidatorCommand>();
            containerBuilder.RegisterType<CrossRecordCommands>().As<IValidatorCommand>();

            containerBuilder.Register(c => new List<IValidatorCommand>(c.Resolve<IEnumerable<IValidatorCommand>>()))
                .As<IList<IValidatorCommand>>();
        }

        private static void RegisterFileLevelValidators(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<FileNameRule08>().As<IFileLevelValidator>();
            containerBuilder.RegisterType<ConRefNumberRule01>().As<IFileLevelValidator>();

            containerBuilder.Register(c => new List<IFileLevelValidator>(c.Resolve<IEnumerable<IFileLevelValidator>>()))
                .As<IList<IFileLevelValidator>>();
        }

        private static void RegisterCrossRecordValidators(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<Duplicate01>().As<ICrossRecordValidator>();

            containerBuilder.Register(c => new List<ICrossRecordValidator>(c.Resolve<IEnumerable<ICrossRecordValidator>>()))
                .As<IList<ICrossRecordValidator>>();
        }

        private static void RegisterBusinessRuleValidators(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<CalendarMonthRule01>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<CalendarYearCalendarMonthRule01>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<CalendarYearCalendarMonthRule02>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<CalendarYearCalendarMonthRule03>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<CalendarYearRule01>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<CostTypeRule01>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<CostTypeRule02>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<DeliverableCodeRule01>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<DeliverableCodeRule02>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<HourlyRateRule01>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<HourlyRateRule02>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<OrgHoursRule01>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<OrgHoursRule02>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<ProjectHoursOrgHoursRule01>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<ProjectHoursRule01>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<ProjectHoursRule02>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<ProviderSpecifiedReferenceRule01>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<ReferenceRule01>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<ReferenceTypeRule01>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<ReferenceTypeRule02>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<StaffNameRule01>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<StaffNameRule02>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<StaffNameRule03>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<TotalHoursWorkedRule01>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<TotalHoursWorkedRule02>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<ULNRule01>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<ULNRule02>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<ULNRule03>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<ULNRule04>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<ValueRule01>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<ValueRule02>().As<IBusinessRuleValidator>();
            containerBuilder.RegisterType<ValueRule03>().As<IBusinessRuleValidator>();

            containerBuilder.Register(c => new List<IBusinessRuleValidator>(c.Resolve<IEnumerable<IBusinessRuleValidator>>()))
                .As<IList<IBusinessRuleValidator>>();
        }

        private static void RegisterFieldDefinitionValidators(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<FDCalendarMonthAL>().As<IFieldDefinitionValidator>();
            containerBuilder.RegisterType<FDCalendarMonthDT>().As<IFieldDefinitionValidator>();
            containerBuilder.RegisterType<FDCalendarMonthMA>().As<IFieldDefinitionValidator>();
            containerBuilder.RegisterType<FDCalendarYearAL>().As<IFieldDefinitionValidator>();
            containerBuilder.RegisterType<FDCalendarYearDT>().As<IFieldDefinitionValidator>();
            containerBuilder.RegisterType<FDCalendarYearMA>().As<IFieldDefinitionValidator>();
            containerBuilder.RegisterType<FDConRefNumberAL>().As<IFieldDefinitionValidator>();
            containerBuilder.RegisterType<FDConRefNumberMA>().As<IFieldDefinitionValidator>();
            containerBuilder.RegisterType<FDCostTypeAL>().As<IFieldDefinitionValidator>();
            containerBuilder.RegisterType<FDCostTypeMA>().As<IFieldDefinitionValidator>();
            containerBuilder.RegisterType<FDDeliverableCodeAL>().As<IFieldDefinitionValidator>();
            containerBuilder.RegisterType<FDDeliverableCodeMA>().As<IFieldDefinitionValidator>();
            containerBuilder.RegisterType<FDHourlyRateAL>().As<IFieldDefinitionValidator>();
            containerBuilder.RegisterType<FDOrgHoursAL>().As<IFieldDefinitionValidator>();
            containerBuilder.RegisterType<FDProjectHoursAL>().As<IFieldDefinitionValidator>();
            containerBuilder.RegisterType<FDProviderSpecifiedReferenceAL>().As<IFieldDefinitionValidator>();
            containerBuilder.RegisterType<FDReferenceAL>().As<IFieldDefinitionValidator>();
            containerBuilder.RegisterType<FDReferenceMA>().As<IFieldDefinitionValidator>();
            containerBuilder.RegisterType<FDReferenceTypeAL>().As<IFieldDefinitionValidator>();
            containerBuilder.RegisterType<FDReferenceTypeMA>().As<IFieldDefinitionValidator>();
            containerBuilder.RegisterType<FDStaffNameAL>().As<IFieldDefinitionValidator>();
            containerBuilder.RegisterType<FDTotalHoursWorkedAL>().As<IFieldDefinitionValidator>();
            containerBuilder.RegisterType<FDULNAL>().As<IFieldDefinitionValidator>();
            containerBuilder.RegisterType<FDULNDT>().As<IFieldDefinitionValidator>();
            containerBuilder.RegisterType<FDValueAL>().As<IFieldDefinitionValidator>();

            containerBuilder.Register(c => new List<IFieldDefinitionValidator>(c.Resolve<IEnumerable<IFieldDefinitionValidator>>()))
                .As<IList<IFieldDefinitionValidator>>();
        }

        private static void RegisterStorage(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<StoreClear>().As<IStoreClear>();
            containerBuilder.RegisterType<StoreFileDetails>().As<IStoreFileDetails>();
            containerBuilder.RegisterType<StoreESF>().As<IStoreESF>();
        }
    }
}
