using ESFA.DC.ESF.Database.EF.Interfaces;

namespace ESFA.DC.ESF.Database.EF
{
    public partial class ESF_DataStoreEntities : IESF_DataStoreEntities
    {
        public ESF_DataStoreEntities(string connectionString)
            : base(connectionString)
        {
            
        }
    }
}