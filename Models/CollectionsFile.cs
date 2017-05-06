using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace osu_collection_manager.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CollectionsFile
    {
        /// <summary>
        /// Version of our program. To be able to change this and still be able to read old files
        /// </summary>
        [JsonProperty(PropertyName = "version")]
        public int Version { get; set; }

        [JsonProperty(PropertyName = "collections")]
        public List<Collection> Collections { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; } = "Untitled collection pack";

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
          
        /// <summary>
        /// Write everything that is in this model to a file in json format.
        /// </summary>
        /// <param name="path"></param>
        public void WriteToFile(string path)
        {            
            //// Serialize to json
            //var file = new DataContractJsonSerializer(typeof(CollectionsFile));
            ////Write to a file
            //using (var stream = File.Create(path))
            //{
            //    file.WriteObject(stream, this);
            //}
            string json = JsonConvert.SerializeObject(this);
            using (StreamWriter sr = new StreamWriter(path, true))
            {
                sr.WriteLine(json);
            }
        }

        /// <summary>
        /// Write everything that is in this model to a string in json format.
        /// </summary>
        /// <returns></returns>
        public string WriteToString()
        {
            ////Write to memory
            //var stream = new MemoryStream();
            //var ser = new DataContractJsonSerializer(typeof(CollectionsFile));
            //ser.WriteObject(stream, this);
            //stream.Position = 0;
            //var sr = new StreamReader(stream);
            ////Read the whole string from memory
            //return sr.ReadToEnd();

            // well, that was easy
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Deserialize CollectionsFile form a file with json contents
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static CollectionsFile ReadFromFile(string path)
        {
            //var ser = new DataContractJsonSerializer(typeof(CollectionsFile));
            //    using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            //    {
            //        return (CollectionsFile)ser.ReadObject(stream);
            //    }

            
            using (StreamReader sr = new StreamReader(path))
            {
                string fileContents = sr.ReadToEnd();
                return JsonConvert.DeserializeObject<CollectionsFile>(fileContents);
            }
        }

        /// <summary>
        /// Deserialize CollectionsFile form a string with json contents
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static CollectionsFile ReadFromString(string json)
        {
            //var deserializer = new DataContractJsonSerializer(typeof(CollectionsFile));
            //using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            //{
            //    return (CollectionsFile) deserializer.ReadObject(ms);
            //}
            return JsonConvert.DeserializeObject<CollectionsFile>(json);
        }
    }
}