using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Validation
{
    public interface IFileLevelValidator : IBaseValidator
    {
        bool RejectFile { get; }

        bool Execute(SourceFileModel sourceFileModel, SupplementaryDataModel model);
    }
}