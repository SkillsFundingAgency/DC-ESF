using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class ULNRule02 : IBusinessRuleValidator
    {
        public string ErrorMessage => "The ULN is not a valid ULN.";

        public string ErrorName => "ULN_02";

        public bool IsWarning => false;

        public bool IsValid { get; private set; }

        public Task Execute(SupplementaryDataModel model)
        {
            // todo .. need lookup
            // IsValid  = !(model.ReferenceType == "LearnRefNumber"
            // &&
            //    (model.ULN does not exist in ULN table)
            // &&
            // (model.ULN ?? 0) != 9999999999)

            return Task.CompletedTask;
        }
    }
}
