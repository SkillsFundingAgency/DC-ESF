using ESFA.DC.ESF.Interfaces.Validation;

namespace ESFA.DC.ESF.ValidationService.Commands.FileLevel
{
    public class FileNameRule01 : IFileLevelValidator
    {
        public string ErrorMessage => "The UKPRN in the filename does not match the UKPRN in the Hub";

        public bool IsValid { get; set; }

        public bool RejectFile => true;

        public void Execute(string fileName, string contents)
        {

        }
    }
}
