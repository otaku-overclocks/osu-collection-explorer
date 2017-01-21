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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using osu_collection_manager.Managers;
using osu_collection_manager.Models;
using osu_collection_manager.UI.Pages;
using osu_collection_manager.UI.UserControls.Models;
using static System.Windows.Forms.MessageBoxButtons;
using MessageBox = System.Windows.MessageBox;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace osu_collection_manager.Pages
{
    /// <summary>
    /// Interaction logic for SelectCollectionsPage.xaml
    /// </summary>
    public partial class SelectCollectionsPage : BaseMainPage
    {
        public enum SelectPurpose
        {
            Export,
            Import
        }

        public SelectPurpose Purpose;

        public SelectCollectionsPage(IEnumerable<Collection> collections, SelectPurpose purpose = SelectPurpose.Export) : base()
        {
            InitializeComponent();
            Purpose = purpose;
            foreach (var collection in collections)
            {
                CollectionsTreeView.Collections.Add(new CollectionHolder(collection, true));
            }
            BtnConfirm.Content = Purpose == SelectPurpose.Export ? "Export" : "Import";
        }

        private void Confirm_OnClick(object sender, RoutedEventArgs e)
        {
            var selected = CollectionsTreeView.GetSelected();
            if (Purpose == SelectPurpose.Export)
            {
                var saveFileDialog = new SaveFileDialog {Filter = "Collections file (*.osc)|*.osc"};
                if (saveFileDialog.ShowDialog() != true) return;
                var file = new CollectionsFile(selected);
                file.WriteToFile(saveFileDialog.FileName);
                System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{saveFileDialog.FileName}\"");
                _mainWindow.WindowContent.Content = null;
                return;
            }
            //TODO: Split methods
            var missing = ImportExportManager.CheckMissingMapSets(selected);
            if (missing.Count > 0)
            {
                foreach (var missingMapSet in missing)
                {
                    MapsetList.Mapsets.Add(new MapsetHolder(null, missingMapSet, true));
                }
                MissingOverlay.Visibility = Visibility.Visible;
                DarkenOverlay.Visibility = Visibility.Visible;
                return;
            }
            ImportMaps(selected);
        }

        private void BtnDownload_OnClick(object sender, RoutedEventArgs e)
        {
            MissingOverlay.Visibility = Visibility.Hidden;
            DownloadOverlay.Visibility = Visibility.Visible;
            foreach (var mapSet in MapsetList.GetSelected())
            {
                var dl = new MapsetDownloadHolder(mapSet, mapSet.GetBloodcatLink(), Preferences.DownloadsPath);
                DownloadManager.Queue.Add(dl);
                DownloadList.Downloads.Add(dl);
            }
            DownloadManager.StartDownload(() =>
            {
                ImportExportManager.ProcessDownloads(DownloadList.Downloads);
                ImportMaps(CollectionsTreeView.GetSelected());
                DownloadOverlay.Visibility = Visibility.Hidden;
                DarkenOverlay.Visibility = Visibility.Hidden;
            });
        }
        private void BtnDownloadCancel_OnClick(object sender, RoutedEventArgs e)
        {
            MissingOverlay.Visibility = Visibility.Hidden;
            DarkenOverlay.Visibility = Visibility.Hidden;
        }

        private void ImportMaps(IEnumerable<Collection> collections)
        {
            CollectionManager.AddCollections(collections);
            CollectionManager.WriteCollectionsDB();
            _mainWindow.WindowContent.Content = null;
            MessageBox.Show(_mainWindow, "Maps are imported");
        }
    }
}