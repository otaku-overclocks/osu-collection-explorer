using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using osu_database_reader;
using Newtonsoft.Json;

namespace osu_collection_manager.Models
{
    /// <summary>
    /// This model should not be serialized alone, only in a MapSet object.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Beatmap
    {
        /// <summary>
        /// An entry in osu.db. Contains all the data about the map. 
        /// May not be present if the instance was created manually.
        /// </summary>
        public BeatmapEntry Entry { get; set; }
        [JsonProperty(PropertyName = "difficulty")]
        public string Difficulty { get; set; }
        [JsonProperty(PropertyName = "beatmap_id")]
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