using Microsoft.EntityFrameworkCore;
using Modding_Assistant.Core;
using Modding_Assistant.MVVM.Model;
using Modding_Assistant.MVVM.Services.Implementations;
using Modding_Assistant.MVVM.Services.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Modding_Assistant.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        private readonly ModContext _db;
        private readonly IMainWindowService _mainWindowService;
        private readonly ISettingsService _settingsService;
        private readonly IMoveModDialogService _moveModDialogService;
        private RelayCommand? loadCommand;
        private RelayCommand? moveBeforeCommand;
        private RelayCommand? deleteCommand;
        private RelayCommand? minimizeCommand;
        private RelayCommand? maximizeCommand;
        private RelayCommand? moveWindowCommand;
        private RelayCommand? exitCommand;
        private Geometry _maximizeButtonGeometry = Geometry.Parse("M0,0 M0.2,0.2 L0.8,0.2 L0.8,0.8 L0.2,0.8 Z M1,1");
        public MainViewModel(ModContext db, IMainWindowService mainWindowService, ISettingsService settingsService, IMoveModDialogService moveModDialogService)
        {
            _db = db;
            _mainWindowService = mainWindowService;
            _settingsService = settingsService;
            _moveModDialogService = moveModDialogService;
            _db.Mods.Load();
            ModList = db.Mods.Local.ToObservableCollection();
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
            if (_mainWindowService is MainWindowService ws)
            {
                UpdateMaximizeButtonGeometry(_mainWindowService.WindowState);
            }
        }
        public ObservableCollection<ModModel> ModList { get; set; }
        public RelayCommand LoadCommand
        {
            get
            {
                return loadCommand ??= new RelayCommand(_ =>
                {
                    _mainWindowService.Left = !double.IsNaN(_settingsService.MainWindowLeft) ? _settingsService.MainWindowLeft : (SystemParameters.WorkArea.Width - _mainWindowService.Width) / 4;
                    _mainWindowService.Top = !double.IsNaN(_settingsService.MainWindowTop) ? _settingsService.MainWindowTop : (SystemParameters.WorkArea.Height - _mainWindowService.Height) / 2;
                    if (_settingsService.MainWindowFullScreen)
                        MaximizeCommand.Execute(null);
                });
            }
        }
        public RelayCommand MoveBeforeCommand
        {
            get
            {
                return moveBeforeCommand ??= new RelayCommand(selectedMods =>
                {
                    if (selectedMods is IList mods && mods.Count > 0)
                    {
                        int? result = _moveModDialogService.ShowNumberDialog();
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
                return deleteCommand ??= new RelayCommand(selectedMods =>
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
                return minimizeCommand ??= new RelayCommand(_ =>
                {
                    _mainWindowService.Minimize();
                });
            }
        }
        public RelayCommand MaximizeCommand
        {
            get
            {
                return maximizeCommand ??= new RelayCommand(_ =>
                {
                    if (_mainWindowService.WindowState == WindowState.Maximized)
                    {
                        _mainWindowService.Restore();
                        MaximizeButtonGeometry = Geometry.Parse("M0,0 M0.2,0.2 L0.8,0.2 L0.8,0.8 L0.2,0.8 Z M1,1");
                    }
                    else
                    {
                        _mainWindowService.Maximize();
                        MaximizeButtonGeometry = Geometry.Parse("M0,0 M0.6,0.4 L0.6,0.8 L0.2,0.8 L0.2,0.4 Z M0.8,0.2 L0.8,0.6 L0.4,0.6 L0.4,0.2 Z M1,1");
                    }
                });
            }
        }
        public RelayCommand MoveWindowCommand
        {
            get
            {
                return moveWindowCommand ??= new RelayCommand(_ =>
                {
                    _mainWindowService.DragMove();
                });
            }
        }
        public RelayCommand ExitCommand
        {
            get
            {
                return exitCommand ??= new RelayCommand(_ =>
                {
                    _settingsService.MainWindowLeft = _mainWindowService.Left;
                    _settingsService.MainWindowTop = _mainWindowService.Top;
                    _settingsService.MainWindowFullScreen = _mainWindowService.WindowState == WindowState.Maximized;
                    _mainWindowService.Hide();
                    _settingsService.Save();
                    for (int i = 0; i < ModList.Count; i++)
                    {
                        ModList[i].Order = i + 1;
                    }
                    _db.SaveChanges();
                    Application.Current.Shutdown();
                });
            }
        }
        public Geometry MaximizeButtonGeometry
        {
            get => _maximizeButtonGeometry;
            set
            {
                _maximizeButtonGeometry = value;
                OnPropertyChanged();
            }
        }
        private void UpdateMaximizeButtonGeometry(WindowState state)
        {
            if (state == WindowState.Maximized)
            {
                MaximizeButtonGeometry = Geometry.Parse("M0,0 M0.6,0.4 L0.6,0.8 L0.2,0.8 L0.2,0.4 Z M0.8,0.2 L0.8,0.6 L0.4,0.6 L0.4,0.2 Z M1,1");
            }
            else
            {
                MaximizeButtonGeometry = Geometry.Parse("M0,0 M0.2,0.2 L0.8,0.2 L0.8,0.8 L0.2,0.8 Z M1,1");
            }
        }
    }
}
