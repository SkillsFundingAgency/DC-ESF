using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Strategies;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Strategies
{
    public class ReportingStrategy : ITaskStrategy
    {
        public bool IsMatch(string taskName)
        {
            return taskName == string.Empty;
        }

        public Task Execute(
            SourceFileModel sourceFile, 
            IList<SupplementaryDataModel> esfRecords, 
            IList<ValidationErrorModel> errors, 
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
