using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using osu_database_reader;

namespace osu_collection_manager.Models
{
    public class Beatmap
    {
        [Obsolete]
        public string Artist { get; set; }
        [Obsolete]
        public string Title { get; set; }

        public BeatmapEntry Entry { get; set; }
        public string Difficulty { get; set; }
        public int MapID { get; set; }

        public string MD5Hash { get; set; } //TODO: if newly added, calc from file

        public Beatmap(BeatmapEntry entry)
        {
            Artist = entry.Artist;
            Title = entry.Title;
            Entry = entry;
            Difficulty = entry.Difficulty;
            MapID = entry.BeatmapId;
            MD5Hash = entry.BeatmapChecksum;
        }

        public static string GetHashFromFile(string path)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    return md5.ComputeHash(stream).ToString();
                }
            }
        }
    }
}