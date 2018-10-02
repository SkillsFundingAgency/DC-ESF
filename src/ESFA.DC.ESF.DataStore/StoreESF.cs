using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Database.EF;
using ESFA.DC.ESF.Interfaces.DataStore;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.DataStore
{
    public class StoreESF : IStoreESF
    {
        private List<SupplementaryData> _supplementaryData;
        private List<SupplementaryDataUnitCost> _supplementaryUnitCosts;

        public async Task StoreAsync(
            SqlConnection connection,
            SqlTransaction transaction,
            int fileId,
            IEnumerable<SupplementaryDataModel> models,
            CancellationToken cancellationToken)
        {
            _supplementaryData = new List<SupplementaryData>();
            _supplementaryUnitCosts = new List<SupplementaryDataUnitCost>();

            foreach (var model in models)
            {
                _supplementaryData.Add(new SupplementaryData
                {
                    ConRefNumber = model.ConRefNumber,
                    DeliverableCode = model.DeliverableCode,
                    CalendarYear = model.CalendarYear ?? 0,
                    CalendarMonth = model.CalendarMonth ?? 0,
                    CostType = model.CostType,
                    StaffName = model.StaffName,
                    ReferenceType = model.ReferenceType,
                    Reference = model.Reference,
                    ULN = model.ULN,
                    ProviderSpecifiedReference = model.ProviderSpecifiedReference,
                    Value = model.Value,
                    HourlyRate = model.HourlyRate,
                    TotalHoursWorked = model.TotalHoursWorked,
                    ProjectHours = model.ProjectHours,
                    OrgHours = model.OrgHours,
                    SourceFileId = fileId
                });
            }

            await SaveData(connection, transaction, cancellationToken);
        }

        private async Task SaveData(SqlConnection connection, SqlTransaction transaction, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            using (var bulkInsert = new BulkInsert(connection, transaction, cancellationToken))
            {
                await bulkInsert.Insert("dbo.SupplementaryDataModel", _supplementaryData);
            }
        }
    }
}
