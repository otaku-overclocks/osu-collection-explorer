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
using osu_collection_manager.Models;
using osu_collection_manager.UI.UserControls.Models;

namespace osu_collection_manager.UI.UserControls
{
    /// <summary>
    /// Interaction logic for MapsetList.xaml
    /// </summary>
    public partial class MapsetList : UserControl
    {
        public BindingList<MapsetHolder> Mapsets { get; set; }

        public MapsetList()
        {
            InitializeComponent();
            Mapsets = new BindingList<MapsetHolder>();
            MapsetListView.ItemsSource = Mapsets;
        }

        public List<MapSet> GetSelected()
        {
            var ret = new List<MapSet>();
            foreach (var mapsetHolder in Mapsets)
            {
                if(mapsetHolder.Selected == true) ret.Add(mapsetHolder.Data);
            }
            return ret;
        }
    }
}
