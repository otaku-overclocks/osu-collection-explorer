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
        public ImportEditPage(IEnumerable<Collection> collections, string name = null)
        {
            InitializeComponent();
            foreach (var collection in collections)
            {
                Tree.Children.Add(new CollectionHolder(Tree, collection, true));
            }
            if (name != null) Tree.Title = name;
        }

        private void BtnImport_OnClick(object sender, RoutedEventArgs e)
        {
            //Get selected collections
            var selected = Tree.GetSelected(true);
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
            var selected = Tree.GetSelected(false);
            //Put our collections in our file model
            var file = new CollectionsFile(selected) { Name = Tree.Title };
            //If saved. Go back to main page
            if (ExportPage.PromptSave(file)) MainWindow.OpenPage(null);
        }
    }
}