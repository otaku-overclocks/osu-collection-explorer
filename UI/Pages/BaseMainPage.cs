using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace osu_collection_manager.UI.Pages
{
    public class BaseMainPage : Page
    {
        protected MainWindow _mainWindow => (MainWindow)Window.GetWindow(this);

        public BaseMainPage()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _mainWindow.DisplayLoadingOverlay(false);
        }
    }
}
