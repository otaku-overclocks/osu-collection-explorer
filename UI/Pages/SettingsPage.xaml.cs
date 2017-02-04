﻿using System;
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

namespace osu_collection_manager.UI.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : BasePage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void SettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            osu_folder_path.Text = Properties.Settings.Default.OsuPath;
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.OsuPath = osu_folder_path.Text;
            Properties.Settings.Default.Save();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string path = Common.OpenOsuExe();
            osu_folder_path.Text = path;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SaveSettings();
            MessageBox.Show("Saved!");
        }

        private void goToHome(object sender, RoutedEventArgs e)
        {
            MainWindow.OpenPage(null);
        }
    }
}