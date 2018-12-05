namespace ESFA.DC.ESF.Interfaces.Validation
{
    public interface IBaseValidator
    {
        string ErrorName { get; }

        bool IsWarning { get; }

        string ErrorMessage { get; }
    }
}