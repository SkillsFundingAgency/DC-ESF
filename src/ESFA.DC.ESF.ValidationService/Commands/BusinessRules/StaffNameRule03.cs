using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class StaffNameRule03 : IBusinessRuleValidator
    {
        public string ErrorMessage => "The StaffName is not required for the selected ReferenceType.";

        public string ErrorName => "StaffName_03";

        public bool IsWarning => true;

        public bool IsValid { get; private set; }

        public Task Execute(SupplementaryDataModel model)
        {
            IsValid = model.ReferenceType != "EmployeeID" || string.IsNullOrEmpty(model.StaffName.Trim());

            return Task.CompletedTask;
        }
    }
}
