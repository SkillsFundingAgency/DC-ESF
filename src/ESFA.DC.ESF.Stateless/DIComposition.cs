using System;
using System.Collections.Generic;
using Autofac;
using ESFA.DC.ESF.DataStore;
using ESFA.DC.ESF.Interfaces;
using ESFA.DC.ESF.Interfaces.Config;
using ESFA.DC.ESF.Interfaces.Controllers;
using ESFA.DC.ESF.Interfaces.DataStore;
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

            // persist data options
            var persistDataConfig =
                configHelper.GetSectionValues<PersistDataConfiguration>("DataStoreSection");
            container.RegisterInstance(persistDataConfig).As<PersistDataConfiguration>().SingleInstance();

            var orgConfiguration = configHelper.GetSectionValues<OrgConfiguration>("OrgSection");
            container.RegisterInstance(orgConfiguration).As<OrgConfiguration>().SingleInstance();

            RegisterPersistence(container, configHelper);
            RegisterServiceBusConfig(container, configHelper);
            RegisterLogger(container);
            RegisterSerializers(container);
            RegisterMessageHandler(container);

            RegisterControllers(container);

            RegisterCommands(container);

            RegisterStorage(container);

            RegisterFileLevelValidators(container);
            RegisterCrossRecordValidators(container);
            RegisterBusinessRuleValidators(container);
            RegisterFieldDefinitionValidators(container);

            RegisterServices(container);
            
            return container;
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

        private static void RegisterServiceBusConfig(ContainerBuilder containerBuilder, IConfigurationHelper configHelper)
        {
            // get ServiceBus, Azurestorage config values and register container
            var serviceBusOptions =
                configHelper.GetSectionValues<ServiceBusOptions>("ServiceBusSettings");
            containerBuilder.RegisterInstance(serviceBusOptions).As<ServiceBusOptions>().SingleInstance();

            // register Jobcontext services
            var topicConfig = new ServiceBusTopicConfig(
                serviceBusOptions.ServiceBusConnectionString,
                serviceBusOptions.TopicName,
                serviceBusOptions.ReportingSubscriptionName,
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
        }

        private static void RegisterMessageHandler(ContainerBuilder containerBuilder)
        {
            // register MessageHandler
            containerBuilder.RegisterType<MessageHandler>().As<IMessageHandler<JobContextMessage>>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultJobContextMessageMapper<JobContextMessage>>().As<IMapper<JobContextMessage, JobContextMessage>>();
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
            containerBuilder.RegisterType<ESFProviderService>().As<IESFProviderService>();
        }

        private static void RegisterControllers(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ValidationController>().As<IValidationController>();
            containerBuilder.RegisterType<StorageController>().As<IValidationController>();
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
            containerBuilder.RegisterType<FDConRefNumberMA>().As<IFileLevelValidator>();
            containerBuilder.RegisterType<FDCostTypeAL>().As<IFieldDefinitionValidator>();
            containerBuilder.RegisterType<FDCostTypeMA>().As<IFieldDefinitionValidator>();
            containerBuilder.RegisterType<FDDeliverableCodeAL>().As<IFieldDefinitionValidator>();
            containerBuilder.RegisterType<FDDeliverableCodeMA>().As<IFileLevelValidator>();
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
