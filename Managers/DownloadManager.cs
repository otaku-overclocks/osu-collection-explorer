using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu_collection_manager.Properties;
using osu_collection_manager.Utils;
using OsuMapDownload;
using OsuMapDownload.Models;
using OsuMapDownload.Providers;

namespace osu_collection_manager.Managers
{
    public class DownloadManager
    {
        public static readonly ObservableCollection<MapsetDownload> QUEUE = new ObservableCollection<MapsetDownload>();
        public static readonly ObservableCollection<MapsetDownload> DOWNLOADING = new ObservableCollection<MapsetDownload>();
        public static readonly ObservableCollection<MapsetDownload> COMPLETED = new ObservableCollection<MapsetDownload>();

        public static B1oodcatDownloadProvider BcProvider = new B1oodcatDownloadProvider();
        public static OsuDownloadProvider OsuProvider = 
            new OsuDownloadProvider(Properties.Settings.Default.Username, FileUtils.Decrypt(Settings.Default.Password), 
                Preferences.CookiesSavePath);

        private static Action FinishCallback { get; set; }

        /// <summary>
        /// Starts all downloads that are in queue
        /// </summary>
        private static void UpdateDownload()
        {
            // Keeps starting the downloads till there a none left in queue or till we reached the max thread count
            while (QUEUE.Count > 0 && DOWNLOADING.Count < Preferences.BloodcatThreadCount)
            {
                // Get first download in queue
                var ms = QUEUE[0];
                QUEUE.Remove(ms); // Remove from queue
                DOWNLOADING.Add(ms); // Add to downloading
                var task = ms.GetTask(); // Create a task to downlaod and extract the mapset
                var cont = task.ContinueWith(delegate (Task task1) // Create a task which will be run once the downlaod is completed
                {
                    DOWNLOADING.Remove(ms); // Remove from downloads
                    COMPLETED.Add(ms);  // Add to completed
                    if (ms.Status == MapsetDownloadStatus.Failed) // Write t log if dl has failed
                    {
                        //LogManager.Open();
                        LogManager.Write("Failed downloading mapset: "+ms.DownloadProvider.GetUrl(ms));
                        LogManager.Write(ms.Error);
                        //LogManager.Close();
                        if (ms.DownloadProvider == OsuProvider) {
#if DEBUG
                            Debug.WriteLine("Adding to the queue again");
#endif
                            LogManager.Write("Probably deleted from osu. retrying via b1oodcat.");
                            COMPLETED.Remove(ms);
                            ms.Reset(BcProvider);
                            QUEUE.Add(ms);
                        }
                    }
                    UpdateDownload(); // Start downloading new maps if there are some in queue
                }); // Task will not be run in ui thread
                ms.DownloadProvider.StartDownloadTask(task, ms); // Start download async
            }
            // If all downloads are completed and we have set a "all downlaods finished" callback, we will run it and remove it
            if (QUEUE.Count == 0 && DOWNLOADING.Count == 0 && FinishCallback != null)
            {
                FinishCallback();
                FinishCallback = null;
            }
        }

        /// <summary>
        /// Starts downloads. Optionally you can specify a callback which will be run after all downloads are finished
        /// </summary>
        /// <param name="callback"></param>
        public static void StartDownload(Action callback  = null)
        {
            DownloadUtils.SetThreadCountMax();
            if (!Directory.Exists(Preferences.SongsPath))
            {
                Directory.CreateDirectory(Preferences.SongsPath);
            }
            FinishCallback = callback;
            UpdateDownload();
        }
    }
}
