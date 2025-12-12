using Modding_Assistant.MVVM.Model;

namespace Modding_Assistant.MVVM.Services.Interfaces
{

    /// <summary>
    /// Service for importing and exporting mod files.
    /// </summary>
    public interface IModFilesService
    {

        /// <summary>
        /// Imports a mod from the specified file path asynchronously.
        /// </summary>
        Task ImportModAsync(string filePath, IProgress<double> progress, CancellationToken cancellationToken = default);

        /// <summary>
        /// Exports a collection of mods to a specified file in a tabular format.
        /// </summary>
        Task ExportModsAsync(IEnumerable<ModModel> mods, string filePath,
            string sheetName = "Mods", CancellationToken cancellationToken = default);
    }
}
