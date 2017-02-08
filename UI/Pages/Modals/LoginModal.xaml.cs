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
using osu_collection_manager.Utils;
using OsuMapDownload.Providers;

namespace osu_collection_manager.UI.Pages.Modals
{
    /// <summary>
    /// Interaction logic for LoginModal.xaml
    /// </summary>
    public partial class LoginModal : BaseModal
    {

        public LoginModal() {
            InitializeComponent();
        }

        private void Login_OnClick(object sender, RoutedEventArgs e) {
            ErrorMessage.Visibility = Visibility.Collapsed;
            var provider = new OsuDownloadProvider(TbxUsername.Text, TbxPassword.Password, Preferences.CookiesSavePath);
            LoggingInOverlay.Visibility = Visibility.Visible;
            var task = new Task(() => {
                //provider.L
            });
        }

        public void LoginResponse(bool loggedIn) {
            if (!loggedIn) {
                ErrorMessage.Visibility = Visibility.Visible;
                LoggingInOverlay.Visibility = Visibility.Hidden;
                TbxPassword.Password = "";
                return;
            }
            Properties.Settings.Default.Username = TbxUsername.Text;
            Properties.Settings.Default.Password = FileUtils.Encrypt(TbxPassword.Password);
            Properties.Settings.Default.Save();
            MessageBox.Show("Successfully logged in.");
            Close(ModalFinishType.Succes);
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e) {
            Close(ModalFinishType.Cancelled);
        }
    }
}
