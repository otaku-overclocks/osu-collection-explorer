using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static readonly B1oodcatDownloadProvider BC_PROVIDER = new B1oodcatDownloadProvider();

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
                    UpdateDownload(); // Start downloading new maps if there are some in queue
                    if (ms.Status == MapsetDownloadStatus.Failed) // Write t log if dl has failed
                    {
                        LogManager.Open();
                        LogManager.Write("Failed downloading mapset.");
                        LogManager.Write(ms.Error);
                        LogManager.Close();
                    }
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
