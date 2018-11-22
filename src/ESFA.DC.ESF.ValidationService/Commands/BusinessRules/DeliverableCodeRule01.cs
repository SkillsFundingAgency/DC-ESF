using System.Collections.Generic;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class DeliverableCodeRule01 : IBusinessRuleValidator
    {
        private readonly List<string> _validValues = new List<string>
        {
            "ST01", "AC01", "CG01", "CG02", "FS01", "SD01", "SD02", "SD03", "SD04", "SD05", "SD06", "SD07",
            "SD08", "SD09", "SD10", "NR01", "RQ01", "PG01", "PG02", "PG03", "PG04", "PG05", "PG06", "SU01",
            "SU02", "SU03", "SU04", "SU05", "SU11", "SU12", "SU13", "SU14", "SU15", "SU21", "SU22", "SU23", "SU24"
        };

        public string ErrorMessage => "The DeliverableCode is not valid.";

        public string ErrorName => "DeliverableCode_01";

        public bool IsWarning => false;

        public bool Execute(SupplementaryDataModel model)
        {
            return _validValues.Contains(model.DeliverableCode.Trim());
        }
    }
}
