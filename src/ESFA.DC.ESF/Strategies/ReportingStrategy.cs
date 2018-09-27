using System;
using System.Collections.Generic;
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

        public Task Execute(IList<SupplementaryDataModel> esfRecords)
        {
            throw new NotImplementedException();
        }
    }
}
