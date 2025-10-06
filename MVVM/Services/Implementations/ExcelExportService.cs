using ClosedXML.Excel;
using Microsoft.Extensions.Logging;
using Modding_Assistant.MVVM.Services.Interfaces;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Modding_Assistant.MVVM.Services.Implementations
{
    /// <summary>
    /// Class for exporting data collections to Excel format
    /// </summary>
    public class ExcelExportService(ILogger<ExcelExportService> logger) : IExcelExportService
    {
        private readonly ILogger<ExcelExportService> _logger = logger;

        /// <summary>
        /// Thread-safe cache for property information to minimize reflection overhead
        /// </summary>
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> _propertyCache = new();

        ///<inheritdoc/>
        /// <exception cref="ArgumentNullException">Thrown when data is null</exception>
        /// <exception cref="ArgumentException">Thrown when filePath is null or whitespace</exception>
        /// <exception cref="OperationCanceledException">Thrown when operation is cancelled</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when write permission is denied</exception>
        public async Task<bool> ExportToExcelAsync<T>(IEnumerable<T> data, string filePath,  
            string sheetName = "Data", CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(data);
            ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

            if (!data.Any())
            {
                _logger.LogWarning("Attempt to export empty data collection");
                return false;
            }

            var dataList = data.ToList();

            return await Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    var properties = GetCachedProperties<T>();

                    using var workbook = new XLWorkbook();

                    var worksheet = workbook.Worksheets.Add(sheetName);

                    CreateHeader(worksheet, properties);
                    FillData(worksheet, dataList, properties, cancellationToken);

                    worksheet.Columns().AdjustToContents();
                    workbook.SaveAs(filePath);

                    _logger.LogInformation("Successfully exported {Count} items to {FilePath}", dataList.Count, filePath);

                    return true;
                }
                catch (OperationCanceledException)
                {
                    _logger.LogWarning("Excel export was cancelled");
                    throw;
                }
                catch (UnauthorizedAccessException ex)
                {
                    _logger.LogError(ex, "Permission denied while exporting to Excel: {FilePath}", filePath);
                    throw new UnauthorizedAccessException($"No permission to write to: {filePath}", ex);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to export data to Excel file: {FilePath}", filePath);
                    throw;
                }
            }, cancellationToken);
        }

        /// <summary>
        /// Gets cached property information for type T to avoid reflection overhead
        /// </summary>
        private static PropertyInfo[] GetCachedProperties<T>()
        {
            var type = typeof(T);
            return _propertyCache.GetOrAdd(type, t => t.GetProperties());
        }

        /// <summary>
        /// Creates the header row in the Excel worksheet using property names
        /// </summary>
        private static void CreateHeader(IXLWorksheet worksheet, PropertyInfo[] properties)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                var cell = worksheet.Cell(1, i + 1);
                cell.Value = properties[i].Name;
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.LightGray;
            }
        }

        /// <summary>
        /// Fills the worksheet with data from the collection
        /// </summary>
        /// <remarks>
        /// Starts filling data from row 2 (row 1 is reserved for headers)
        /// Checks for cancellation before processing each row
        /// </remarks>
        private static void FillData<T>(IXLWorksheet worksheet, IEnumerable<T> data, PropertyInfo[] properties,
            CancellationToken cancellationToken)
        {
            int row = 2;
            foreach (var item in data)
            {
                cancellationToken.ThrowIfCancellationRequested();
                for (int col = 0; col < properties.Length; col++)
                {
                    var property = properties[col];
                    var value = property.GetValue(item);

                    SetCellValue(worksheet.Cell(row, col + 1), value);
                }
                row++;
            }
        }

        /// <summary>
        /// Sets the value of an Excel cell with appropriate formatting based on data type
        /// </summary>
        private static void SetCellValue<T>(IXLCell cell, T? value)
        {
            switch (value)
            {
                case null:
                    cell.Value = string.Empty;
                    break;
                case DateTime dateTime:
                    cell.Value = dateTime;
                    break;
                case bool boolValue:
                    cell.Value = boolValue ? "Yes" : "No";
                    break;
                case int intValue:
                    cell.Value = intValue;
                    cell.Style.NumberFormat.Format = "0";
                    break;
                case double doubleValue:
                    cell.Value = doubleValue;
                    cell.Style.NumberFormat.Format = "0.00";
                    break;
                default:
                    cell.Value = value.ToString();
                    break;
            }
        }
    }
}
