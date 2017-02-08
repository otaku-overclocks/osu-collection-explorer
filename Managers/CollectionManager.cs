using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using osu_collection_manager.Models;
using osu_database_reader;
using Collection = osu_collection_manager.Models.Collection;

namespace osu_collection_manager.Managers
{
    public class CollectionManager
    {
        /// <summary>
        /// Version of osu to properly write the .db file. Some properties are different in various versions.
        /// Just store it to be safe. We also need to write it back
        /// </summary>
        private static int _osuVersion;

        /// <summary>
        /// Only privately settable collections list so we dont overwrite it. 
        /// If you really need to empty it and write a list to it through addRange.
        /// </summary>
        private static List<Collection> _collections;

        /// <summary>
        /// Collections instance. If its null it reads collections.db first.
        /// If you really need to empty it and write a list to it through addRange.
        /// </summary>
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

        /// <summary>
        /// Reads collection database and writes it to .Collections
        /// </summary>
        /// <remarks>
        /// It throws some exceptions if file is not found, not readable or does not contain the expected data.
        /// </remarks>  
        public static void ReadCollectionsDB()
        {
            try
            {
#if DEBUG
                LogManager.Write("Reading collections at: " + Preferences.CollectionsDBPath);
#endif
                //Create an empty list
                _collections = new List<Collection>();
                //Open collections.db in our custom binary reader
                using (var r = new CustomReader(File.OpenRead(Preferences.CollectionsDBPath)))
                {
                    //Here we are reading the data
                    //More about it here: https://osu.ppy.sh/wiki/Db_(file_format)#collection.db
                    _osuVersion = r.ReadInt32();
                    var amount = r.ReadInt32();
                    for (var i = 0; i < amount; i++)
                    {
                        var name = r.ReadString();
                        var size = r.ReadInt32();
                        // We dont need the hashes. Instead, we check if osu.db contains some info about them.
                        // If it does we add a List of beatmaps to the collection model. 
                        // Note: There are beatmaps from same set through the list. Sorting happens in collections contructor
                        var beatmaps = new List<Beatmap>(size);
                        for (var j = 0; j < size; j++)
                        {
                            var hash = r.ReadString();
                            var map = LocalSongManager.FindByHash(hash);
                            if (map != null) beatmaps.Add(new Beatmap(map));
                        }
                        _collections.Add(new Collection(name, beatmaps));
                    }
                }
            }
            catch (Exception e)
            {
                //LogManager.Open();
                LogManager.Write($"Failed to read collections.db at {Preferences.CollectionsDBPath}", LogManager.ERROR_TAG);
                LogManager.Write(e);
                //LogManager.Close();
            }
        }

        /// <summary>
        /// Writes .Collections to collections.db
        /// </summary>
        /// <remarks>
        /// It throws some exceptions if directory is not found, not writeable or does not contain the expected data.
        /// </remarks>  
        public static void WriteCollectionsDB()
        {
            try
            {
#if DEBUG
                LogManager.Write("Writing collections at: " + Preferences.CollectionsDBPath);
#endif
                //Open collections.db in our custom binary writer
                using (var w = new CustomWriter(File.Open(Preferences.CollectionsDBPath, FileMode.Create)))
                {
                    //Here we are writing the data
                    //More about it here: https://osu.ppy.sh/wiki/Db_(file_format)#collection.db
                    w.Write(_osuVersion);
                    w.Write(_collections.Count);
                    foreach (var collection in _collections)
                    {
                        w.Write(collection.Name);
                        w.Write(collection.BeatmapCount);
                        // Write hashes from all beatmaps in the hash. Osu does it too
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
                //LogManager.Open();
                LogManager.Write($"Failed to write collections.db at {Preferences.CollectionsDBPath}", LogManager.ERROR_TAG);
                LogManager.Write(e);
                //LogManager.Close();
            }
        }

        /// <summary>
        /// Adds all collections to local collections. Merges if a collectiosn with same name exists
        /// </summary>
        /// <param name="cols"></param>
        public static void AddCollections(IEnumerable<Collection> cols)
        {
            foreach (var collection in cols)
            {
                //Check for existing collections with same name
                var existing = Collections.Find(col => col.Name.Equals(collection.Name));
                if (existing != null)
                {
                    //Check for existing mapsets so we dont place duplicates
                    foreach (var mapSet in collection.MapSets)
                    {
                        if (mapSet.Maps.Count == 0) continue;
                        if (existing.MapSets.Find(set => set.SetID.Equals(mapSet.SetID)) == null)
                            existing.MapSets.Add(mapSet);
                    }
                    continue;
                }
                Collections.Add(collection);
            }
        }
    }
}