using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace osu_collection_manager.Models
{
    [DataContract]
    public class MapSet
    {
        private List<Beatmap> _maps = new List<Beatmap>();

        [DataMember]
        public string Artist { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public int SetID { get; set; }

        /// <summary>
        /// Empty when read from file, but never null
        /// </summary>
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
            Artist = artist;
            Title = title;
            SetID = setId;
            Maps = maps;
        }

        public string GetBloodcatLink()
        {
            return $"{Preferences.BloodcatDownloadLink}{SetID}";
        }
    }
}