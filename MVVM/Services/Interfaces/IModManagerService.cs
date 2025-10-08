using Modding_Assistant.MVVM.Model;
using System.Collections.ObjectModel;

namespace Modding_Assistant.MVVM.Services.Interfaces
{
    /// <summary>
    /// Service for managing a collection of mods, including loading, adding, deleting,  reordering, and
    /// saving changes to the collection.
    /// </summary>
    /// <remarks>
    /// This service is designed to handle operations on a mod collection, represented by the
    /// <see cref="ObservableCollection{T}"/> of <see cref="ModModel"/>.
    /// </remarks>
    public interface IModManagerService
    {
        /// <summary>
        /// Gets the collection of mods currently loaded in the application.
        /// </summary>
        ObservableCollection<ModModel> Mods { get; }

        /// <summary>
        /// Asynchronously loads and initializes all available mods.
        /// </summary>
        Task LoadModsAsync();

        /// <summary>
        /// Adds a new mod to the system asynchronously.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the added <see cref="ModModel"/>
        /// with any updates applied during the addition process.
        /// </returns>
        Task<ModModel> AddModAsync(ModModel mod);

        /// <summary>
        /// Deletes the specified collection of mods in the list asynchronously.
        /// </summary>
        Task DeleteModsAsync(IEnumerable<ModModel> mods);

        /// <summary>
        /// Moves the specified collection of mods to the target position in the list.
        /// </summary>
        Task MoveModsAsync(IEnumerable<ModModel> mods, int targetPosition);

        /// <summary>
        /// Asynchronously saves all changes made in the current context to the underlying database.
        /// </summary>
        Task SaveChangesAsync();

        /// <summary>
        /// Reorders the list of mods asynchronously based on the current configuration or criteria.
        /// </summary>
        Task ReorderModsAsync();
    }
}
