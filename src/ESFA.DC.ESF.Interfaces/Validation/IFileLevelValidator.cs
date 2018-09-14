namespace ESFA.DC.ESF.Interfaces.Validation
{
    public interface IFileLevelValidator
    {
        string ErrorMessage { get; }

        bool IsValid { get; }

        bool RejectFile { get; }

        void Execute(string fileName, string contents);
    }
}