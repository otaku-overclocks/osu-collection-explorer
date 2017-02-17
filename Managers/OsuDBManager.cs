using osu_database_reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_collection_manager.Managers
{
    class OsuDBManager
    {
        public static void ReadCollectionFolder()
        {
            OsuDb mapList = OsuDb.Read(Preferences.OsuDBPath);
        }
    }
}
