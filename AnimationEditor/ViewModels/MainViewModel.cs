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

namespace AnimationEditor.ViewModels
{
    public class MainViewModel
    {
        #region Animations and Frames
        public Animation LoadedAnimationFile;

        public EngineType AnimationType = EngineType.RSDKv5;

        public bool FullFrameMode = false;
        public string AnimationFilepath { get; set; }
        public string AnimationDirectory { get; set; }

        public string SpriteDirectory { get; set; }
        public List<Spritesheet> SpriteSheets { get; set; }

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
            }

            public Spritesheet(System.Windows.Media.Imaging.BitmapImage _Image, System.Windows.Media.Imaging.BitmapImage _TransparentImage, bool _isInvalid)
            {
                Image = _Image;
                isInvalid = _isInvalid;
            }
        }

        public List<string> NullSpriteSheetList { get => SpriteSheetNullList; set => SpriteSheetNullList = value; }

        private List<string> SpriteSheetNullList = new List<string>();

        public Animation.AnimationEntry _SelectedAnimation;
        public List<Animation.AnimationEntry> Animations { get => GetAnimations(); set => SetAnimations(value); }
        public List<string> SpriteSheetPaths { get => GetSpriteSheetsList(); }
        public List<string> Hitboxes { get => GetHitboxesList(); set => SetHitboxesList(value); }
        public List<string> CollisionBoxesNames { get => GetCollisionBoxes(); }
        public List<Animation.HitBox> CollisionBoxes { get => GetHitBoxes(); }
        public Animation.AnimationEntry SelectedAnimation { get => _SelectedAnimation; set { _SelectedAnimation = value; } }
        public int SelectedAnimationIndex { get; set; }

        public int FramesCount { get => GetCurrentFrameCount(); }
        public int AnimationsCount { get => GetCurrentAnimationCount(); }

        public double ViewWidth { get; set; }
        public double ViewHeight { get; set; }

        public List<Animation.Frame> AnimationFrames { get => GetAnimationsFrames(); }
        public int SelectedFrameIndex { get; set; }
        public byte? Loop { get => GetLoopIndex(); set => SetLoopIndex(value); }
        public short? Speed { get => GetSpeedMultiplyer(); set => SetSpeedMultiplyer(value); }
        public byte? Flags { get => GetRotationFlag(); set => SetRotationFlag(value); }

        private Dictionary<string, BitmapSource> _textures = new Dictionary<string, BitmapSource>(24);
        private Dictionary<Tuple<string, int>, BitmapSource> _frames = new Dictionary<Tuple<string, int>, BitmapSource>(1024);

        public BitmapSource GetCroppedFrame(int texture, Animation.Frame frame)
        {
            if (texture < 0 || texture >= LoadedAnimationFile.SpriteSheets.Count || frame == null) return null;
            var name = LoadedAnimationFile.SpriteSheets[texture];
            var tuple = new Tuple<string, int>(name, frame.GetHashCode());
            if (_frames.TryGetValue(tuple, out BitmapSource bitmap))
                return bitmap;
            var textureBitmap = SpriteSheets[texture];

            if (NullSpriteSheetList.Contains(name))
            {
                bitmap = BitmapSource.Create(1, 1, 96, 96, PixelFormats.Bgr24, null, new byte[3] { 0, 0, 0 }, 3);
                return _frames[tuple] = bitmap;
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
            return _frames[tuple] = bitmap;
        }

        public void InvalidateFrame(int texture, Animation.Frame frame)
        {
            if (texture < 0 || texture >= LoadedAnimationFile.SpriteSheets.Count)
                return;
            var name = LoadedAnimationFile.SpriteSheets[texture];
            _frames.Remove(new Tuple<string, int>(name, frame.GetHashCode()));
        }

        public BitmapSource GetFrameImage(int index)
        {
            if (GetAnimationFrame(index) == null) return null;
            return GetCroppedFrame(GetAnimationFrame(index).SpriteSheet, GetAnimationFrame(index));
        }

        public void InvalidateFrameImage(int index)
        {
            if (GetAnimationFrame(index) == null) return;
            InvalidateFrame(GetAnimationFrame(index).SpriteSheet, GetAnimationFrame(index));
        }

        public List<Animation.AnimationEntry> GetAnimations()
        {
            if (LoadedAnimationFile != null) return LoadedAnimationFile.Animations;
            else return null;
        }
        public void SetAnimations(List<Animation.AnimationEntry> value)
        {
            if (LoadedAnimationFile != null) LoadedAnimationFile.Animations = value;
            else return;
        }

        public int GetCurrentFrameIndexForAllAnimations()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1)
            {
                int frames = 0;
                for (int i = 0; i < SelectedAnimationIndex; i++)
                {
                    frames += LoadedAnimationFile.Animations[i].Frames.Count();
                }
                frames += SelectedFrameIndex;
                return frames;
            }
            else return -1;
        }

        public int GetCurrentFrameCount()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1) return LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames.Count - 1;
            else return -1;
        }

        public int GetCurrentAnimationCount()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1) return LoadedAnimationFile.Animations.Count - 1;
            else return -1;
        }

        public double BorderLeft => GetBorderLeft();
        public double BorderTop => GetBorderTop();

        public double SpriteLeft => GetSpriteLeft();
        public double SpriteTop => GetSpriteTop();
        public double SpriteRight => GetSpriteRight();
        public double SpriteBottom => GetSpriteBottom();

        public double HitboxLeft => GetHitboxLeft();
        public double HitboxTop => GetHitboxTop();
        public Rect SpriteFrame => GetFrame();
        public Point SpriteCenter
        {
            get
            {
                return new Point(0, 0);
            }
        }

        public double GetBorderTop()
        {
            double Center = ViewHeight / 2.0;
            double FrameTop = SelectedFrameTop ?? 0;
            double FrameCenterY = SelectedFramePivotY ?? 0;
            return (int)((Center) + FrameCenterY * Zoom);
        }

        public double GetBorderLeft()
        {
            double Center = ViewWidth / 2.0;
            double FrameLeft = SelectedFrameLeft ?? 0;
            double FrameCenterX = SelectedFramePivotX ?? 0;
            return (int)((Center) + FrameCenterX * Zoom);
        }


        public double GetSpriteTop()
        {
            double Center = ViewHeight / 2.0;
            double FrameTop = SelectedFrameTop ?? 0;
            double FrameCenterY = SelectedFramePivotY ?? 0;
            double FrameHeight = SelectedFrameHeight ?? 0;
            return (int)(Center - FrameTop * Zoom) + FrameCenterY * Zoom;
        }

        public double GetSpriteLeft()
        {
            double Center = ViewWidth / 2.0;
            double FrameLeft = SelectedFrameLeft ?? 0;
            double FrameCenterX = SelectedFramePivotX ?? 0;
            double FrameWidth = SelectedFrameWidth ?? 0;
            return (int)(Center - FrameLeft * Zoom) + FrameCenterX * Zoom;
        }

        public double GetSpriteRight()
        {
            if (FullFrameMode)
            {
                return 0;
            }
            else
            {
                double FrameWidth = SelectedFrameWidth ?? 0;
                return (int)(SpriteLeft + FrameWidth * Zoom);
            }

        }

        public double GetSpriteBottom()
        {
            if (FullFrameMode)
            {
                return 0;
            }
            else
            {
                double FrameHeight = SelectedFrameHeight ?? 0;
                return (int)(SpriteTop + FrameHeight * Zoom);
            }

        }

        public double GetHitboxTop()
        {
            double FrameCenterY = SelectedFramePivotY ?? 0;
            double Center = ViewHeight / 2.0;
            double HitboxOffset = SelectedHitbox_Width * Zoom;
            return Center + HitboxOffset;
        }

        public double GetHitboxLeft()
        {
            double Center = ViewWidth / 2.0;
            double HitboxOffset = SelectedHitbox_X * Zoom;
            return Center + HitboxOffset;
        }

        public double SpriteScaleX { get => Zoom; set => Zoom = value; }

        public double Zoom = 1;
        public Rect GetFrame()
        {
            if (SelectedFrameLeft != null && SelectedFrameTop != null && SelectedFrameWidth != null && SelectedFrameHeight != null)
            {
                if (SpriteSheets != null && isCurrentSpriteSheetValid())
                {
                    if (FullFrameMode)
                    {
                        if (!NullSpriteSheetList.Contains(SpriteSheetPaths[(int)CurrentSpriteSheet.Value])) return new Rect(0, 0, SpriteSheets[(int)CurrentSpriteSheet.Value].Image.Width, SpriteSheets[(int)CurrentSpriteSheet.Value].Image.Height);
                        else return new Rect(SelectedFrameLeft.Value, SelectedFrameTop.Value, SelectedFrameWidth.Value, SelectedFrameHeight.Value);

                    }
                    else
                    {
                        return new Rect(SelectedFrameLeft.Value, SelectedFrameTop.Value, SelectedFrameWidth.Value, SelectedFrameHeight.Value);
                    }
                }
                else
                {
                    return new Rect(0, 0, 0.5, 0.5);

                }


            }
            return new Rect(0, 0, 0.5, 0.5);

        }


        public bool isCurrentSpriteSheetValid()
        {
            if ((int)CurrentSpriteSheet < SpriteSheets.Count)
            {
                if (SpriteSheets[(int)CurrentSpriteSheet].isReady) return true;
                else return false;
            }
            else return false;

        }

        public Animation.Frame GetAnimationFrame(int index)
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1) return LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[index];
            else return null;
        }

        public List<string> GetSpriteSheetsList()
        {
            if (LoadedAnimationFile != null) return LoadedAnimationFile.SpriteSheets;
            else return null;
        }

        public List<string> GetHitboxesList()
        {
            if (LoadedAnimationFile != null) return LoadedAnimationFile.CollisionBoxes;
            else return null;
        }

        public List<Animation.Frame> GetAnimationsFrames()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1) return LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames;
            else return null;
        }

        public byte GetLoopIndex()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1) return LoadedAnimationFile.Animations[SelectedAnimationIndex].LoopIndex;
            else return 0;
        }
        public short GetSpeedMultiplyer()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1) return LoadedAnimationFile.Animations[SelectedAnimationIndex].SpeedMultiplyer;
            else return 0;
        }
        public byte GetRotationFlag()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1) return LoadedAnimationFile.Animations[SelectedAnimationIndex].RotationFlags;
            else return 0;
        }

        public void SetLoopIndex(byte? value)
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && value.HasValue) LoadedAnimationFile.Animations[SelectedAnimationIndex].LoopIndex = value.Value;
            else return;
        }
        public void SetSpeedMultiplyer(short? value)
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && value.HasValue) LoadedAnimationFile.Animations[SelectedAnimationIndex].SpeedMultiplyer = value.Value;
            else return;
        }
        public void SetRotationFlag(byte? value)
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && value.HasValue) LoadedAnimationFile.Animations[SelectedAnimationIndex].RotationFlags = value.Value;
            else return;
        }

        public void SetHitboxesList(List<string> value)
        {
            if (LoadedAnimationFile != null) LoadedAnimationFile.CollisionBoxes = value;
            else return;
        }

        #region Hitboxes

        public List<string> GetCollisionBoxes()
        {
            if (LoadedAnimationFile != null) return LoadedAnimationFile.CollisionBoxes;
            else return new List<string> { "" };

        }

        public List<Animation.HitBox> GetHitBoxes()
        {
            return null;
        }

        #endregion


        #region Frame Info

        public short? SelectedFramePivotX { get => GetPivotX(); set => SetPivotX(value); }
        public short? SelectedFramePivotY { get => GetPivotY(); set => SetPivotY(value); }
        public short? SelectedFrameHeight { get => GetHeight(); set => SetHeight(value); }
        public short? SelectedFrameWidth { get => GetWidth(); set => SetWidth(value); }
        public short? SelectedFrameLeft { get => GetX(); set => SetX(value); }
        public short? SelectedFrameTop { get => GetY(); set => SetY(value); }
        public ushort? SelectedFrameId { get => GetID(); set => SetID(value); }
        public short? SelectedFrameDuration { get { return GetDelay(); } set { SetDelay(value); } }
        public byte? CurrentSpriteSheet { get => GetSpriteSheet(); set => SetSpriteSheet(value); }

        #region Get Methods
        public byte GetSpriteSheet()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1) return LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].SpriteSheet;
            else return 0;
        }
        public short GetPivotX()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1) return LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].PivotX;
            else return 0;
        }
        public short GetPivotY()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1) return LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].PivotY;
            else return 0;
        }
        public short GetHeight()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1) return LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].Height;
            else return 0;
        }
        public short GetWidth()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1) return LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].Width;
            else return 0;
        }
        public short GetX()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1) return LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].X;
            else return 0;
        }
        public short GetY()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1) return LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].Y;
            else return 0;
        }
        public ushort GetID()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1) return LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].ID;
            else return 0;
        }
        public short GetDelay()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1) return LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].Delay;
            else return 0;
        }
        #endregion
        #region Set Methods
        public void SetSpriteSheet(byte? value)
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1 && value.HasValue) LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].SpriteSheet = value.Value;
            else return;
        }
        public void SetPivotX(short? value)
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1 && value.HasValue) LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].PivotX = value.Value;
            else return;
        }
        public void SetPivotY(short? value)
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1 && value.HasValue) LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].PivotY = value.Value;
            else return;
        }
        public void SetHeight(short? value)
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1 && value.HasValue) LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].Height = value.Value;
            else return;
        }
        public void SetWidth(short? value)
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1 && value.HasValue) LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].Width = value.Value;
            else return;
        }
        public void SetX(short? value)
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1 && value.HasValue) LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].X = value.Value;
            else return;
        }
        public void SetY(short? value)
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1 && value.HasValue) LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].Y = value.Value;
            else return;
        }
        public void SetID(ushort? value)
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1 && value.HasValue) LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].ID = value.Value;
            else return;
        }
        public void SetDelay(short? value)
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1 && value.HasValue) LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].Delay = value.Value;
            else return;
        }
        #endregion

        #region Hitbox Info

        public int SelectedFrameHitboxIndex { get; set; }

        public Animation.HitBox SelectedHitbox { get => GetSelectedHitbox(); set => SetSelectedHitbox(value); }

        public short SelectedHitbox_X { get => GetCurrentHitboxX(); set => SetCurrentHitboxX(value); }

        public short SelectedHitbox_Y { get => GetCurrentHitboxY(); set => SetCurrentHitboxY(value); }

        public short SelectedHitbox_Width { get => GetCurrentHitboxWidth(); set => SetCurrentHitboxWidth(value); }

        public short SelectedHitbox_Height { get => GetCurrentHitboxHeight(); set => SetCurrentHitboxHeight(value); }

        #region Get Methods

        public Animation.HitBox GetSelectedHitbox()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1 && SelectedFrameHitboxIndex != -1 && LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].HitBoxes.Count > 0) return LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].HitBoxes[SelectedFrameHitboxIndex];
            else return null;
        }
        public int GetCurrentHitboxIndex()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1 && LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].HitBoxes.Count > 0) return LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].CollisionBox;
            else return -1;
        }

        public short GetCurrentHitboxX()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1 && SelectedFrameHitboxIndex != -1 && LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].HitBoxes.Count > 0) return LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].HitBoxes[SelectedFrameHitboxIndex].X;
            else return 0;
        }

        public short GetCurrentHitboxY()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1 && SelectedFrameHitboxIndex != -1 && LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].HitBoxes.Count > 0) return LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].HitBoxes[SelectedFrameHitboxIndex].Y;
            else return 0;
        }

        public short GetCurrentHitboxWidth()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1 && SelectedFrameHitboxIndex != -1 && LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].HitBoxes.Count > 0) return LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].HitBoxes[SelectedFrameHitboxIndex].Width;
            else return 0;
        }

        public short GetCurrentHitboxHeight()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1 && SelectedFrameHitboxIndex != -1 && LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].HitBoxes.Count > 0) return LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].HitBoxes[SelectedFrameHitboxIndex].Height;
            else return 0;
        }

        #endregion

        #region Set Methods

        public void SetSelectedHitbox(Animation.HitBox value)
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1 && SelectedFrameHitboxIndex != -1) LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].HitBoxes[SelectedFrameHitboxIndex] = value;
            else return;
        }
        public void SetCurrentHitboxIndex(int value)
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1) LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].CollisionBox = (byte)value;
            else return;
        }

        public void SetCurrentHitboxX(short? value)
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1 && SelectedFrameHitboxIndex != -1)
            {
                Animation.HitBox box = LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].HitBoxes.ElementAt(SelectedFrameHitboxIndex);
                box.X = value.Value;
                LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].HitBoxes[SelectedFrameHitboxIndex] = box;
            }
            else return;
        }

        public void SetCurrentHitboxY(short? value)
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1 && SelectedFrameHitboxIndex != -1)
            {
                Animation.HitBox box = LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].HitBoxes.ElementAt(SelectedFrameHitboxIndex);
                box.Y = value.Value;
                LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].HitBoxes[SelectedFrameHitboxIndex] = box;
            }
            else return;
        }

        public void SetCurrentHitboxWidth(short? value)
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1 && SelectedFrameHitboxIndex != -1)
            {
                Animation.HitBox box = LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].HitBoxes.ElementAt(SelectedFrameHitboxIndex);
                box.Width = value.Value;
                LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].HitBoxes[SelectedFrameHitboxIndex] = box;
            }
            else return;
        }

        public void SetCurrentHitboxHeight(short? value)
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1 && SelectedFrameHitboxIndex != -1)
            {
                Animation.HitBox box = LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].HitBoxes.ElementAt(SelectedFrameHitboxIndex);
                box.Height = value.Value;
                LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].HitBoxes[SelectedFrameHitboxIndex] = box;
            }
            else return;
        }

        #endregion



        #endregion

        #endregion

        #endregion


        #region Animation and Frame Management

        public void ShiftFrameRight(int frameID)
        {
            int parentID = frameID + 1;
            if (parentID < 0 || parentID > LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames.Count()) return;
            var targetFrame = LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[frameID];
            var parentFrame = LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[parentID];

            int parentIndex = LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames.IndexOf(parentFrame);
            int targetIndex = LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames.IndexOf(parentFrame);

            LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames.Remove(targetFrame);
            LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames.Insert(parentIndex, targetFrame);


        }

        public void ShiftFrameLeft(int frameID)
        {
            int parentID = frameID - 1;
            if (parentID < 0 || parentID > LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames.Count()) return;
            var targetFrame = LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[frameID];
            var parentFrame = LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[parentID];

            int parentIndex = LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames.IndexOf(parentFrame);
            int targetIndex = LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames.IndexOf(parentFrame);

            LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames.Remove(targetFrame);
            LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames.Insert(parentIndex, targetFrame);
        }

        public void ShiftAnimationUp(int animID)
        {
            if (LoadedAnimationFile == null) return;
            int parentID = animID - 1;
            if (parentID < 0 || parentID > LoadedAnimationFile.Animations.Count()) return;
            var targetAnimation = LoadedAnimationFile.Animations[animID];
            var parentAnimation = LoadedAnimationFile.Animations[parentID];

            int parentIndex = LoadedAnimationFile.Animations.IndexOf(parentAnimation);

            LoadedAnimationFile.Animations.Remove(targetAnimation);
            LoadedAnimationFile.Animations.Insert(parentIndex, targetAnimation);
        }

        public void ShiftAnimationDown(int animID)
        {
            if (LoadedAnimationFile == null) return;
            int parentID = animID + 1;
            if (parentID < 0 || parentID > LoadedAnimationFile.Animations.Count()) return;
            var targetAnimation = LoadedAnimationFile.Animations[animID];
            var parentAnimation = LoadedAnimationFile.Animations[parentID];

            int parentIndex = LoadedAnimationFile.Animations.IndexOf(parentAnimation);

            LoadedAnimationFile.Animations.Remove(targetAnimation);
            LoadedAnimationFile.Animations.Insert(parentIndex, targetAnimation);
        }

        public void RemoveFrame(int frameID)
        {
            LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames.RemoveAt(frameID);
        }

        public void AddFrame(int frameID)
        {
            var frame = new Animation.Frame(AnimationType);
            LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames.Insert(frameID, frame);
        }

        public void DuplicateFrame(int frameID)
        { 
            var frame = (Animation.Frame)LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[frameID].Clone();
            LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames.Insert(frameID, frame);
        }

        public void RemoveAnimation(int animID)
        {
            LoadedAnimationFile.Animations.RemoveAt(animID);
        }

        public void AddAnimation(int animID)
        {
            var animation = new Animation.AnimationEntry(AnimationType);
            animation.AnimName = "New Entry";
            animation.Frames = new List<Animation.Frame>();
            LoadedAnimationFile.Animations.Insert(animID, animation);
        }

        public void DuplicateAnimation(int animID)
        {
            var animation = LoadedAnimationFile.Animations[animID];
            LoadedAnimationFile.Animations.Insert(animID, animation);
        }



        #endregion

    }
}
