using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimationEditor.ViewModels;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace AnimationEditor
{
    public class SkinResourceDictionary : ResourceDictionary
    {
        private Uri _darkSource;
        private Uri _lightSource;

        public Uri LightSource
        {
            get { return _lightSource; }
            set
            {
                _lightSource = value;
                UpdateSource();
            }
        }
        public Uri DarkSource
        {
            get { return _darkSource; }
            set
            {
                _darkSource = value;
                UpdateSource();
            }
        }

        public void UpdateSource()
        {
            var val = App.Skin == Skin.Dark ? DarkSource : LightSource;
            if (val != null && base.Source != val)
                base.Source = val;
        }
    }
}
