using System.Threading.Tasks;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Validation
{
    public interface IFieldDefinitionValidator : IBaseValidator
    {
        Task Execute(SupplementaryDataModel model);
    }
}