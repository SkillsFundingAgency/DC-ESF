using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Aspose.Cells;
using CsvHelper.Configuration;
using ESFA.DC.ESF.Interfaces;
using ESFA.DC.ESF.Models.Generation;

namespace ESFA.DC.ESF.ReportingService.Helpers
{
    public class ExcelHelper
    {
        // todo completely rework to use different models that extend it to allow styling
        protected void BuildXlsReport<TMapper, TModel>(MemoryStream writer, TMapper classMap, IEnumerable<TModel> records)
            where TMapper : ClassMap, IClassMapper
            where TModel : class
        {
            ModelProperty[] names = classMap.MemberMaps.OrderBy(x => x.Data.Index).Select(x => new ModelProperty(x.Data.Names[0], (PropertyInfo)x.Data.Member)).ToArray();

            Workbook wb = new Workbook();
            Worksheet sheet = wb.Worksheets[0];

            sheet.Cells.ImportObjectArray(names.Select(x => x.Name).ToArray(), 0, 0, false);

            int row = 1;
            object[] values = new object[names.Length];
            foreach (TModel record in records)
            {
                for (int i = 0; i < names.Length; i++)
                {
                    values[i] = names[i].MethodInfo.GetValue(record);
                }

                sheet.Cells.ImportObjectArray(values, row++, 0, false);
            }

            wb.Save(writer, SaveFormat.Xlsx);
        }
    }
}
