using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;

namespace ESFA.DC.ESF.ValidationService.Commands.FileLevel
{
    public class FileNameRule08 : IFileLevelValidator
    {
        public string ErrorMessage => "The date/time of the file is not greater than a previous transmission with the same ConRefNumber and UKPRN.";

        public bool IsValid { get; private set; }

        public bool RejectFile => true;

        public Task Execute(string fileName, string contents)
        {
            return Task.CompletedTask;
        }
    }
}
