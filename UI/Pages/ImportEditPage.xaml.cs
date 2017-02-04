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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using osu_collection_manager.Managers;
using osu_collection_manager.Models;
using osu_collection_manager.UI.Pages.Modals;
using osu_collection_manager.UI.UserControls;
using osu_collection_manager.UI.UserControls.Models;

namespace osu_collection_manager.UI.Pages
{
    /// <summary>
    /// Interaction logic for ImportEditPage.xaml
    /// </summary>
    public partial class ImportEditPage : BasePage
    {
        public ImportEditPage(IEnumerable<Collection> collections)
        {
            InitializeComponent();
            foreach (var collection in collections)
            {
                Tree.Children.Add(new CollectionHolder(Tree, collection, true));
            }
        }

        private void BtnImport_OnClick(object sender, RoutedEventArgs e)
        {
            //Get selected collections
            var selected = Tree.GetSelected();
            //Check if some maps are missing
            var missing = ImportExportManager.CheckMissingMapSets(selected);
            if (missing.Count > 0)
            {
                MainWindow.OpenModal(new DownloadMissingModal(missing, delegate(ModalFinishType type)
                {
                    if (type.Equals(ModalFinishType.Succes)) 
                        ImportCollections(selected);
                }));
                return;
            }
            ImportCollections(selected);
        }

        public void ImportCollections(List<Collection> collections)
        {
            //Backup if checked
            if(ChkBackup.IsChecked == true) BackupManager.BackupCollections();
            // Merges current collections with the collections we want to add
            CollectionManager.AddCollections(collections);
            // Write the database
            CollectionManager.WriteCollectionsDB();
            // Go back to main menu
            MainWindow.OpenPage(null);
            MessageBox.Show(MainWindow, "Maps are imported. Don't forget to restart twice the game if new maps have been downloaded to make sure the maps are imported and shown in the collections! (This is not a bug in either osu! or OCM. osu!'s behavior causes it to not reload the collections after importing beatmaps.)");
        }

        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            //Get selected collections
            var selected = Tree.GetSelected();
            //Prompt a dialog to get the path to export to.
            var saveFileDialog = new SaveFileDialog {Filter = "Collections file (*.osc)|*.osc"};
            if (saveFileDialog.ShowDialog() != true) return; // Action is cancelled
            //Put our collections in our file model
            var file = new CollectionsFile(selected);
            //Serialize and write the file to our selected path
            file.WriteToFile(saveFileDialog.FileName);
            //Select the file in explorer
            System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{saveFileDialog.FileName}\"");
            //Go back to main page
            MainWindow.OpenPage(null);
        }
    }
}