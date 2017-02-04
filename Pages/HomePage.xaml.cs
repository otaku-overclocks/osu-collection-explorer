using Microsoft.Win32;
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


namespace osu_collection_manager.Pages
{
    /// <summary>
    /// Logique d'interaction pour HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void linkOsu_Click(object sender, RoutedEventArgs e)
        {
            string osupath = null;
            Process[] pnames = new Process[] { null };
            // Process[] pnames = Common.LoadOsuExe() as Process[];
            if (pnames==null)
            {
                // osu! hasn't been found
                OpenFileDialog openfiledialog = new OpenFileDialog(){ Multiselect = false, Filter = "osu!.exe|osu!.exe"};
                bool? result = openfiledialog.ShowDialog();
                if (result == true)
                {
                    osupath = openfiledialog.FileName;
                }
            }
            else
            {
                // osu! has been found, store first path
                osupath = pnames[0].MainModule.FileName;
            }
            if (osupath != null)
            {
                link_hint.Content = "Current installation is located at " + osupath;
            }
        }
    }
}
