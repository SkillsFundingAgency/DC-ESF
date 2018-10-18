using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class DeliverableCodeRule02 : IBusinessRuleValidator
    {
        private readonly IReferenceDataRepository _referenceDataRepository;

        public DeliverableCodeRule02(IReferenceDataRepository referenceDataRepository)
        {
            _referenceDataRepository = referenceDataRepository;
        }

        public string ErrorMessage => "The DeliverableCode is not valid for the approved contract allocation.";

        public string ErrorName => "DeliverableCode_02";

        public bool IsWarning => false;

        public bool IsValid { get; private set; }

        public Task Execute(SupplementaryDataModel model)
        {
            // todo ... need FCA

            return Task.CompletedTask;
        }
    }
}
