using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Strategies;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Strategies
{
    public class PersistenceStrategy : ITaskStrategy
    {
        public bool IsMatch(string taskName)
        {
            throw new NotImplementedException();
        }

        public Task Execute(IList<ESFModel> esfRecords)
        {
            throw new NotImplementedException();
        }
    }
}
