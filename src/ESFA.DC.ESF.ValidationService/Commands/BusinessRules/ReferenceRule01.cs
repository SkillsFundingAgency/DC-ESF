using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class ReferenceRule01 : IBusinessRuleValidator
    {
        private const string Pattern = @"^[A-Z,a-z,0-9,\s\.,;:~!”@#\$&’()\/\+-<=>\[\]\{}\^£€]*$";

        public string ErrorMessage => "The Reference contains invalid characters.";

        public bool IsValid { get; private set; }

        public Task Execute(ESFModel model)
        {
            IsValid = Regex.IsMatch(model.Reference, Pattern);
            return Task.CompletedTask;
        }
    }
}
