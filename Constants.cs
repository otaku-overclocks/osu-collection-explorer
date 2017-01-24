using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_collection_manager
{
    public static class Constants
    {
        public const string REG_KEY_OSU_UNINSTALLID = @"HKEY_CURRENT_USER\SOFTWARE\osu!";
        public const string REG_VALUE_OSU_UNINSTALLID = "UninstallID";

        public const string REG_KEY_OSU_64_UNINSTALLLOC =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\";

        public const string REG_KEY_OSU_32_UNINSTALLLOC =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\";

        public const string REG_VALUE_OSU_UNINSTALLLOC = @"DisplayIcon";
    }
}
