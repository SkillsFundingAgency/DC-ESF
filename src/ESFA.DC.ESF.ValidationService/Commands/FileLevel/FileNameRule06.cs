using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;

namespace ESFA.DC.ESF.ValidationService.Commands.FileLevel
{
    public class FileNameRule06 : IFileLevelValidator
    {
        public string ErrorMessage => "For a given UKPRN and ConRefNumber, this combination does not exist on the FCS contract lookup.";
        public bool IsValid { get; private set; }

        public bool RejectFile => true;

        public Task Execute(string fileName, string contents)
        {
            return Task.CompletedTask;
        }
    }
}
