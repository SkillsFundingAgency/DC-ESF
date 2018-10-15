using ESFA.DC.ESF.Interfaces.Config;

namespace ESFA.DC.ESF.Service.Config
{
    public sealed class VersionInfo : IVersionInfo
    {
        public string ServiceReleaseVersion { get; set; }
    }
}
