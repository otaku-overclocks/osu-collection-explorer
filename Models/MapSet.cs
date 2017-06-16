using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace osu_collection_manager.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MapSet
    {
        private List<Beatmap> _maps = new List<Beatmap>();

        [JsonProperty(PropertyName = "artist")]
        public string Artist { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "beatmapset_id")]
        public int SetID { get; set; }

        [JsonProperty(PropertyName = "beatmaps")]
        public List<Beatmap> Maps
        {
            get
            {
                if(_maps == null) _maps = new List<Beatmap>();
                return _maps;
            }
            set
            {
                _maps = value;
                
            }
        }

        public string Folder { get; set; }

        public MapSet(List<Beatmap> beatmaps)
        {
            Debug.Assert(beatmaps != null && beatmaps.Count > 0,
                "You need at to have at least one beatmap to create a MapSet");
            Debug.Assert(beatmaps[0].Entry != null, "This constructor is only for Local MapSets");
            Artist = beatmaps[0].Entry.Artist;
            Title = beatmaps[0].Entry.Title;
            SetID = beatmaps[0].Entry.BeatmapSetId;
            Maps = beatmaps;
            Folder = beatmaps[0].Entry.FolderName;
        }

        public MapSet(string artist, string title, int setId, List<Beatmap> maps)
        {
            this.Artist = artist;
            this.Title = title;
            SetID = setId;
            Maps = maps;
        }

        public string GetBloodcatLink()
        {
            return $"{Preferences.BloodcatDownloadLink}{SetID}";
        }
    }
}