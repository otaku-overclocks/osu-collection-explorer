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
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (pname.Length == 0) {
                return 0;
            } else if (pname.Length == 1) {
                try {
                    path = pname[0].MainModule.FileName;
                    OsuFilePath osuPath = new OsuFilePath();
                    osuPath.osuExePath = path;
                    //This code block (repeated in case pname.Length >1) first adds the path string as value to the
                    //OsuExecutablePath key, then saves the configuration and finally reloads the configuration into the program.
                    config.AppSettings.Settings.Add("OsuExecutablePath", osuPath.osuExePath);
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
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
                    //Since in this case multiple instances of osu! are running, we can near-safely assume,
                    //that the user is running the tournament client.
                    config.AppSettings.Settings.Add("OsuTExecutablePath", osuPath.osuExePath);
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                }
                catch {
                    MessageBox.Show("Unable to receive osu! process path. Try running as an administrator.");
                }
                return pname.Length;
            } else {
                return -1;
            }
        }

        /// <summary>
        /// Iterates through all keys in the configuration file. If it finds an "OsuExecutablePath" it returns the value.
        /// If it doesn't, it throws an ArgumentException.
        /// </summary>
        /// <returns>
        /// value should be a valid path to the osu!.exe - In case of an exception it returns "Error"
        /// ... in case that returning anything is still necessary for that. Better sure than sorry.
        /// </returns>
        public string getOsuExecutablePath() {
            foreach(string key in ConfigurationManager.AppSettings) {
                string value = ConfigurationManager.AppSettings[key];
                if (key == "OsuExecutablePath") {
                    return value;
                }   
            }
            throw new System.ArgumentException("Configuration key \"OsuExecutablePath\" does not exist."
                                                + "\n Either you messed with the configuration file"
                                                + "\n or you have got the program from a non-original source.");
            //return "Error"; //Unnecessary piece of code. Kept for clarity.
        }

        /// <summary>
        /// Iterates through all keys in the configuration file. If it finds an "OsuTExecutablePath" it returns the value.
        /// If it doesn't, it throws an ArgumentException.
        /// </summary>
        /// <returns>
        /// value should be a valid path to the tournament version of osu!.exe - In case of an exception it returns "Error"
        /// </returns>
        public string getOsuTExecutablePath() {
            foreach (string key in ConfigurationManager.AppSettings) {
                string value = ConfigurationManager.AppSettings[key];
                if (key == "OsuTExecutablePath") {
                    return value;
                }
            }
            throw new System.ArgumentException("Configuration key \"OsuTExecutablePath\" does not exist."
                                                + "\n Either you messed with the configuration file"
                                                + "\n or you have got the program from a non-original source.");
            //return "Error"; //Unnecessary piece of code. Kept for clarity.
        }
    }
}
