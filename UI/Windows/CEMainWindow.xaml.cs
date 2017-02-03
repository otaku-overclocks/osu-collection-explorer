
using System.CodeDom;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using osu_collection_manager.UI.Pages;
using osu_collection_manager.UI.Pages.Modals;

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

        public sealed override void OpenPage(BasePage page)
        {
            if(page == null) page = new MainMenuPage();
            WindowContent.Content = page;
        }

        public override void OpenModal(BaseModal dialog)
        {
            if (dialog == null)
            {
                ModalContent.Content = null;
                ModalOverlay.Visibility = Visibility.Hidden;
                return;
            }
          
            ModalContent.Content = dialog;
            ModalOverlay.Visibility = Visibility.Visible;
        }

    }
}
