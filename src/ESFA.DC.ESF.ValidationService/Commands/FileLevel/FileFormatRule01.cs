using ESFA.DC.ESF.Interfaces.Validation;

namespace ESFA.DC.ESF.ValidationService.Commands.FileLevel
{
    public class FileFormatRule01 : IFileLevelValidator
    {
        public string ErrorMessage => "The file is not in the format described.";
        public bool IsValid { get; set; }

        public bool RejectFile => true;

        public void Execute(string fileName, string contents)
        {

        }
    }
}
