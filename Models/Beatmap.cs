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
        /// <summary>
        /// An entry in osu.db. Contains all the data bout the map. 
        /// May not be present if the instance was created manually.
        /// </summary>
        public BeatmapEntry Entry { get; set; }
        public string Difficulty { get; set; }
        public int MapID { get; set; }
        public string AudioFileName { get; set; }
        public string FolderName { get; set; }
        public string MD5Hash { get; set; } //TODO: if newly added, calc from file

        public Beatmap(BeatmapEntry entry)
        {
            Entry = entry;
            Difficulty = entry.Difficulty;
            MapID = entry.BeatmapId;
            MD5Hash = entry.BeatmapChecksum;
            AudioFileName = entry.AudioFileName;
            FolderName = entry.FolderName;
        }

        public Beatmap(string hash)
        {
            MD5Hash = hash;
        }
    }
}