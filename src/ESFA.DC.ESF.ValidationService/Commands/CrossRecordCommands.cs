using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ValidationService.Builders;

namespace ESFA.DC.ESF.ValidationService.Commands
{
    public class CrossRecordCommands : ICrossRecordCommand
    {
        private readonly IList<ICrossRecordValidator> _validators;

        public IList<ValidationErrorModel> Errors { get; }

        public bool RejectFile => false;

        public bool IsValid { get; private set; }

        public int Priority => 3;

        public IList<SupplementaryDataModel> AllRecords { get; set; }

        public CrossRecordCommands(IList<ICrossRecordValidator> validators)
        {
            _validators = validators;

            Errors = new List<ValidationErrorModel>();
        }

        public async Task Execute(SupplementaryDataModel model)
        {
            IsValid = true;
            foreach (var validator in _validators)
            {
                validator.AllRecords = AllRecords;
                await validator.Execute(model);
            }

            var failed = _validators.Where(v => !v.IsValid).ToList();
            if (failed.Any())
            {
                IsValid = false;
                foreach (var validator in failed)
                {
                    Errors.Add(ValidationErrorBuilder.BuildValidationErrorModel(model, validator));
                }
            }
        }
    }
}
