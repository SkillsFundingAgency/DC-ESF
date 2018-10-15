namespace ESFA.DC.ESF.Interfaces.Reports.Services
{
    public interface IReferenceDataService
    {
        string GetPostcodeVersion();

        string GetLarsVersion();

        string GetOrganisationVersion();

        string GetProviderName(int ukPrn);
    }
}