using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.ESF.Interfaces;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models.Configuration;
using ESFA.DC.ESF.Service.Stateless.Config;
using ESFA.DC.ESF.Service.Stateless.Config.Interfaces;
using ESFA.DC.ESF.Service.Stateless.Handlers;
using ESFA.DC.ESF.ValidationService.Commands.FileLevel;
using ESFA.DC.IO.AzureStorage;
using ESFA.DC.IO.AzureStorage.Config.Interfaces;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.JobContext;
using ESFA.DC.Logging;
using ESFA.DC.Logging.Config;
using ESFA.DC.Logging.Config.Interfaces;
using ESFA.DC.Logging.Enums;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Queueing;
using ESFA.DC.Queueing.Interface;
using ESFA.DC.Serialization.Interfaces;
using ESFA.DC.Serialization.Json;
using ESFA.DC.ServiceFabric.Helpers.Interfaces;
using ExecutionContext = ESFA.DC.Logging.ExecutionContext;

namespace ESFA.DC.ESF.Stateless
{
    public class DIComposition
    {
        public static ContainerBuilder BuildContainer(IConfigurationHelper configHelper)
        {
            var container = new ContainerBuilder();

            var orgConfiguration = configHelper.GetSectionValues<OrgConfiguration>("OrgSection");
            container.RegisterInstance(orgConfiguration).As<OrgConfiguration>().SingleInstance();

            RegisterPersistence(container, configHelper);

            RegisterServiceBusConfig(container, configHelper);

            RegisterLogger(container);

            RegisterSerializers(container);

            RegisterMessageHandler(container);

            RegisterFileLevelValidators(container);
            
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
                        c.Resolve<ILogger>(),
                        c.Resolve<IDateTimeProvider>());
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
            containerBuilder.RegisterType<MessageHandler>().As<IMessageHandler>().InstancePerLifetimeScope();

            // register the  callback handle when a new message is received from ServiceBus
            containerBuilder.Register<Func<JobContextMessage, CancellationToken, Task<bool>>>(c =>
                c.Resolve<IMessageHandler>().Handle).InstancePerLifetimeScope();

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

        }

        private static void RegisterControllers(ContainerBuilder containerBuilder)
        {
            // containerBuilder.RegisterType<ValidationController>().As<IValidationController>()
        }

        private static void RegisterFileLevelValidators(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<FileFormatRule01>().As<IFileLevelValidator>();
            containerBuilder.RegisterType<FileNameRule01>().As<IFileLevelValidator>();
            containerBuilder.RegisterType<FileNameRule02>().As<IFileLevelValidator>();
            containerBuilder.RegisterType<FileNameRule03>().As<IFileLevelValidator>();
            containerBuilder.RegisterType<FileNameRule04>().As<IFileLevelValidator>();
            containerBuilder.RegisterType<FileNameRule05>().As<IFileLevelValidator>();
            containerBuilder.RegisterType<FileNameRule06>().As<IFileLevelValidator>();
            containerBuilder.RegisterType<FileNameRule07>().As<IFileLevelValidator>();
            containerBuilder.RegisterType<FileNameRule08>().As<IFileLevelValidator>();
            containerBuilder.RegisterType<ConRefNumberRule01>().As<IFileLevelValidator>();

            containerBuilder.Register(c => new List<IFileLevelValidator>(c.Resolve<IEnumerable<IFileLevelValidator>>()))
                .As<IList<IFileLevelValidator>>();
        }
    }
}
