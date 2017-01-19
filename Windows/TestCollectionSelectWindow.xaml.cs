using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using osu_collection_manager.Managers;
using osu_collection_manager.UI.UserControls.Models;

namespace osu_collection_manager
{
    /// <summary>
    /// Interaction logic for TestCollectionSelectWindow.xaml
    /// </summary>
    public partial class TestCollectionSelectWindow : Window
    {
        public ObservableCollection<CollectionHolder> Collections { get; set; }

        public TestCollectionSelectWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var cols = CollectionManager.Collections;
           
            foreach (var collection in cols)
            {
                var tn = new CollectionHolder(collection, true);
                test.Items.Add(tn);
            }
        }
    }
}
