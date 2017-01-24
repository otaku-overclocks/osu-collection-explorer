﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace osu_collection_manager
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
            var arglist = "";
            foreach (var arg in e.Args)
            {
                arglist += arg + "\n";
            }
            var path = Preferences.OsuPath;
#if DEBUG
            Debug.WriteLine($"Found osu path: {path}");
#endif
        }
    }
}
