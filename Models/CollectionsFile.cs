using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
    }
}
