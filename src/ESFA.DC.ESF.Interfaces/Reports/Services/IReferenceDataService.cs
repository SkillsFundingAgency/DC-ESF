using System.Threading;

namespace ESFA.DC.ESF.Interfaces.Reports.Services
{
    public interface IReferenceDataService
    {
        string GetPostcodeVersion(CancellationToken cancellationToken);

        string GetLarsVersion(CancellationToken cancellationToken);

        string GetOrganisationVersion(CancellationToken cancellationToken);

        string GetProviderName(int ukPrn, CancellationToken cancellationToken);
    }
}