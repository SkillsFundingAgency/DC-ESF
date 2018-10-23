using System;

namespace ESFA.DC.ESF.Utils
{
    public class FileNameHelper
    {
        public static int GetFundingYearFromFileName(
            string fileName)
        {
            if (fileName == null)
            {
                return 0;
            }

            var fileNameParts = SplitFileName(fileName);
            if (fileNameParts.Length < 4)
            {
                return 0;
            }

            var constructedYear = $"{fileNameParts[3].Substring(0, 4)}";

            int.TryParse(constructedYear, out var year);
            return year;
        }

        public static int GetFundingYearFromILRFileName(
            string fileName)
        {
            if (fileName == null)
            {
                return 0;
            }

            var fileNameParts = fileName.Split('-');
            if (fileNameParts.Length < 6)
            {
                return 0;
            }

            var constructedYear = $"{fileNameParts[3].Substring(0, 4)}";

            int.TryParse(constructedYear, out var year);
            return year;
        }

        public static string GetSecondYearFromReportYear(int year)
        {
            return year.ToString().Length > 3 ?
                (Convert.ToInt32(year.ToString().Substring(year.ToString().Length - 2)) + 1).ToString() :
                string.Empty;
        }

        public static string GetPreparedDateFromFileName(
            string fileName)
        {
            if (fileName == null)
            {
                return null;
            }

            var fileNameParts = SplitFileName(fileName);
            return fileNameParts.Length < 5 || fileNameParts[3].Length < 8 || fileNameParts[4].Length < 6
                ? string.Empty
                : $"{fileNameParts[3].Substring(0, 4)}/{fileNameParts[3].Substring(4, 2)}/{fileNameParts[3].Substring(6, 2)} " +
                  $"{fileNameParts[4].Substring(0, 2)}:{fileNameParts[4].Substring(2, 2)}:{fileNameParts[4].Substring(4, 2)}";
        }


        public static string[] SplitFileName(string fileName)
        {
            const int lengthOfDateTimePart = 15;

            fileName = fileName.Replace(".csv", string.Empty);

            var parts = new string[5];
            parts[0] = fileName.Substring(0, fileName.IndexOf('-'));
            parts[1] = fileName.Substring(parts[0].Length + 1, fileName.IndexOf('-', parts[0].Length));

            var startOfConRefNumIndex = parts[0].Length + parts[1].Length + 2;
            var lengthOfConRefNum = (fileName.Length - lengthOfDateTimePart -1) - startOfConRefNumIndex;
            parts[2] = fileName.Substring(startOfConRefNumIndex, lengthOfConRefNum);

            var dateTimePart = fileName.Substring(fileName.Length - lengthOfDateTimePart);
            parts[3] = dateTimePart.Substring(0, dateTimePart.IndexOf('-'));
            parts[4] = dateTimePart.Substring(parts[3].Length + 1);

            return parts;
        }
    }
}
