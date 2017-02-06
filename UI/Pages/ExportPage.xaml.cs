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
using osu_collection_manager.UI.UserControls.Models;

namespace osu_collection_manager.UI.Pages
{
    /// <summary>
    /// Interaction logic for ExportPage.xaml
    /// </summary>
    public partial class ExportPage : BasePage
    {
        public ExportPage()
        {
            InitializeComponent();
            foreach (var collection in CollectionManager.Collections)
            {
                Tree.Children.Add(new CollectionHolder(Tree, collection, true));
            }
            Tree.Title = $"{LocalSongManager.LocalSongs.AccountName}'s collections";
        }

        private void BtnConfirm_OnClick(object sender, RoutedEventArgs e)
        {
            //Get selected collections
            var selected = Tree.GetSelected(false);
            //Put our collections in our file model
            var file = new CollectionsFile(selected) {Name = Tree.Title};
            //If saved. Go back to main page
            if (PromptSave(file)) MainWindow.OpenPage(null);
        }

        public static bool PromptSave(CollectionsFile file)
        {
            //Prompt a dialog to get the path to export to.
            var saveFileDialog = new SaveFileDialog { Filter = "Collections file (*.osc)|*.osc" };
            if (saveFileDialog.ShowDialog() != true) return false; // Action is cancelled
            //Serialize and write the file to our selected path
            file.WriteToFile(saveFileDialog.FileName);
            //Select the file in explorer
            System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{saveFileDialog.FileName}\"");
            return true;
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            MainWindow.OpenPage(null);
        }

        private void BtnSelect_OnClick(object sender, RoutedEventArgs e)
        {
            //var dialog = new OpenFileDialog() { Multiselect = false, Filter = "collection.db|*.db", InitialDirectory = Preferences.OsuPath };
            //var result = dialog.ShowDialog();
            //if (result == true) Preferences.CustomCollectionsDBPath = System.IO.Path.GetFullPath(dialog.FileName);
            
            //MainWindow.OpenPage(new ExportPage());
            // TODO
        }
    }
}
