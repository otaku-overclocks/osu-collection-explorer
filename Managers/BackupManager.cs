using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_collection_manager.Managers
{
    public class BackupManager
    {
        public static void BackupCollections()
        {
            CreateBackupPath();
            var backupName = $"collections-{DateTime.Now:dd-MM-yy_H-mm}.db";
            File.Copy(Preferences.CollectionsDBPath, $"{Preferences.BackupsPath}\\{backupName}");
        }

        private static void CreateBackupPath()
        {
            if (!Directory.Exists(Preferences.BackupsPath))
            {
                Directory.CreateDirectory(Preferences.BackupsPath);
            }
        }
    }
}
