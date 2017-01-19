using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu_collection_manager.Models;

namespace osu_collection_manager.UI.UserControls.Models
{
    public class CollectionHolder
    {
        public Collection Data { get; set; }
        public bool Selected { get; set; }
        public ObservableCollection<MapsetHolder> Mapsets { get; set; }

        public CollectionHolder(Collection data, bool selected)
        {
            Data = data;
            Selected = selected;

            Mapsets = new ObservableCollection<MapsetHolder>();
            foreach (var mapSet in data.MapSets)
            {
                Mapsets.Add(new MapsetHolder(mapSet, selected));
                Debug.WriteLine("Added");
            }
        }
    }
}