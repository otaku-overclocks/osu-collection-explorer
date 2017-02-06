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
using OsuMapDownload;
using OsuMapDownload.Models;

namespace osu_collection_manager.UI.UserControls.Models
{
    public class MapsetDownloadHolder : MapsetExtractDownload, INotifyPropertyChanged
    {
        private static readonly SolidColorBrush FAILED_BRUSH = new SolidColorBrush(Color.FromRgb(0xF4, 0x43, 0x36));
        private static readonly SolidColorBrush SUCCES_BRUSH = new SolidColorBrush(Color.FromRgb(0x4C, 0xAF, 0x50));

        public override float Progress
        {
            get { return base.Progress*100; }
            protected set
            {
                base.Progress = value;
                OnPropertyChanged(nameof(Progress));
                OnPropertyChanged(nameof(StatusDisplayField));
            }
        }

        public SolidColorBrush ProgressBrush { get; set; } = new SolidColorBrush(Color.FromRgb(0xC5, 0x11, 0x62));

        public MapSet Mapset { get; set; }
        public string Title => $"{Mapset.Artist} - {Mapset.Title}";

        public override MapsetDownloadStatus Status {
            get { return base.Status; }
            set {
                base.Status = value;
                OnPropertyChanged(nameof(StatusDisplayField));
            }
        }

        public string StatusDisplayField {
            get {
                switch (Status) {
                    case MapsetDownloadStatus.Failed:
                        ProgressBrush = FAILED_BRUSH;
                        base.Progress = 1;
                        OnPropertyChanged(nameof(Progress));
                        OnPropertyChanged(nameof(ProgressBrush));
                        return "Failed!";
                    case MapsetDownloadStatus.Completed:
                        ProgressBrush = SUCCES_BRUSH;
                        base.Progress = 1;
                        OnPropertyChanged(nameof(Progress));
                        OnPropertyChanged(nameof(ProgressBrush));
                        return "Finished!";
                    case MapsetDownloadStatus.Waiting:
                        return "Waiting";
                    case MapsetDownloadStatus.Downloading:
                        return $"{(Progress/100):P1}";
                    case MapsetDownloadStatus.Extracting:
                        return "Extracting";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public MapsetDownloadHolder(MapSet mapset, string path, BeatmapDownloadProvider provider) : base(mapset.SetID, path, provider) {
            Mapset = mapset;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected override void AfterDownload() {
            base.AfterDownload();
            var filePath = $"{Path}/{FileName}";
            if (!File.Exists(filePath)) return;
            if (!Directory.Exists(Preferences.SongsPath)) Directory.CreateDirectory(Preferences.SongsPath);
            var dest = $"{Preferences.SongsPath}/{FileName}";
            if (File.Exists(dest)) {
                File.Delete(dest);
            }
            File.Move(filePath, dest);
        }
    }
}