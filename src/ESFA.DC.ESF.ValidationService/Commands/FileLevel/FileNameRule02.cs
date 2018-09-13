using ESFA.DC.ESF.Interfaces.Validation;

namespace ESFA.DC.ESF.ValidationService.Commands.FileLevel
{
    public class FileNameRule02: IFileLevelValidator
    {
        public string ErrorMessage => "The UKPRN in the filename is invalid";
        public bool IsValid { get; set; }

        public bool RejectFile => true;

        public void Execute(string fileName, string contents)
        {

        }
    }
}
