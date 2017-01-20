using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace osu_collection_manager
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var wind = new TestCollectionSelectWindow();
            wind.Show();
        }

        // BEGIN menubar code

        private void goToTestPage_Click(object sender, RoutedEventArgs e)
        {
            WindowContent.Content = new Pages.RandomTestsPage();
        }

        // END menubar code
    }
}
