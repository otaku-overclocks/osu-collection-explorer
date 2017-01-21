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
    public class MapsetHolder : INotifyPropertyChanged
    {
        public CollectionHolder Parent;
        public MapSet Data { get; set; }
        private bool _selected = false;

        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                Parent?.UpdateSelected();
                OnPropertyChanged(nameof(Selected));
            }
        }

        public string Name => $"{Data.SetID} {Data.Artist} - {Data.Title}";

        public MapsetHolder(CollectionHolder parent, MapSet data, bool selected)
        {
            Parent = parent;
            Data = data;
            _selected = selected;
        }

        public void SetSelected(bool selected)
        {
            _selected = selected;
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