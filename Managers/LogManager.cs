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

        /// <summary>
        /// Save the log in the same path as the program
        /// </summary>
        public static readonly string LogPath = $"{Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)}/log.txt";

        private static StreamWriter _writer { get; set; }

        /// <summary>
        /// Writes an error to log with default error tag
        /// </summary>
        /// <param name="e"></param>
        public static void Write(Exception e)
        {
           Write(e.ToString(), ERROR_TAG); //Write as a string with custom tag
        }

        /// <summary>
        /// Writes a string to log with a cutom tag.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="tag"></param>
        public static void Write(string msg, string tag = INFO_TAG)
        {
            var close = (_writer == null); //If already opened keep open; Means we want to write multiple lines
            if (close) 
            {
                Open(); //If closed open a new one 
            }
            _writer.WriteLine($"[{DateTime.Now:dd/MM/yy H:mm:ss}] {tag} {msg}"); //Write with tag perpended
            if (close) Close();
        }

        /// <summary>
        /// Opens a new streamwriter. Use it if you need to write multiple log entries in a short time.
        /// </summary>
        public static void Open()
        {
            _writer = new StreamWriter(LogPath, true); // Open in append mode
        }

        /// <summary>
        /// Closes current streamwriter
        /// </summary>
        public static void Close()
        {
            if (_writer == null) return;
            _writer.Close(); // Close the stream
            _writer = null;
        }
    }
}
