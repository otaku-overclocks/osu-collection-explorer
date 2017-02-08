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

namespace osu_collection_manager.UI.Pages.Modals
{
    /// <summary>
    /// Interaction logic for AskLogin.xaml
    /// </summary>
    public partial class AskLogin : BaseModal
    {
        public AskLogin() {
            InitializeComponent();
            Properties.Settings.Default.ShownLogin = true;
            Properties.Settings.Default.Save();
        }

        private void Login_OnClick(object sender, RoutedEventArgs e) {
            MainWindow.OpenDialog(new LoginModal());
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e) {
            Close(ModalFinishType.Cancelled);
        }
    }
}
