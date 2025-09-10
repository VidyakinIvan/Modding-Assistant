using Microsoft.EntityFrameworkCore;
using Modding_Assistant.Core;
using Modding_Assistant.MVVM.Model;
using Modding_Assistant.MVVM.Services;
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
        private readonly ModContext db = new();
        private IMoveModDialogService moveModDialogService;
        private RelayCommand? loadCommand;
        private RelayCommand? moveAfterCommand;
        private RelayCommand? deleteCommand;
        private RelayCommand? minimizeCommand;
        private RelayCommand? maximizeCommand;
        private RelayCommand? moveWindowCommand;
        private RelayCommand? exitCommand;
        private Geometry maximizeButtonGeometry = Geometry.Parse("M0,0 M0.2,0.2 L0.8,0.2 L0.8,0.8 L0.2,0.8 Z M1,1");
        public MainViewModel(IMoveModDialogService moveModDialogService)
        {
            db.Mods.Load();
            ModList = new ObservableCollection<ModModel>(
                db.Mods.Local.OrderBy(m => m.Order)
            );
            this.moveModDialogService = moveModDialogService;
        }
        public ObservableCollection<ModModel> ModList { get; set; }
        public RelayCommand? LoadCommand
        {
            get
            {
                return loadCommand ??= new RelayCommand(window =>
                {
                    if (window is Window w)
                    {
                        double left = Properties.Settings.Default.MainWindowLeft;
                        double top = Properties.Settings.Default.MainWindowTop;
                        bool fullscreen = Properties.Settings.Default.MainWindowFullScreen;
                        w.Left = !double.IsNaN(left) ? left : (SystemParameters.WorkArea.Width - w.Width) / 4;
                        w.Top = !double.IsNaN(top) ? top : (SystemParameters.WorkArea.Height - w.Height) / 2;
                        if (fullscreen)
                            MaximizeCommand.Execute(w);
                    }
                });
            }
        }
        public RelayCommand MoveAfterCommand
        {
            get
            {
                return moveAfterCommand ??= new RelayCommand(selectedMods =>
                {
                    if (selectedMods is IList mods && mods.Count > 0)
                    {
                        int? result = moveModDialogService.ShowNumberDialog();
                        if (result.HasValue)
                        {
                            foreach (var mod in mods)
                            {
                                if (mod is ModModel m && ModList.Contains(m))
                                {
                                    int oldIndex = ModList.IndexOf(m);
                                    int newIndex = Math.Min(result.Value, ModList.Count);
                                    if (oldIndex != newIndex)
                                    {
                                        ModList.Move(oldIndex, newIndex);
                                    }
                                }
                            }
                            CollectionViewSource.GetDefaultView(ModList)?.Refresh();
                            db.SaveChanges();
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
                            db.Mods.Remove(mod);
                        db.SaveChanges();
                        CollectionViewSource.GetDefaultView(ModList)?.Refresh();
                    }
                });
            }
        }
        public RelayCommand MinimizeCommand
        {
            get
            {
                return minimizeCommand ??= new RelayCommand(window =>
                {
                    if (window is Window w)
                    {
                        w.WindowState = WindowState.Minimized;
                    }
                });
            }
        }
        public RelayCommand MaximizeCommand
        {
            get
            {
                return maximizeCommand ??= new RelayCommand(window =>
                {
                    if (window is Window w)
                    {
                        if (w.WindowState == WindowState.Maximized)
                        {
                            MaximizeButtonGeometry = Geometry.Parse("M0,0 M0.2,0.2 L0.8,0.2 L0.8,0.8 L0.2,0.8 Z M1,1");
                            w.WindowState = WindowState.Normal;
                        }
                        else
                        {
                            MaximizeButtonGeometry = Geometry.Parse("M0,0 M0.6,0.4 L0.6,0.8 L0.2,0.8 L0.2,0.4 Z M0.8,0.2 L0.8,0.6 L0.4,0.6 L0.4,0.2 Z M1,1");
                            w.WindowState = WindowState.Maximized;
                        }
                    }
                });
            }
        }
        public RelayCommand MoveWindowCommand
        {
            get
            {
                return moveWindowCommand ??= new RelayCommand(window =>
                {
                    if (window is Window w)
                    {
                        w.DragMove();
                    }
                });
            }
        }
        public RelayCommand ExitCommand
        {
            get
            {
                return exitCommand ??= new RelayCommand(window =>
                {
                    if (window is Window w)
                    {
                        Properties.Settings.Default.MainWindowLeft = w.Left;
                        Properties.Settings.Default.MainWindowTop = w.Top;
                        Properties.Settings.Default.MainWindowFullScreen = w.WindowState == WindowState.Maximized;
                        w.Hide();
                    }
                    Properties.Settings.Default.Save();
                    for (int i = 0; i < ModList.Count; i++)
                    {
                        ModList[i].Order = i + 1;
                    }
                    db.SaveChanges();
                    Application.Current.Shutdown();
                });
            }
        }
        public Geometry MaximizeButtonGeometry
        {
            get => maximizeButtonGeometry;
            set
            {
                maximizeButtonGeometry = value;
                OnPropertyChanged();
            }
        }
    }
}
