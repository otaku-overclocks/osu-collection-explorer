using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using osu_collection_manager.Models;
using osu_collection_manager.UI.Pages.Modals;
using osu_collection_manager.Properties;
using osu_collection_manager.Utils;

namespace osu_collection_manager.UI.Pages
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenuPage : BasePage
    {
        public MainMenuPage()
        {
            InitializeComponent();
            if (Settings.Default.TourneyMLLoggedOn)
            {
                BtnLogOut.IsEnabled = true;
                LoginStatus.Content = "Logged in as " + Settings.Default.TourneyMLUser;
            }
            else
            {
                BtnLogOut.IsEnabled = false;
            }
        }

        public void OpenExportPage()
        {
            MainWindow.OpenPage(new ExportPage());
        }

        private void BtnExport_OnClick(object sender, RoutedEventArgs e)
        {
            OpenExportPage();
        }

        public void OpenImportEditPage(IEnumerable<Collection> collections, string name = null)
        {
            MainWindow.OpenPage(new ImportEditPage(collections, name));
        }

        public void ImportFromFile(string path)
        {
            var collections = CollectionsFile.ReadFromFile(path);
            OpenImportEditPage(collections.Collections, collections.Name);
        }

        private void BtnImport_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog() { Filter = "Collections file|*.osc" };
            if (openFileDialog.ShowDialog() != true) return;
            var collections = CollectionsFile.ReadFromFile(openFileDialog.FileName);
            OpenImportEditPage(collections.Collections, collections.Name);
        }

        private void BtnResetCOnfig_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.OsuPath = Preferences.OsuPath;
            Settings.Default.Save();
            Settings.Default.Reload();
        }

        // show online collections
        private void BtnTestLogin_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Default.TourneyMLLoggedOn)
            {
                MainWindow.OpenPage(new Online.CollectionListPage());
            }
            else
            {
                MainWindow.OpenDialog(new Online.Modals.TournamentMLLoginModal());
            }
        }

        private void BtnLogOut_Click(object sender, RoutedEventArgs e)
        {
            LoginStatus.Content = "Not logged in";
            BtnLogOut.IsEnabled = false;
            Settings.Default.TourneyMLLoggedOn = false;
            Settings.Default.Save();
        }

        private void BtnExportFolderOsz_OnClick(object sender, RoutedEventArgs e)
        {
            FileUtils.FolderToOsz();
        }
    }
}
