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

        public string Name { get; set; }

        public CollectionHolder(CollectionsTree parent, Collection data, bool selected)
        {
            Parent = parent;
            Data = data;
            Selected = selected;
            Name = Data.Title;

            Children = new BindingList<MapsetHolder>();
            foreach (var mapSet in data.Mapsets)
            {
                Children.Add(new MapsetHolder(this, mapSet, selected));
            }
        }

        public Collection GetData(bool apply = false)
        {
            if (!apply) return new Collection(Name, Data.Mapsets);
            Data.Title = Name;
            return Data;
        }
    }
}