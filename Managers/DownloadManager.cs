using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using osu_collection_manager.Models;

namespace osu_collection_manager.Managers
{
    public class DownloadManager
    {
        public static readonly List<MapSet> DownloadQueue = new List<MapSet>();
        private static readonly Dictionary<MapSet, Task> DownloadsProcessing = new Dictionary<MapSet, Task>();

        public static void StartDownloads()
        {
            while (DownloadQueue.Count > 0 && DownloadsProcessing.Count < Preferences.BloodcatThreadCount)
            {
                WebClient client = new WebClient();
                //todo: download all
            }
        }

        //Synchronous!!!!
        public static void StartDownload(MapSet set)
        {
            TestTask(set).Wait();
            //TODO: Only for tests
        }

        private static Task TestTask(MapSet set)
        {
            return Task.Run(() =>
            {
                using (var client = new WebClient())
                {
                    client.OpenRead(set.GetBloodcatLink());
                    var header_contentDisposition = client.ResponseHeaders["content-disposition"];
                    var filename = new ContentDisposition(header_contentDisposition).FileName;
                    client.DownloadFile(set.GetBloodcatLink(), $"{Preferences.OsuInstallationPath}/temp/{filename}");
                }
            });
        }
    }
}