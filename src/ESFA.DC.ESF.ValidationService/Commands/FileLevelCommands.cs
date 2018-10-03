using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ValidationService.Builders;

namespace ESFA.DC.ESF.ValidationService.Commands
{
    public class FileLevelCommands : IValidatorCommand
    {
        private readonly IList<IFileLevelValidator> _fileLevelValidators;

        public bool IsValid { get; private set; }

        public bool RejectFile { get; private set; }

        public IList<ValidationErrorModel> Errors { get; }

        public int Priority => 1;

        public FileLevelCommands(IList<IFileLevelValidator> fileLevelValidators)
        {
            _fileLevelValidators = fileLevelValidators;

            Errors = new List<ValidationErrorModel>();
        }

        public async Task Execute(SupplementaryDataModel model)
        {
            foreach (var validator in _fileLevelValidators)
            {
                await validator.Execute(string.Empty, model);
            }

            var failed = _fileLevelValidators.Where(v => !v.IsValid).ToList();
            if (failed.Any())
            {
                IsValid = false;
                RejectFile = true;
            }
        }
    }
}
