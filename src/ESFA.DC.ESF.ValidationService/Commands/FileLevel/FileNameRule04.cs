using ESFA.DC.ESF.Interfaces.Validation;

namespace ESFA.DC.ESF.ValidationService.Commands.FileLevel
{
    public class FileNameRule04 : IFileLevelValidator
    {
        public string ErrorMessage => "A file with the same filename has already successfully been processed.";

        public bool IsValid { get; private set; }

        public bool RejectFile => true;

        public void Execute(string fileName, string contents)
        {

        }
    }
}
