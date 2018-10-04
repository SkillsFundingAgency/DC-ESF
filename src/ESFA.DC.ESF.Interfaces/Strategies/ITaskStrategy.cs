using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Strategies
{
    public interface ITaskStrategy
    {
        int Order { get; }

        bool IsMatch(string taskName);
        Task Execute(SourceFileModel sourceFile,
            IList<SupplementaryDataModel> esfRecords,
            IList<ValidationErrorModel> errors,
            CancellationToken cancellationToken);
    }
}