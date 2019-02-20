using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AnimationEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>

    public enum Skin { Light, Dark }

    public partial class App : Application
    {
        public static Skin Skin { get; set; } = Skin.Dark;
    }
}
