using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AnimationEditor.Styles;

namespace AnimationEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>

    public enum Skin { Light, Dark }

    public partial class App : Application
    {
        public App()
        {
            AnimationEditor.Animation.Classes.Settings.Init();
            if (AnimationEditor.Properties.Settings.Default.UseDarkTheme)
            {
                Skin = Skin.Dark;
            }
            else
            {
                Skin = Skin.Light;
            }
        }
        public static Skin Skin { get; set; }

        public static void ChangeSkin(Skin newSkin)
        {
            Skin = newSkin;

            foreach (ResourceDictionary dict in App.Current.Resources.MergedDictionaries)
            {

                if (dict is SkinResourceDictionary skinDict)
                    skinDict.UpdateSource();
                else
                    dict.Source = dict.Source;
            }
        }

        private void ApplyResources(string src)
        {
            var dict = new ResourceDictionary() { Source = new Uri(src, UriKind.Relative) };
            foreach (var mergeDict in dict.MergedDictionaries)
            {
                Resources.MergedDictionaries.Add(mergeDict);
            }

            foreach (var key in dict.Keys)
            {
                Resources[key] = dict[key];
            }
        }
    }
}
