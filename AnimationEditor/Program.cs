using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimationEditor
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            MainMethod(args);
        }

        public static void MainMethod()
        {
            Methods.ProgramBase.StartLogging();
            App.ExternalLaunch();
            Methods.ProgramBase.EndLogging();
        }

        public static void MainMethod(string[] args)
        {
            Methods.ProgramBase.StartLogging();
            Launch();
            Methods.ProgramBase.EndLogging();
        }

        static void Launch()
        {
            App app = new App();
            app.Load();
        }
    }
}
