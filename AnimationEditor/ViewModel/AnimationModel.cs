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

namespace AnimationEditor.ViewModel
{
    public class AnimationModel
    {
        #region Loaded Animation Data
        public EditorAnimation LoadedAnimationFile { get; set; }
        #endregion

        #region Methods

        public int GetFirstIndex(int count)
        {
            if (count - 1 >= 0)
            {
                return 0;
            }
            else return -1;
        }
        public int GetIndexWithinRange(int count, int index)
        {
            if (count - 1 >= index && index >= 0)
            {
                return index;
            }
            else if (index >= count - 1)
            {
                return count - 1;
            }
            else if (count - 1 >= 0)
            {
                return 0;
            }
            else return -1;
        }

        #endregion

        #region IO Paths
        public string AnimationFilepath { get; set; }
        public string AnimationDirectory { get; set; }
        public string SpriteDirectory { get; set; }
        #endregion

        #region Modes/States
        public double Zoom { get; set; } = 1;

        #endregion

        #region Details
        public EngineType AnimationType { get; set; } = EngineType.RSDKv5;
        public int FramesCount
        {
            get
            {
                if (isAnimationInfoSelected) return SelectedAnimation.Frames.Count - 1;
                else return -1;
            }
        }
        public int AnimationsCount
        {
            get
            {
                if (isAnimationInfoSelected) return LoadedAnimationFile.Animations.Count - 1;
                else return -1;
            }
        }
        #endregion

        #region Selected Items
        public List<EditorAnimation.EditorAnimationInfo> SelectedAnimationEntries
        {
            get
            {
                if (isAnimationFileLoaded) return LoadedAnimationFile.Animations;
                else return null;
            }
            set
            {
                if (isAnimationFileLoaded) LoadedAnimationFile.Animations = value;
            }
        }
        public EditorAnimation.EditorAnimationInfo SelectedAnimation
        {
            get
            {
                if (isAnimationFileLoaded && isAnimationIndexInRange()) return LoadedAnimationFile.Animations[SelectedAnimationIndex];
                else return null;
            }
            set
            {
                if (isAnimationFileLoaded && isAnimationIndexInRange()) LoadedAnimationFile.Animations[SelectedAnimationIndex] = value;
            }
        }
        public List<EditorAnimation.EditorFrame> SelectedAnimationFrameSet
        {
            get
            {
                if (isAnimationInfoSelected) return SelectedAnimation.Frames;
                else return null;
            }
            set
            {
                if (isAnimationInfoSelected)
                {
                    SelectedAnimation.Frames = value;
                }
            }
        }
        public EditorAnimation.EditorFrame SelectedFrame
        {
            get
            {
                if (isAnimationInfoSelected && isFrameIndexInRange()) return SelectedAnimation.Frames[SelectedFrameIndex];
                else return null;
            }
            set 
            {
                if (isAnimationInfoSelected && isFrameIndexInRange()) SelectedAnimation.Frames[SelectedFrameIndex] = value;
            }
        }
        public EditorAnimation.EditorHitbox SelectedHitbox
        {
            get
            {
                if (isAnimationFrameSelected && SelectedFrameHitboxIndex != -1 && SelectedFrame.HitBoxes != null && SelectedFrame.HitBoxes.Count > 0) return SelectedFrame.HitBoxes[SelectedFrameHitboxIndex];
                else return null;
            }
            set
            {
                if (isAnimationFrameSelected && SelectedFrameHitboxIndex != -1 && SelectedFrame.HitBoxes != null) SelectedFrame.HitBoxes[SelectedFrameHitboxIndex] = value;
                else return;
            }
        }
        #endregion

        #region Selected Indexes

