using System.Collections.Generic;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands
{
    public class FileLevelCommands : IValidatorCommand
    {
        private readonly IList<IFileLevelValidator> _fileLevelValidators;

        public bool IsValid { get; private set; }

        public bool RejectFile { get; private set; }    

        public FileLevelCommands(IList<IFileLevelValidator> fileLevelValidators)
        {
            _fileLevelValidators = fileLevelValidators;
        }

        public void Execute(ESFModel model)
        {
            foreach (var validator in _fileLevelValidators)
            {
                validator.Execute(string.Empty, string.Empty);
            }
        }
    }
}
