using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.CrossRecord
{
    public class Duplicate01 : ICrossRecordValidator
    {
        public string ErrorName => "Duplicate_01";

        public bool IsWarning => false;

        public string ErrorMessage => "This record is a duplicate.";

        public bool IsValid { get; private set; }
        public IList<SupplementaryDataModel> AllRecords { get; set; }

        public Task Execute(SupplementaryDataModel model)
        {
            IsValid = AllRecords != null && AllRecords.Count(
                          m => m.ConRefNumber == model.ConRefNumber &&
                               m.DeliverableCode == model.DeliverableCode &&
                               m.CalendarYear == model.CalendarYear &&
                               m.CalendarMonth == model.CalendarMonth &&
                               m.CostType == model.CostType &&
                               m.ReferenceType == model.ReferenceType &&
                               m.Reference == model.Reference) == 1;

            return Task.CompletedTask;
        }
    }
}
