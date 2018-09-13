using ESFA.DC.ESF.Interfaces.Validation;

namespace ESFA.DC.ESF.ValidationService.Commands.FileLevel
{
    public class FileNameRule05 : IFileLevelValidator
    {
        public string ErrorMessage => "The date/time in the filename > current date/time.";

        public bool IsValid { get; set; }

        public bool RejectFile => true;

        public void Execute(string fileName, string contents)
        {

        }
    }
}
