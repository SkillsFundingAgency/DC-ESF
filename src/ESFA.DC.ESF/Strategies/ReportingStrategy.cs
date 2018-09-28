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
            throw new NotImplementedException();
        }

        public Task Execute(IList<SupplementaryDataModel> esfRecords, IDictionary<string, ValidationErrorModel> errors, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
