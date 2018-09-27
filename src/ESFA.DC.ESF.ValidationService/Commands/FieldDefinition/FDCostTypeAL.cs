using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.FieldDefinition
{
    public class FDCostTypeAL : IFieldDefinitionValidator
    {
        public string Level => "Error";

        public string ErrorMessage => $"The CostType must not exceed {FieldLength} characters in length. Please adjust the value and resubmit the file.";

        public bool IsValid { get; private set; }

        private const int FieldLength = 20;

        public Task Execute(SupplementaryDataModel model)
        {
            IsValid = !string.IsNullOrEmpty(model.CostType.Trim()) && model.ConRefNumber.Length <= FieldLength;

            return Task.CompletedTask;
        }
    }
}
