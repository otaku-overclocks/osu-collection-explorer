

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
using OsuMapDownload.Models;

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

        [TestMethod]
        public void TestAsyncDownload()
        {
            CreateTempDlPath();
            var download = new MapSetDownload("http://bloodcat.com/osu/s/138554", Preferences.DownloadsPath);
            var task = download.CreateTask(Preferences.SongsPath);
            task.Start();
            while (!task.IsCompleted)
            {
                Debug.WriteLine(download.Progress + " with speed " + download.Speed);
                Thread.Sleep(100);
            }
            Debug.WriteLine(download.Completed);
            Debug.WriteLine(download.Failed);
        }

        [TestMethod]
        public void TestAsyncDownloadMultiple()
        {
            CreateTempDlPath();
            var download = new MapSetDownload("http://bloodcat.com/osu/s/138554", Preferences.DownloadsPath);
            var task = download.CreateTask(Preferences.SongsPath);
            var download2 = new MapSetDownload("http://bloodcat.com/osu/s/553711", Preferences.DownloadsPath);
            var task2 = download2.CreateTask(Preferences.SongsPath);
            task.Start();
            task2.Start();
            while (!task.IsCompleted || !task2.IsCompleted)
            {
                Debug.WriteLine($"Download 1 - Progress: {download.Progress} Speed: {download.Speed} kb/s");
                Debug.WriteLine($"Download 2 - Progress: {download2.Progress} Speed: {download2.Speed} kb/s");
                Thread.Sleep(100);
            }
            Debug.WriteLine($"Download 1 Completed: {download.Completed} or Failed: {download.Failed}");
            Debug.WriteLine($"Download 2 Completed: {download2.Completed} or Failed: {download2.Failed}");
        }

        private static void CreateTempDlPath()
        {
            if (!Directory.Exists(Preferences.DownloadsPath))
            {
                var di = Directory.CreateDirectory(Preferences.DownloadsPath);
            }
        }

        [TestMethod]
        public void TestLog()
        {
            LogManager.Write("Hello world!");
            Debug.WriteLine(LogManager.LogPath);
        }
    }
}