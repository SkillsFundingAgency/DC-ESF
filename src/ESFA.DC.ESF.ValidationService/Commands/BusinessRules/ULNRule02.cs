using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class ULNRule02 : IBusinessRuleValidator
    {
        private readonly IReferenceDataRepository _referenceDataRepository;

        public ULNRule02(IReferenceDataRepository referenceDataRepository)
        {
            _referenceDataRepository = referenceDataRepository;
        }

        public string ErrorMessage => "The ULN is not a valid ULN.";

        public string ErrorName => "ULN_02";

        public bool IsWarning => false;

        public bool Execute(SupplementaryDataModel model)
        {
            return model.ReferenceType != "LearnRefNumber" ||
                      (model.ULN ?? 0) == 9999999999 ||
                      _referenceDataRepository.GetUlnLookup(new List<long?> { model.ULN ?? 0 }, CancellationToken.None).Any(u => u.ULN == model.ULN);
        }
    }
}
