using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_collection_manager
{
    public class Preferences
    {
        //public static string OsuInstallationPath = $@"C:\Users\{Environment.UserName}\AppData\Local\osu!\";
        public static string OsuInstallationPath = $@"G:\Games\OsuTest\";

        public static string CollectionsDBPath { get { return OsuInstallationPath + "collection.db"; } }
        public static string OsuDBPath { get { return OsuInstallationPath + "osu!.db"; } }
    }
}
