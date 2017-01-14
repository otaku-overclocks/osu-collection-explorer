using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_collection_manager.Models
{
    public class Collection
    {
        public string Name { get; set; }
        public List<MapSet> MapSets { get; set; }

        public Collection(string name, List<Beatmap> beatmaps)
        {
            Name = name;
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
