using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GenerationsLib.WPF.Themes;

namespace AnimationEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>

    public partial class App : Application
    {
        public App()
        {
            AnimationEditor.Classes.Settings.Init();
            ChangeSkin(AnimationEditor.Classes.Settings.Default.CurrentTheme);
            InitializeComponent();
        }

        public void Load()
        {
            Pages.MainWindow mainWindow = new Pages.MainWindow();
            MainWindow = mainWindow;
            MainWindow.ShowDialog();
        }

        public static Skin Skin
        {
            get
            {
                return GenerationsLib.WPF.Themes.SkinResourceDictionary.CurrentTheme;
            }
            set
            {
                GenerationsLib.WPF.Themes.SkinResourceDictionary.CurrentTheme = Skin;
            }
        }

        public static void ChangeSkin(Skin newSkin)
        {
            GenerationsLib.WPF.Themes.SkinResourceDictionary.ChangeSkin(newSkin, App.Current.Resources.MergedDictionaries);
        }
    }
}
