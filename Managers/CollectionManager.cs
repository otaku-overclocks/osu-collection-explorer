using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu_collection_manager.Models;
using osu_database_reader;
using Collection = osu_collection_manager.Models.Collection;

namespace osu_collection_manager.Managers
{
    public class CollectionManager
    {
        private static int _osuVersion;

        private static List<Collection> _collections;

        public static List<Collection> Collections
        {
            get
            {
                if (_collections == null)
                {
                    ReadCollections();
                }
                return _collections;
            }
        }

        private static void ReadCollections()
        {
            _collections = new List<Collection>();
            try
            {
                using (CustomReader r = new CustomReader(File.OpenRead(Preferences.CollectionsDBPath)))
                {
                    _osuVersion = r.ReadInt32();
                    int amount = r.ReadInt32();
                    for (int i = 0; i < amount; i++)
                    {
                        var name = r.ReadString();
                        int size = r.ReadInt32();
                        var beatmaps = new List<Beatmap>(size);
                        for (int j = 0; j < size; j++)
                        {
                            var map = LocalSongManager.FindByHash(r.ReadString());
                            if (map != null) beatmaps.Add(new Beatmap(map));
                        }
                        _collections.Add(new Collection(name, beatmaps));
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Fail(e.Message);
            }
        }

        public static void WriteCollections()
        {
            try
            {
                using (CustomWriter w = new CustomWriter(File.Open(Preferences.CollectionsDBPath, FileMode.Create)))
                {
                    w.Write(_osuVersion);
                    w.Write(_collections.Count);
                    foreach (var collection in _collections)
                    {
                        w.Write(collection.Name);
                        w.Write(collection.MapSets.Count);
                        foreach (var mapSet in collection.MapSets)
                        {
                            foreach (var map in mapSet.Maps)
                            {
                                w.Write(map.MD5Hash);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Fail(e.Message);
            }
        }
    }
}