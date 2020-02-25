using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Windows;

namespace AnimationEditor.Methods
{
    public static class ProgramBase
    {
        #region Definitions
        public static bool IsDebug { get; private set; } = false;
        public static bool AllowDebugOutput { get; set; } = true;
        public static bool AllowFullDebugOutput { get; set; } = true;
        public static Version RuntimeVersion { get; set; } = null;
        public static Version GetVersion()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            if (IsDebug)
            {
                if (RuntimeVersion == null)
                {
                    string runtimeVer = AssemblyAttributeAccessors.GetBuildDate.ToString("y.M.d.0");
                    RuntimeVersion = new Version(runtimeVer);
                }
                return RuntimeVersion;
            }

            return version;
        }
        public static string GetCasualVersion()
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            if (IsDebug)
            {
                if (RuntimeVersion == null)
                {
                    string runtimeVer = AssemblyAttributeAccessors.GetBuildDate.ToString("y.M.d.0");
                    RuntimeVersion = new Version(runtimeVer);
                }

                string verString = RuntimeVersion.ToString();
                string devVersion = verString.TrimEnd(verString[verString.Length - 1]) + "DEV";
                return devVersion;
            }

            return version;
        }
        public static log4net.ILog Log { get; set; }

        public static class AssemblyAttributeAccessors
        {
            public static string AssemblyTitle
            {
                get
                {
                    object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                    if (attributes.Length > 0)
                    {
                        AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                        if (titleAttribute.Title != "")
                        {
                            return titleAttribute.Title;
                        }
                    }
                    return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
                }
            }

            public static string GetProgramType
            {
                get
                {
                    if (Environment.Is64BitProcess)
                    {
                        return "x64";
                    }
                    else
                    {
                        return "x86";
                    }
                }
            }

            public static string AssemblyProduct
            {
                get
                {
                    object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                    if (attributes.Length == 0)
                    {
                        return "";
                    }
                    return ((AssemblyProductAttribute)attributes[0]).Product;
                }
            }

            public static string AssemblyCopyright
            {
                get
                {
                    object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                    if (attributes.Length == 0)
                    {
                        return "";
                    }
                    return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
                }
            }

            public static string GetBuildTime
            {
                get
                {
                    DateTime buildDate = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).LastWriteTime;
                    String buildTimeString = buildDate.ToString();
                    return buildTimeString;
                }

            }

            public static DateTime GetBuildDate
            {
                get
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    return GetLinkerTime(assembly);
                }
            }

            private static DateTime GetLinkerTime(Assembly assembly, TimeZoneInfo target = null)
            {
                var filePath = assembly.Location;
                const int c_PeHeaderOffset = 60;
                const int c_LinkerTimestampOffset = 8;

                var buffer = new byte[2048];

                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    stream.Read(buffer, 0, 2048);

                var offset = BitConverter.ToInt32(buffer, c_PeHeaderOffset);
                var secondsSince1970 = BitConverter.ToInt32(buffer, offset + c_LinkerTimestampOffset);
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

                var linkTimeUtc = epoch.AddSeconds(secondsSince1970);

                var tz = target ?? TimeZoneInfo.Local;
                var localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tz);

                return localTime;
            }
        }
        #endregion

        #region Logging

        public static void StartLogging()
        {
            log4net.GlobalContext.Properties["AnimEdVersion"] = GetCasualVersion().ToString();
            log4net.Config.XmlConfigurator.Configure();
            Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            //ConsoleManager.Show();
            AppDomain.CurrentDomain.FirstChanceException += (sender, e) =>
            {
                if ((!IsDebug && AllowDebugOutput))
                {
                    if (e.Exception.TargetSite != null && e.Exception.TargetSite.DeclaringType.Assembly == Assembly.GetExecutingAssembly())
                    {
                        if (Log != null) Log.ErrorFormat("[Exception Thrown] {0} {1}", RemoveNewLineChars(e.Exception.Message), RemoveNewLineChars(e.Exception.StackTrace));
                    }
                    else if (AllowFullDebugOutput)
                    {
                        if (Log != null) Log.ErrorFormat("[FULL] [Exception Thrown] {0} {1}", RemoveNewLineChars(e.Exception), RemoveNewLineChars(e.Exception.StackTrace));
                    }
                }

            };
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                if ((!IsDebug && AllowDebugOutput))
                {
                    if (e.ExceptionObject != null && e.ExceptionObject is Exception)
                    {
                        Exception ex = e.ExceptionObject as Exception;
                        if (ex.TargetSite != null && ex.TargetSite.DeclaringType.Assembly == Assembly.GetExecutingAssembly())
                        {
                            if (Log != null) Log.ErrorFormat("[Unhandled Exception Thrown] {0} {1}", RemoveNewLineChars(ex.Message), RemoveNewLineChars(ex.StackTrace));
                        }
                        else if (AllowFullDebugOutput)
                        {
                            if (Log != null) Log.ErrorFormat("[FULL] [Unhandled Exception Thrown] {0} {1}", RemoveNewLineChars(ex), RemoveNewLineChars(ex.StackTrace));
                        }
                    }
                }

            };
        }

        static string RemoveNewLineChars(Exception exception_to_search, string replacement_string = " ")
        {
            if (exception_to_search.Message != null) return System.Text.RegularExpressions.Regex.Replace(exception_to_search.Message, @"\t|\n|\r", replacement_string);
            return exception_to_search.ToString();
        }

        static string RemoveNewLineChars(string string_to_search, string replacement_string = " ")
        {
            if (string_to_search != null) return System.Text.RegularExpressions.Regex.Replace(string_to_search, @"\t|\n|\r", replacement_string);
            return "";
        }

        static string GetLoggingFolder()
        {
            string path = System.IO.Path.Combine(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RSDK Animation Editor"), "Logs");
            if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
            return path;
        }
        static void CleanUpLogsFolder()
        {
            string folder = GetLoggingFolder();
            if (Directory.Exists(folder))
            {
                DirectoryInfo logsFolder = new DirectoryInfo(folder);
                var fileList = logsFolder.GetFiles("*.log", SearchOption.AllDirectories).ToList();
                if (fileList.Count > 10)
                {
                    foreach (var file in fileList.OrderByDescending(file => file.CreationTime).Skip(10))
                    {
                        file.Delete();
                    }
                }

            }
        }

        public static void EndLogging()
        {
            CleanUpLogsFolder();
            //ConsoleManager.Hide();
        }

        #endregion
    }
}
