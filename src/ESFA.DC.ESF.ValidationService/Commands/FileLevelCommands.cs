using System.Collections.Generic;
using ESFA.DC.ESF.Interfaces.Validation;

namespace ESFA.DC.ESF.ValidationService.Commands
{
    public class FileLevelCommands : IValidatorCommand
    {
        private readonly IList<IFileLevelValidator> _fileLevelValidators;

        public bool IsValid { get; set; }

        public bool RejectFile { get; set; }    

        public FileLevelCommands(IList<IFileLevelValidator> fileLevelValidators)
        {
            _fileLevelValidators = fileLevelValidators;
        }

        public void Execute()
        {
            foreach (var validator in _fileLevelValidators)
            {
                validator.Execute(string.Empty, string.Empty);
            }
        }
    }
}
