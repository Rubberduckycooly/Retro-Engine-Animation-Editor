using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AnimationEditor.Services
{
    public static class PathService
    {
        public static string GetAppDataPath()
        {
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\RSDK Animation Editor")) Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\RSDK Animation Editor");
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\RSDK Animation Editor";
        }

        public static string GetSettingsPath()
        {
            return GetAppDataPath() + "settings.json";
        }

        public static string GetShortcutsPath()
        {
            if (!Directory.Exists(GetAppDataPath() + "\\Workspaces")) Directory.CreateDirectory(GetAppDataPath() + "\\Workspaces");
            return GetAppDataPath() + "\\Workspaces";
        }
    }
}
