using System.Collections.Generic;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Validation
{
    public interface ICrossRecordValidator : IBaseValidator
    {
        bool Execute(IList<SupplementaryDataModel> allRecords, SupplementaryDataModel model);
    }
}