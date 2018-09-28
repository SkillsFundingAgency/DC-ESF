using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class ProjectHoursOrgHoursRule01 : IBusinessRuleValidator
    {
        public string ErrorMessage => "The ProjectHours must be less or equal to the OrgHours";

        public string ErrorName => "ProjectHoursOrgHours_01";

        public bool IsWarning => false;

        public bool IsValid { get; private set; }

        public Task Execute(SupplementaryDataModel model)
        {
            IsValid = model.CostType != Constants.CostTypeApportionedCost || model.ProjectHours <= model.OrgHours;

            return Task.CompletedTask;
        }
    }
}
