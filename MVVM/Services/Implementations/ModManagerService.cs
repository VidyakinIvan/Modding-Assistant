using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modding_Assistant.Core.Data.Models;
using Modding_Assistant.MVVM.Model;
using Modding_Assistant.MVVM.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Modding_Assistant.MVVM.Services.Implementations
{
    /// Class for managing a collection of mods, including loading, adding, deleting,  reordering, and
    /// saving changes to the collection.
    /// </summary>
    /// <remarks>
    /// This class is designed to handle operations on a mod collection, represented by the
    /// <see cref="ObservableCollection{T}"/> of <see cref="ModModel"/>.
    /// </remarks>
    public class ModManagerService : IModManagerService
    {
        private readonly ModContext _db;
        private readonly ILogger<ModManagerService> _logger;
        private readonly INotificationService _notificationService;
        private readonly ILocalizationService _localizationService;

        private bool _isInitialized = false;

        public ObservableCollection<ModModel> Mods { get; } = [];

        public ModManagerService(ModContext db, 
            ILogger<ModManagerService> logger,
            INotificationService notificationService,
            ILocalizationService localizationService)
        {
            _db = db;
            _logger = logger;
            _notificationService = notificationService;
            _localizationService = localizationService;

            Mods.CollectionChanged += OnModsCollectionChanged;
        }

        /// <inheritdoc/>
        public async Task LoadModsAsync()
        {
            if (_isInitialized) 
                return;

            try
            {
                _logger.LogInformation("Loading mods from database...");

                await _db.Mods.LoadAsync();

                Mods.Clear();

                foreach (var mod in _db.Mods.Local.OrderBy(m => m.Order))
                {
                    Mods.Add(mod);
                }

                _isInitialized = true;

                _logger.LogInformation("Successfully loaded {Count} mods", Mods.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load mods from database");

                _notificationService.ShowError(_localizationService["DatabaseErrorHeader"],
                    $"{_localizationService["Failed to load mods"]}: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<ModModel> AddModAsync(ModModel mod)
        {
            ArgumentNullException.ThrowIfNull(mod);

            try
            {
                _logger.LogInformation("Adding new mod: {ModName}", mod.Name);

                mod.Order = Mods.Count + 1;

                _db.Mods.Add(mod);
                Mods.Add(mod);

                await SaveChangesAsync();

                _logger.LogInformation("Successfully added mod with ID: {ModId}", mod.Id);

                return mod;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add mod: {ModName}", mod.Name);

                _notificationService.ShowError(
                    _localizationService["AddModErrorHeader"],
                    $"{_localizationService["AddModErrorMessage"]} '{mod.Name}': {ex.Message}");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task DeleteModsAsync(IEnumerable<ModModel> mods)
        {
            if (mods == null)
            {
                _logger.LogWarning("No mods provided to delete.");
                return;
            }

            var modList = mods.ToList();
            if (modList.Count == 0) return;

            try
            {
                _logger.LogInformation("Deleting {Count} mods", modList.Count);

                foreach (var mod in modList)
                {
                    _db.Mods.Remove(mod);
                    Mods.Remove(mod);
                }

                await ReorderModsAsync();
                await SaveChangesAsync();

                _logger.LogInformation("Successfully deleted {Count} mods", modList.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete {Count} mods", modList.Count);

                _notificationService.ShowError(_localizationService["DeleteModErrorHeader"],
                    $"{_localizationService["DeleteModErrorMessage"]}: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task MoveModsAsync(IEnumerable<ModModel> mods, int targetPosition)
        {
            if (mods == null)
            {
                _logger.LogWarning("No mods provided to move.");
                return;
            }

            var modList = mods.ToList();
            if (modList.Count == 0) return;

            targetPosition = Math.Clamp(targetPosition, 1, Mods.Count - modList.Count + 1);

            try
            {
                _logger.LogInformation("Moving {Count} mods to position {Position}", modList.Count, targetPosition);

                foreach (var mod in modList)
                {
                    Mods.Remove(mod);
                }

                int insertIndex = targetPosition - 1;
                foreach (var mod in modList)
                {
                    Mods.Insert(insertIndex++, mod);
                }

                await ReorderModsAsync();
                await SaveChangesAsync();

                _logger.LogInformation("Successfully moved {Count} mods to position {Position}", modList.Count, 
                    targetPosition);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to move {Count} mods to position {Position}", modList.Count, targetPosition);

                _notificationService.ShowError(_localizationService["MoveModErrorHeader"], 
                    $"{_localizationService["MoveModErrorMessage"]}: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task ReorderModsAsync()
        {
            try
            {
                for (int i = 0; i < Mods.Count; i++)
                {
                    Mods[i].Order = i + 1;
                }

                await SaveChangesAsync();

                _logger.LogDebug("Successfully reordered {Count} mods", Mods.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to reorder mods");
            }
        }

        /// <inheritdoc/>
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var changes = await _db.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Saved {Count} changes to database", changes);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error while saving changes");

                _notificationService.ShowError(_localizationService["SaveChangesErrorHeader"],
                    $"{_localizationService["SaveChangesDatabaseErrorMessage"]}: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while saving changes");

                _notificationService.ShowError(_localizationService["SaveChangesErrorHeader"],
                    $"{_localizationService["SaveChangesUnexpectedErrorMessage"]}: {ex.Message}");
            }
        }

        private void OnModsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    _ = HandleAddedModsAsync(e.NewItems);
                    _ = ReorderModsAsync();
                    break;

                case NotifyCollectionChangedAction.Remove:
                    _ = HandleRemovedModsAsync(e.OldItems);
                    _ = ReorderModsAsync();
                    break;

                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Reset:
                    _ = ReorderModsAsync();
                    break;
            }
        }

        private async Task HandleAddedModsAsync(System.Collections.IList? newItems)
        {
            if (newItems == null || !_isInitialized) 
                return;

            try
            {
                var addedMods = newItems.Cast<ModModel>().ToList();

                foreach (var mod in addedMods)
                {
                    if (_db.Entry(mod).State == EntityState.Detached)
                    {
                        _db.Mods.Add(mod);
                    }
                }

                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to auto-save added mods");
            }
        }

        private async Task HandleRemovedModsAsync(System.Collections.IList? oldItems)
        {
            if (oldItems == null || !_isInitialized) 
                return;

            try
            {
                var removedMods = oldItems.Cast<ModModel>().ToList();

                foreach (var mod in removedMods)
                {
                    if (_db.Entry(mod).State != EntityState.Deleted)
                    {
                        _db.Mods.Remove(mod);
                    }
                }

                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to auto-save removed mods");
            }
        }
    }
}
