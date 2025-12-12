using Microsoft.Extensions.Logging;
using Modding_Assistant.Core.Utilities;
using Modding_Assistant.MVVM.Model;
using Modding_Assistant.MVVM.Services.Interfaces;
using System.IO;

namespace Modding_Assistant.MVVM.Services.Implementations
{
    /// <summary>
    /// Class for importing and exporting mod files.
    /// </summary>
    public class ModFilesService(IModManagerService modManagerService, ISettingsService settingsService,
        INotificationService notificationService, IExcelExportService excelExportService,
        ILocalizationService localizationService,
        ILogger<ModFilesService> logger) : IModFilesService
    {
        private readonly IModManagerService _modManagerService = modManagerService;
        private readonly ISettingsService _settingsService = settingsService;
        private readonly INotificationService _notificationService = notificationService;
        private readonly IExcelExportService _excelExportService = excelExportService;
        private readonly ILocalizationService _localizationService = localizationService;
        private readonly ILogger<ModFilesService> _logger = logger;

        /// <summary>
        /// Copy file with progress reporting
        /// </summary>
        private async Task CopyFileAsync(string sourcePath, string destinationPath, IProgress<double> progress, 
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Copying file from {SourcePath} to {DestinationPath}", sourcePath, destinationPath);
            const int bufferSize = 1024 * 1024;
            using var sourceStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read, 
                bufferSize, useAsync: true);
            using var destinationStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None, 
                bufferSize, useAsync: true);

            var totalBytes = sourceStream.Length;
            var totalRead = 0L;
            var buffer = new byte[bufferSize];
            int bytesRead;

            while ((bytesRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await destinationStream.WriteAsync(buffer, 0, bytesRead, cancellationToken);
                totalRead += bytesRead;
                progress?.Report((double)totalRead / totalBytes * 100);
            }

            await destinationStream.FlushAsync(cancellationToken);
        }
        /// <inheritdoc/>
        public async Task ImportModAsync(string filePath, IProgress<double> progress, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Import mod {ModFilePath}", filePath);

                var fileName = Path.GetFileName(filePath);
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
                var modsFolder = _settingsService.ModsFolder;

                _logger.LogInformation("Moving file to specialized folder...");
                
                if (!string.IsNullOrWhiteSpace(modsFolder) && Directory.Exists(modsFolder))
                {
                    var destinationPath = Path.Combine(modsFolder, fileName);
                    await CopyFileAsync(filePath, destinationPath, progress, cancellationToken);

                    var sourceInfo = new FileInfo(filePath);
                    var destInfo = new FileInfo(destinationPath);

                    if (destInfo.Length != sourceInfo.Length)
                    {
                        throw new IOException($"File copy incomplete. Source: {sourceInfo.Length} bytes, " +
                                              $"Destination: {destInfo.Length} bytes");
                    }

                    File.Delete(filePath);
                }

                var newMod = new ModModel
                {
                    Name = ModNameHelper.GetFriendlyModName(fileNameWithoutExtension),
                    ModRawName = fileName,
                    LastUpdated = DateOnly.FromDateTime(DateTime.Today)
                };

                await _modManagerService.AddModAsync(newMod);

                _logger.LogInformation("Mod imported and moved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to import mod from {FilePath}", filePath);
                _notificationService.ShowError(_localizationService["ImportErrorHeader"],
                    $"{_localizationService["ImportErrorMessage"]}: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task ExportModsAsync(IEnumerable<ModModel> mods, string filePath, 
            string sheetName = "Mods", CancellationToken cancellationToken = default)
        {
            try
            {
                if (mods == null || !mods.Any())
                {
                    _logger.LogWarning("No mods to export");

                    _notificationService.ShowWarning(_localizationService["ExportWarningHeader"], 
                        _localizationService["ExportWarningMessage"]);

                    return;
                }
                var success = await _excelExportService.ExportToExcelAsync(mods, 
                    filePath, sheetName, cancellationToken);

                if (success)
                {

                    _logger.LogInformation("Mods exported successfully to {FilePath}", filePath);

                    _notificationService.ShowInformation(_localizationService["SuccessHeader"], 
                        _localizationService["ExportSuccessMessage"]);
                }
                else
                {
                    _logger.LogWarning("Export to {FilePath} did not complete successfully", filePath);

                    _notificationService.ShowWarning(_localizationService["ErrorHeader"],
                        _localizationService["ExportErrorMessage"]);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to export mods to {FilePath}", filePath);

                _notificationService.ShowError(_localizationService["ErrorHeader"],
                    $"{_localizationService["ExportErrorMessage"]}: {ex.Message}");
            }
        }
    }
}
