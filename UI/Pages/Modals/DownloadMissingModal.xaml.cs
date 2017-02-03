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
using osu_collection_manager.Managers;
using osu_collection_manager.Models;
using osu_collection_manager.UI.UserControls.Models;

namespace osu_collection_manager.UI.Pages.Modals
{
    /// <summary>
    /// Interaction logic for DownloadMissingModal.xaml
    /// </summary>
    public partial class DownloadMissingModal : BaseModal
    {


        public DownloadMissingModal(IEnumerable<MapSet> missing, Action<ModalFinishType> closeCallback = null) : base(closeCallback)
        {
            InitializeComponent();
            foreach (var mapSet in missing)
            {
                MapsetList.Mapsets.Add(new MapsetHolder(null, mapSet, true));
            }
        }

        private void BtnDownloadCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close(ModalFinishType.Cancelled);
        }

        private void BtnDownload_OnClick(object sender, RoutedEventArgs e)
        {
            MissingSelector.Visibility = Visibility.Hidden; // Hide missing overlay 
            ProgressView.Visibility = Visibility.Visible;// Show download overlay
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
                Close();
            });
        }
    }
}
