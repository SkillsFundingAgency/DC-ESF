using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.FileLevel
{
    public class FileNameRule08 : IFileLevelValidator
    {
        public string ErrorMessage => "The date/time of the file is not greater than a previous transmission with the same ConRefNumber and UKPRN.";

        public bool IsValid { get; private set; }

        public bool RejectFile => true;

        public string ErrorName => "Filename_08";

        public bool IsWarning => false;

        public Task Execute(string fileName, SupplementaryDataModel model)
        {
            return Task.CompletedTask;
        }
    }
}
