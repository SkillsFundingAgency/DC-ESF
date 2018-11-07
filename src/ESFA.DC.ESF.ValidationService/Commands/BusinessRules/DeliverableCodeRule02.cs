using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class DeliverableCodeRule02 : IBusinessRuleValidator
    {
        private readonly IReferenceDataRepository _referenceDataRepository;
        private readonly IFcsCodeMappingHelper _mappingHelper;

        public DeliverableCodeRule02(
            IReferenceDataRepository referenceDataRepository,
            IFcsCodeMappingHelper mappingHelper)
        {
            _referenceDataRepository = referenceDataRepository;
            _mappingHelper = mappingHelper;
        }

        public string ErrorMessage => "The DeliverableCode is not valid for the approved contract allocation.";

        public string ErrorName => "DeliverableCode_02";

        public bool IsWarning => false;

        public bool Execute(SupplementaryDataModel model)
        {
            var fcsDeliverableCode = _mappingHelper.GetFcsDeliverableCode(model, CancellationToken.None);
            var contractAllocation = _referenceDataRepository.GetContractAllocation(model.ConRefNumber, fcsDeliverableCode, CancellationToken.None);

            return contractAllocation != null;
        }
    }
}
