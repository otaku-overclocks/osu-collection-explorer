using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace osu_collection_manager.Models
{
    [DataContract]
    public class CollectionsFile
    {
        [DataMember]
        public int Version { get; set; }
        [DataMember]
        public List<Collection> Collections { get; set; }

        public CollectionsFile(List<Collection> collections)
        {
            Version = Preferences.VERSION;
            Collections = collections;
        }

        public CollectionsFile(int version, List<Collection> collections)
        {
            Version = version;
            Collections = collections;
        }

        public void WriteToFile(string path)
        {
            var file = new DataContractJsonSerializer(typeof(CollectionsFile));
            using (var stream = File.Create(path))
            {
                file.WriteObject(stream, this);
            }
        }

        public string WriteToString()
        {
            var stream = new MemoryStream();
            var ser = new DataContractJsonSerializer(typeof(CollectionsFile));
            ser.WriteObject(stream, this);
            stream.Position = 0;
            var sr = new StreamReader(stream);
            return sr.ReadToEnd();
        }

        public static CollectionsFile ReadFromFile(string path)
        {
            var ser = new DataContractJsonSerializer(typeof(CollectionsFile));
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return (CollectionsFile)ser.ReadObject(stream);
            }
        }

        public static CollectionsFile ReadFromString(string json)
        {
            var deserializer = new DataContractJsonSerializer(typeof(CollectionsFile));
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                return (CollectionsFile) deserializer.ReadObject(ms);
            }
        }
    }
}
