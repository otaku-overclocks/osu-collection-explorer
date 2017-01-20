using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using osu_collection_manager.UI.UserControls.Models;

namespace osu_collection_manager.UI.UserControls
{
    /// <summary>
    /// Interaction logic for DownloadList.xaml
    /// </summary>
    public partial class DownloadList : UserControl
    {
        public BindingList<MapsetDownloadHolder> Downloads { get; set; }

        public DownloadList()
        {
            InitializeComponent();
            Downloads = new BindingList<MapsetDownloadHolder>();
            DownloadsListView.ItemsSource = Downloads;
        }
    }
}
