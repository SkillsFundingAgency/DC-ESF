using System;
using System.Collections.Generic;
using ESFA.DC.ESF.Interfaces.Services;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;

namespace ESFA.DC.ESF.ReportingService.Services
{
    public sealed class ValueProvider : IValueProvider
    {
        public void GetFormattedValue(List<object> values, object value)
        {
            if (value == null)
            {
                values.Add(string.Empty);
                return;
            }

            if (value is decimal d1)
            {
                values.Add(decimal.Round(d1, 2));
                return;
            }

            if (IsOfNullableType<decimal>(value))
            {
                decimal? d = (decimal?)value;
                values.Add(decimal.Round(d.Value, 2));
                return;
            }

            if (value is List<decimal> listOfDecimals)
            {
                foreach (decimal dec in listOfDecimals)
                {
                    values.Add(decimal.Round(dec, 2));
                }

                return;
            }

            if (value is string[] arrayOfStrings)
            {
                foreach (string str in arrayOfStrings)
                {
                    values.Add(str);
                }

                return;
            }

            if (value is List<FundingSummaryReportYearlyValueModel> totals)
            {
                foreach (FundingSummaryReportYearlyValueModel fundingSummaryReportYearlyValueModel in totals)
                {
                    foreach (decimal dec in fundingSummaryReportYearlyValueModel.Values)
                    {
                        values.Add(decimal.Round(dec, 2));
                    }
                }

                return;
            }

            values.Add(value);
        }

        private bool IsOfNullableType<T>(object o)
        {
            return Nullable.GetUnderlyingType(o.GetType()) != null && o is T;
        }
    }
}
