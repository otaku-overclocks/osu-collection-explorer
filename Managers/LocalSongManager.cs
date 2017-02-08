using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu_collection_manager.Models;
using osu_database_reader;

namespace osu_collection_manager.Managers
{
    public class LocalSongManager
    {
        // OsuDb instance which contains the info for all the maps
        private static OsuDb _localSongs;
        // Its readonly so we dont overwrite it
        public static OsuDb LocalSongs
        {
            get
            {
                if (_localSongs == null)
                {
                    ReadFromDb();
                }
                return _localSongs;
            }
        }

        public static void ReadFromDb()
        {
            try
            {
                _localSongs = OsuDb.Read(Preferences.OsuDBPath);
            }
            catch (Exception e)
            {
                //LogManager.Open();
                LogManager.Write($"Failed to read osu.db at {Preferences.OsuDBPath}", LogManager.ERROR_TAG);
                LogManager.Write(e);
                //LogManager.Close();
            }
        }

        /// <summary>
        /// Finds a beatmap entry for given hash.
        /// </summary>
        /// <param name="hash">An md5 hash of a .osu beatmap</param>
        /// <returns>Null if not found, otherwise a beatmap entry</returns>
        public static BeatmapEntry FindByHash(string hash)
        {
            return LocalSongs.Beatmaps.Find(map => map.BeatmapChecksum.Equals(hash));
        }


        [Obsolete]
        public static List<BeatmapEntry> FindAllBySetID(int id)
        {
            return LocalSongs.Beatmaps.FindAll(map => map.BeatmapSetId.Equals(id));
        }

        /// <summary>
        /// Finds a Mapset by its set id with all the beatmaps inside.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>null or a MapSet</returns>
        public static MapSet FindSetByID(int id)
        {
            var maps = LocalSongs.Beatmaps.FindAll(map => map.BeatmapSetId.Equals(id));
            if (maps.Count == 0) return null;
            var beatmaps = new List<Beatmap>();
            foreach (var map in maps)
            {
                beatmaps.Add(new Beatmap(map));
            }
            return new MapSet(beatmaps);
        }
    }
}