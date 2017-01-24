using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Management;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

/// <summary>
/// Class Abilities: Check if osu! is currently running, if yes under which process.
/// Furthermore it gets the file path of the first detected process and writes it into the config.
/// This allows the user to open osu! from the tool's GUI
/// </summary>

namespace osu_collection_manager.Managers
{
    public class OsuInstanceManager
    {
        /// <summary>
        /// This method returns the amount of running osu! instances.
        /// </summary>
        /// <returns>
        /// 0 if no osu! process is found
        /// the amount of osu! processes running if one or multiple osu! processes are found
        /// -1 in case anything else happens.
        /// </returns>
        public int getOsuInstanceCount()
        {
            Process[] pname = Process.GetProcessesByName("osu!.exe");
            string path;
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (pname.Length == 0)
            {
                return 0;
            }
            else if (pname.Length == 1)
            {
                try
                {
                    path = pname[0].MainModule.FileName;
                    OsuFilePath osuPath = new OsuFilePath();
                    osuPath.osuExePath = path;
                    //This code block (repeated in case pname.Length >1) first adds the path string as value to the
                    //OsuExecutablePath key, then saves the configuration and finally reloads the configuration into the program.
                    config.AppSettings.Settings.Add("OsuExecutablePath", osuPath.osuExePath);
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                }
                catch
                {
                    MessageBox.Show("Unable to receive osu! process path. Try running as an administrator.");
                }
                return 1;
            }
            else if (pname.Length > 1)
            {
                try
                {
                    path = pname[0].MainModule.FileName;
                    OsuFilePath osuPath = new OsuFilePath();
                    osuPath.osuExePath = path;
                    //Since in this case multiple instances of osu! are running, we can near-safely assume,
                    //that the user is running the tournament client.
                    config.AppSettings.Settings.Add("OsuTExecutablePath", osuPath.osuExePath);
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                }
                catch
                {
                    MessageBox.Show("Unable to receive osu! process path. Try running as an administrator.");
                }
                return pname.Length;
            }
            else
            {
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
        public string getOsuExecutablePath()
        {
            foreach (string key in ConfigurationManager.AppSettings)
            {
                string value = ConfigurationManager.AppSettings[key];
                if (key == "OsuExecutablePath")
                {
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
        public string getOsuTExecutablePath()
        {
            foreach (string key in ConfigurationManager.AppSettings)
            {
                string value = ConfigurationManager.AppSettings[key];
                if (key == "OsuTExecutablePath")
                {
                    return value;
                }
            }
            throw new System.ArgumentException("Configuration key \"OsuTExecutablePath\" does not exist."
                                               + "\n Either you messed with the configuration file"
                                               + "\n or you have got the program from a non-original source.");
            //return "Error"; //Unnecessary piece of code. Kept for clarity.
        }

        // adjusted copypaste of getOsuInstanceCount
        public object GetPathOfRunningOsu()
        {
            var pname = Process.GetProcessesByName("osu!.exe");
            if (pname.Length != 0) // Processes found
            {
                try
                {
                    return pname[0].MainModule.FileName; // Give path of the process
                }
                catch
                {
                    MessageBox.Show("Unable to receive osu! process path. Try running as an administrator.");
                }
            }
            return null; // None found or unable to recieve path.
        }

        /// <summary>
        /// Gets osu installation path from registry
        /// </summary>
        /// <returns></returns>
        public static string GetPathFromRegistry()
        {
            //Get windows's uninstall id which is assigned to osu!
            var uninstallId = Registry.GetValue(Constants.REG_KEY_OSU_UNINSTALLID, Constants.REG_VALUE_OSU_UNINSTALLID, null);
            if (uninstallId == null) return null; // None found, return null

            var uninstallKeyAdress = $"{{{uninstallId}}}"; // Give it a proper format; {id}
            // This is a simple hack since this is set to a file; An icon to be precise.  This icon is luckily located in installation path
            // We check with ?? both registry paths 32 bit and 64 bit. If one of them isnt null we set it to it
            var displayIconPath = (string) 
                (Registry.GetValue(Constants.REG_KEY_OSU_32_UNINSTALLLOC + uninstallKeyAdress, Constants.REG_VALUE_OSU_UNINSTALLLOC, null) 
                     ??
                 Registry.GetValue(Constants.REG_KEY_OSU_64_UNINSTALLLOC + uninstallKeyAdress,  Constants.REG_VALUE_OSU_UNINSTALLLOC, null));
            return Path.GetDirectoryName(displayIconPath); // None found btw, then path of null is null
        }
    }
}