using System.Collections.Generic;
using System.IO;
using System.Text;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ReportingService.Mappers;

namespace ESFA.DC.ESF.ReportingService.Reports
{
    public class ValidationErrorReport : AbstractReportBuilder
    {
        public ValidationErrorReport(IDateTimeProvider dateTimeProvider) 
            : base(dateTimeProvider)
        {

            ReportFileName = "ESF Supplementary Data Rule Violation Report";
        }

        private string GetCsv(IList<ValidationErrorModel> validationErrorModels)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BuildCsvReport<ValidationErrorMapper, ValidationErrorModel>(ms, validationErrorModels);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }
}
