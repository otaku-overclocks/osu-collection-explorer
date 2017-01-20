using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Management;
using System.Configuration;
using System.Windows.Forms;

/// <summary>
/// Class Abilities: Check if osu! is currently running, if yes under which process.
/// Furthermore it gets the file path of the first detected process and writes it into the config.
/// This allows the user to open osu! from the tool's GUI
/// </summary>
namespace osu_collection_manager.Managers {
    class OsuInstanceManager {

        /// <summary>
        /// This method returns the amount of running osu! instances.
        /// </summary>
        /// <returns>
        /// 0 if no osu! process is found
        /// the amount of osu! processes running if one or multiple osu! processes are found
        /// -1 in case anything else happens.
        /// </returns>
        public int getOsuInstanceCount() {
            Process[] pname = Process.GetProcessesByName("osu!.exe");
            string path;

            if (pname.Length == 0) {
                return 0;
            } else if (pname.Length == 1) {
                try {
                    path = pname[0].MainModule.FileName;
                    OsuFilePath osuPath = new OsuFilePath();
                    osuPath.osuExePath = path;
                    //TODO Write path to config file
                }
                catch {
                    MessageBox.Show("Unable to receive osu! process path. Try running as an administrator.");
                }
                return 1;
            } else if (pname.Length > 1) {
                try {
                    path = pname[0].MainModule.FileName;
                    OsuFilePath osuPath = new OsuFilePath();
                    osuPath.osuExePath = path;
                    //TODO Write path to config file
                }
                catch {
                    MessageBox.Show("Unable to receive osu! process path. Try running as an administrator.");
                }
                return pname.Length;
            } else {
                return -1;
            }
        }
    }
}
