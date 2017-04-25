using osu_collection_manager.Managers;
using osu_collection_manager.Models;
using osu_collection_manager.UI.UserControls.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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

namespace osu_collection_manager.UI.Pages
{
    /// <summary>
    /// Interaction logic for ExportMP3.xaml
    /// </summary>
    public partial class ExportMP3 : BasePage
    {
        public ExportMP3()
        {
            InitializeComponent();
            foreach (var collection in CollectionManager.Collections)
            {
                Tree.Children.Add(new CollectionHolder(Tree, collection, true));
            }
            Tree.Title = $"{LocalSongManager.LocalSongs.AccountName}'s collections";
            exportSave.Visibility = Visibility.Hidden;
            //exportToRepo.Unchecked += exportToRepo_Unchecked;
        }

        private void BtnConfirm_OnClick(object sender, RoutedEventArgs e)
        {
            //Get selected collections
            var selected = Tree.GetSelected(false);

            // Check if the user wants to zip the collections.
            if ((bool)exportToZip.IsChecked)
            {
                List<Collection> fileLocations = selected;      // get our selected maps
                // int key: setID
                // dictionary value: names dictionary
                Dictionary<int, Dictionary<string,string>> mapidname = new Dictionary<int, Dictionary<string,string>>();
                // string key: audio file name
                // string value: folder name
                Dictionary<string, string> names = new Dictionary<string, string>();

                if (Directory.EnumerateFileSystemEntries(Preferences.DownloadsPath + "/").Any()) // Check if the OCM Temp folder has files in it.
                {
                    foreach (FileInfo file in Common.ocmDirInfo.GetFiles())
                        file.Delete();                                                           // If we do, wipe the OCM Temp folder.
                }

                foreach (Collection c in fileLocations)
                {
                    foreach (MapSet s in c.MapSets)
                    {
                        foreach (Beatmap b in s.Maps)
                        {
                            if (mapidname.ContainsKey(s.SetID)) { continue; }
                            names.Add(b.AudioFileName, b.FolderName);
                            mapidname.Add(s.SetID, names);
                            names.Clear();
                        }
                    }
                }

                foreach (KeyValuePair<int, Dictionary<string, string>> i in mapidname)
                {
                    foreach (KeyValuePair<string, string> x in i.Value)
                    {
                        File.Copy($"{Preferences.SongsPath}/{x.Value}/{x.Key}.mp3", $"{Preferences.DownloadsPath}/");
                    }
                }

                System.Windows.Forms.MessageBox.Show("done!");
                MainWindow.OpenPage(null);
            }
        }
    }
}
