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
    public class CollectionHolder : INotifyPropertyChanged
    {
        public Collection Data { get; set; }
        private bool? _selected = true;

        public bool? Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                if (Mapsets != null && value != null)
                    foreach (var mapset in Mapsets)
                    {
                        mapset.SetSelected((bool) value);
                    }
                OnPropertyChanged(nameof(Selected));
            }
        }

        public string Name => Data.Name;
        public ObservableCollection<MapsetHolder> Mapsets { get; set; }

        public CollectionHolder(Collection data, bool selected)
        {
            Data = data;
            Selected = selected;

            Mapsets = new ObservableCollection<MapsetHolder>();
            foreach (var mapSet in data.MapSets)
            {
                Mapsets.Add(new MapsetHolder(this, mapSet, selected));
            }
        }

        public void UpdateChildren(bool check)
        {
            foreach (var mapset in Mapsets)
            {
                mapset.Selected = check;
            }
        }

        public void UpdateSelected()
        {
            _selected = Mapsets[0].Selected;
            for (var i = 1; i < Mapsets.Count; i++)
            {
                if (_selected == Mapsets[i].Selected) continue;
                _selected = null;
                OnPropertyChanged(nameof(Selected));
                break;
            }
            OnPropertyChanged(nameof(Selected));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}