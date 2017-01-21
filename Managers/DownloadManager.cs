using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OsuMapDownload.Models;

namespace osu_collection_manager.Managers
{
    public class DownloadManager
    {
        public static readonly List<MapSetDownload> Queue = new List<MapSetDownload>();
        public static readonly List<MapSetDownload> Downloading = new List<MapSetDownload>();
        public static readonly List<MapSetDownload> Completed = new List<MapSetDownload>();

        private static Action _finishCallback { get; set; }

        private static void UpdateDownload()
        {
            while (Queue.Count > 0 && Downloading.Count < Preferences.BloodcatThreadCount)
            {
                var ms = Queue[0];
                Queue.Remove(ms);
                Downloading.Add(ms);
                var task = ms.CreateTask(Preferences.SongsPath);
                var cont = task.ContinueWith(delegate (Task task1)
                {
                    Downloading.Remove(ms);
                    Completed.Add(ms);
                    UpdateDownload();
                }, TaskScheduler.FromCurrentSynchronizationContext());
                task.Start();
            }
            if (Queue.Count == 0 && Downloading.Count == 0 && _finishCallback != null)
            {
                _finishCallback();
                _finishCallback = null;
            }
        }

        public static void StartDownload(Action callback  = null)
        {
            _finishCallback = callback;
            UpdateDownload();
        }
    }
}
