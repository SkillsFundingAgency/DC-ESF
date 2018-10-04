using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.FileLevel
{
    public class ConRefNumberRule01 : IFileLevelValidator
    {
        public string ErrorName => "ConRefNumber_01";

        public bool IsWarning => false;

        public string ErrorMessage => "There is a discrepency between the filename ConRefNumber and ConRefNumbers within the file.";

        public bool IsValid { get; private set; }

        public bool RejectFile => true;

        public Task Execute(SourceFileModel sourceFileModel, SupplementaryDataModel model)
        {
            string[] filenameParts = sourceFileModel.FileName.Split('-');

            IsValid = filenameParts[2] == model.ConRefNumber;

            return Task.CompletedTask;
        }
    }
}
