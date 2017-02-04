using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OsuMapDownload;
using OsuMapDownload.Models;

namespace osu_collection_manager.Managers
{
    public class DownloadManager
    {
        public static readonly List<MapSetDownload> Queue = new List<MapSetDownload>();
        public static readonly List<MapSetDownload> Downloading = new List<MapSetDownload>();
        public static readonly List<MapSetDownload> Completed = new List<MapSetDownload>();

        private static Action _finishCallback { get; set; }

        /// <summary>
        /// Starts all downloads that are in queue
        /// </summary>
        private static void UpdateDownload()
        {
            // Keeps starting the downloads till there a none left in queue or till we reached the max thread count
            while (Queue.Count > 0 && Downloading.Count < Preferences.BloodcatThreadCount)
            {
                // Get first download in queue
                var ms = Queue[0];
                Queue.Remove(ms); // Remove from queue
                Downloading.Add(ms); // Add to downloading
                var task = ms.CreateTask(); // Create a task to downlaod and extract the mapset
                var cont = task.ContinueWith(delegate (Task task1) // Create a task which will be run once the downlaod is completed
                {
                    Downloading.Remove(ms); // Remove from downloads
                    Completed.Add(ms);  // Add to completed
                    UpdateDownload(); // Start downloading new maps if there are some in queue
                    if (ms.Failed) // Write t log if dl has failed
                    {
                        LogManager.Open();
                        LogManager.Write($"Failed downloading mapset from url: {ms.Url}");
                        LogManager.Write(ms.Error);
                        LogManager.Close();
                    }
                }); // Task will be run in ui thread
                task.Start(); // Start download async
            }
            // If all downloads are completed and we have set a "all downlaods finished" callback, we will run it and remove it
            if (Queue.Count == 0 && Downloading.Count == 0 && _finishCallback != null)
            {
                _finishCallback();
                _finishCallback = null;
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
            _finishCallback = callback;
            UpdateDownload();
        }
    }
}
