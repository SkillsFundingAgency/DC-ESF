namespace ESFA.DC.ESF.Interfaces.Config
{
    public interface IReferenceDataConfig
    {
        string LARSConnectionString { get; set; }

        string PostcodesConnectionString { get; set; }

        string OrganisationConnectionString { get; set; }
    }
}