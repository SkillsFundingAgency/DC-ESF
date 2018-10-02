using System.Collections.Generic;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Validation
{
    public interface ICrossRecordValidator : IBaseValidator
    {
        IList<SupplementaryDataModel> AllRecords { get; set; }
    }
}