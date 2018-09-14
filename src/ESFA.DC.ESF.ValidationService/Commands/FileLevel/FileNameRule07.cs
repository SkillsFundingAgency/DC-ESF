using ESFA.DC.ESF.Interfaces.Validation;

namespace ESFA.DC.ESF.ValidationService.Commands.FileLevel
{
    public class FileNameRule07 : IFileLevelValidator
    {
        public string ErrorMessage => "The file extension is not .csv";

        public bool IsValid { get; private set; }

        public bool RejectFile => true;

        public void Execute(string fileName, string contents)
        {

        }
    }
}
