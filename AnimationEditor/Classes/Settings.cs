using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace AnimationEditor.Classes
{
    public static class Settings
    {
        private static string FilePath { get => GetFilePath(); }

        private static string GetFilePath()
        {
            return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\settings.json";
        }
        public class Instance
        {
            public List<RecentFile> RecentFiles { get; set; } = new List<RecentFile>();

            public class RecentFile
            {
                public string Name { get; set; }
                public string FilePath { get; set; }
                public string Format { get; set; }

                public RecentFile(string _name = "", string _filePath = "", string _format = "")
                {
                    Name = _name;
                    FilePath = _filePath;
                    Format = _format;
                }
            }
        }

        public static Instance Default { get; set; }

        public static void Init()
        {
            Reload();
        }
        public static void Save()
        {
            try
            {
                string json = JsonConvert.SerializeObject(Default);
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void Reload()
        {
            try
            {
                if (!File.Exists(FilePath)) File.Create(FilePath).Close();
                string json = File.ReadAllText(FilePath);
                try
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
                    Instance result = JsonConvert.DeserializeObject<Instance>(json, settings);
                    if (result != null) Default = result;
                    else Default = new Instance();
                }
                catch
                {
                    Default = new Instance();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Default = new Instance();
            }

        }
        public static void Reset()
        {
            Default = new Instance();
            Save();
            Reload();
        }
        public static void Upgrade()
        {

        }
    }
}
