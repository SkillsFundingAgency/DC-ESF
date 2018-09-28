using System.Collections.Generic;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

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
        }

        public async Task Execute(SupplementaryDataModel model)
        {
            foreach (var validator in _fileLevelValidators)
            {
                await validator.Execute(string.Empty, model);
            }
        }
    }
}
