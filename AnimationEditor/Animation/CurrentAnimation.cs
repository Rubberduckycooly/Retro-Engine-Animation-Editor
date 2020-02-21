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

namespace AnimationEditor.Animation
{
    public class CurrentAnimation
    {
        #region Loaded Animation Data
        public BridgedAnimation LoadedAnimationFile { get; set; }
        #endregion

        #region IO Paths
        public string AnimationFilepath { get; set; }
        public string AnimationDirectory { get; set; }
        public string SpriteDirectory { get; set; }
        #endregion

        #region Modes/States
        public bool FullFrameMode { get; set; } = false;
        public double Zoom { get; set; } = 1;

        #endregion

        #region Details
        public EngineType AnimationType { get; set; } = EngineType.RSDKv5;
        public int FramesCount
        {
            get
            {
                if (CanCollectAnimationEntryInformation) return SelectedAnimation.Frames.Count - 1;
                else return -1;
            }
        }
        public int AnimationsCount
        {
            get
            {
                if (CanCollectAnimationEntryInformation) return LoadedAnimationFile.Animations.Count - 1;
                else return -1;
            }
        }
        #endregion

        #region Selected Items
        public List<BridgedAnimation.BridgedAnimationEntry> SelectedAnimationEntries
        {
            get
            {
                if (LoadedAnimationFile != null) return LoadedAnimationFile.Animations;
                else return null;
            }
            set
            {
                if (LoadedAnimationFile != null) LoadedAnimationFile.Animations = value;
            }
        }
        public BridgedAnimation.BridgedAnimationEntry SelectedAnimation
        {
            get
            {
                try
                {
                    return LoadedAnimationFile.Animations[SelectedAnimationIndex];
                }
                catch
                {
                    return new BridgedAnimation.BridgedAnimationEntry(AnimationType, LoadedAnimationFile);
                }
            }
            set
            {
                try
                {
                    LoadedAnimationFile.Animations[SelectedAnimationIndex] = value;
                }
                catch
                {

                }

            }
        }
        public List<BridgedAnimation.BridgedFrame> SelectedAnimationFrameSet
        {
            get
            {
                if (CanCollectAnimationEntryInformation) return SelectedAnimation.Frames;
                else return null;
            }
            set
            {
                if (CanCollectAnimationEntryInformation)
                {
                    SelectedAnimation.Frames = value;
                }
            }
        }
        public BridgedAnimation.BridgedFrame SelectedFrame
        {
            get
            {
                try
                {
                    return SelectedAnimation.Frames[SelectedFrameIndex];
                }
                catch
                {
                    return new BridgedAnimation.BridgedFrame();
                }
            }
            set 
            {
                try
                {
                    SelectedAnimation.Frames[SelectedFrameIndex] = value;
                }
                catch
                {

                }

            }
        }
        public BridgedAnimation.BridgedHitBox SelectedHitbox
        {
            get
            {
                if (CanCollectFrameInformation && SelectedFrameHitboxIndex != -1 && SelectedFrame.HitBoxes.Count > 0) return SelectedFrame.HitBoxes[SelectedFrameHitboxIndex];
                else return null;
            }
            set
            {
                if (CanCollectFrameInformation && SelectedFrameHitboxIndex != -1) SelectedFrame.HitBoxes[SelectedFrameHitboxIndex] = value;
                else return;
            }
        }
        #endregion

        #region Selected Indexes

