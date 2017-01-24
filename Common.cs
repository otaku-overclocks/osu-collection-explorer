using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Diagnostics;

// uhh, this file will most likely be used for literally everysingle method in the menubar
// since they'll also most likely have a page where they are used
// tell me if not clear. -jeremiidesu

namespace osu_collection_manager
{
    class Common
    {
        /// <summary>
        /// Searches the memory for a running instance of osu! (ps: i genuinely feel i just made something useless.)
        /// </summary>
        /// <returns>
        /// Returns null if nothing is found running, else returns a Process[] array with all the running instances
        /// </returns>
        static public object[] LoadOsuExe()
        {
            var runningosu = new Managers.OsuInstanceManager().GetPathOfRunningOsu();
            if (runningosu==null)
            {
                // osu! doesn't seem to be running
                // this is never reached
                return null;
            }
            else
            {
                // at least one osu! instance has been found
                // return all the paths and let the caller handle what should be done.
                // eventually move recurring code here and return something else
                return runningosu as Process[];
            }
        }
    }
}
