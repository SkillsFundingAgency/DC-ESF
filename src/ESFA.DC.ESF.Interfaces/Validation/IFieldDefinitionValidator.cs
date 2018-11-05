using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Validation
{
    public interface IFieldDefinitionValidator : IBaseValidator
    {
        bool Execute(SupplementaryDataModel model);
    }
}