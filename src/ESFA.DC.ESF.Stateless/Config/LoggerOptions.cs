using ESFA.DC.ESF.Stateless.Config.Interfaces;

namespace ESFA.DC.ESF.Stateless.Config
{
    public class LoggerOptions : ILoggerOptions
    {
        public string LoggerConnectionString { get; set; }
    }
}
