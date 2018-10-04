using System.Threading.Tasks;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Validation
{
    public interface IFileLevelValidator : IBaseValidator
    {
        bool RejectFile { get; }

        Task Execute(SourceFileModel sourceFileModel, SupplementaryDataModel model);
    }
}