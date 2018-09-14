using ESFA.DC.ESF.Service.Stateless.Config.Interfaces;

namespace ESFA.DC.ESF.Service.Stateless.Config
{
    public class LoggerOptions : ILoggerOptions
    {
        public string LoggerConnectionString { get; set; }
    }
}
