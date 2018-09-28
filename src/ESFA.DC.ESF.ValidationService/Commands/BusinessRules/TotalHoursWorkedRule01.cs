using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class TotalHoursWorkedRule01 : IBusinessRuleValidator
    {
        public string ErrorMessage => "The TotalHoursWorked must be returned for the selected CostType.";

        public string ErrorName => "TotalHoursWorked_01";

        public bool IsWarning => false;

        public bool IsValid { get; private set; }

        public Task Execute(SupplementaryDataModel model)
        {
            IsValid = !((model.CostType == Constants.CostTypeStaffPT || model.CostType == Constants.CostTypeStaffFT)
                        &&
                        model.TotalHoursWorked == null);

            return Task.CompletedTask;
        }
    }
}
