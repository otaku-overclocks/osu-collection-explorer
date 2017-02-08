using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using osu_collection_manager.UI.Pages.Modals;
using OsuMapDownload;

namespace osu_collection_manager.UI.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : BasePage
    {
        public SettingsPage() {
            InitializeComponent();
            UpdateLoginBtn();
        }

        private void SettingsPage_Loaded(object sender, RoutedEventArgs e) {
            LoadSettings();
        }

        private void LoadSettings() {
            osu_folder_path.Text = Properties.Settings.Default.OsuPath;
            bd_thread_count.Value = Properties.Settings.Default.BloodcatThreadCount;
        }

        private void SaveSettings() {
            Properties.Settings.Default.OsuPath = osu_folder_path.Text;
            Properties.Settings.Default.BloodcatThreadCount = (int) bd_thread_count.Value;
            Properties.Settings.Default.Save();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            string path = Common.OpenOsuExe();
            osu_folder_path.Text = path;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            SaveSettings();
            MessageBox.Show("Saved!");
        }

        private void goToHome(object sender, RoutedEventArgs e) {
            MainWindow.OpenPage(null);
        }

        private void button_Click_2(object sender, RoutedEventArgs e) {
            Properties.Settings.Default.Reset();
            Properties.Settings.Default.OsuPath = Preferences.OsuPath;
            Properties.Settings.Default.BloodcatThreadCount = 4;
            Properties.Settings.Default.ShownWarning = false;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }

        private void Login_OnClick(object sender, RoutedEventArgs e) {
            if (Preferences.LoginDefined) {
                Properties.Settings.Default.Username = "";
                Properties.Settings.Default.Password = "";
                Properties.Settings.Default.Save();
                UpdateLoginBtn();
                return;
            }
            MainWindow.OpenDialog(new LoginModal(type => {
                if (type == ModalFinishType.Succes) {
                    this.Dispatcher.Invoke(UpdateLoginBtn);
                }
            }));
        }

        private void UpdateLoginBtn() {
            if (Preferences.LoginDefined) {
                LoginOpen.Content = "Logout: " + Properties.Settings.Default.Username;
            } else {
                LoginOpen.Content = "Login";
            }
        }
    }
}