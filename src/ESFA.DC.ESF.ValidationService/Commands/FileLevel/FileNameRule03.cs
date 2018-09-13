using ESFA.DC.ESF.Interfaces.Validation;

namespace ESFA.DC.ESF.ValidationService.Commands.FileLevel
{
    public class FileNameRule03 : IFileLevelValidator
    {
        public string ErrorMessage => "The filename does not meet the agreed format.";
        public bool IsValid { get; set; }

        public bool RejectFile => true;

        public void Execute(string fileName, string contents)
        {

        }
    }
}
