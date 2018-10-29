using System.Data.Entity;

namespace ESFA.DC.ESF.Database.EF.Interfaces
{
    public interface IESF_DataStoreEntities
    {
        DbSet<SourceFile> SourceFiles { get; set; }

        DbSet<SupplementaryData> SupplementaryDatas { get; set; }

        DbSet<SupplementaryDataUnitCost> SupplementaryDataUnitCosts { get; set; }

        DbSet<ValidationError> ValidationErrors { get; set; }

        DbSet<VersionInfo> VersionInfoes { get; set; }
    }
}