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
using System.IO.Compression;
using System.Diagnostics;
using System.IO;
using TagLib;
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
            exportSave.Visibility = Visibility.Hidden;
            exportToRepo.Unchecked += exportToRepo_Unchecked;

        }

        private void BtnConfirm_OnClick(object sender, RoutedEventArgs e)
        {
            //Get selected collections
            var selected = Tree.GetSelected(false);

            #region mp3 functions
            if ((bool)exportMP3.IsChecked)
            {
                List<Collection> fileLocations = selected;      // get our selected maps
                // string key: bd dictionary
                // dictionary value: names dictionary
                Dictionary<Dictionary<string, Beatmap>, Dictionary<string, string>> mapidname = new Dictionary<Dictionary<string, Beatmap>, Dictionary<string, string>>();
                if (Directory.EnumerateFileSystemEntries(Preferences.DownloadsPath + "/").Any()) // Check if the OCM Temp folder has files in it.
                {
                    foreach (FileInfo file in Common.ocmDirInfo.GetFiles())
                        file.Delete();                                                           // If we do, wipe the OCM Temp folder.
                }

                bool breakc = false;

                foreach (Collection c in fileLocations)
                {
                    foreach (MapSet s in c.MapSets)
                    {
                        foreach (Beatmap b in s.Maps)
                        {
                            foreach (Dictionary<string, Beatmap> n in mapidname.Keys)
                            {
                                    if (n.ContainsKey(b.Entry.Title)) { breakc = true; break; } // If we already have the map, no need to put it in.
                                breakc = false;
                            }
                            if (breakc)
                            {
                                break;
                            }
                            // string key: audio file name
                            // string value: folder name
                            var names = new Dictionary<string, string>();

                            // string key : beatmap title
                            // Beatmap value : beatmap
                            var vb = new Dictionary<string,Beatmap>();

                            names.Add(b.AudioFileName, b.FolderName);   // If we don't, add it to the dictionary.
                            vb.Add(b.Entry.Title, b);                   // Add it again to another dictionary...
                            mapidname.Add(vb, names);                    // Add everything together.
                        }
                    }
                }

                foreach (KeyValuePair<Dictionary<string,Beatmap>, Dictionary<string, string>> y in mapidname)
                {
                    foreach (KeyValuePair<string, Beatmap> i in y.Key)
                    {
                            foreach (KeyValuePair<string, string> x in y.Value)
                            {
                                #region Replace invalid characters to be used as a filename.
                                string filename = "null";
                                filename = i.Value.Entry.Title;
                                filename = filename.Replace('~', '_');
                                filename = filename.Replace('#', '_');
                                filename = filename.Replace('%', '_');
                                filename = filename.Replace('&', '_');
                                filename = filename.Replace('*', '_');
                                filename = filename.Replace('[', '_');
                                filename = filename.Replace(']', '_');
                                filename = filename.Replace('(', '_');
                                filename = filename.Replace(')', '_');
                                filename = filename.Replace('{', '_');
                                filename = filename.Replace('}', '_');
                                filename = filename.Replace('\\', '_');
                                filename = filename.Replace(';', '_');
                                filename = filename.Replace(':', '_');
                                filename = filename.Replace('|', '_');
                                filename = filename.Replace('?', '_');
                                filename = filename.Replace('/', '_');
                                filename = filename.Replace('\'', '_');
                                filename = filename.Replace('"', '_');
                                #endregion

                                // Copy the mp3 to the osu! folder/OCMTemp.
                                System.IO.File.Copy($"{Preferences.SongsPath}/{x.Value}/{x.Key}", $"{Preferences.DownloadsPath}/{filename}.mp3", true);

                                #region Remove the tags, and add our tags.
                                TagLib.File taglibfile = TagLib.File.Create($"{Preferences.DownloadsPath}/{filename}.mp3");
                                taglibfile.RemoveTags(TagTypes.AllTags);
                                taglibfile.Tag.Title = i.Value.Entry.Title;
                                taglibfile.Tag.AlbumArtists = new string[1] { i.Value.Entry.Artist };
                                taglibfile.Save();
                                #endregion
                            }
                    }
                }

                if ((bool)exportToZip.IsChecked)
                {
                    //Prompt a dialog to get the path to export to.
                    var saveFileDialog = new SaveFileDialog { Filter = "Zip file (*.zip)|*.zip", AddExtension = true, FileName = $"{LocalSongManager.LocalSongs.AccountName }'s songs.zip" };
                    saveFileDialog.ShowDialog();
                    if (System.IO.File.Exists(saveFileDialog.FileName))
                    {
                        MessageBox.Show($"You have a {saveFileDialog.FileName} in your selected folder. Please move it. If it still exists after pressing OK, it will be overriden.");
                        if (System.IO.File.Exists(saveFileDialog.FileName))
                        {
                            System.IO.File.Delete(saveFileDialog.FileName);
                        }
                    }
                    ZipFile.CreateFromDirectory(Preferences.DownloadsPath, saveFileDialog.FileName); // Finally, create a zip file from our song files.
                    System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{saveFileDialog.FileName}\"");

                    MessageBox.Show($"Successfully created!");
                    MainWindow.OpenPage(null);          // Return to homepage.
                }

            }
            #endregion

            // Check if the user wants to zip the collections.
            if ((bool)exportToZip.IsChecked)
            {
                List<Collection> fileLocations = selected;      // get our selected maps
                List<MapSet> mps = new List<MapSet>();          // initialize the mapset list
                List<string> folderList = new List<string>();   // initalize our folderList

                if (Directory.EnumerateFileSystemEntries(Preferences.DownloadsPath + "/").Any()) // Check if the OCM Temp folder has files in it.
                {
                    foreach (FileInfo file in Common.ocmDirInfo.GetFiles())
                        file.Delete();                                                           // If we do, wipe the OCM Temp folder.
                }

                foreach (Collection i in selected)
                {
                    foreach (MapSet j in i.MapSets)
                    {
                        folderList.Add(j.Folder);               // Add the folder value into the list of strings
                    }
                }

                foreach (string i in folderList)                // Check if we have duplicates, If we do, delete em.
                {
                    if (System.IO.File.Exists(Preferences.DownloadsPath + $"/{i}.osz"))
                    {
                        System.IO.File.Delete(Preferences.DownloadsPath + $"/{i}.osz");
                    }
                    ZipFile.CreateFromDirectory(Preferences.SongsPath + $"/{i}", Preferences.DownloadsPath + $"/{i}.osz");
                }

                //Prompt a dialog to get the path to export to.
                var saveFileDialog = new SaveFileDialog { Filter = "Zip file (*.zip)|*.zip", AddExtension = true, FileName = $"{LocalSongManager.LocalSongs.AccountName }'s songs.zip" };
                saveFileDialog.ShowDialog();
                if (System.IO.File.Exists(saveFileDialog.FileName))
                    {
                        MessageBox.Show($"You have a {saveFileDialog.FileName} in your selected folder. Please move it. If it still exists after pressing OK, it will be overriden.");
                        if (System.IO.File.Exists(saveFileDialog.FileName))
                        {
                            System.IO.File.Delete(saveFileDialog.FileName);
                        }
                    }
                ZipFile.CreateFromDirectory(Preferences.DownloadsPath, saveFileDialog.FileName); // Finally, create a zip file from our osz files.
                System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{saveFileDialog.FileName}\"");
                MessageBox.Show($"Successfully created!");

                if ((bool)exportToRepo.IsChecked)
                {
                    MessageBox.Show("Exporting to an online repository has not been implemented yet, sorry!");
                    if ((bool)exportSave.IsChecked) { } // If export to repo is checked with export to computer
                    MainWindow.OpenPage(null);
                } // export to repo, zipped.
            }
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
            var dialog = new OpenFileDialog() { Multiselect = false, Filter = "collection.db|*.db", InitialDirectory = Preferences.OsuPath };
            var result = dialog.ShowDialog();
            if (result == true) Preferences.CollectionsDBPath = System.IO.Path.GetFullPath(dialog.FileName);
            CollectionManager.ReadCollectionsDB();
            MainWindow.OpenPage(null);
            MainWindow.OpenPage(new ExportPage());
        }

        private void exportToZip_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void exportToRepo_Checked(object sender, RoutedEventArgs e)
        {
            exportSave.Visibility = Visibility.Visible;
        }
        private void exportToRepo_Unchecked(object sender, RoutedEventArgs e)
        {
            exportSave.Visibility = Visibility.Hidden;
        }

        private void exportMP3_Checked(object sender, RoutedEventArgs e)
        {
            exportToRepo.Visibility = Visibility.Hidden;
        }
    }
}
