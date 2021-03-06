﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using osu_collection_manager.Managers;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace osu_collection_manager    
{
    public class Preferences
    {
    
        public static readonly int VERSION = 1;
        public static string CollectionsDBPath { get; set; } = OsuPath + "\\collection.db";
        public static string OsuDBPath { get { return OsuPath + "\\osu!.db"; } }
        public static string SongsPath { get { return OsuPath + "\\Songs"; } }
        public static string DownloadsPath { get { return OsuPath + "\\OCMTemp"; } }
        public static string BackupsPath { get { return OsuPath + "\\Backup"; } }
        public static string CookiesSavePath => Application.UserAppDataPath + "\\cookies.txt";

        public static bool LoginDefined => !string.IsNullOrEmpty(Properties.Settings.Default.Username) && 
            !string.IsNullOrEmpty(Properties.Settings.Default.Password);

        public static int BloodcatThreadCount
        {
            get
            {
                return Properties.Settings.Default.BloodcatThreadCount;
            }
            set
            {
                Properties.Settings.Default.BloodcatThreadCount = value;
                Properties.Settings.Default.Save();
            }
        }
        public static string BloodcatDownloadLink = "http://bloodcat.com/osu/s/";

        public const string COLLECTION_FORMAT = ".osc";

        public static string OsuPath
        {
            get
            {
                var ret = Properties.Settings.Default.OsuPath;
                if (ret != null && !ret.Equals(string.Empty)) return ret;
                ret = OsuInstanceManager.GetPathFromRegistry();
                if (ret == null || !File.Exists($"{ret}\\osu!.exe"))
                {
                    ret = Common.OpenOsuExe();
                }
                OsuPath = ret;
                return ret;
            }
            set
            {
                Properties.Settings.Default.OsuPath = value;
                Properties.Settings.Default.Save();
            }
        }
    }
}
