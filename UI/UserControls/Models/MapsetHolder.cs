using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using osu_collection_manager.Annotations;
using osu_collection_manager.Models;

namespace osu_collection_manager.UI.UserControls.Models
{
    public sealed class MapsetHolder : BaseCheckTreeItemHolder<CollectionHolder, MapsetHolder>, INotifyPropertyChanged
    {
        public MapSet Data { get; set; }
        public string Name => $"{Data.SetID} {Data.Artist} - {Data.Title}";

        public MapsetHolder(CollectionHolder parent, MapSet data, bool selected)
        {
            Parent = parent;
            Data = data;
            SetSelected(selected);
        }
    }
}