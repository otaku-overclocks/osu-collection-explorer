

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using osu_collection_manager;
using osu_collection_manager.Managers;
using osu_collection_manager.Models;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestReadCollections()
        {
            var collections = CollectionManager.Collections;
           LogCollections(collections);
        }

        [TestMethod]
        public void TestCollectionSerialize()
        {
            var collections = CollectionManager.Collections;
            var file = new CollectionsFile(collections);
            Console.WriteLine(file.WriteToString());
        }

        [TestMethod]
        public void TestCollectionsToFile()
        {
            var collections = CollectionManager.Collections;
            var file = new CollectionsFile(collections);
            file.WriteToFile( Preferences.OsuInstallationPath + "\\export.txt");
        }

        [TestMethod]
        public void TestCollectionsFromFile()
        {
            var file = CollectionsFile.ReadFromFile(Preferences.OsuInstallationPath + "\\export.txt");
            var collections = file.Collections;
            LogCollections(collections);
        }

        public void LogCollections(List<Collection> collections)
        {
            foreach (var collection in collections)
            {
                Debug.WriteLine($"Collection: {collection.Name}");
                foreach (var set in collection.MapSets)
                {
                    Debug.WriteLine($"Mapset: {set.Artist} - {set.Title}; Set ID {set.SetID}");
                    if(set.Maps != null)
                    foreach (var beatmap in set.Maps)
                    {
                        Debug.WriteLine($"Beatmap: {beatmap.Difficulty}; Hash {beatmap.MD5Hash}");
                    }
                }
            }
        }

        [TestMethod]
        public void TestYeahBoi()
        {
            while (true)
            {
                Debug.Write("i");
            }
        }
    }
}