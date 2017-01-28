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
        /// <summary>
        /// The purpose of the page. Do we want to import or export maps
        /// </summary>
        public SelectPurpose Purpose;

        public SelectCollectionsPage(IEnumerable<Collection> collections, SelectPurpose purpose = SelectPurpose.Export) : base()
        {
            InitializeComponent();
            Purpose = purpose;
            //We add all the collections into the treelist
            foreach (var collection in collections)
            {
                CollectionsTreeView.Collections.Add(new CollectionHolder(collection, true));
            }
            //Set the confirmatiopn button text. Export or Import
            BtnConfirm.Content = Purpose == SelectPurpose.Export ? "Export" : "Import";
        }

        private void Confirm_OnClick(object sender, RoutedEventArgs e)
        {
            //Pick what action we want to run with selected beatmaps
            if (Purpose == SelectPurpose.Export)
            {
                Export();
            }
            else
            {
                Import();
            }
            
        }

        private void Export()
        {
            //Get selected collections
            var selected = CollectionsTreeView.GetSelected();
            //Prompt a dialog to get the path to export to.
            var saveFileDialog = new SaveFileDialog { Filter = "Collections file (*.osc)|*.osc" };
            if (saveFileDialog.ShowDialog() != true) return; // Action is cancelled
            //Put our collections in our file model
            var file = new CollectionsFile(selected);
            //Serialize and write the file to our selected path
            file.WriteToFile(saveFileDialog.FileName);
            //Select the file in explorer
            System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{saveFileDialog.FileName}\"");
            //Go back to main page
            _mainWindow.WindowContent.Content = null;
        }

        private void Import()
        {
            //Get selected collections
            var selected = CollectionsTreeView.GetSelected();
            //Check if some maps are missing
            var missing = ImportExportManager.CheckMissingMapSets(selected);
            if (missing.Count > 0)
            {
                //Add all missing mapsets to a list with checkboxes where we can pick which we want to download
                foreach (var missingMapSet in missing)
                {
                    MapsetList.Mapsets.Add(new MapsetHolder(null, missingMapSet, true));
                }
                //Show the "some maps are missing" dialog
                MissingOverlay.Visibility = Visibility.Visible;
                DarkenOverlay.Visibility = Visibility.Visible;
                return;
            }
            //Import maps if none are missing. We dont need to download anything
            ImportMaps(selected);
        }

        /// <summary>
        /// Run once the maps we want to download are selected and downlaod button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDownload_OnClick(object sender, RoutedEventArgs e)
        {
            MissingOverlay.Visibility = Visibility.Hidden; // Hide missing overlay 
            DownloadOverlay.Visibility = Visibility.Visible; // Show downlaod overlay
            //Add all maps to download queue
            foreach (var mapSet in MapsetList.GetSelected())
            {
                var dl = new MapsetDownloadHolder(mapSet, mapSet.GetBloodcatLink(), Preferences.DownloadsPath);
                DownloadManager.Queue.Add(dl); // Add to queue
                DownloadList.Downloads.Add(dl); // Add to download list (ui)
            }
            //Start downloads; And add a callback once done
            DownloadManager.StartDownload(() =>
            {
                // Process the maps. This gets all the beatmaps in folder, 
                // gets their md5 hash an puts them into Mapset model
                ImportExportManager.ProcessDownloads(DownloadList.Downloads);
                // Imports all the maps
                ImportMaps(CollectionsTreeView.GetSelected());
                // Hide all overlays
                DownloadOverlay.Visibility = Visibility.Hidden;
                DarkenOverlay.Visibility = Visibility.Hidden;
            });
        }
        private void BtnDownloadCancel_OnClick(object sender, RoutedEventArgs e)
        {
            // Closes the missing maps dialog
            MissingOverlay.Visibility = Visibility.Hidden;
            DarkenOverlay.Visibility = Visibility.Hidden;
        }

        private void ImportMaps(List<Collection> collections)
        {
            // Merges current collections with the collections we want to add
            CollectionManager.AddCollections(collections);
            // Write the database
            CollectionManager.WriteCollectionsDB();
            // Go back to main menu
            _mainWindow.WindowContent.Content = null;
            MessageBox.Show(_mainWindow, "Maps are imported");
        }
    }
}