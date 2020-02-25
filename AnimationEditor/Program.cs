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