        public int SelectedFrameIndex { get; set; }
        public int SelectedAnimationIndex { get; set; }
        public int SelectedFrameHitboxIndex { get; set; }
        public int GetTotalFrameCount()
        {
            int count = -1;
            if (LoadedAnimationFile != null)
            {
                count = 0;
                foreach (var entry in LoadedAnimationFile.Animations)
                {
                    count += entry.Frames.Count;
                }
            }
            return count;
        }
        public int GetCurrentFrameIndexForAllAnimations()
        {
            if (isAnimationFrameSelected)
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
                if (isAnimationFileLoaded) return LoadedAnimationFile.CollisionBoxes;
                else return new List<string>();
            }
            set
            {
                if (isAnimationFileLoaded) LoadedAnimationFile.CollisionBoxes = value;
                else return;
            }
        }
        public List<string> RetroHitboxStrings
        {
            get
            {
                if (isAnimationFileLoaded)
                {
                    List<string> output = new List<string>();
                    for (int i = 1; i <= LoadedAnimationFile.RetroCollisionBoxes.Count; i++)
                    {
                        output.Add(string.Format("Collision Set #{0}", i));
                    }
                    return output;
                }
                else return new List<string>();
            }
        }
        public List<EditorAnimation.EditorRetroHitBox> RetroHitboxes
        {
            get
            {
                if (isAnimationFileLoaded) return LoadedAnimationFile.RetroCollisionBoxes;
                else return new List<EditorAnimation.EditorRetroHitBox>();
            }
            set
            {
                if (isAnimationFileLoaded) LoadedAnimationFile.RetroCollisionBoxes = value;
                else return;
            }
        }
        public byte PlayerType
        {
            get
            {
                if (isAnimationFrameSelected)
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
                if (isAnimationFrameSelected)
                {
                    if (LoadedAnimationFile.EngineType == EngineType.RSDKvRS)
                    {
                        LoadedAnimationFile.PlayerType = value;
                    }
                }
                return;
            }
        }
        public bool DreamcastVer
        {
            get
            {
                if (isAnimationFileLoaded)
                {
                    return LoadedAnimationFile.DreamcastVer;
                }
                return false;
            }
            set
            {
                if (isAnimationFileLoaded)
                {
                    LoadedAnimationFile.DreamcastVer = value;
                }
                return;
            }
        }
        public byte Unknown
        {
            get
            {
                if (isAnimationFileLoaded)
                {
                    return LoadedAnimationFile.Unknown;
                }
                return 0;
            }
            set
            {
                if (isAnimationFileLoaded)
                {
                    LoadedAnimationFile.Unknown = value;
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
                if (isAnimationFileLoaded) return LoadedAnimationFile.SpriteSheets;
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
        public bool isAnimationFrameSelected
        {
            get => isAnimationFileLoaded && isAnimationInfoSelected && SelectedFrame != null;
        }
        public bool isAnimationIndexInRange(int? index = null)
        {
            if (index == null) index = SelectedAnimationIndex;

            if (index >= 0 && index < SelectedAnimationEntries.Count) return true;
            else return false;
        }
        public bool isFrameIndexInRange(int? index = null)
        {
            if (index == null) index = SelectedFrameIndex;

            if (index >= 0 && index < SelectedAnimationFrameSet.Count) return true;
            else return false;
        }
        public bool isAnimationInfoSelected
        {
            get => isAnimationFileLoaded && SelectedAnimation != null;
        }
        public bool isAnimationFileLoaded
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
                if (isAnimationInfoSelected) return SelectedAnimation.LoopIndex;
                else return 0;
            }
            set
            {
                if (isAnimationInfoSelected && value.HasValue) SelectedAnimation.LoopIndex = value.Value;
                else return;
            }
        }
        public short? Speed
        {
            get
            {
                if (isAnimationInfoSelected) return SelectedAnimation.SpeedMultiplyer;
                else return 0;
            }
            set
            {
                if (isAnimationInfoSelected && value.HasValue) SelectedAnimation.SpeedMultiplyer = value.Value;
                else return;
            }
        }
        public byte? Flags
        {
            get
            {
                if (isAnimationInfoSelected) return SelectedAnimation.RotationFlags;
                else return 0;
            }
            set
            {
                if (isAnimationInfoSelected && value.HasValue) SelectedAnimation.RotationFlags = value.Value;
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
                if (isAnimationFrameSelected) return SelectedFrame.X;
                else return 0;
            }
            set
            {
                if (isAnimationFrameSelected && value.HasValue) SelectedFrame.X = value.Value;
                else return;
            }
        }
        public short? CurrentFrame_Y
        {
            get
            {
                if (isAnimationFrameSelected) return SelectedFrame.Y;
                else return 0;
            }
            set
            {
                if (isAnimationFrameSelected && value.HasValue) SelectedFrame.Y = value.Value;
                else return;
            }
        }
        public short? CurrentFrame_Height
        {
            get
            {
                if (isAnimationFrameSelected) return SelectedFrame.Height;
                else return 0;
            }
            set
            {
                if (isAnimationFrameSelected && value.HasValue) SelectedFrame.Height = value.Value;
                else return;
            }
        }
        public short? CurrentFrame_Width
        {
            get
            {
                if (isAnimationFrameSelected) return SelectedFrame.Width;
                else return 0;
            }
            set
            {
                if (isAnimationFrameSelected && value.HasValue) SelectedFrame.Width = value.Value;
                else return;
            }
        }
        public short? CurrentFrame_PivotX
        {
            get
            {
                if (isAnimationFrameSelected) return SelectedFrame.PivotX;
                else return 0;
            }
            set
            {
                if (isAnimationFrameSelected && value.HasValue) SelectedFrame.PivotX = value.Value;
                else return;
            }
        }
        public short? CurrentFrame_PivotY
        {
            get
            {
                if (isAnimationFrameSelected) return SelectedFrame.PivotY;
                else return 0;
            }
            set
            {
                if (isAnimationFrameSelected && value.HasValue) SelectedFrame.PivotY = value.Value;
                else return;
            }
        }
        #endregion

        #region Indentifier Value
        public ushort? CurrentFrame_FrameID
        {
            get
            {
                if (isAnimationFrameSelected)
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
                if (isAnimationFrameSelected)
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
                if (isAnimationFrameSelected) return SelectedFrame.CollisionBox;
                else return 0x0;
            }
            set
            {
                if (isAnimationFrameSelected) SelectedFrame.CollisionBox = value.Value;
                else return;
            }
        }
        public short? CurrentFrame_FrameDuration
        {
            get
            {
                if (isAnimationFrameSelected) return SelectedFrame.Delay;
                else return 0;
            }
            set
            {
                if (isAnimationFrameSelected && value.HasValue) SelectedFrame.Delay = value.Value;
                else return;
            }
        }
        public byte? CurrentFrame_SpriteSheet
        {
            get
            {
                if (isAnimationFrameSelected) return SelectedFrame.SpriteSheet;
                else return 0;
            }
            set
            {
                if (isAnimationFrameSelected && value.HasValue) SelectedFrame.SpriteSheet = value.Value;
                else return;
            }
        }
        #endregion

        #region Hitbox Info
        public short? SelectedHitboxLeft
        {
            get
            {
                if (AnimationType == EngineType.RSDKv5) return SelectedHitboxLeft_v5;
                else return SelectedHitboxLeft_v3;
            }
            set
            {
                if (AnimationType == EngineType.RSDKv5) SelectedHitboxLeft_v5 = value;
                else SelectedHitboxLeft_v3 = value;
            }
        }
        public short? SelectedHitboxTop
        {
            get
            {
                if (AnimationType == EngineType.RSDKv5) return SelectedHitboxTop_v5;
                else return SelectedHitboxTop_v3;
            }
            set
            {
                if (AnimationType == EngineType.RSDKv5) SelectedHitboxTop_v5 = value;
                else SelectedHitboxTop_v3 = value;
            }
        }
        public short? SelectedHitboxRight
        {
            get
            {
                if (AnimationType == EngineType.RSDKv5) return SelectedHitboxRight_v5;
                else return SelectedHitboxRight_v3;
            }
            set
            {
                if (AnimationType == EngineType.RSDKv5) SelectedHitboxRight_v5 = value;
                else SelectedHitboxRight_v3 = value;
            }
        }
        public short? SelectedHitboxBottom
        {
            get
            {
                if (AnimationType == EngineType.RSDKv5) return SelectedHitboxBottom_v5;
                else return SelectedHitboxBottom_v3;
            }
            set
            {
                if (AnimationType == EngineType.RSDKv5) SelectedHitboxBottom_v5 = value;
                else SelectedHitboxBottom_v3 = value;
            }
        }

        #region RSDKv5 Hitboxes
        public short? SelectedHitboxLeft_v5
        {
            get
            {
                if (isAnimationFrameSelected && SelectedFrameHitboxIndex != -1 && SelectedFrame.HitBoxes.Count > 0) return SelectedFrame.HitBoxes[SelectedFrameHitboxIndex].Left;
                else return 0;
            }
            set
            {
                if (isAnimationFrameSelected && SelectedFrameHitboxIndex != -1 && value.HasValue)
                {
                    EditorAnimation.EditorHitbox box = SelectedFrame.HitBoxes.ElementAt(SelectedFrameHitboxIndex);
                    box.Left = value.Value;
                    SelectedFrame.HitBoxes[SelectedFrameHitboxIndex] = box;
                }
                else return;
            }
        }
        public short? SelectedHitboxTop_v5
        {
            get
            {
                if (isAnimationFrameSelected && SelectedFrameHitboxIndex != -1 && SelectedFrame.HitBoxes.Count > 0) return SelectedFrame.HitBoxes[SelectedFrameHitboxIndex].Top;
                else return 0;
            }
            set
            {
                if (isAnimationFrameSelected && SelectedFrameHitboxIndex != -1 && value.HasValue)
                {
                    EditorAnimation.EditorHitbox box = SelectedFrame.HitBoxes.ElementAt(SelectedFrameHitboxIndex);
                    box.Top = value.Value;
                    SelectedFrame.HitBoxes[SelectedFrameHitboxIndex] = box;
                }
                else return;
            }
        }
        public short? SelectedHitboxRight_v5
        {
            get
            {
                if (isAnimationFrameSelected && SelectedFrameHitboxIndex != -1 && SelectedFrame.HitBoxes.Count > 0) return SelectedFrame.HitBoxes[SelectedFrameHitboxIndex].Right;
                else return 0;
            }
            set
            {
                if (isAnimationFrameSelected && SelectedFrameHitboxIndex != -1 && value.HasValue)
                {
                    EditorAnimation.EditorHitbox box = SelectedFrame.HitBoxes.ElementAt(SelectedFrameHitboxIndex);
                    box.Right = value.Value;
                    SelectedFrame.HitBoxes[SelectedFrameHitboxIndex] = box;
                }
                else return;
            }
        }
        public short? SelectedHitboxBottom_v5
        {
            get
            {
                if (isAnimationFrameSelected && SelectedFrameHitboxIndex != -1 && SelectedFrame.HitBoxes.Count > 0) return SelectedFrame.HitBoxes[SelectedFrameHitboxIndex].Bottom;
                else return 0;
            }
            set
            {
                if (isAnimationFrameSelected && SelectedFrameHitboxIndex != -1 && value.HasValue)
                {
                    EditorAnimation.EditorHitbox box = SelectedFrame.HitBoxes.ElementAt(SelectedFrameHitboxIndex);
                    box.Bottom = value.Value;
                    SelectedFrame.HitBoxes[SelectedFrameHitboxIndex] = box;
                }
                else return;
            }
        }
        #endregion

        #region Pre-RSDKv5 Hitboxes
        public short? SelectedHitboxLeft_v3
        {
            get
            {
                int index = (int)CurrentFrame_CollisionBox;
                if (isAnimationFrameSelected && SelectedFrameHitboxIndex != -1 && RetroHitboxes.Count() - 1 >= index)
                {
                    return RetroHitboxes[index].Hitboxes[SelectedFrameHitboxIndex].Left;
                }
                else return 0;
            }
            set
            {
                int index = (int)CurrentFrame_CollisionBox;
                if (isAnimationFrameSelected && SelectedFrameHitboxIndex != -1 && value.HasValue && RetroHitboxes.Count() - 1 >= index)
                {
                    RetroHitboxes[index].Hitboxes[SelectedFrameHitboxIndex].Left = (sbyte)value.Value;
                }
                else return;
            }
        }
        public short? SelectedHitboxTop_v3
        {
            get
            {
                int index = (int)CurrentFrame_CollisionBox;
                if (isAnimationFrameSelected && SelectedFrameHitboxIndex != -1 && RetroHitboxes.Count() - 1 >= index)
                {
                    return RetroHitboxes[index].Hitboxes[SelectedFrameHitboxIndex].Top;
                }
                else return 0;
            }
            set
            {
                int index = (int)CurrentFrame_CollisionBox;
                if (isAnimationFrameSelected && SelectedFrameHitboxIndex != -1 && value.HasValue && RetroHitboxes.Count() - 1 >= index)
                {
                    RetroHitboxes[index].Hitboxes[SelectedFrameHitboxIndex].Top = (sbyte)value.Value;
                }
                else return;
            }
        }
        public short? SelectedHitboxRight_v3
        {
            get
            {
                int index = (int)CurrentFrame_CollisionBox;
                if (isAnimationFrameSelected && SelectedFrameHitboxIndex != -1 && RetroHitboxes.Count() - 1 >= index)
                {
                    return RetroHitboxes[index].Hitboxes[SelectedFrameHitboxIndex].Right;
                }
                else return 0;
            }
            set
            {
                int index = (int)CurrentFrame_CollisionBox;
                if (isAnimationFrameSelected && SelectedFrameHitboxIndex != -1 && value.HasValue && RetroHitboxes.Count() - 1 >= index)
                {
                    RetroHitboxes[index].Hitboxes[SelectedFrameHitboxIndex].Right = (sbyte)value.Value;
                }
                else return;
            }
        }
        public short? SelectedHitboxBottom_v3
        {
            get
            {
                int index = (int)CurrentFrame_CollisionBox;
                if (isAnimationFrameSelected && SelectedFrameHitboxIndex != -1 && RetroHitboxes.Count() - 1 >= index)
                {
                    return RetroHitboxes[index].Hitboxes[SelectedFrameHitboxIndex].Bottom;
                }
                else return 0;
            }
            set
            {
                int index = (int)CurrentFrame_CollisionBox;
                if (isAnimationFrameSelected && SelectedFrameHitboxIndex != -1 && value.HasValue && RetroHitboxes.Count() - 1 >= index)
                {
                    RetroHitboxes[index].Hitboxes[SelectedFrameHitboxIndex].Bottom = (sbyte)value.Value;
                }
                else return;
            }
        }
        #endregion

        #endregion

        #endregion

        #region Cropped Frames (for Frame Viewer)
        private List<System.Windows.Controls.Image> _AnimationFrameListSource { get; set; } = new List<System.Windows.Controls.Image>();
        public List<System.Windows.Controls.Image> AnimationFrameListSource
        {
            get
            {
                if (isAnimationInfoSelected)
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
        public EditorAnimation.EditorFrame GetAnimationFrameForCropping(int index)
        {
            if (isAnimationInfoSelected) return SelectedAnimation.Frames.TryGetElement(index, null);
            else return null;
        }
        private Dictionary<Tuple<string, int>, BitmapSource> CroppedFrames { get; set; } = new Dictionary<Tuple<string, int>, BitmapSource>(1024);
        public BitmapSource GetCroppedFrame(int texture, EditorAnimation.EditorFrame frame)
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
        public void InvalidateCroppedFrame(int texture, EditorAnimation.EditorFrame frame)
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
            if (parentID < 0 || parentID > SelectedAnimation.Frames.Count() - 1) return;
            var targetFrame = SelectedAnimation.Frames[frameID];
            var parentFrame = SelectedAnimation.Frames[parentID];

            int parentIndex = SelectedAnimation.Frames.IndexOf(parentFrame);

            SelectedAnimation.Frames.Remove(targetFrame);
            SelectedAnimation.Frames.Insert(parentIndex, targetFrame);

            SelectedFrameIndex = SelectedAnimation.Frames.IndexOf(targetFrame);
        }

        public void ShiftFrameLeft(int frameID)
        {
            int parentID = frameID - 1;
            if (parentID < 0 || parentID > SelectedAnimation.Frames.Count() - 1) return;
            var targetFrame = SelectedAnimation.Frames[frameID];
            var parentFrame = SelectedAnimation.Frames[parentID];

            int parentIndex = SelectedAnimation.Frames.IndexOf(parentFrame);

            SelectedAnimation.Frames.Remove(targetFrame);
            SelectedAnimation.Frames.Insert(parentIndex, targetFrame);

            SelectedFrameIndex = SelectedAnimation.Frames.IndexOf(targetFrame);
        }

        public void ShiftAnimationUp(int animID)
        {
            if (LoadedAnimationFile == null) return;
            int parentID = animID - 1;
            if (parentID < 0 || parentID > LoadedAnimationFile.Animations.Count() - 1) return;
            var targetAnimation = LoadedAnimationFile.Animations[animID];
            var parentAnimation = LoadedAnimationFile.Animations[parentID];

            int parentIndex = LoadedAnimationFile.Animations.IndexOf(parentAnimation);

            LoadedAnimationFile.Animations.Remove(targetAnimation);
            LoadedAnimationFile.Animations.Insert(parentIndex, targetAnimation);


            SelectedAnimationIndex = LoadedAnimationFile.Animations.IndexOf(targetAnimation);
        }

        public void ShiftAnimationDown(int animID)
        {
            if (LoadedAnimationFile == null) return;
            int parentID = animID + 1;
            if (parentID < 0 || parentID > LoadedAnimationFile.Animations.Count() - 1) return;
            var targetAnimation = LoadedAnimationFile.Animations[animID];
            var parentAnimation = LoadedAnimationFile.Animations[parentID];

            int parentIndex = LoadedAnimationFile.Animations.IndexOf(parentAnimation);

            LoadedAnimationFile.Animations.Remove(targetAnimation);
            LoadedAnimationFile.Animations.Insert(parentIndex, targetAnimation);

            SelectedAnimationIndex = LoadedAnimationFile.Animations.IndexOf(targetAnimation);
        }

        public void RemoveFrame(int frameID)
        {
            if (GenerationsLib.Core.ListHelpers.IsInRange(SelectedAnimation.Frames, frameID)) SelectedAnimation.Frames.RemoveAt(frameID);
        }

        public void AddFrame(int frameID)
        {
            var frame = new EditorAnimation.EditorFrame(AnimationType, SelectedAnimation);
            if (GenerationsLib.Core.ListHelpers.IsInRange(SelectedAnimation.Frames, frameID) || SelectedAnimation.Frames.Count == 0) SelectedAnimation.Frames.Insert(frameID, frame);
        }

        public void DuplicateFrame(int frameID)
        {           
            var frame = (EditorAnimation.EditorFrame)SelectedAnimation.Frames[frameID].Clone();
            if (GenerationsLib.Core.ListHelpers.IsInRange(SelectedAnimation.Frames, frameID)) SelectedAnimation.Frames.Insert(frameID, frame);
        }

        public void RemoveAnimation(int animID)
        {
            if (GenerationsLib.Core.ListHelpers.IsInRange(LoadedAnimationFile.Animations, animID)) LoadedAnimationFile.Animations.RemoveAt(animID);
        }

        public void AddAnimation(int animID)
        {
            var animation = new EditorAnimation.EditorAnimationInfo(AnimationType, LoadedAnimationFile);
            animation.AnimName = "New Entry";
            animation.Frames = new List<EditorAnimation.EditorFrame>();
            if (GenerationsLib.Core.ListHelpers.IsInRange(LoadedAnimationFile.Animations, animID) || LoadedAnimationFile.Animations.Count == 0) LoadedAnimationFile.Animations.Insert(animID, animation);
        }

        public void DuplicateAnimation(int animID)
        {
            var animation = LoadedAnimationFile.Animations[animID];
            if (GenerationsLib.Core.ListHelpers.IsInRange(LoadedAnimationFile.Animations, animID)) LoadedAnimationFile.Animations.Insert(animID, animation);
        }


        #endregion
    }
}
