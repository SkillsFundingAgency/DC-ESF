namespace ESFA.DC.ESF.Models
{
    public class ValidationErrorModel
    {
        public bool IsWarning { get; set; }

        public string RuleName { get; set; }

        public string ErrorMessage { get; set; }
    }
}