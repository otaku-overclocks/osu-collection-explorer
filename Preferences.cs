using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_collection_manager
{
    public class Preferences
    {
        public static readonly int VERSION = 1;
        
        //public static string OsuInstallationPath = $@"C:\Users\{Environment.UserName}\AppData\Local\osu!";
        public static string OsuInstallationPath = $@"G:\Games\OsuTest";
        public static string CollectionsDBPath { get { return OsuInstallationPath + "\\collection.db"; } }
        public static string OsuDBPath { get { return OsuInstallationPath + "\\osu!.db"; } }
        public static string SongsPath { get { return OsuInstallationPath + "\\Songs"; } }
        public static string DownloadsPath { get { return OsuInstallationPath + "\\OCMTemp"; } }

        public static int BloodcatThreadCount = 8;
        public static string BloodcatDownloadLink = "http://bloodcat.com/osu/s/";

        public static string OsuPath
        {
            get
            {
                return Properties.Settings.Default.OsuPath;
            }
        }
    }
}
