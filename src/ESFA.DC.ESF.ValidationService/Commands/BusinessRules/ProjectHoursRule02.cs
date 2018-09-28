using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class ProjectHoursRule02 : IBusinessRuleValidator
    {
        public string ErrorMessage => "The ProjectHours is not required for the selected CostType.";

        public string ErrorName => "ProjectHours_02";

        public bool IsWarning => true;

        public bool IsValid { get; private set; }

        public Task Execute(SupplementaryDataModel model)
        {
            IsValid = model.CostType == Constants.CostTypeApportionedCost || model.ProjectHours == null;

            return Task.CompletedTask;
        }
    }
}
