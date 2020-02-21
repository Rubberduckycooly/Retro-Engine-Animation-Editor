using System;
using System.Collections.Generic;
using RSDKv5;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AnimationEditor.Services;
using AnimationEditor.Animation;
using AnimationEditor.Animation.Classes;
using AnimationEditor.Animation.Methods;
using System.ComponentModel;

namespace AnimationEditor.Animation.Classes
{
    public class Spritesheet
    {
        public System.Windows.Media.Imaging.BitmapImage Image;
        public System.Windows.Media.Imaging.BitmapImage TransparentImage;
        public System.Windows.Media.Color TransparentColor = (System.Windows.Media.Color)ColorConverter.ConvertFromString("#303030");

        public bool isReady = false;
        public bool isInvalid = false;
        public Spritesheet(System.Windows.Media.Imaging.BitmapImage _Image, System.Windows.Media.Imaging.BitmapImage _TransparentImage, System.Windows.Media.Color _TransparentColor)
        {
            Image = _Image;
            TransparentImage = _TransparentImage;
            TransparentColor = _TransparentColor;
        }

        public Spritesheet(System.Windows.Media.Imaging.BitmapImage _Image, System.Windows.Media.Imaging.BitmapImage _TransparentImage, bool _isInvalid)
        {
            Image = _Image;
            isInvalid = _isInvalid;
        }
    }
}
