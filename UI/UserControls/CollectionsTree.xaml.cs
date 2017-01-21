using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using osu_collection_manager.Models;
using osu_collection_manager.UI.UserControls.Models;
using UserControl = System.Windows.Controls.UserControl;

namespace osu_collection_manager.UI.UserControls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class CollectionsTree : UserControl
    {
        public BindingList<CollectionHolder> Collections { get; set; }

        public CollectionsTree()
        {
            InitializeComponent();
            Collections = new BindingList<CollectionHolder>();
            TreeViewCollections.ItemsSource = Collections;
        }
    }
}
