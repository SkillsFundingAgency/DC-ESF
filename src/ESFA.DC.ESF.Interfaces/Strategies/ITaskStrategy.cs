using System.Collections.Generic;
using System.Threading.Tasks;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Strategies
{
    public interface ITaskStrategy
    {
        bool IsMatch(string taskName);
        Task Execute(IList<ESFModel> esfRecords);
    }
}