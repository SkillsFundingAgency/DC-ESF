using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ValidationService.Builders;

namespace ESFA.DC.ESF.ValidationService.Commands
{
    public class CrossRecordCommands : ICrossRecordCommand
    {
        private readonly IList<ICrossRecordValidator> _validators;

        public CrossRecordCommands(IList<ICrossRecordValidator> validators)
        {
            _validators = validators;
        }

        public IList<ValidationErrorModel> Errors { get; private set; }

        public bool RejectFile => false;

        public int Priority => 3;

        public IList<SupplementaryDataModel> AllRecords { get; set; }

        public bool Execute(SupplementaryDataModel model)
        {
            Errors = new List<ValidationErrorModel>();

            foreach (var validator in _validators)
            {
                if (!validator.Execute(AllRecords, model))
                {
                    Errors.Add(ValidationErrorBuilder.BuildValidationErrorModel(model, validator));
                }
            }

            return !Errors.Any();
        }
    }
}
