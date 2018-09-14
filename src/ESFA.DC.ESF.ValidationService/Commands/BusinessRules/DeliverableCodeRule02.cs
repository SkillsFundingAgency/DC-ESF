using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class DeliverableCodeRule02 : IBusinessRuleValidator
    {
        public string ErrorMessage => "The DeliverableCode is not valid for the approved contract allocation.";

        public bool IsValid { get; private set; }

        public Task Execute(ESFModel model)
        {
            return Task.CompletedTask;
        }
    }
}
