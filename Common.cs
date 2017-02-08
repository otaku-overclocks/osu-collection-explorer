using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using osu_collection_manager.Models;

// common methods used in multiple places

namespace osu_collection_manager
{
    public class Common
    {
        public static string OpenOsuExe()
        {
            string path = null;
            var dialog = new OpenFileDialog() { Multiselect = false, Filter = "osu!.exe|osu!.exe"};
            var result = dialog.ShowDialog();
            if (result == true) path = Path.GetDirectoryName(dialog.FileName);
            return path;
        }
        public static bool IsOsuOpen()
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Contains("osu!"))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
