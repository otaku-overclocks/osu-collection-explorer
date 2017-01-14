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
        private static OsuDb _localSongs;

        public static OsuDb LocalSongs
        {
            get
            {
                if (_localSongs == null)
                {
                    _localSongs = OsuDb.Read(Preferences.OsuDBPath);
                }
                return _localSongs;
            }
        }

        public static string OsuLocation; //TODO: preferences

        public static BeatmapEntry FindByHash(string hash)
        {
            return LocalSongs.Beatmaps.Find(map => map.BeatmapChecksum.Equals(hash));
        }

        [Obsolete]
        public static List<BeatmapEntry> FindAllBySetID(int id)
        {
            return LocalSongs.Beatmaps.FindAll(map => map.BeatmapSetId.Equals(id));
        }

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