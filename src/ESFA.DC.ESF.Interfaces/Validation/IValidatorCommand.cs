using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Validation
{
    public interface IValidatorCommand
    {
        void Execute(ESFModel model);
    }
}