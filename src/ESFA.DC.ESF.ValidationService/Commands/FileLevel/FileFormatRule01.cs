using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.FileLevel
{
    public class FileFormatRule01 : IFileLevelValidator
    {
        public string ErrorMessage => "The file is not in the format described.";
        public bool IsValid { get; private set; }

        public bool RejectFile => true;

        public Task Execute(string fileName, SupplementaryDataModel model)
        {
            return Task.CompletedTask;
        }
    }
}
