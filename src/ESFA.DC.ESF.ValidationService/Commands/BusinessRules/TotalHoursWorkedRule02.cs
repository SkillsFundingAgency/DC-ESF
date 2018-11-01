using System.Collections.Generic;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class TotalHoursWorkedRule02 : IBusinessRuleValidator
    {
        public string ErrorMessage => "The TotalHoursWorked is not required for the selected CostType.";

        public string ErrorName => "TotalHoursWorked_02";

        public bool IsWarning => true;

        public bool IsValid { get; private set; }

        public Task Execute(SupplementaryDataModel model)
        {
            var staffCostTypes = new List<string> { Constants.CostTypeStaffPT, Constants.CostTypeStaffFT };

            IsValid = staffCostTypes.Contains(model.CostType) || model.TotalHoursWorked == null;

            return Task.CompletedTask;
        }
    }
}
