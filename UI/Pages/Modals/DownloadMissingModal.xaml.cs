using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
using osu_collection_manager.Annotations;
using osu_collection_manager.Managers;
using osu_collection_manager.Models;
using osu_collection_manager.UI.UserControls.Models;
using OsuMapDownload;

namespace osu_collection_manager.UI.Pages.Modals
{
    /// <summary>
    /// Interaction logic for DownloadMissingModal.xaml
    /// </summary>
    public partial class DownloadMissingModal : BaseModal, INotifyPropertyChanged
    {
        private string _downloadingProgress = "0/0";
        public string DownloadingProgress
        {
            get { return _downloadingProgress; }
            set
            {
                _downloadingProgress = value; 
                OnPropertyChanged(nameof(DownloadingProgress));
            }
        }

        public DownloadMissingModal(IEnumerable<MapSet> missing, Action<ModalFinishType> closeCallback = null) : base(closeCallback)
        {
            InitializeComponent();
            foreach (var mapSet in missing)
            {
                MapsetList.Mapsets.Add(new MapsetHolder(null, mapSet, true));
            }
            DownloadManager.DOWNLOADING.CollectionChanged += DownloadingCollectionChanged; 
        }

        private void DownloadingCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            DownloadingProgress =
                $"{DownloadManager.COMPLETED.Count}/{DownloadManager.QUEUE.Count + DownloadManager.DOWNLOADING.Count + DownloadManager.COMPLETED.Count}";
        }

        private void BtnDownloadCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close(ModalFinishType.Cancelled);
        }

        private void BtnDownload_OnClick(object sender, RoutedEventArgs e)
        {
            MissingSelector.Visibility = Visibility.Hidden; // Hide missing overlay 
            ProgressView.Visibility = Visibility.Visible;// Show download overlay
            BeatmapDownloadProvider provider;
            if (Preferences.LoginDefined) {
                provider = DownloadManager.OsuProvider;
            } else {
                provider = DownloadManager.BcProvider;
            }
            foreach (var mapSet in MapsetList.GetSelected())
            {
                var dl = new MapsetDownloadHolder(mapSet, Preferences.DownloadsPath, provider);
                DownloadManager.QUEUE.Add(dl); // Add to queue
                DownloadList.Downloads.Add(dl); // Add to download list (ui)
            }
            //Start downloads; And add a callback once done
            DownloadManager.StartDownload(() =>
            {
                // Process the maps. This gets all the beatmaps in folder, 
                // gets their md5 hash an puts them into Mapset model
                ImportExportManager.ProcessDownloads(DownloadList.Downloads);
                this.Dispatcher.Invoke(() =>
                {
                    Close();
                });
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
