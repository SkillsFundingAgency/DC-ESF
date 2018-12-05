using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.Utils;

namespace ESFA.DC.ESF.ValidationService.Commands.FileLevel
{
    public class ConRefNumberRule01 : IFileLevelValidator
    {
        public string ErrorName => "ConRefNumber_01";

        public bool IsWarning => false;

        public string ErrorMessage => "There is a discrepency between the filename ConRefNumber and ConRefNumbers within the file.";

        public bool RejectFile => true;

        public bool Execute(SourceFileModel sourceFileModel, SupplementaryDataLooseModel model)
        {
            string[] filenameParts = FileNameHelper.SplitFileName(sourceFileModel.FileName);

            return filenameParts[2] == model.ConRefNumber;
        }
    }
}
