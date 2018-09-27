using ESFA.DC.ESF.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Database.EF;

namespace ESFA.DC.ILR1819.DataStore.PersistData
{
    public class StoreESF
    {
        private List<SupplementaryDataModel> _SupplementaryDataModel;
        private List<SupplementaryDataUnitCost> _supplementaryUnitCosts;

        public async Task StoreAsync(SqlConnection connection, SqlTransaction transaction, IEnumerable<SupplementaryDataModel> models, CancellationToken cancellationToken)
        {
            _SupplementaryDataModel = new List<SupplementaryDataModel>();
            foreach (var model in models)
            {
                _SupplementaryDataModel.Add(new SupplementaryDataModel
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
                    SourceFileId = model.SourceFileId ?? 0
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
                await bulkInsert.Insert("dbo.SupplementaryDataModel", _SupplementaryDataModel);
            }
        }
    }
}
