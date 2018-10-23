using System.Collections.Generic;

namespace ESFA.DC.ESF.Models
{
    public class SupplementaryDataWrapper
    {
        public IList<SupplementaryDataModel> SupplementaryDataModels { get; set; }

        public IList<ValidationErrorModel> ValidErrorModels { get; set; }

        public SupplementaryDataWrapper()
        {
            SupplementaryDataModels = new List<SupplementaryDataModel>();
            ValidErrorModels = new List<ValidationErrorModel>();
        }
    }
}
