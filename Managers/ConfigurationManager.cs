using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Xml;
using System.IO;

/// <summary>
/// Custom Configuration Manager for different kinds of Variables that are needed upon startup
/// </summary>
namespace osu_collection_manager.Managers {
    class ConfigurationManager : ConfigurationSection {

        [ConfigurationProperty("osuFilePath")]
        public OsuFilePath OsuPath {
            get {
                return (OsuFilePath)this["osuFilePath"];
            }
            set {
                this["osuFilePath"] = value;
            }       
        }
    }

    public class OsuFilePath : ConfigurationElement {
        [ConfigurationProperty("path", DefaultValue = null, IsRequired = false)]
        [StringValidator(InvalidCharacters = "<>\"|?*")]
        public string osuExePath {
            get {
                return (string)this["path"];
            }
            set {
                this["path"] = value;
            }
        }
    }
}
