using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

// common methods used in multiple places

namespace osu_collection_manager
{
    class Common
    {
        public static string OpenOsuExe()
        {
            string path = "";
            var dialog = new OpenFileDialog() { Multiselect = false, Filter = "osu!.exe|osu!.exe" };
            var result = dialog.ShowDialog();
            if (result == true) path = Path.GetDirectoryName(dialog.FileName);
            return path;
        }
    }
}
