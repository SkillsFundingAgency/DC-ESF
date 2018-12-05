using ESFA.DC.ESF.Database.EF;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.DataAccessLayer
{
    public interface ISupplementaryDataModelMapper
    {
        SupplementaryDataModel GetModelFromEntity(SupplementaryData entity);

        SupplementaryDataModel GetSupplementaryDataModelFromLooseModel(SupplementaryDataLooseModel looseModel);
    }
}