using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
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
                    ReadCollectionsDB();
                }
                return _collections;
            }
        }

        private static void ReadCollectionsDB()
        {
            _collections = new List<Collection>();
            try
            {
                using (var r = new CustomReader(File.OpenRead(Preferences.CollectionsDBPath)))
                {
                    _osuVersion = r.ReadInt32();
                    var amount = r.ReadInt32();
                    for (var i = 0; i < amount; i++)
                    {
                        var name = r.ReadString();
                        var size = r.ReadInt32();
                        var beatmaps = new List<Beatmap>(size);
                        for (var j = 0; j < size; j++)
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

        public static void WriteCollectionsDB()
        {
            try
            {
                using (var w = new CustomWriter(File.Open(Preferences.CollectionsDBPath, FileMode.Create)))
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

        //TODO: make it write to file or automatically upload somewhere
        public static string WriteToString(CollectionsFile fileModel)
        {
            var stream1 = new MemoryStream();
            var ser = new DataContractJsonSerializer(typeof(CollectionsFile));
            ser.WriteObject(stream1, fileModel);
            stream1.Position = 0;
            var sr = new StreamReader(stream1);
            return sr.ReadToEnd();
        }
    }
}