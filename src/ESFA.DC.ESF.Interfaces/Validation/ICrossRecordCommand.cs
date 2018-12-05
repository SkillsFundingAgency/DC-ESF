using System.Collections.Generic;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Validation
{
    public interface ICrossRecordCommand : IValidatorCommand
    {
        IList<SupplementaryDataModel> AllRecords { get; set; }
    }
}