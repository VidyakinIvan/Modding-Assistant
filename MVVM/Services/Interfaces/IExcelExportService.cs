using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modding_Assistant.MVVM.Services.Interfaces
{
    public interface IExcelExportService
    {
        Task<bool> ExportToExcelAsync<T>(IEnumerable<T> data, string filePath, string sheetName = "Data");
    }
}
