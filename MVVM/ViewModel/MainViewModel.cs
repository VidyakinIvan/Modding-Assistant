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
        private readonly IMoveModsDialogService _moveModsDialogService;
        private RelayCommand? _loadCommand;
        private RelayCommand? _fromFileCommand;
        private RelayCommand? _moveBeforeCommand;
        private RelayCommand? _deleteCommand;
        private RelayCommand? _minimizeCommand;
        private RelayCommand? _maximizeCommand;
        private RelayCommand? _moveWindowCommand;
        private RelayCommand? _settingsCommand;
        private RelayCommand? _exportCommand;
        private RelayCommand? _exitCommand;
        public MainViewModel(ModContext db, IMainWindowService mainWindowService, ISettingsService settingsService, IMoveModsDialogService moveModsDialogService)
        {
            _db = db;
            _mainWindowService = mainWindowService;
            _settingsService = settingsService;
            _moveModsDialogService = moveModsDialogService;
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
        private static string GetFriendlyModName(string fileName)
        {
            var name = System.Text.RegularExpressions.Regex.Replace(
                fileName,
                @"([-_ (]*\d{4,}.*$)|(\s*[\(\[]?v?\d+(\.\d+)*[\)\]]?$)",
                string.Empty,
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
            );
            name = name.Replace('_', ' ').Replace('-', ' ').Trim();
            name = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());
            return string.IsNullOrWhiteSpace(name) ? fileName : name;
        }
        public RelayCommand FromFileCommand
        {
            get
            {
                return _fromFileCommand ??= new RelayCommand(_ =>
                {
                    var openFileDialog = new Microsoft.Win32.OpenFileDialog
                    {
                        Filter = "Archive Files (*.zip;*.rar;*.7z)|*.zip;*.rar;*.7z|All Files (*.*)|*.*",
                        Title = "Select Mod Archive"
                    };
                    if (openFileDialog.ShowDialog() == true)
                    {
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName);

                        var newMod = new ModModel { Name = GetFriendlyModName(fileName), ModRawName = fileName, LastUpdated = DateOnly.FromDateTime(DateTime.Today) };
                        ModList.Add(newMod);
                    }
                    _db.SaveChanges();
                    string modsFolder = _settingsService.ModsFolder;
                    if (!string.IsNullOrWhiteSpace(_settingsService.ModsFolder) && Directory.Exists(modsFolder))
                    {
                        string sourceFilePath = openFileDialog.FileName;
                        if (Directory.Exists(modsFolder))
                        {
                            string destFilePath = Path.Combine(modsFolder, Path.GetFileName(sourceFilePath));
                            try
                            {
                                File.Move(sourceFilePath, destFilePath, overwrite: true);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Failed to copy file:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                        int? result = _moveModsDialogService.ShowNumberDialog();
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
                        _mainWindowService.Restore();
                    else
                        _mainWindowService.Maximize();
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
                return _settingsCommand ??= new RelayCommand(_ =>
                {
                    var pickFolderDialog = new Microsoft.Win32.OpenFolderDialog
                    {
                        Title = "Select Mod Folder"
                    };
                    if (pickFolderDialog.ShowDialog() == true)
                    {
                        string folderName = pickFolderDialog.FolderName;
                        _settingsService.ModsFolder = pickFolderDialog.FolderName;
                    }
                });
            }
        }

        public RelayCommand ExportCommand
        {
            get
            {
                return _exportCommand ??= new RelayCommand(_ =>
                {
                    var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                    {
                        Filter = "Excel files (*.xlsx)|*.xlsx",
                        FileName = $"Export_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.xlsx"
                    };
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        try
                        {
                            using (var workbook = new XLWorkbook())
                            {
                                var worksheet = workbook.Worksheets.Add("Mods");
                                var headers = new[]
                                {
                                    "Order", "Name", "Version", "Install Instructions", "Url",
                                    "Dependencies", "Mod Raw Name", "Last Updated", "Description", "Potential Issues"
                                };

                                for (int i = 0; i < headers.Length; i++)
                                {
                                    worksheet.Cell(1, i + 1).Value = headers[i];
                                    worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                                    worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                                }
                                int row = 2;
                                foreach (var mod in ModList)
                                {
                                    worksheet.Cell(row, 1).Value = mod.Order;
                                    worksheet.Cell(row, 2).Value = mod.Name;
                                    worksheet.Cell(row, 3).Value = mod.Version;
                                    worksheet.Cell(row, 4).Value = mod.InstallInstructions;
                                    worksheet.Cell(row, 5).Value = mod.Url;
                                    worksheet.Cell(row, 6).Value = mod.Dependencies;
                                    worksheet.Cell(row, 7).Value = mod.ModRawName;
                                    worksheet.Cell(row, 8).Value = mod.LastUpdated.ToString();
                                    worksheet.Cell(row, 9).Value = mod.Description;
                                    worksheet.Cell(row, 10).Value = mod.PotentialIssues;
                                    row++;
                                }

                                worksheet.Columns().AdjustToContents();
                                workbook.SaveAs(saveFileDialog.FileName);
                                MessageBox.Show("Export successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Failed to create file:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
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
