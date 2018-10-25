using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class ULNRule02 : IBusinessRuleValidator
    {
        private readonly IReferenceDataRepository _referenceDataRepository;

        public string ErrorMessage => "The ULN is not a valid ULN.";

        public string ErrorName => "ULN_02";

        public bool IsWarning => false;

        public bool IsValid { get; private set; }

        public ULNRule02(IReferenceDataRepository referenceDataRepository)
        {
            _referenceDataRepository = referenceDataRepository;
        }

        public Task Execute(SupplementaryDataModel model)
        {
            IsValid = true;
            //IsValid = model.ReferenceType != "LearnRefNumber" ||
            //          (model.ULN ?? 0) == 9999999999 ||
            //          _referenceDataRepository.GetUlnLookup(CancellationToken.None).Any(u => u.ULN == model.ULN);

            return Task.CompletedTask;
        }
    }
}
