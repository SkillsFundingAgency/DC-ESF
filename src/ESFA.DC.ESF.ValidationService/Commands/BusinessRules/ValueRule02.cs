using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class ValueRule02 : IBusinessRuleValidator
    {
        public string ErrorMessage => "The Value is not required for the selected CostType";

        public string ErrorName => "Value_02";

        public bool IsWarning => false;

        public bool IsValid { get; private set; }

        public Task Execute(SupplementaryDataModel model)
        {
            IsValid = !(model.CostType == "Unit Cost" || model.CostType == "Unit Cost Deduction")
                        ||
                        model.Value == null;

            return Task.CompletedTask;
        }
    }
}
