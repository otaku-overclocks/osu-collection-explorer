
using System.Windows;
using osu_collection_manager.UI.Pages;

namespace osu_collection_manager.UI.Windows
{
    /// <summary>
    /// Interaction logic for CEMainWindow.xaml
    /// </summary>
    public partial class CEMainWindow : BaseNavigationWindow
    {
        public CEMainWindow()
        {
            InitializeComponent();
            OpenPage(new MainMenuPage());
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Maximize(object sender, RoutedEventArgs e)
        {
            if (WindowState.Equals(WindowState.Maximized))
            {
                WindowState = WindowState.Normal;
                return;
            }
            WindowState = WindowState.Maximized;
        }

        private void Minimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        public override void OpenPage(BasePage page)
        {
            WindowContent.Content = page;
        }
    }
}
