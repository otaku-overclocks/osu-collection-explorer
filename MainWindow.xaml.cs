using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using osu_collection_manager.Managers;
using osu_collection_manager.UI.Pages;
using System.Diagnostics;
using Microsoft.Win32;
using osu_collection_manager.Models;
using osu_collection_manager.Pages;

namespace osu_collection_manager
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // temp fix
            string osupath = null;
            Process[] pnames = Common.LoadOsuExe() as Process[];
            if (pnames == null)
            {
                // osu! hasn't been found
                OpenFileDialog openfiledialog = new OpenFileDialog() { Multiselect = false, Filter = "osu!.exe|osu!.exe" };
                bool? result = openfiledialog.ShowDialog();
                if (result == true)
                {
                    osupath = openfiledialog.FileName;
                }
            }
            else
            {
                // osu! has been found, store first path
                osupath = pnames[0].MainModule.FileName;
            }
            if (osupath != null)
            {
                Preferences.OsuInstallationPath = System.IO.Path.GetDirectoryName(osupath);
            }
            // temp fix end
            new Task(() =>
            {
                var col = CollectionManager.Collections; // First time read.
            }).Start();
           
        }

        // BEGIN menubar code

        private void goToTestPage_Click(object sender, RoutedEventArgs e)
        {
            WindowContent.Content = new Pages.RandomTestsPage();
        }

        // END menubar code
        private void ExportCollections_OnClick(object sender, RoutedEventArgs e)
        {
            OpenPage(new Pages.SelectCollectionsPage(CollectionManager.Collections, SelectCollectionsPage.SelectPurpose.Export));
        }

        private void ImportCollections_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() != true) return;
            var collections = CollectionsFile.ReadFromFile(openFileDialog.FileName);
            OpenPage(new Pages.SelectCollectionsPage(collections.Collections, SelectCollectionsPage.SelectPurpose.Import));
        }

        public void DisplayLoadingOverlay(bool display)
        {
            LoadingOverlay.Visibility = display ? Visibility.Visible : Visibility.Hidden;
        }

        public void OpenPage(BaseMainPage page)
        {
            DisplayLoadingOverlay(true);
            WindowContent.Content = page;
        }
    }
}
