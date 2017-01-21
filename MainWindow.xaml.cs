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
using osu_collection_manager.Managers;
using osu_collection_manager.UI.Pages;


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
            new Task(() =>
            {
                var col = CollectionManager.Collections; // First time read.
            }).Start();
           
        }

        // BEGIN menubar code

        private void goToTestPage_Click(object sender, RoutedEventArgs e)
        {
            WindowContent.Content = new Pages.RandomTestsPage();
        }

        // END menubar code
        private void NewMapPack_OnClick(object sender, RoutedEventArgs e)
        {
            OpenPage(new Pages.SelectCollectionsPage(CollectionManager.Collections));
        }

        public void DisplayLoadingOverlay(bool display)
        {
            LoadingOverlay.Visibility = display ? Visibility.Visible : Visibility.Hidden;
        }

        public void OpenPage(BaseMainPage page)
        {
            DisplayLoadingOverlay(true);
            WindowContent.Content = page;
        }
    }
}
