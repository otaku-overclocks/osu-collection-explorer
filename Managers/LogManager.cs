using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_collection_manager.Managers
{
    public class LogManager
    {
        public const string ERROR_TAG = "[ERROR] ";
        public const string INFO_TAG = "[INFO] ";

        public static readonly string LogPath = $"{System.Environment.CurrentDirectory}/log.txt";

        public static StreamWriter Writer { get; set; }

        public static void Write(Exception e)
        {
           Write(e.ToString(), ERROR_TAG);
        }

        public static void Write(string msg, string tag = INFO_TAG)
        {
            var close = Writer == null; //If already opened keep open; Means we want to write multiple lines
            if (close) //If closed open a new one in append mode
            {
                Open();
            }
            Writer.WriteLine($"{tag} {msg}"); //Write with tag perpended
            if (close) Close();
        }

        public static void Open()
        {
            Writer = new StreamWriter(LogPath, true);
        }

        public static void Close()
        {
            Writer.Close();
        }
    }
}
