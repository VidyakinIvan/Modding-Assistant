using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Modding_Assistant.Core;
using Modding_Assistant.MVVM.Model;
using Modding_Assistant.MVVM.Services.Implementations;
using Modding_Assistant.MVVM.Services.Interfaces;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Modding_Assistant.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private readonly ModContext _db;
        private readonly IMainWindowService _mainWindowService;
        private readonly ISettingsService _settingsService;
        private readonly IExcelExportService _excelExportService;
        private readonly IOpenDialogService _openDialogService;
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private RelayCommand? _loadCommand;
        private RelayCommand? _fromFileCommand;
        private RelayCommand? _moveBeforeCommand;
        private RelayCommand? _deleteCommand;
        private RelayCommand? _minimizeCommand;
        private RelayCommand? _maximizeCommand;
        private RelayCommand? _moveWindowCommand;
        private RelayCommand? _settingsCommand;
        private RelayCommand? _exportCommand;
        private RelayCommand? _languageCommand;
        private RelayCommand? _exitCommand;
        public MainViewModel(ModContext db, IMainWindowService mainWindowService, ISettingsService settingsService, 
            IExcelExportService excelExportService, IOpenDialogService openDialogService, 
            ILocalizationService localizationService, INotificationService notificationService)
        {
            _db = db;
            _mainWindowService = mainWindowService;
            _settingsService = settingsService;
            _excelExportService = excelExportService;
            _openDialogService = openDialogService;
            _localizationService = localizationService;
            _notificationService = notificationService;
            _db.Mods.Load();
            ModList = db.Mods.Local.ToObservableCollection();
            ModList.CollectionChanged += ModList_CollectionChanged;
            var sorted = ModList.OrderBy(m => m.Order).ToList();
            for (int i = 0; i < sorted.Count; i++)
            {
                if (!Equals(ModList[i], sorted[i]))
                {
                    int sortedIndex = ModList.IndexOf(sorted[i]);
                    if (sortedIndex >= 0)
                        ModList.Move(sortedIndex, i);
                }
            }
            _db.SaveChanges();
        }
        private void ModList_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (ModModel newMod in e.NewItems!)
                {
                    newMod.Order = ModList.IndexOf(newMod) + 1;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                for (int i = 0; i < ModList.Count; i++)
                {
                    ModList[i].Order = i + 1;
                }
            }
        }
        private WindowState _windowState;
        public WindowState WindowState
        {
            get => _windowState;
            set => SetProperty(ref _windowState, value);
        }
        public string CurrentLanguage => _localizationService.CurrentCulture.TwoLetterISOLanguageName;
        public ObservableCollection<ModModel> ModList { get; set; }
        public RelayCommand LoadCommand
        {
            get
            {
                return _loadCommand ??= new RelayCommand(_ =>
                {
                    _mainWindowService.Left = !double.IsNaN(_settingsService.MainWindowLeft) ? _settingsService.MainWindowLeft : (SystemParameters.WorkArea.Width - _mainWindowService.Width) / 4;
                    _mainWindowService.Top = !double.IsNaN(_settingsService.MainWindowTop) ? _settingsService.MainWindowTop : (SystemParameters.WorkArea.Height - _mainWindowService.Height) / 2;
                    if (_settingsService.MainWindowFullScreen)
                        MaximizeCommand.Execute(null);
                });
            }
        }

        public RelayCommand FromFileCommand
        {
            get
            {
                return _fromFileCommand ??= new RelayCommand(async _ =>
                {
                    var fileName = await _openDialogService.ShowOpenFileDialogAsync(
                        "Select Mod Archive",
                        "Archive Files (*.zip;*.rar;*.7z)|*.zip;*.rar;*.7z|All Files (*.*)|*.*");
                    if (string.IsNullOrEmpty(fileName))
                        return;

                    string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fileName);

                    var newMod = new ModModel { Name = ModNameHelper.GetFriendlyModName(fileNameWithoutExtension), 
                       ModRawName = System.IO.Path.GetFileName(fileName), 
                        LastUpdated = DateOnly.FromDateTime(DateTime.Today) };

                    ModList.Add(newMod);
                    _db.SaveChanges();
                    string modsFolder = _settingsService.ModsFolder;
                    if (!string.IsNullOrWhiteSpace(_settingsService.ModsFolder) && Directory.Exists(modsFolder))
                    {
                        string sourceFilePath = fileName;
                        if (Directory.Exists(modsFolder))
                        {
                            string destFilePath = Path.Combine(modsFolder, Path.GetFileName(sourceFilePath));
                            try
                            {
                                File.Move(sourceFilePath, destFilePath, overwrite: true);
                            }
                            catch (Exception ex)
                            {
                                _notificationService.ShowError("Error", $"Failed to move file:\n{ex.Message}");
                            }
                        }
                    }
                });
            }
        }
        public RelayCommand MoveBeforeCommand
        {
            get
            {
                return _moveBeforeCommand ??= new RelayCommand(selectedMods =>
                {
                    if (selectedMods is IList mods && mods.Count > 0)
                    {
                        int? result = _openDialogService.ShowMoveModsDialog();
                        if (result.HasValue)
                        {
                            foreach (var mod in mods)
                            {
                                if (mod is ModModel m && ModList.Contains(m))
                                {
                                    int oldIndex = ModList.IndexOf(m);
                                    int newIndex = Math.Min(result.Value - 1, ModList.Count);
                                    if (oldIndex < newIndex)
                                        newIndex--;
                                    if (oldIndex != newIndex)
                                    {
                                        ModList.Move(oldIndex, newIndex);
                                    }
                                }
                            }
                            var viewSource = CollectionViewSource.GetDefaultView(ModList);
                            viewSource.SortDescriptions.Clear();
                            viewSource.SortDescriptions.Add(new(nameof(ModModel.Order), ListSortDirection.Ascending));
                            viewSource.Refresh();
                            for (int i = 0; i < ModList.Count; i++)
                            {
                                ModList[i].Order = i + 1;
                            }
                            CollectionViewSource.GetDefaultView(ModList)?.Refresh();
                            _db.SaveChanges();
                        }
                    }
                });
            }
        }
        public RelayCommand DeleteCommand
        {
            get
            {
                return _deleteCommand ??= new RelayCommand(selectedMods =>
                {
                    if (selectedMods is IList mods)
                    {
                        foreach (var mod in mods.OfType<ModModel>().ToList())
                            _db.Mods.Remove(mod);
                        _db.SaveChanges();
                        CollectionViewSource.GetDefaultView(ModList)?.Refresh();
                    }
                });
            }
        }
        public RelayCommand MinimizeCommand
        {
            get
            {
                return _minimizeCommand ??= new RelayCommand(_ =>
                {
                    _mainWindowService.Minimize();
                });
            }
        }
        public RelayCommand MaximizeCommand
        {
            get
            {
                return _maximizeCommand ??= new RelayCommand(_ =>
                {
                    if (_mainWindowService.WindowState == WindowState.Maximized)
                    {   
                        _mainWindowService.Restore();
                        WindowState = WindowState.Normal;
                    }
                    else
                    {
                        _mainWindowService.Maximize();
                        WindowState = WindowState.Maximized;
                    }
                });
            }
        }
        public RelayCommand MoveWindowCommand
        {
            get
            {
                return _moveWindowCommand ??= new RelayCommand(_ =>
                {
                    _mainWindowService.DragMove();
                });
            }
        }
        public RelayCommand SettingsCommand
        {
            get
            {
                return _settingsCommand ??= new RelayCommand(async _ =>
                {
                    var folderName = await _openDialogService.ShowPickFolderDialogAsync("Select Mods Folder");
                    if (!string.IsNullOrEmpty(folderName))
                    {
                        _settingsService.ModsFolder = folderName;
                    }
                });
            }
        }

        public RelayCommand ExportCommand
        {
            get
            {
                return _exportCommand ??= new RelayCommand(async _ =>
                {
                    var fileName = await _openDialogService.ShowSaveFileDialogAsync(
                        $"Export to Excel", "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*");
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        try
                        {
                            var success = await _excelExportService.ExportToExcelAsync(
                                ModList,
                                fileName,
                                "Mods"
                            );
                            if (success)
                            {
                                await _openDialogService.ShowMessageAsync("Success", "Export successful!");
                            }
                            else
                            {
                                await _openDialogService.ShowErrorAsync("Error", "Failed to create file");
                            }
                        }
                        catch (Exception ex)
                        {
                            await _openDialogService.ShowErrorAsync("Error", $"Failed to create file:\n{ex.Message}");
                        }
                    }
                });
            }
        }
        public RelayCommand LanguageCommand
        {
            get
            {
                return _languageCommand ??= new RelayCommand(_ =>
                {
                    var cultures = _localizationService.SupportedCultures.ToList();
                    var current = _localizationService.CurrentCulture;
                    int idx = cultures.FindIndex(c => c.Equals(current));
                    int nextIdx = (idx + 1) % cultures.Count;
                    var culture = cultures[nextIdx];

                    if (culture != null && !_localizationService.CurrentCulture.Equals(culture))
                    {
                        _localizationService.CurrentCulture = culture;
                        OnPropertyChanged(string.Empty);
                    }
                });
            }
        }
        public RelayCommand ExitCommand
        {
            get
            {
                return _exitCommand ??= new RelayCommand(_ =>
                {
                    _settingsService.MainWindowLeft = _mainWindowService.Left;
                    _settingsService.MainWindowTop = _mainWindowService.Top;
                    _settingsService.MainWindowFullScreen = _mainWindowService.WindowState == WindowState.Maximized;
                    _mainWindowService.Hide();
                    _settingsService.Save();
                    _db.SaveChanges();
                    Application.Current.Shutdown();
                });
            }
        }
    }
}
