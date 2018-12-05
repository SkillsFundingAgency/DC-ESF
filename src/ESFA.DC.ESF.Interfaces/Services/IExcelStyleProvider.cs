using Aspose.Cells;
using ESFA.DC.ESF.Models.Styling;

namespace ESFA.DC.ESF.Interfaces.Services
{
    public interface IExcelStyleProvider
    {
        CellStyle[] GetFundingSummaryStyles(Workbook workbook);

        CellStyle GetCellStyle(CellStyle[] cellStyles, int index);
    }
}
