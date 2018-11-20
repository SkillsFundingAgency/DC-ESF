using System.Collections.Generic;

namespace ESFA.DC.ESF.Models
{
    public class SupplementaryDataWrapper
    {
        public SupplementaryDataWrapper()
        {
            SupplementaryDataLooseModels = new List<SupplementaryDataLooseModel>();
            SupplementaryDataModels = new List<SupplementaryDataModel>();
            ValidErrorModels = new List<ValidationErrorModel>();
        }

        public IList<SupplementaryDataLooseModel> SupplementaryDataLooseModels { get; set; }

        public IList<SupplementaryDataModel> SupplementaryDataModels { get; set; }

        public IList<ValidationErrorModel> ValidErrorModels { get; set; }
    }
}
