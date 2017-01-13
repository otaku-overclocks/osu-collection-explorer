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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); 
        }

        OsuDb mapList = OsuDb.Read(@"C:\Users\Jérémy\AppData\Local\osu!\osu!.db");

        private void readCollection_Click(object sender, RoutedEventArgs e)
        {
            CollectionDb db = CollectionDb.Read(@"C:\Users\Jérémy\AppData\Local\osu!\collection.db");
            version.Text = Convert.ToString(db.OsuVersion);
            foreach (Collection dbCollec in db.Collections)
            {
                TreeViewItem listBoxCollec = new TreeViewItem();
                listBoxCollec.Header = dbCollec.Name;
                foreach (string map in dbCollec.Md5Hashes)
                {
                    listBoxCollec.Items.Add(map);
                }
                CollectionTreeView.Items.Add(listBoxCollec);
            }
        }

        private void readOsuDatabase(object sender, RoutedEventArgs e)
        {
            OsuDatabaseVersion.Text = Convert.ToString(mapList.OsuVersion);
            foreach (BeatmapEntry map in mapList.Beatmaps)
            {
                TreeViewItem beatmap = new TreeViewItem();
                beatmap.Header = map.Artist + " - " + map.Title + "[" + map.Difficulty + "]";
                
            }
        }
    }
}
