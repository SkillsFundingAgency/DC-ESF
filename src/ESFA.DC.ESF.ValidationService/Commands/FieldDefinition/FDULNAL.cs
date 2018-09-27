using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.FieldDefinition
{
    public class FDULNAL : IFieldDefinitionValidator
    {
        public string Level => "Error";

        public string ErrorMessage => $"The ULN must not exceed {FieldLength} characters in length. Please adjust the value and resubmit the file.";

        public bool IsValid { get; private set; }

        private const int FieldLength = 10;

        public Task Execute(SupplementaryDataModel model)
        {
            //var uln = model.ULN.ToString();

            //IsValid = !string.IsNullOrEmpty(month)
            //          && !string.IsNullOrWhiteSpace(month)
            //          && month.Length <= FieldLength;


            //IsValid = string.IsNullOrEmpty(model.ULN.ToString()) || model.ULN.ToString().Length <= FieldLength;

            return Task.CompletedTask;
        }
    }
}
