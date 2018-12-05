using ESFA.DC.ESF.Interfaces.Config;

namespace ESFA.DC.ESF.Service.Config
{
    public class ReferenceDataConfig : IReferenceDataConfig
    {
        public string LARSConnectionString { get; set; }

        public string PostcodesConnectionString { get; set; }

        public string OrganisationConnectionString { get; set; }

        public string ULNConnectionString { get; set; }
    }
}
