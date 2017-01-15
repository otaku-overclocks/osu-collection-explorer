using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu_collection_manager.Models;

namespace osu_collection_manager.Managers
{
    public class ImportExportManager
    {
        /// <summary>
        /// Check a list of collections if they contain missing mapsets. The missing ones should be downloaded manually.
        /// </summary>
        /// <param name="collections"></param>
        /// <returns>List with missing mapsets</returns>
        public static List<MapSet> CheckMissingMapSets(List<Collection> collections)
        {
            var ret = new List<MapSet>();
            foreach (var collection in collections)
            {
                foreach (var mapSet in collection.MapSets)
                {
                    if (LocalSongManager.FindSetByID(mapSet.SetID) == null)
                        ret.Add(mapSet);
                }
            }
            return ret;
        }
    }
}
