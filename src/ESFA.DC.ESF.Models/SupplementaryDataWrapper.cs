using System.Collections.Generic;

namespace ESFA.DC.ESF.Models
{
    public class SupplementaryDataWrapper
    {
        public SupplementaryDataWrapper()
        {
            SupplementaryDataModels = new List<SupplementaryDataModel>();
            ValidErrorModels = new List<ValidationErrorModel>();
        }

        public IList<SupplementaryDataModel> SupplementaryDataModels { get; set; }

        public IList<ValidationErrorModel> ValidErrorModels { get; set; }
    }
}
