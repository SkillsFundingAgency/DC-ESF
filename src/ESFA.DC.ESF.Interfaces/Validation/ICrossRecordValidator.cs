using System.Collections.Generic;
using System.Threading.Tasks;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Validation
{
    public interface ICrossRecordValidator : IBaseValidator
    {
        Task Execute(IList<SupplementaryDataModel> allRecords, SupplementaryDataModel model);
    }
}