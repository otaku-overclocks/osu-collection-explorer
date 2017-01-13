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
        public List<Beatmap> Beatmaps { get; set; }

        public Collection(string name, List<Beatmap> beatmaps)
        {
            Name = name;
            Beatmaps = beatmaps;
        }


    }
}
