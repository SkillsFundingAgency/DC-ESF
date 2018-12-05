using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Validation
{
    public interface IBusinessRuleValidator : IBaseValidator
    {
        bool Execute(SupplementaryDataModel model);
    }
}