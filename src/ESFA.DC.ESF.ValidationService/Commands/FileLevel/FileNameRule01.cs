using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;

namespace ESFA.DC.ESF.ValidationService.Commands.FileLevel
{
    public class FileNameRule01 : IFileLevelValidator
    {
        public string ErrorMessage => "The UKPRN in the filename does not match the UKPRN in the Hub";

        public bool IsValid { get; private set; }

        public bool RejectFile => true;

        public Task Execute(string fileName, string contents)
        {
            return Task.CompletedTask;
        }
    }
}
