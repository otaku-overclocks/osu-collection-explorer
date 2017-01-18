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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace osu_collection_manager.Pages
{
    /// <summary>
    /// Logique d'interaction pour RandomTestsPage.xaml
    /// </summary>
    public partial class RandomTestsPage : Page
    {
        public RandomTestsPage()
        {
            InitializeComponent();
        }

        private void think_running_Click(object sender, RoutedEventArgs e)
        {
            circle.Source = new ImageSourceConverter().ConvertFromString("pack://application:,,,/osu-collection-manager;component/Images/osu-outside circle-pink.png") as ImageSource;
            logo.Source = new ImageSourceConverter().ConvertFromString("pack://application:,,,/osu-collection-manager;component/Images/osu-full logo-pink.png") as ImageSource;
            ossicon.Opacity = 1;
        }

        private void think_unlinked_Click(object sender, RoutedEventArgs e)
        {
            circle.Source = new ImageSourceConverter().ConvertFromString("pack://application:,,,/osu-collection-manager;component/Images/osu-outside circle.png") as ImageSource;
            logo.Source = new ImageSourceConverter().ConvertFromString("pack://application:,,,/osu-collection-manager;component/Images/osu-full logo.png") as ImageSource;
            ossicon.Opacity = 0.5;
        }

        private void think_linked_Click(object sender, RoutedEventArgs e)
        {
            circle.Source = new ImageSourceConverter().ConvertFromString("pack://application:,,,/osu-collection-manager;component/Images/osu-outside circle.png") as ImageSource;
            logo.Source = new ImageSourceConverter().ConvertFromString("pack://application:,,,/osu-collection-manager;component/Images/osu-full logo.png") as ImageSource;
            ossicon.Opacity = 1;
        }
    }
}
