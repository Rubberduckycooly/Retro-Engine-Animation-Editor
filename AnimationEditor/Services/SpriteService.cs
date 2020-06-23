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
        private Dictionary<Tuple<string, int>, BitmapSource> CroppedTransparentFrames { get; set; } = new Dictionary<Tuple<string, int>, BitmapSource>(1024);

        private Dictionary<Tuple<string, int>, BitmapSource> GetCroppedFrames(bool isTransparent)
        {
            return (isTransparent ? CroppedTransparentFrames : CroppedFrames);
        }

        private BitmapSource SetCroppedFrames(bool isTransparent, Tuple<string, int> tuple, BitmapSource bitmap)
        {
            if (isTransparent) return CroppedTransparentFrames[tuple] = bitmap;
            else return CroppedFrames[tuple] = bitmap;
        }

        public BitmapSource GetCroppedFrame(int texture, EditorAnimation.EditorFrame frame, bool isTransparent = false)
        {
            if (!isAnimationFileLoaded) return null;
            if (texture < 0 || texture >= LoadedAnimationFile.SpriteSheets.Count || frame == null) return null;
            var name = LoadedAnimationFile.SpriteSheets[texture];
            var tuple = new Tuple<string, int>(name, frame.GetHashCode());
            if (GetCroppedFrames(isTransparent).TryGetValue(tuple, out BitmapSource bitmap)) return bitmap;
            var textureBitmap = SpriteSheets[texture];

            if (NullSpriteSheetList.Contains(name))
            {
                bitmap = BitmapSource.Create(1, 1, 96, 96, PixelFormats.Bgr24, null, new byte[3] { 0, 0, 0 }, 3);
                return SetCroppedFrames(isTransparent, tuple, bitmap);
            }

            if (frame.Width > 0 && frame.Height > 0 && textureBitmap != null && textureBitmap.isReady)
            {
                try
                {
                    var desiredType = (isTransparent ? textureBitmap.TransparentImage : textureBitmap.Image);
                    
                    
                    int max_width = desiredType.PixelWidth;
                    int max_height = desiredType.PixelHeight;

                    int ofb_width = max_width - frame.X + frame.Width;
                    int ofb_height = max_height - frame.Y + frame.Height;

                    int image_width = (frame.X + frame.Width > max_width ? frame.Width + ofb_width : frame.Width);
                    int image_height = (frame.Y + frame.Height > max_height ? frame.Height + ofb_height : frame.Height);

                    bool oversized_X = frame.X + frame.Width > max_width;
                    bool oversized_Y = frame.Y + frame.Height > max_height;

                    if (oversized_X || oversized_Y)
                    {
                        bitmap = BitmapSource.Create(1, 1, frame.Height, frame.Width, PixelFormats.Bgr24, null, new byte[3] { 0, 0, 0 }, 3);
                    }
                    else
                    {
                        bitmap = new CroppedBitmap(desiredType,
                        new System.Windows.Int32Rect()
                        {
                            X = frame.X,
                            Y = frame.Y,
                            Width = frame.Width,
                            Height = frame.Height
                        });
                    }
                    


                }
                catch (ArgumentException)
                {
                    bitmap = BitmapSource.Create(1, 1, frame.Height, frame.Width, PixelFormats.Bgr24, null, new byte[3] { 0, 0, 0 }, 3);
                }
            }
            else
            {
                bitmap = BitmapSource.Create(1, 1, 96, 96, PixelFormats.Bgr24, null, new byte[3] { 0, 0, 0 }, 3);
            }
            return SetCroppedFrames(isTransparent, tuple, bitmap);
        }
        public void InvalidateCroppedFrame(int texture, EditorAnimation.EditorFrame frame)
        {
            if (texture < 0 || texture >= LoadedAnimationFile.SpriteSheets.Count)
                return;
            var name = LoadedAnimationFile.SpriteSheets[texture];
            CroppedFrames.Remove(new Tuple<string, int>(name, frame.GetHashCode()));
            CroppedTransparentFrames.Remove(new Tuple<string, int>(name, frame.GetHashCode()));
        }
        #endregion
    }
}
