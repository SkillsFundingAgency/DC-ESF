using System;

namespace ESFA.DC.ESF.ValidationService.Helpers
{
    public class MonthYearHelper
    {
        public static DateTime GetCalendarDateTime(int? calendarYear, int? calendarMonth)
        {
            if (calendarYear == null || calendarMonth == null)
            {
                return DateTime.MinValue;
            }

            return new DateTime(calendarYear.Value, calendarMonth.Value, 1);
        }
    }
}
