using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using osu_collection_manager.Annotations;
using OsuMapDownload.Models;

namespace osu_collection_manager.UI.UserControls.Models
{
    public class MapsetDownloadHolder : MapSetDownload, INotifyPropertyChanged
    {
        //public override float Progress { get; set; }

        public MapsetDownloadHolder(string url, string path, string name = null) : base(url, path, name)
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
