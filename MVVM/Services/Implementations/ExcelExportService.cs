using ClosedXML.Excel;
using Modding_Assistant.MVVM.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modding_Assistant.MVVM.Services.Implementations
{
    public class ExcelExportService : IExcelExportService
    {
        public async Task<bool> ExportToExcelAsync<T>(IEnumerable<T> data, string filePath, string sheetName = "Data")
        {
            return await Task.Run(() =>
            {
                try
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add(sheetName);
                        var properties = typeof(T).GetProperties();
                        for (int i = 0; i < properties.Length; i++)
                        {
                            worksheet.Cell(1, i + 1).Value = properties[i].Name;
                            worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                            worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                        }
                        int row = 2;
                        foreach (var item in data)
                        {
                            for (int col = 0; col < properties.Length; col++)
                            {
                                var value = properties[col].GetValue(item);
                                worksheet.Cell(row, col + 1).Value = value?.ToString() ?? string.Empty;
                            }
                            row++;
                        }
                        worksheet.Columns().AdjustToContents();
                        workbook.SaveAs(filePath);
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            });
        }
    }
}
