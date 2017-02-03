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
        }

        public void OpenExportPage()
        {
            MainWindow.OpenPage(new ExportPage());
        }

        private void BtnExport_OnClick(object sender, RoutedEventArgs e)
        {
            OpenExportPage();
        }

        public void OpenImportEditPage(IEnumerable<Collection> collections)
        {
            MainWindow.OpenPage(new ImportEditPage(collections));
        }

        private void BtnImport_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() != true) return;
            var collections = CollectionsFile.ReadFromFile(openFileDialog.FileName);
            OpenImportEditPage(collections.Collections);
        }
    }
}
