using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
    public class MapsetDownloadHolder : MapSetExtractDownload, INotifyPropertyChanged
    {
        private float _progress = 0;
        private bool _extracted = false;
        private Exception _error;

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

        public override Exception Error
        {
            get { return _error; }
            set
            {
                _error = value;
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

        public override void AfterComplete()
        {
            base.AfterComplete();
            var filePath = $"{Path}/{Name}";
            if (!File.Exists(filePath)) return;
            if (!Directory.Exists(Preferences.SongsPath)) Directory.CreateDirectory(Preferences.SongsPath);
            var dest = $"{Preferences.SongsPath}/{Name}";
            if (File.Exists(dest))
            {
                File.Delete(dest);
            }
            File.Move(filePath, dest);
        }
    }
}