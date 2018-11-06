using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Database.EF;
using ESFA.DC.ESF.Interfaces.DataStore;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.DataStore
{
    public class StoreValidation : IStoreValidation
    {
        private List<ValidationError> _validationData;

        public async Task StoreAsync(
            SqlConnection connection,
            SqlTransaction transaction,
            int fileId,
            IEnumerable<ValidationErrorModel> models,
            CancellationToken cancellationToken)
        {
            _validationData = new List<ValidationError>();

            foreach (var model in models)
            {
                _validationData.Add(new ValidationError
                {
                    Severity = model.IsWarning ? "W" : "E",
                    RuleId = model.RuleName,
                    ErrorMessage = model.ErrorMessage,
                    CreatedOn = DateTime.Now,
                    ConRefNumber = model.ConRefNumber,
                    DeliverableCode = model.DeliverableCode,
                    CalendarYear = (model.CalendarYear ?? 0).ToString(),
                    CalendarMonth = (model.CalendarMonth ?? 0).ToString(),
                    CostType = model.CostType,
                    StaffName = model.StaffName,
                    ReferenceType = model.ReferenceType,
                    Reference = model.Reference,
                    ULN = model.ULN.ToString(),
                    ProviderSpecifiedReference = model.ProviderSpecifiedReference,
                    Value = model.Value.ToString(),
                    HourlyRate = model.HourlyRate.ToString(),
                    TotalHoursWorked = model.TotalHoursWorked.ToString(),
                    ProjectHours = model.ProjectHours.ToString(),
                    OrgHours = model.OrgHours.ToString(),
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
                await bulkInsert.Insert("dbo.ValidationError", _validationData);
            }
        }
    }
}
