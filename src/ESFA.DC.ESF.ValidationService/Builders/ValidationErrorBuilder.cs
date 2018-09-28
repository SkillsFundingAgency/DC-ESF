using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Builders
{
    public class ValidationErrorBuilder
    {
        public static ValidationErrorModel BuildValidationErrorModel(SupplementaryDataModel model,  IBaseValidator validator)
        {
            return new ValidationErrorModel();
        }
    }
}
