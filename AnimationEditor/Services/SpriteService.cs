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
using AnimationEditor.ViewModel;
using AnimationEditor.Classes;
using AnimationEditor.Methods;
using System.ComponentModel;
using GenerationsLib.Core;


namespace AnimationEditor.Services
{
    public class SpriteService
    {
        private bool isAnimationFileLoaded
        {
            get
            {
                return LoadedAnimationFile != null;
            }
        }

        private EditorAnimation LoadedAnimationFile;

        public SpriteService()
        {

        }

        public void SetAnimation(EditorAnimation Anim)
        {
            LoadedAnimationFile = Anim;
        }

        #region Current Spritesheets
        public ObservableCollection<Spritesheet> SpriteSheets { get; set; } = new ObservableCollection<Spritesheet>();
        public ObservableCollection<string> NullSpriteSheetList { get; set; } = new ObservableCollection<string>();

        #endregion

        #region Cropped Frames
        private Dictionary<Tuple<string, int>, BitmapSource> CroppedFrames { get; set; } = new Dictionary<Tuple<string, int>, BitmapSource>(1024);
        public BitmapSource GetCroppedFrame(int texture, EditorAnimation.EditorFrame frame)
        {
            if (!isAnimationFileLoaded) return null;
            if (texture < 0 || texture >= LoadedAnimationFile.SpriteSheets.Count || frame == null) return null;
            var name = LoadedAnimationFile.SpriteSheets[texture];
            var tuple = new Tuple<string, int>(name, frame.GetHashCode());
            if (CroppedFrames.TryGetValue(tuple, out BitmapSource bitmap))
                return bitmap;
            var textureBitmap = SpriteSheets[texture];

            if (NullSpriteSheetList.Contains(name))
            {
                bitmap = BitmapSource.Create(1, 1, 96, 96, PixelFormats.Bgr24, null, new byte[3] { 0, 0, 0 }, 3);
                return CroppedFrames[tuple] = bitmap;
            }

            if (frame.Width > 0 && frame.Height > 0 && textureBitmap != null && textureBitmap.isReady)
            {
                try
                {
                    bitmap = new CroppedBitmap(textureBitmap.Image,
                    new System.Windows.Int32Rect()
                    {
                        X = frame.X,
                        Y = frame.Y,
                        Width = frame.Width,
                        Height = frame.Height
                    });
                }
                catch (ArgumentException)
                {
                }
            }
            else
            {
                bitmap = BitmapSource.Create(1, 1, 96, 96, PixelFormats.Bgr24, null, new byte[3] { 0, 0, 0 }, 3);
            }
            return CroppedFrames[tuple] = bitmap;
        }
        public void InvalidateCroppedFrame(int texture, EditorAnimation.EditorFrame frame)
        {
            if (texture < 0 || texture >= LoadedAnimationFile.SpriteSheets.Count)
                return;
            var name = LoadedAnimationFile.SpriteSheets[texture];
            CroppedFrames.Remove(new Tuple<string, int>(name, frame.GetHashCode()));
        }
        #endregion
    }
}
