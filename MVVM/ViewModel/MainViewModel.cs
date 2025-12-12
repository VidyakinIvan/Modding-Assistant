using Modding_Assistant.MVVM.Base;
using Modding_Assistant.MVVM.Model;
using Modding_Assistant.MVVM.Services.Interfaces;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Modding_Assistant.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private readonly IMainWindowService _mainWindowService;
        private readonly IModManagerService _modManagerService;
        private readonly IOpenDialogService _openDialogService;
        private readonly IModFilesService _modFilesService;
        private readonly ILocalizationManagerService _localizationManagerService;
        private readonly ILocalizationService _localizationService;
        private readonly ISettingsService _settingsService;

        private static readonly TimeSpan ExportTimeout = TimeSpan.FromMinutes(2);

        private WindowState _windowState;

        private bool _isImporting;
        private double _importProgress;
        public WindowState WindowState
        {
            get => _windowState;
            set => SetProperty(ref _windowState, value);
        }
        public bool IsImporting
        {
            get => _isImporting;
            set => SetProperty(ref _isImporting, value);
        }
        public double ImportProgress
        {
            get => _importProgress;
            set => SetProperty(ref _importProgress, value);
        }
        public IProgress<double> ImportProgressReporter { get; }
        public ObservableCollection<ModModel> ModList => _modManagerService.Mods;

        public string CurrentLanguage => _localizationManagerService.CurrentLanguage;

        public MainViewModel(IMainWindowService mainWindowService,
            IModManagerService modManagerService,
            IOpenDialogService openDialogService,
            IModFilesService modFilesService,
            ILocalizationManagerService localizationManagerService,
            ILocalizationService localizationService,
            ISettingsService settingsService)
        {
            _mainWindowService = mainWindowService;
            _modManagerService = modManagerService;
            _openDialogService = openDialogService;
            _modFilesService = modFilesService;
            _localizationManagerService = localizationManagerService;
            _localizationService = localizationService;
            _settingsService = settingsService;

            _mainWindowService.WindowStateChanged += (s, state) => WindowState = state;
            _localizationManagerService.LanguageChanged += (s, e) => OnPropertyChanged(nameof(CurrentLanguage));

            ImportProgressReporter = new Progress<double>(value =>
            {
                ImportProgress = value;
            });

            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await _modManagerService.LoadModsAsync();
        }

        public RelayCommand LoadCommand =>
            new(_ => _mainWindowService.InitializeWindow());

        public RelayCommand MoveWindowCommand =>
            new(_ => _mainWindowService.DragMove());

        public RelayCommand MinimizeCommand =>
            new(_ => _mainWindowService.Minimize());

        public RelayCommand MaximizeCommand =>
            new(_ =>
            {
                if (_mainWindowService.WindowState == WindowState.Maximized)
                {
                    _mainWindowService.Restore();
                    
                }
                else
                {
                    _mainWindowService.Maximize();
                }
            });

        public RelayCommand ExitCommand =>
            new(_ =>
            {
                _mainWindowService.SaveWindowSettings();
                _mainWindowService.Close();
            });

        public RelayCommandAsync FromFileCommandAsync =>
            new(async _ =>
            {
                if (IsImporting)
                    return;
                IsImporting = true;
                var progress = new Progress<double>(p =>
                {
                    ImportProgress = p;
                });
                try
                {
                    var fileName = _openDialogService.ShowOpenFileDialog(
                        _localizationService["SelectArchivePrompt"],
                        "Archive Files (*.zip;*.rar;*.7z)|*.zip;*.rar;*.7z|All Files (*.*)|*.*");

                    if (string.IsNullOrEmpty(fileName))
                        return;

                    await _modFilesService.ImportModAsync(fileName, progress);
                }
                finally
                {
                    IsImporting = false;
                    ImportProgress = 0;
                }
            });

        public RelayCommandAsync MoveModsCommandAsync =>
            new(async selectedMods =>
            {
                if (selectedMods is not IList mods || mods.Count == 0) 
                    return;

                var targetPosition = 
                    _openDialogService.ShowMoveModsDialog(_mainWindowService.GetMainWindow());

                if (!targetPosition.HasValue)
                    return;

                var modModels = mods.OfType<ModModel>();
                await _modManagerService.MoveModsAsync(modModels, targetPosition.Value);
            });

        public RelayCommandAsync DeleteModsCommandAsync =>
            new(async selectedMods =>
            {
                if (selectedMods is not IList mods) 
                    return;

                var modModels = mods.OfType<ModModel>().ToList();
                if (modModels.Count > 0)
                {
                    await _modManagerService.DeleteModsAsync(modModels);
                }
            });

        public RelayCommand SettingsCommand =>
            new(_ =>
            {
                var folderName = _openDialogService.ShowPickFolderDialog(
                    _localizationService["SelectFolderPrompt"]);
                if (!string.IsNullOrEmpty(folderName))
                {
                    _settingsService.ModsFolder = folderName;
                }
            });

        public RelayCommandAsync ExportCommandAsync =>
            new(async _ =>
            {
                var fileName = _openDialogService.ShowSaveFileDialog(
                    $"Export to Excel", "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*");

                if (!string.IsNullOrEmpty(fileName))
                {
                    await _modFilesService.ExportModsAsync(
                        ModList,
                        fileName,
                        "Mods", new CancellationTokenSource(ExportTimeout).Token
                    );
                }
            });

        public RelayCommand LanguageCommand => new(_ =>
        {
            _localizationManagerService.SwitchToNextLanguage();
        });
    }
}
