using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace osu_collection_manager.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Collection
    {
        // public properties going to JSON

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "mapsets")]
        public List<MapSet> Mapsets { get; set; }
        [JsonProperty(PropertyName = "user_id")]
        public int CreatorID { get; set; }
        [JsonProperty(PropertyName = "visibility")]
        public int Visibility { get; set; }


        // -----------------------
        // properties that don't go to JSON

        public int BeatmapCount => Mapsets.Sum(mapSet => mapSet.Maps.Count);

        // -----------------------
        // Constructors

        public Collection(string name, List<Beatmap> beatmaps)
        {
            Title = name;
            // Loops through all the beatmaps and adds the ones with a same mapset id to the same list to create a
            // Mapset out of it
            var mapSets = new Dictionary<int, MapSet>();
            foreach (var beatmap in beatmaps)
            {
                if (!mapSets.ContainsKey(beatmap.Entry.BeatmapSetId))
                {
                    mapSets.Add(beatmap.Entry.BeatmapSetId, new MapSet(new List<Beatmap>() {beatmap}));
                    continue;
                }
                mapSets[beatmap.Entry.BeatmapSetId].Maps.Add(beatmap);
            }
            Mapsets = mapSets.Values.ToList();
        }
        
        public Collection(string name, List<MapSet> mapSets)
        {
            Title = name;
            Mapsets = mapSets;
        }


    }
}
