using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using osu_collection_manager.Models;
using osu_collection_manager.UI.UserControls.Models;
using OsuMapDownload.Models;

namespace osu_collection_manager.Managers
{
    public class ImportExportManager
    {
        /// <summary>
        /// Check a list of collections if they contain missing mapsets. The missing ones should be downloaded.
        /// Ths also copies over the mapset of existing ones since imported dont contain that data
        /// </summary>
        /// <param name="collections"></param>
        /// <returns>List with missing mapsets</returns>
        public static List<MapSet> CheckMissingMapSets(List<Collection> collections)
        {
            var ret = new List<MapSet>();
            foreach (var collection in collections)
            {
                foreach (var mapSet in collection.MapSets)
                {
                    // If no set found in osu.db we add it to the list of not found maps.
                    var found = LocalSongManager.FindSetByID(mapSet.SetID);
                    if (found == null)
                    {
                        ret.Add(mapSet);
                    }
                    else
                    {
                        // This is more reliable
                        mapSet.Maps = found.Maps;
                    }
                }
            }
            return ret;
        }

        public static void ProcessDownloads(IEnumerable<MapsetDownloadHolder> downloads)
        {
#if DEBUG
            LogManager.Open();
#endif
            foreach (var mapSetDownload in downloads)
            {
                if(mapSetDownload.Failed || !mapSetDownload.Completed) continue;
#if DEBUG
                LogManager.Write($"Processing map: {mapSetDownload.Name}");
#endif
                try
                {
                    var extractedPath =
                        $"{Preferences.SongsPath}/{MapSetDownload.MakeOsuFolderName(mapSetDownload.Name)}";
                    var files = System.IO.Directory.GetFiles(extractedPath, "*.osu");
                    foreach (var file in files)
                    {
                        var beatmap = new Beatmap(GetHashFromFile(file));
                        mapSetDownload.Mapset.Maps.Add(beatmap);
                    }
                }
                catch (Exception e)
                {
                    LogManager.Write($"Could not process download for map {mapSetDownload.Name}", LogManager.ERROR_TAG);
                    LogManager.Write(e);
                }
            }
#if DEBUG
            LogManager.Close();
#endif
        }

        /// <summary>
        /// Calculate md5 hash from a file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetHashFromFile(string path)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty).ToLower();
                }
            }
        }
    }
}
