using ESFA.DC.ESF.Database.EF;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.DataAccessLayer.Mappers
{
    public class SourceFileModelMapper : ISourceFileModelMapper
    {
        public SourceFileModel GetModelFromEntity(SourceFile entity)
        {
            return new SourceFileModel
            {
                SourceFileId = entity.SourceFileId,
                ConRefNumber = entity.ConRefNumber,
                UKPRN = entity.UKPRN,
                FileName = entity.FileName,
                PreparationDate = entity.FilePreparationDate,
                SuppliedDate = entity.DateTime
            };
        }
    }
}