﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Windows.Forms;
using Ookii.Dialogs.Wpf;

namespace osu_collection_manager.Utils
{
    public class FileUtils
    {
        private const string PASSWORD_HASH = "YouSHallNotPass";
        private const string SALT_KEY = "SaltyDAB!!";
        private const string VI_KEY = "SsdfsddfDfdfdfd8";

        public static string Encrypt(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            var keyBytes = new Rfc2898DeriveBytes(PASSWORD_HASH, Encoding.ASCII.GetBytes(SALT_KEY)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VI_KEY));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string Decrypt(string encryptedText)
        {
            var cipherTextBytes = Convert.FromBase64String(encryptedText);
            var keyBytes = new Rfc2898DeriveBytes(PASSWORD_HASH, Encoding.ASCII.GetBytes(SALT_KEY)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VI_KEY));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            var plainTextBytes = new byte[cipherTextBytes.Length];

            var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }

        public static bool FolderToOsz()
        {
            VistaFolderBrowserDialog browseFolder = new VistaFolderBrowserDialog();
            browseFolder.ShowDialog();
            if (Directory.GetFiles(browseFolder.SelectedPath).Length == 0)
            {
                MessageBox.Show("This folder does not contain any files!");
                return false;
            }
            else if (Directory.GetFiles(browseFolder.SelectedPath, "*.osu").Length == 0)
            {
                MessageBox.Show("Not a valid folder! A valid folder requires at least 1 .osu file!");
                return false;
            }
            else
            {
                VistaSaveFileDialog saveFile = new VistaSaveFileDialog();
                saveFile.FileName = new DirectoryInfo(browseFolder.SelectedPath).Name;
                saveFile.Title = "Save your .osz file";
                ZipFile.CreateFromDirectory(browseFolder.SelectedPath, $"{saveFile.FileName}.osz");
                return true;
            }
        }
    }
}
