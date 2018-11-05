using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ValidationService.Builders;

namespace ESFA.DC.ESF.ValidationService.Commands
{
    public class FieldDefinitionCommand : IValidatorCommand
    {
        private readonly IList<IFieldDefinitionValidator> _validators;

        public FieldDefinitionCommand(IList<IFieldDefinitionValidator> validators)
        {
            _validators = validators;
        }

        public IList<ValidationErrorModel> Errors { get; private set; }

        public bool RejectFile => false;

        public int Priority => 2;

        public bool Execute(SupplementaryDataModel model)
        {
            Errors = new List<ValidationErrorModel>();

            foreach (var validator in _validators)
            {
                if (!validator.Execute(model))
                {
                    Errors.Add(ValidationErrorBuilder.BuildValidationErrorModel(model, validator));
                }
            }

            return !Errors.Any();
        }
    }
}
