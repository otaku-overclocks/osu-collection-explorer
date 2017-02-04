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
        private static SolidColorBrush _failedBrush = new SolidColorBrush(Color.FromRgb(0xF4, 0x43, 0x36));
        private static SolidColorBrush _succesBrush = new SolidColorBrush(Color.FromRgb(0x4C, 0xAF, 0x50));

        private float _progress = 0;
        private string _progressText = "Waiting";
        private Exception _error;
        private SolidColorBrush _progressBrush = new SolidColorBrush(Color.FromRgb(0xC5, 0x11, 0x62));
        public override float Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                ProgressText = $"{(_progress/100):P1}";
                OnPropertyChanged(nameof(Progress));
            }
        }

        public string ProgressText
        {
            get { return _progressText; }
            set
            {
                _progressText = value;
                OnPropertyChanged(nameof(ProgressText));
            }
        }

        public SolidColorBrush ProgressBrush
        {
            get { return _progressBrush; }
            set
            {
                _progressBrush = value;
                OnPropertyChanged(nameof(ProgressBrush));
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
                ProgressText = "Failed!";
                ProgressBrush = _failedBrush;
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
            ProgressText = "Success!";
            ProgressBrush = _succesBrush;
        }
    }
}