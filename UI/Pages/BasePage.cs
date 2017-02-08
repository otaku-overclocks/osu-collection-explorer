using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using osu_collection_manager.UI.Windows;

namespace osu_collection_manager.UI.Pages
{
    public abstract class BasePage : Page
    {
        protected BaseNavigationWindow MainWindow => (BaseNavigationWindow)Window.GetWindow(this);

        public BasePage()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
           // _mainWindow.DisplayLoadingOverlay(false);
        }

        

    }
}
