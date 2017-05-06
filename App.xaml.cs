using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using osu_collection_manager.Managers;
using OsuMapDownload;
using System.Net.Http;

namespace osu_collection_manager
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        public  static readonly HttpClient client = new HttpClient();
        private void App_Startup(object sender, StartupEventArgs e)
        {
#if DEBUG
            // debug stuff
            var arglist = "";
            foreach (var arg in e.Args)
            {
                arglist += arg + "\n";
            }
            if (arglist == "")
                MessageBox.Show("No args passed");
            else
                MessageBox.Show(arglist);
            var path = Preferences.OsuPath;
            Debug.WriteLine($"Found osu path: {path}");
#endif
            // actual stuff
            // nothing yet but it's gonna go here
        }
    }
}
