using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using osu_collection_manager.Annotations;
using osu_collection_manager.Models;

namespace osu_collection_manager.UI.UserControls.Models
{
    public sealed class CollectionHolder : BaseCheckTreeItemHolder<CollectionsTree, MapsetHolder>, INotifyPropertyChanged
    {
        public Collection Data { get; set; }
        
        public string Name => Data.Name;

        public CollectionHolder(CollectionsTree parent, Collection data, bool selected)
        {
            Parent = parent;
            Data = data;
            Selected = selected;

            Children = new BindingList<MapsetHolder>();
            foreach (var mapSet in data.MapSets)
            {
                Children.Add(new MapsetHolder(this, mapSet, selected));
            }
        }
    }
}