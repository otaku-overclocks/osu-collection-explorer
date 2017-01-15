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
using osu_database_reader;


namespace osu_collection_manager
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        public TestWindow()
        {
            InitializeComponent(); 
        }

        

        // note to devs: this is only testing so i can get something working, and maybe design UI around it
        // what's in there is most likely working, which means: game folder at c:\users\jérémy\appdata\local\osu! (changed path in Preferences.cs)

        private void readCollection_Click(object sender, RoutedEventArgs e)
        {
            CollectionDb db = CollectionDb.Read(Preferences.CollectionsDBPath);
            version.Text = Convert.ToString(db.OsuVersion);
            List<Collection> collections = new List<Collection>();
            foreach (Collection dbCollec in db.Collections)
            {
                TreeViewItem listBoxCollec = new TreeViewItem();
                listBoxCollec.Header = dbCollec.Name;
                Dictionary<int, List<BeatmapEntry>> mapsets = new Dictionary<int, List<BeatmapEntry>>();
                bool isMapsetInCollection = false;
                foreach (string map in dbCollec.Md5Hashes)
                {
                    int mapsetId = 0;
                    BeatmapEntry beatmap = Managers.LocalSongManager.FindByHash(map);
                    // listBoxCollec.Items.Add(beatmap.Artist + " - " + beatmap.Title + " [" + beatmap.Difficulty + "] (mapped by " + beatmap.Creator + ")");
                    foreach (KeyValuePair<int,List<BeatmapEntry>> key in mapsets)
                    {
                        if (Convert.ToInt32(beatmap.BeatmapSetId)==key.Key)
                        {
                            isMapsetInCollection = true;
                            mapsetId = Convert.ToInt32(beatmap.BeatmapSetId);
                        }
                    }
                    if (!isMapsetInCollection)
                    {
                        mapsets.Add(Convert.ToInt32(beatmap.BeatmapSetId), new List<BeatmapEntry> { beatmap });
                    }
                    else
                    {
                        mapsets[mapsetId].Add(beatmap);
                    }                    
                }
                foreach (KeyValuePair<int, List<BeatmapEntry>> mapset in mapsets)
                {
                    TreeViewItem collecMapset = new TreeViewItem();
                    BeatmapEntry temp = mapset.Value[0];
                    collecMapset.Header = temp.Artist + " - " + temp.Title + " (mapped by " + temp.Creator + ")";
                    foreach (BeatmapEntry map in mapset.Value)
                    {
                        collecMapset.Items.Add(map.Difficulty);
                    }
                    listBoxCollec.Items.Add(collecMapset);
                }
                CollectionTreeView.Items.Add(listBoxCollec);
            }
        }

        private void readOsuDatabase_Click(object sender, RoutedEventArgs e)
        {
            OsuDb mapList = OsuDb.Read(Preferences.OsuDBPath);
            OsuDatabaseVersion.Text = Convert.ToString(mapList.OsuVersion);
            foreach (BeatmapEntry map in mapList.Beatmaps)
            {
                TreeViewItem beatmap = new TreeViewItem();
                beatmap.Tag = map.BeatmapSetId;
                bool isMapSetFound = false;
                foreach (TreeViewItem item in OsuDatabaseTreeView.Items)
                {
                    if (Convert.ToInt32(item.Tag) == map.BeatmapSetId)
                    {
                        isMapSetFound = true;
                        //item.Items.Add(map.Difficulty);
                        //break;
                    }
                }
                if (!isMapSetFound)
                {
                    List<BeatmapEntry> mapset = Managers.LocalSongManager.FindAllBySetID(map.BeatmapSetId);
                    TreeViewItem beatmapSet = new TreeViewItem();
                    beatmapSet.Tag = map.BeatmapSetId;
                    beatmapSet.Header = map.Artist + " - " + map.Title + " (mapped by " + map.Creator + ")";
                    foreach (BeatmapEntry difficulty in mapset)
                    {
                        beatmapSet.Items.Add(difficulty.Difficulty);
                    }
                    OsuDatabaseTreeView.Items.Add(beatmapSet);
                }


            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            path.Text = dialog.SelectedPath;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Preferences.OsuInstallationPath = path.Text;
        }
    }
}
