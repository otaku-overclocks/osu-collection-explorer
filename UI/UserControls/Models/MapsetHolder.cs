using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu_collection_manager.Models;

namespace osu_collection_manager.UI.UserControls.Models
{
    public class MapsetHolder
    {
        public MapSet Data { get; set; }
        public bool Selected { get; set; }

        public MapsetHolder(MapSet data, bool selected)
        {
            Data = data;
            Selected = selected;
        }
    }
}
