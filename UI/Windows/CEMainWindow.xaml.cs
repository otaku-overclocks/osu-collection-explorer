
using System.CodeDom;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using osu_collection_manager.UI.Pages;
using osu_collection_manager.UI.Pages.Modals;
using System.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using osu_collection_manager.Managers;
using osu_collection_manager.Models;

namespace osu_collection_manager.UI.Windows
{
    /// <summary>
    /// Interaction logic for CEMainWindow.xaml
    /// </summary>
    public partial class CEMainWindow : BaseNavigationWindow
    {
        public Vector ModalSize { get; set; }

        public static Task InitialLoadTask { get; private set; }

        public CEMainWindow()
        {
            ModalSize = new Vector(460, 300);
            InitializeComponent();
            OpenPage(new MainMenuPage());
            LoadingOverlay.Visibility = Visibility.Visible;
            InitialLoadTask = new Task(() =>
            {
                var col = CollectionManager.Collections; // First time read.
            });
            InitialLoadTask.ContinueWith((task, o) =>
            {
                Dispatcher.Invoke(() => {
                    LoadingOverlay.Visibility = Visibility.Hidden;
                });
            }, TaskScheduler.FromCurrentSynchronizationContext());
            InitialLoadTask.Start();
            if (!Properties.Settings.Default.ShownWarning) {
                OpenDialog(new WarningModal());
            } else {
                ShowLogin();
            }
        }

        public void ShowLogin() {
            if(!Properties.Settings.Default.ShownLogin)
            OpenDialog(new AskLogin());
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            // Application.Current.Shutdown();
            Environment.Exit(0);
        }

        private void Maximize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Equals(WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized;
        }

        private void Minimize(object sender, RoutedEventArgs e)
        {
           WindowState = WindowState.Minimized;
        }

        public sealed override void OpenPage(BasePage page)
        {
            if(page == null) page = new MainMenuPage();
            WindowContent.Content = page;
        }

        public override void OpenDialog(BaseModal dialog)
        {
            if (dialog == null)
            {
                MainDialog.IsOpen = false;
                ShowLogin();
                return;
            }
          
            ModalContent.Content = dialog;
            MainDialog.IsOpen = true;
        }

        private void OnFileDrop(object sender, DragEventArgs e)
        {
            if (WindowContent.Content.GetType() != typeof(MainMenuPage)) return;
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if(files == null || files.Length == 0 || Path.GetExtension(files[0]) != Preferences.COLLECTION_FORMAT) return;
            ((MainMenuPage)WindowContent.Content).ImportFromFile(files[0]);
        }

        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            OpenPage(new SettingsPage());
        }

        private void GoGithub(object sender, RoutedEventArgs e)
        {
            Process.Start("http://github.com/otaku-overclocks/osu-collection-explorer");
        }
    }
}