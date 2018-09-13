using System.Collections.Generic;
using Autofac;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Stateless.Config;
using ESFA.DC.ESF.Stateless.Config.Interfaces;
using ESFA.DC.ESF.ValidationService.Commands.FileLevel;
using ESFA.DC.Logging;
using ESFA.DC.Logging.Config;
using ESFA.DC.Logging.Config.Interfaces;
using ESFA.DC.Logging.Enums;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;
using ESFA.DC.Serialization.Json;

namespace ESFA.DC.ESF.Stateless
{
    public class DIComposition
    {
        public static ContainerBuilder BuildContainer()
        {
            var container = new ContainerBuilder();

            RegisterLogger(container);
            RegisterSerializers(container);

            RegisterFileLevelValidators(container);


            return container;
        }

        private static void RegisterSerializers(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<JsonSerializationService>().As<IJsonSerializationService>();
        }

        private static void RegisterLogger(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterInstance(new LoggerOptions()
            {
                LoggerConnectionString = @"Server=(local);Database=Logging;Trusted_Connection=True;"
            }).As<ILoggerOptions>().SingleInstance();

            containerBuilder.Register(c =>
            {
                var loggerOptions = c.Resolve<ILoggerOptions>();
                return new ApplicationLoggerSettings
                {
                    ApplicationLoggerOutputSettingsCollection = new List<IApplicationLoggerOutputSettings>()
                    {
                        new MsSqlServerApplicationLoggerOutputSettings()
                        {
                            MinimumLogLevel = LogLevel.Verbose,
                            ConnectionString = loggerOptions.LoggerConnectionString
                        },
                        new ConsoleApplicationLoggerOutputSettings()
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
