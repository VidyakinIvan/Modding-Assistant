using System.Threading;

namespace Modding_Assistant.MVVM.Services.Interfaces
{
    /// <summary>
    /// Service for exporting data collections to Excel format
    /// </summary>
    public interface IExcelExportService
    {

        /// <summary>
        /// Exports a collection of items to Excel file
        /// </summary>
        /// <returns>True if export succeeded, throws exception otherwise</returns>
        Task<bool> ExportToExcelAsync<T>(IEnumerable<T> data, string filePath, 
            string sheetName = "Data", CancellationToken cancellationToken = default);
    }
}