        public int SelectedFrameIndex { get; set; }
        public int SelectedAnimationIndex { get; set; }
        public int SelectedFrameHitboxIndex
        {
            get
            {
                if (CanCollectFrameInformation && SelectedFrame.HitBoxes.Count > 0) return SelectedFrame.CollisionBox;
                else return -1;
            }
            set
            {
                if (CanCollectFrameInformation) SelectedFrame.CollisionBox = (byte)value;
                else return;
            }
        }
        public int GetCurrentFrameIndexForAllAnimations()
        {
            if (CanCollectFrameInformation)
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
        #endregion

        #region Current Animation
        public List<string> Hitboxes
        {
            get
            {
                if (LoadedAnimationFile != null) return LoadedAnimationFile.CollisionBoxes;
                else return new List<string>();
            }
            set
            {
                if (LoadedAnimationFile != null) LoadedAnimationFile.CollisionBoxes = value;
                else return;
            }
        }
        public byte PlayerType
        {
            get
            {
                if (CanCollectFrameInformation)
                {
                    if (LoadedAnimationFile.EngineType == EngineType.RSDKvRS)
                    {
                        return LoadedAnimationFile.PlayerType;
                    }
                }
                return 0x0;
            }
            set
            {
                if (CanCollectFrameInformation)
                {
                    if (LoadedAnimationFile.EngineType == EngineType.RSDKvRS)
                    {
                        LoadedAnimationFile.PlayerType = value;
                    }
                }
                return;
            }
        }

        #endregion

        #region Current Spritesheets
        public List<Spritesheet> SpriteSheets { get; set; } = new List<Spritesheet>();
        public List<string> SpriteSheetPaths
        {
            get
            {
                if (LoadedAnimationFile != null) return LoadedAnimationFile.SpriteSheets;
                else return null;
            }
        }
        public List<string> NullSpriteSheetList { get; set; } = new List<string>();
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

        #endregion

        #region Current Status Variables
        public bool CanCollectFrameInformation
        {
            get => CanCollectAnimationInformation && CanCollectAnimationEntryInformation && SelectedFrameIndex != -1 && SelectedAnimation.Frames.Count > 0;
        }
        public bool CanCollectAnimationEntryInformation
        {
            get => CanCollectAnimationInformation && SelectedAnimationIndex != -1;
        }
        public bool CanCollectAnimationInformation
        {
            get => LoadedAnimationFile != null;
        }
        public bool isCurrentSpriteSheetValid
        {
            get
            {
                if ((int)CurrentFrame_SpriteSheet < SpriteSheets.Count)
                {
                    if (SpriteSheets[(int)CurrentFrame_SpriteSheet].isReady) return true;
                    else return false;
                }
                else return false;
            }

        }
        #endregion

        #region Current Animation Entry

        public byte? Loop
        {
            get
            {
                if (CanCollectAnimationEntryInformation) return SelectedAnimation.LoopIndex;
                else return 0;
            }
            set
            {
                if (CanCollectAnimationEntryInformation && value.HasValue) SelectedAnimation.LoopIndex = value.Value;
                else return;
            }
        }
        public short? Speed
        {
            get
            {
                if (CanCollectAnimationEntryInformation) return SelectedAnimation.SpeedMultiplyer;
                else return 0;
            }
            set
            {
                if (CanCollectAnimationEntryInformation && value.HasValue) SelectedAnimation.SpeedMultiplyer = value.Value;
                else return;
            }
        }
        public byte? Flags
        {
            get
            {
                if (CanCollectAnimationEntryInformation) return SelectedAnimation.RotationFlags;
                else return 0;
            }
            set
            {
                if (CanCollectAnimationEntryInformation && value.HasValue) SelectedAnimation.RotationFlags = value.Value;
                else return;
            }
        }


        #endregion

        #region Current Animation Frame

        #region General Coordinates
        public short? CurrentFrame_X
        {
            get
            {
                if (CanCollectFrameInformation) return SelectedFrame.X;
                else return 0;
            }
            set
            {
                if (CanCollectFrameInformation && value.HasValue) SelectedFrame.X = value.Value;
                else return;
            }
        }
        public short? CurrentFrame_Y
        {
            get
            {
                if (CanCollectFrameInformation) return SelectedFrame.Y;
                else return 0;
            }
            set
            {
                if (CanCollectFrameInformation && value.HasValue) SelectedFrame.Y = value.Value;
                else return;
            }
        }
        public short? CurrentFrame_Height
        {
            get
            {
                if (CanCollectFrameInformation) return SelectedFrame.Height;
                else return 0;
            }
            set
            {
                if (CanCollectFrameInformation && value.HasValue) SelectedFrame.Height = value.Value;
                else return;
            }
        }
        public short? CurrentFrame_Width
        {
            get
            {
                if (CanCollectFrameInformation) return SelectedFrame.Width;
                else return 0;
            }
            set
            {
                if (CanCollectFrameInformation && value.HasValue) SelectedFrame.Width = value.Value;
                else return;
            }
        }
        public short? CurrentFrame_PivotX
        {
            get
            {
                if (CanCollectFrameInformation) return SelectedFrame.PivotX;
                else return 0;
            }
            set
            {
                if (CanCollectFrameInformation && value.HasValue) SelectedFrame.PivotX = value.Value;
                else return;
            }
        }
        public short? CurrentFrame_PivotY
        {
            get
            {
                if (CanCollectFrameInformation) return SelectedFrame.PivotY;
                else return 0;
            }
            set
            {
                if (CanCollectFrameInformation && value.HasValue) SelectedFrame.PivotY = value.Value;
                else return;
            }
        }
        #endregion

        #region Indentifier Value
        public ushort? CurrentFrame_FrameID
        {
            get
            {
                if (CanCollectFrameInformation && LoadedAnimationFile.EngineType == EngineType.RSDKv5)
                {
                    return SelectedFrame.ID;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (CanCollectFrameInformation && value.HasValue && LoadedAnimationFile.EngineType == EngineType.RSDKv5)
                {
                    SelectedFrame.ID = value.Value;
                }
                else
                {
                    return;
                }
            }
        }
        public byte? CurrentFrame_CollisionBox
        {
            get
            {
                if (CanCollectFrameInformation && LoadedAnimationFile.EngineType == (EngineType.RSDKvB | EngineType.RSDKv2 | EngineType.RSDKv1)) return SelectedFrame.CollisionBox;
                else return 0x0;
            }
            set
            {
                if (CanCollectFrameInformation && LoadedAnimationFile.EngineType == (EngineType.RSDKvB | EngineType.RSDKv2 | EngineType.RSDKv1) && value.HasValue) SelectedFrame.CollisionBox = value.Value;
                else return;
            }
        }
        public short? CurrentFrame_FrameDuration
        {
            get
            {
                if (CanCollectFrameInformation) return SelectedFrame.Delay;
                else return 0;
            }
            set
            {
                if (CanCollectFrameInformation && value.HasValue) SelectedFrame.Delay = value.Value;
                else return;
            }
        }
        public byte? CurrentFrame_SpriteSheet
        {
            get
            {
                if (CanCollectFrameInformation) return SelectedFrame.SpriteSheet;
                else return 0;
            }
            set
            {
                if (CanCollectFrameInformation && value.HasValue) SelectedFrame.SpriteSheet = value.Value;
                else return;
            }
        }
        #endregion

        #region Hitbox Info
        public short? SelectedHitboxLeft
        {
            get
            {
                if (CanCollectFrameInformation && SelectedFrameHitboxIndex != -1 && SelectedFrame.HitBoxes.Count > 0) return SelectedFrame.HitBoxes[SelectedFrameHitboxIndex].Left;
                else return 0;
            }
            set
            {
                if (CanCollectFrameInformation && SelectedFrameHitboxIndex != -1 && value.HasValue)
                {
                    BridgedAnimation.BridgedHitBox box = SelectedFrame.HitBoxes.ElementAt(SelectedFrameHitboxIndex);
                    box.Left = value.Value;
                    SelectedFrame.HitBoxes[SelectedFrameHitboxIndex] = box;
                }
                else return;
            }
        }
        public short? SelectedHitboxTop
        {
            get
            {
                if (CanCollectFrameInformation && SelectedFrameHitboxIndex != -1 && SelectedFrame.HitBoxes.Count > 0) return SelectedFrame.HitBoxes[SelectedFrameHitboxIndex].Top;
                else return 0;
            }
            set
            {
                if (CanCollectFrameInformation && SelectedFrameHitboxIndex != -1 && value.HasValue)
                {
                    BridgedAnimation.BridgedHitBox box = SelectedFrame.HitBoxes.ElementAt(SelectedFrameHitboxIndex);
                    box.Top = value.Value;
                    SelectedFrame.HitBoxes[SelectedFrameHitboxIndex] = box;
                }
                else return;
            }
        }
        public short? SelectedHitboxRight
        {
            get
            {
                if (CanCollectFrameInformation && SelectedFrameHitboxIndex != -1 && SelectedFrame.HitBoxes.Count > 0) return SelectedFrame.HitBoxes[SelectedFrameHitboxIndex].Right;
                else return 0;
            }
            set
            {
                if (CanCollectFrameInformation && SelectedFrameHitboxIndex != -1 && value.HasValue)
                {
                    BridgedAnimation.BridgedHitBox box = SelectedFrame.HitBoxes.ElementAt(SelectedFrameHitboxIndex);
                    box.Right = value.Value;
                    SelectedFrame.HitBoxes[SelectedFrameHitboxIndex] = box;
                }
                else return;
            }
        }
        public short? SelectedHitboxBottom
        {
            get
            {
                if (CanCollectFrameInformation && SelectedFrameHitboxIndex != -1 && SelectedFrame.HitBoxes.Count > 0) return SelectedFrame.HitBoxes[SelectedFrameHitboxIndex].Bottom;
                else return 0;
            }
            set
            {
                if (CanCollectFrameInformation && SelectedFrameHitboxIndex != -1 && value.HasValue)
                {
                    BridgedAnimation.BridgedHitBox box = SelectedFrame.HitBoxes.ElementAt(SelectedFrameHitboxIndex);
                    box.Bottom = value.Value;
                    SelectedFrame.HitBoxes[SelectedFrameHitboxIndex] = box;
                }
                else return;
            }
        }
        #endregion

        #endregion

        #region Cropped Frames (for Frame Viewer)
        private List<System.Windows.Controls.Image> _AnimationFrameListSource { get; set; } = new List<System.Windows.Controls.Image>();
        public List<System.Windows.Controls.Image> AnimationFrameListSource
        {
            get
            {
                if (CanCollectAnimationEntryInformation)
                {
                    _AnimationFrameListSource.Clear();
                    for (int i = 0; i < SelectedAnimation.Frames.Count; i++)
                    {
                        System.Windows.Controls.Image frame = new System.Windows.Controls.Image();
                        frame.Width = 45;
                        frame.Height = 45;
                        frame.Source = GetCroppedFrame(i);
                        _AnimationFrameListSource.Add(frame);
                    }
                }
                return _AnimationFrameListSource;
            }
        }
        public BridgedAnimation.BridgedFrame GetAnimationFrameForCropping(int index)
        {
            if (CanCollectAnimationEntryInformation) return SelectedAnimation.Frames[index];
            else return null;
        }
        private Dictionary<Tuple<string, int>, BitmapSource> CroppedFrames { get; set; } = new Dictionary<Tuple<string, int>, BitmapSource>(1024);
        public BitmapSource GetCroppedFrame(int texture, BridgedAnimation.BridgedFrame frame)
        {
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
        public BitmapSource GetCroppedFrame(int index)
        {
            if (GetAnimationFrameForCropping(index) == null) return null;
            return GetCroppedFrame(GetAnimationFrameForCropping(index).SpriteSheet, GetAnimationFrameForCropping(index));
        }
        public void InvalidateCroppedFrame(int texture, BridgedAnimation.BridgedFrame frame)
        {
            if (texture < 0 || texture >= LoadedAnimationFile.SpriteSheets.Count)
                return;
            var name = LoadedAnimationFile.SpriteSheets[texture];
            CroppedFrames.Remove(new Tuple<string, int>(name, frame.GetHashCode()));
        }
        public void InvalidateCroppedFrame(int index)
        {
            if (GetAnimationFrameForCropping(index) == null) return;
            InvalidateCroppedFrame(GetAnimationFrameForCropping(index).SpriteSheet, GetAnimationFrameForCropping(index));
        }
        #endregion

        #region Animation and Frame Management

        public void ShiftFrameRight(int frameID)
        {
            int parentID = frameID + 1;
            if (parentID < 0 || parentID > SelectedAnimation.Frames.Count()) return;
            var targetFrame = SelectedAnimation.Frames[frameID];
            var parentFrame = SelectedAnimation.Frames[parentID];

            int parentIndex = SelectedAnimation.Frames.IndexOf(parentFrame);
            int targetIndex = SelectedAnimation.Frames.IndexOf(parentFrame);

            SelectedAnimation.Frames.Remove(targetFrame);
            SelectedAnimation.Frames.Insert(parentIndex, targetFrame);


        }

        public void ShiftFrameLeft(int frameID)
        {
            int parentID = frameID - 1;
            if (parentID < 0 || parentID > SelectedAnimation.Frames.Count()) return;
            var targetFrame = SelectedAnimation.Frames[frameID];
            var parentFrame = SelectedAnimation.Frames[parentID];

            int parentIndex = SelectedAnimation.Frames.IndexOf(parentFrame);
            int targetIndex = SelectedAnimation.Frames.IndexOf(parentFrame);

            SelectedAnimation.Frames.Remove(targetFrame);
            SelectedAnimation.Frames.Insert(parentIndex, targetFrame);
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
            if (frameID != -1) SelectedAnimation.Frames.RemoveAt(frameID);
        }

        public void AddFrame(int frameID)
        {
            var frame = new BridgedAnimation.BridgedFrame(AnimationType, SelectedAnimation);
            SelectedAnimation.Frames.Insert(frameID, frame);
        }

        public void DuplicateFrame(int frameID)
        { 
            var frame = (BridgedAnimation.BridgedFrame)SelectedAnimation.Frames[frameID].Clone();
            SelectedAnimation.Frames.Insert(frameID, frame);
        }

        public void RemoveAnimation(int animID)
        {
            LoadedAnimationFile.Animations.RemoveAt(animID);
        }

        public void AddAnimation(int animID)
        {
            var animation = new BridgedAnimation.BridgedAnimationEntry(AnimationType, LoadedAnimationFile);
            animation.AnimName = "New Entry";
            animation.Frames = new List<BridgedAnimation.BridgedFrame>();
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
