using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Features.AttributeFilters;
using ESFA.DC.Auditing.Interface;
using ESFA.DC.Data.LARS.Model;
using ESFA.DC.Data.LARS.Model.Interfaces;
using ESFA.DC.Data.Organisatons.Model;
using ESFA.DC.Data.Organisatons.Model.Interface;
using ESFA.DC.Data.Postcodes.Model;
using ESFA.DC.Data.Postcodes.Model.Interfaces;
using ESFA.DC.Data.ULN.Model;
using ESFA.DC.Data.ULN.Model.Interfaces;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.ESF.DataAccessLayer;
using ESFA.DC.ESF.Database.EF;
using ESFA.DC.ESF.Database.EF.Interfaces;
using ESFA.DC.ESF.DataStore;
using ESFA.DC.ESF.Helpers;
using ESFA.DC.ESF.Interfaces;
using ESFA.DC.ESF.Interfaces.Config;
using ESFA.DC.ESF.Interfaces.Controllers;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ESF.Interfaces.DataStore;
using ESFA.DC.ESF.Interfaces.Helpers;
using ESFA.DC.ESF.Interfaces.Reports;
using ESFA.DC.ESF.Interfaces.Reports.Strategies;
using ESFA.DC.ESF.Interfaces.Services;
using ESFA.DC.ESF.Interfaces.Strategies;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.ReportingService;
using ESFA.DC.ESF.ReportingService.Reports;
using ESFA.DC.ESF.ReportingService.Reports.FundingSummary;
using ESFA.DC.ESF.ReportingService.Repositories;
using ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.CSVRowHelpers;
using ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.Ilr;
using ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.SuppData;
using ESFA.DC.ESF.Service.Config;
using ESFA.DC.ESF.Service.Stateless.Handlers;
using ESFA.DC.ESF.Services;
using ESFA.DC.ESF.Strategies;
using ESFA.DC.ESF.ValidationService;
using ESFA.DC.ESF.ValidationService.Commands;
using ESFA.DC.ESF.ValidationService.Commands.BusinessRules;
using ESFA.DC.ESF.ValidationService.Commands.CrossRecord;
using ESFA.DC.ESF.ValidationService.Commands.FieldDefinition;
using ESFA.DC.ESF.ValidationService.Commands.FileLevel;
using ESFA.DC.ILR1819.DataStore.EF;
using ESFA.DC.ILR1819.DataStore.EF.Interfaces;
using ESFA.DC.ILR1819.DataStore.EF.Valid;
using ESFA.DC.ILR1819.DataStore.EF.Valid.Interfaces;
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
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Model.Interface;
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

            var versionInfo = configHelper.GetSectionValues<Config.VersionInfo>("VersionSection");
            container.RegisterInstance(versionInfo).As<IVersionInfo>().SingleInstance();

            RegisterPersistence(container, configHelper);
            RegisterServiceBusConfig(container, configHelper);
            RegisterJobContextManagementServices(container);
            RegisterLogger(container, configHelper);
            RegisterSerializers(container);
            RegisterMessageHandler(container);

            RegisterControllers(container);

            RegisterCommands(container);

            RegisterStrategies(container);

            RegisterStorage(container);

            RegisterHelpers(container);

            RegisterFileLevelValidators(container);
            RegisterCrossRecordValidators(container);
            RegisterBusinessRuleValidators(container);
            RegisterFieldDefinitionValidators(container);

            RegisterReports(container);

            RegisterServices(container);

            RegisterRepositories(container);

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
                .As<IStreamableKeyValuePersistenceService>().As<IKeyValuePersistenceService>()
                .InstancePerLifetimeScope();

            var ilrConfig = configHelper.GetSectionValues<IRL1819Configuration>("ILR1819Section");
            containerBuilder.Register(c => new ILR1819_DataStoreEntities(ilrConfig.ILR1819ConnectionString))
                .As<IILR1819_DataStoreEntities>()
                .InstancePerLifetimeScope();
            containerBuilder.Register(c => new ILR1819_DataStoreEntitiesValid(ilrConfig.ILR1819ConnectionString))
                .As<IILR1819_DataStoreEntitiesValid>()
                .InstancePerLifetimeScope();

            var esfConfig = configHelper.GetSectionValues<ESFConfiguration>("ESFSection");
            containerBuilder.Register(c => new ESF_DataStoreEntities(esfConfig.ESFConnectionString))
                .As<IESF_DataStoreEntities>()
                .InstancePerLifetimeScope();
            containerBuilder.RegisterInstance(esfConfig).As<ESFConfiguration>().SingleInstance();

            var fcsConfig = configHelper.GetSectionValues<FCSConfiguration>("FCSSection");
            containerBuilder.Register(c => new FcsContext(fcsConfig.FCSConnectionString))
                .As<IFcsContext>()
                .InstancePerLifetimeScope();

            var referenceData = configHelper.GetSectionValues<ReferenceDataConfig>("ReferenceDataSection");
            containerBuilder.RegisterInstance(referenceData).As<IReferenceDataConfig>().SingleInstance();

            containerBuilder.Register(c =>
            {
                var referenceDataConfig = c.Resolve<IReferenceDataConfig>();
                return new LARS(referenceDataConfig.LARSConnectionString);
            }).As<ILARS>().InstancePerLifetimeScope();

            containerBuilder.Register(c =>
            {
                var referenceDataConfig = c.Resolve<IReferenceDataConfig>();
                return new Postcodes(referenceDataConfig.PostcodesConnectionString);
            }).As<IPostcodes>().InstancePerLifetimeScope();

            containerBuilder.Register(c =>
            {
                var referenceDataConfig = c.Resolve<IReferenceDataConfig>();
                return new Organisations(referenceDataConfig.OrganisationConnectionString);
            }).As<IOrganisations>().InstancePerLifetimeScope();

            containerBuilder.Register(c =>
            {
                var referenceDataConfig = c.Resolve<IReferenceDataConfig>();
                return new ULN(referenceDataConfig.ULNConnectionString);
            }).As<IULN>().InstancePerLifetimeScope();
        }

        private static void RegisterServiceBusConfig(
            ContainerBuilder containerBuilder,
            IConfigurationHelper configHelper)
        {
            var serviceBusOptions =
                configHelper.GetSectionValues<ServiceBusOptions>("ServiceBusSettings");
            containerBuilder.RegisterInstance(serviceBusOptions).As<ServiceBusOptions>().SingleInstance();

            var topicConfig = new ServiceBusTopicConfig(
                serviceBusOptions.ServiceBusConnectionString,
                serviceBusOptions.TopicName,
                serviceBusOptions.SubscriptionName,
                Environment.ProcessorCount,
                TimeSpan.FromMinutes(30));

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
                var config = new QueueConfiguration(
                    serviceBusOptions.ServiceBusConnectionString,
                    serviceBusOptions.AuditQueueName,
                    1);

                return new QueuePublishService<AuditingDto>(
                    config,
                    c.Resolve<IJsonSerializationService>());
            }).As<IQueuePublishService<AuditingDto>>();

            containerBuilder.Register(c =>
            {
                var config = new QueueConfiguration(
                    serviceBusOptions.ServiceBusConnectionString,
                    serviceBusOptions.JobStatusQueueName,
                    1);

                return new QueuePublishService<JobStatusDto>(
                    config,
                    c.Resolve<IJsonSerializationService>());
            }).As<IQueuePublishService<JobStatusDto>>();
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

        private static void RegisterLogger(ContainerBuilder containerBuilder, IConfigurationHelper configHelper)
        {
            var loggerConfig = configHelper.GetSectionValues<LoggerOptions>("LoggerSection");

            containerBuilder.RegisterInstance(new LoggerOptions
            {
                LoggerConnectionstring = loggerConfig.LoggerConnectionstring
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
                            ConnectionString = loggerOptions.LoggerConnectionstring,
                            LogsTableName = "Logs"
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

            containerBuilder.RegisterType<FileValidationService>().As<IFileValidationService>();
        }

        private static void RegisterControllers(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ServiceController>().As<IServiceController>();
            containerBuilder.RegisterType<ReportingController>().As<IReportingController>();
            containerBuilder.RegisterType<ValidationController>().As<IValidationController>();
            containerBuilder.RegisterType<StorageController>().As<IStorageController>();
        }

        private static void RegisterRepositories(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<EsfRepository>().As<IEsfRepository>();
            containerBuilder.RegisterType<FM70Repository>().As<IFM70Repository>();
            containerBuilder.RegisterType<ValidRepository>().As<IValidRepository>();
            containerBuilder.RegisterType<ReferenceDataRepository>().As<IReferenceDataRepository>();
            containerBuilder.RegisterType<ReferenceDataCache>().As<IReferenceDataCache>()
                .InstancePerLifetimeScope();
        }

        private static void RegisterHelpers(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<FileHelper>().As<IFileHelper>();
            containerBuilder.RegisterType<TaskHelper>().As<ITaskHelper>();
        }

        private static void RegisterCommands(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<BusinessRuleCommands>().As<IValidatorCommand>();
            containerBuilder.RegisterType<FieldDefinitionCommand>().As<IValidatorCommand>();
            containerBuilder.RegisterType<CrossRecordCommands>().As<IValidatorCommand>();

            containerBuilder.Register(c => new List<IValidatorCommand>(c.Resolve<IEnumerable<IValidatorCommand>>()))
                .As<IList<IValidatorCommand>>();
        }

        private static void RegisterStrategies(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<PersistenceStrategy>().As<ITaskStrategy>();
            containerBuilder.RegisterType<ValidationStrategy>().As<ITaskStrategy>();
            containerBuilder.RegisterType<ReportingStrategy>().As<ITaskStrategy>();
            containerBuilder.Register(c => new List<ITaskStrategy>(c.Resolve<IEnumerable<ITaskStrategy>>()))
                .As<IList<ITaskStrategy>>();

            containerBuilder.RegisterType<DataRowHelper>().As<IRowHelper>();
            containerBuilder.RegisterType<TitleRowHelper>().As<IRowHelper>();
            containerBuilder.RegisterType<SpacerRowHelper>().As<IRowHelper>();
            containerBuilder.RegisterType<TotalRowHelper>().As<IRowHelper>();
            containerBuilder.RegisterType<CumulativeRowHelper>().As<IRowHelper>();
            containerBuilder.Register(c => new List<IRowHelper>(c.Resolve<IEnumerable<IRowHelper>>()))
                .As<IList<IRowHelper>>();

            containerBuilder.RegisterType<AC01ActualCosts>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<CG01CommunityGrantPayment>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<CG02CommunityGrantManagementCost>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<FS01AdditionalProgrammeCostAdjustments>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<NR01NonRegulatedActivityAuditAdjustment>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<NR01NonRegulatedActivityAuthorisedClaims>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<PG01ProgressionPaidEmploymentAdjustments>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<PG02ProgressionUnpaidEmploymentAdjustments>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<PG03ProgressionEducationAdjustments>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<PG04ProgressionApprenticeshipAdjustments>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<PG05ProgressionTraineeshipAdjustments>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<PG06ProgressionJobSearchAdjustments>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<RQ01RegulatedLearningAuditAdjustment>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<RQ01RegulatedLearningAuthorisedClaims>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<SD01FCSDeliverableDescription>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<SD02FCSDeliverableDescription>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<SD03FCSDeliverableDescription>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<SD04FCSDeliverableDescription>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<SD05FCSDeliverableDescription>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<SD06FCSDeliverableDescription>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<SD07FCSDeliverableDescription>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<SD08FCSDeliverableDescription>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<SD09FCSDeliverableDescription>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<SD10FCSDeliverableDescription>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<ST01LearnerAssessmentAndPlanAdjustments>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<SU01SustainedPaidEmployment3MonthsAdjustments>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<SU02SustainedUnpaidEmployment3MonthsAdjustments>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<SU03SustainedEducation3MonthsAdjustments>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<SU04SustainedApprenticeship3MonthsAdjustments>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<SU05SustainedTraineeship3MonthsAdjustments>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<SU11SustainedPaidEmployment6MonthsAdjustments>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<SU12SustainedUnpaidEmployment6MonthsAdjustments>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<SU13SustainedEducation6MonthsAdjustments>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<SU14SustainedApprenticeship6MonthsAdjustments>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<SU15SustainedTraineeship6MonthsAdjustments>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<SU21SustainedPaidEmployment12MonthsAdjustments>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<SU22SustainedUnpaidEmployment12MonthsAdjustments>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<SU23SustainedEducation12MonthsAdjustments>().As<ISupplementaryDataStrategy>();
            containerBuilder.RegisterType<SU24SustainedApprenticeship12MonthsAdjustments>().As<ISupplementaryDataStrategy>();
            containerBuilder.Register(c => new List<ISupplementaryDataStrategy>(c.Resolve<IEnumerable<ISupplementaryDataStrategy>>()))
                .As<IList<ISupplementaryDataStrategy>>();

            containerBuilder.RegisterType<FS01AdditionalProgrammeCost>().As<IILRDataStrategy>();
            containerBuilder.RegisterType<NR01NonRegulatedActivityAchievementEarnings>().As<IILRDataStrategy>();
            containerBuilder.RegisterType<NR01NonRegulatedActivityStartFunding>().As<IILRDataStrategy>();
            containerBuilder.RegisterType<PG01ProgressionPaidEmployment>().As<IILRDataStrategy>();
            containerBuilder.RegisterType<PG02ProgressionUnpaidEmployment>().As<IILRDataStrategy>();
            containerBuilder.RegisterType<PG03ProgressionEducation>().As<IILRDataStrategy>();
            containerBuilder.RegisterType<PG04ProgressionApprenticeship>().As<IILRDataStrategy>();
            containerBuilder.RegisterType<PG05ProgressionTraineeship>().As<IILRDataStrategy>();
            containerBuilder.RegisterType<PG06ProgressionJobSearch>().As<IILRDataStrategy>();
            containerBuilder.RegisterType<RQ01RegulatedLearningAchievementEarnings>().As<IILRDataStrategy>();
            containerBuilder.RegisterType<RQ01RegulatedLearningStartFunding>().As<IILRDataStrategy>();
            containerBuilder.RegisterType<ST01LearnerAssessmentAndPlan>().As<IILRDataStrategy>();
            containerBuilder.RegisterType<SU01SustainedPaidEmployment3Months>().As<IILRDataStrategy>();
            containerBuilder.RegisterType<SU02SustainedUnpaidEmployment3Months>().As<IILRDataStrategy>();
            containerBuilder.RegisterType<SU03SustainedEducation3Months>().As<IILRDataStrategy>();
            containerBuilder.RegisterType<SU04SustainedApprenticeship3Months>().As<IILRDataStrategy>();
            containerBuilder.RegisterType<SU05SustainedTraineeship3Months>().As<IILRDataStrategy>();
            containerBuilder.RegisterType<SU11SustainedPaidEmployment6Months>().As<IILRDataStrategy>();
            containerBuilder.RegisterType<SU12SustainedUnpaidEmployment6Months>().As<IILRDataStrategy>();
            containerBuilder.RegisterType<SU13SustainedEducation6Months>().As<IILRDataStrategy>();
            containerBuilder.RegisterType<SU14SustainedApprenticeship6Months>().As<IILRDataStrategy>();
            containerBuilder.RegisterType<SU15SustainedTraineeship6Months>().As<IILRDataStrategy>();
            containerBuilder.RegisterType<SU21SustainedPaidEmployment12Months>().As<IILRDataStrategy>();
            containerBuilder.RegisterType<SU22SustainedUnpaidEmployment12Months>().As<IILRDataStrategy>();
            containerBuilder.RegisterType<SU23SustainedEducation12Months>().As<IILRDataStrategy>();
            containerBuilder.RegisterType<SU24SustainedApprenticeship12Months>().As<IILRDataStrategy>();
            containerBuilder.Register(c => new List<IILRDataStrategy>(c.Resolve<IEnumerable<IILRDataStrategy>>()))
                .As<IList<IILRDataStrategy>>();
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

        private static void RegisterReports(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ValidationResultReport>().As<IValidationResultReport>();

            containerBuilder.RegisterType<ValidationResultReport>().As<IValidationReport>()
                .WithAttributeFiltering()
                .InstancePerLifetimeScope();
            containerBuilder.RegisterType<ValidationErrorReport>().As<IValidationReport>()
                .WithAttributeFiltering()
                .InstancePerLifetimeScope();
            containerBuilder.Register(c => new List<IValidationReport>(c.Resolve<IEnumerable<IValidationReport>>()))
                .As<IList<IValidationReport>>();

            containerBuilder.RegisterType<FundingSummaryReport>().As<IModelReport>()
                .WithAttributeFiltering()
                .InstancePerLifetimeScope();
            containerBuilder.RegisterType<AimAndDeliverableReport>().As<IModelReport>()
                .WithAttributeFiltering()
                .InstancePerLifetimeScope();
            containerBuilder.Register(c => new List<IModelReport>(c.Resolve<IEnumerable<IModelReport>>()))
                .As<IList<IModelReport>>();
        }

        private static void RegisterStorage(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<StoreFileDetails>().As<IStoreFileDetails>();
            containerBuilder.RegisterType<StoreESF>().As<IStoreESF>();
        }
    }
}
