using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using osu_collection_manager.Annotations;
using osu_collection_manager.Models;
using OsuMapDownload.Models;

namespace osu_collection_manager.UI.UserControls.Models
{
    public class MapsetDownloadHolder : MapSetDownload, INotifyPropertyChanged
    {
        private float _progress = 0;
        private bool _extracted = false;
        private bool _failed = false;

        public override float Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                OnPropertyChanged(nameof(Progress));
            }
        }
        public MapSet Mapset { get; set; }
        public string Title => $"{Mapset.Artist} - {Mapset.Title}";
        public override bool Extracted
        {
            get { return _extracted; }
            set
            {
                _extracted = value;
                OnPropertyChanged(nameof(Extracted));
            }
        }
        public override bool Failed
        {
            get { return _failed; }
            set
            {
                _failed = value;
                Progress = 100f;
                OnPropertyChanged(nameof(Failed));
            }
        }

        public MapsetDownloadHolder(MapSet mapset, string url, string path, string name = null) : base(url, path, name)
        {
            Mapset = mapset;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}