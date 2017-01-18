using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace osu_collection_manager.Models
{
    /// <summary>
    /// Using DataContract here to be able to serialize it to json
    /// </summary>
    [DataContract]
    public class Collection
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public List<MapSet> MapSets { get; set; }

        public Collection(string name, List<Beatmap> beatmaps)
        {
            Name = name;
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
            MapSets = mapSets.Values.ToList();
        }
        
        public Collection(string name, List<MapSet> mapSets)
        {
            Name = name;
            MapSets = mapSets;
        }


    }
}
