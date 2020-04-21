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
    public class AnimationModel : Xe.Tools.Wpf.BaseNotifyPropertyChanged
    {
        #region OnPropertyChanged Extension Methods

        public void CallPropertyChanged(String entry)
        {
            OnPropertyChanged(entry);
        }

        #endregion

        #region Loaded Animation Data
        private EditorAnimation _LoadedAnimationFile;
        public EditorAnimation LoadedAnimationFile
        {
            get
            {
                return _LoadedAnimationFile;
            }
            set
            {
                _LoadedAnimationFile = value;
                GlobalService.SpriteService.SetAnimation(value);
                OnPropertyChanged(nameof(LoadedAnimationFile));
            }
        }
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
        public ObservableCollection<EditorAnimation.EditorAnimationInfo> SelectedAnimationEntries
        {
            get
            {
                if (isAnimationFileLoaded) return new ObservableCollection<EditorAnimation.EditorAnimationInfo>(LoadedAnimationFile.Animations);
                else return null;
            }
            set
            {
                if (isAnimationFileLoaded) LoadedAnimationFile.Animations = value.ToList();
                OnPropertyChanged(nameof(SelectedAnimationEntries));
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
        public ObservableCollection<EditorAnimation.EditorFrame> SelectedAnimationFrameSet
        {
            get
            {
                if (isAnimationInfoSelected) return new ObservableCollection<EditorAnimation.EditorFrame>(SelectedAnimation.Frames);
                else return null;
            }
            set
            {
                if (isAnimationInfoSelected)
                {
                    SelectedAnimation.Frames = value.ToList();
                }
                OnPropertyChanged(nameof(SelectedAnimationFrameSet));
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

        public int _SelectedFrameIndex;
        public int _SelectedAnimationIndex;
        public int _SelectedFrameHitboxIndex;

        public int SelectedFrameIndex
        {
            get
            {
                return _SelectedFrameIndex;
            }
            set
            {
                _SelectedFrameIndex = value;
                OnPropertyChanged(nameof(SelectedFrameIndex));
            }
        }
        public int SelectedAnimationIndex
        {
            get
            {
                return _SelectedAnimationIndex;
            }
            set
            {
                _SelectedAnimationIndex = value;
                OnPropertyChanged(nameof(SelectedAnimationIndex));
            }
        }
        public int SelectedFrameHitboxIndex
        {
            get
            {
                if (SelectedFrame != null) return _SelectedFrameHitboxIndex;
                else return -1;
            }
            set
            {
                _SelectedFrameHitboxIndex = value;
                OnPropertyChanged(nameof(SelectedFrameHitboxIndex));
            }
        }




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
        public ObservableCollection<string> Hitboxes
        {
            get
            {
                if (isAnimationFileLoaded) return new ObservableCollection<string>(LoadedAnimationFile.CollisionBoxes);
                else return new ObservableCollection<string>();
            }
            set
            {
                if (isAnimationFileLoaded) LoadedAnimationFile.CollisionBoxes = value.ToList();
                OnPropertyChanged(nameof(Hitboxes));
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
        public ObservableCollection<EditorAnimation.EditorRetroHitBox> RetroHitboxes
        {
            get
            {
                if (isAnimationFileLoaded) return new ObservableCollection<EditorAnimation.EditorRetroHitBox>(LoadedAnimationFile.RetroCollisionBoxes);
                else return new ObservableCollection<EditorAnimation.EditorRetroHitBox>();
            }
            set
            {
                if (isAnimationFileLoaded) LoadedAnimationFile.RetroCollisionBoxes = value.ToList();
                OnPropertyChanged(nameof(RetroHitboxes));
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
                if ((int)CurrentFrame_SpriteSheet < Services.GlobalService.SpriteService.SpriteSheets.Count)
                {
                    if (Services.GlobalService.SpriteService.SpriteSheets[(int)CurrentFrame_SpriteSheet].isReady) return true;
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
                OnPropertyChanged(nameof(Loop));
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
                OnPropertyChanged(nameof(Speed));
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
                OnPropertyChanged(nameof(Flags));
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
                OnPropertyChanged(nameof(PlayerType));
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
                OnPropertyChanged(nameof(DreamcastVer));
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
                OnPropertyChanged(nameof(Unknown));
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
                OnPropertyChanged(nameof(CurrentFrame_X));
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
                OnPropertyChanged(nameof(CurrentFrame_Y));
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
                OnPropertyChanged(nameof(CurrentFrame_Height));
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
                OnPropertyChanged(nameof(CurrentFrame_Width));
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
                OnPropertyChanged(nameof(CurrentFrame_PivotX));
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
                OnPropertyChanged(nameof(CurrentFrame_PivotY));
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
                OnPropertyChanged(nameof(CurrentFrame_FrameID));
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
                OnPropertyChanged(nameof(CurrentFrame_CollisionBox));
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
                OnPropertyChanged(nameof(CurrentFrame_FrameDuration));
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
                OnPropertyChanged(nameof(CurrentFrame_SpriteSheet));
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

        #region Sprite Sheet Service

        public ObservableCollection<Spritesheet> SpriteSheets
        {
            get
            {
                return Services.GlobalService.SpriteService.SpriteSheets;
            }
            set
            {
                Services.GlobalService.SpriteService.SpriteSheets = value;
                OnPropertyChanged(nameof(SpriteSheets));
            }
        }
        public List<string> SpriteSheetPaths
        {
            get
            {
                if (isAnimationFileLoaded) return LoadedAnimationFile.SpriteSheets;
                else return null;
            }
        }
        public ObservableCollection<string> NullSpriteSheetList
        {
            get
            {
                return Services.GlobalService.SpriteService.NullSpriteSheetList;
            }
            set
            {
                Services.GlobalService.SpriteService.NullSpriteSheetList = value;
                OnPropertyChanged(nameof(NullSpriteSheetList));
            }
        }
        public EditorAnimation.EditorFrame GetAnimationFrameForCropping(int index)
        {
            if (isAnimationInfoSelected) return SelectedAnimation.Frames.TryGetElement(index, null);
            else return null;
        }
        public void InvalidateCroppedFrame(int index)
        {
            var frame = GetAnimationFrameForCropping(index);
            if (frame == null) return;
            Services.GlobalService.SpriteService.InvalidateCroppedFrame(frame.SpriteSheet, frame);
        }

        public BitmapSource GetCroppedFrame(int index)
        {
            var frame = GetAnimationFrameForCropping(index);
            if (frame == null) return null;
            return Services.GlobalService.SpriteService.GetCroppedFrame(frame.SpriteSheet, frame);
        }

        #endregion

        #region Animation and Frame Management

        public void ShiftFrameRight(int frameID)
        {
            var currentFrames = SelectedAnimationFrameSet;
            int parentID = frameID + 1;
            if (parentID < 0 || parentID > currentFrames.Count() - 1) return;
            var targetFrame = currentFrames[frameID];
            var parentFrame = currentFrames[parentID];

            int parentIndex = currentFrames.IndexOf(parentFrame);

            currentFrames.Remove(targetFrame);
            currentFrames.Insert(parentIndex, targetFrame);

            SelectedFrameIndex = currentFrames.IndexOf(targetFrame);
            SelectedAnimationFrameSet = currentFrames;
        }

        public void ShiftFrameLeft(int frameID)
        {
            var currentFrames = SelectedAnimationFrameSet;
            int parentID = frameID - 1;
            if (parentID < 0 || parentID > currentFrames.Count() - 1) return;
            var targetFrame = currentFrames[frameID];
            var parentFrame = currentFrames[parentID];

            int parentIndex = currentFrames.IndexOf(parentFrame);

            currentFrames.Remove(targetFrame);
            currentFrames.Insert(parentIndex, targetFrame);

            SelectedFrameIndex = currentFrames.IndexOf(targetFrame);
            SelectedAnimationFrameSet = currentFrames;
        }

        public void ShiftAnimationUp(int animID)
        {
            var currentAnimations = SelectedAnimationEntries;
            if (LoadedAnimationFile == null) return;
            int parentID = animID - 1;
            if (parentID < 0 || parentID > currentAnimations.Count() - 1) return;
            var targetAnimation = currentAnimations[animID];
            var parentAnimation = currentAnimations[parentID];

            int parentIndex = currentAnimations.IndexOf(parentAnimation);

            currentAnimations.Remove(targetAnimation);
            currentAnimations.Insert(parentIndex, targetAnimation);


            SelectedAnimationIndex = LoadedAnimationFile.Animations.IndexOf(targetAnimation);
            SelectedAnimationEntries = currentAnimations;
        }

        public void ShiftAnimationDown(int animID)
        {
            var currentAnimations = SelectedAnimationEntries;
            if (LoadedAnimationFile == null) return;
            int parentID = animID + 1;
            if (parentID < 0 || parentID > currentAnimations.Count() - 1) return;
            var targetAnimation = currentAnimations[animID];
            var parentAnimation = currentAnimations[parentID];

            int parentIndex = currentAnimations.IndexOf(parentAnimation);

            currentAnimations.Remove(targetAnimation);
            currentAnimations.Insert(parentIndex, targetAnimation);

            SelectedAnimationIndex = currentAnimations.IndexOf(targetAnimation);
            SelectedAnimationEntries = currentAnimations;
        }

        public void RemoveFrame(int frameID)
        {
            var currentFrames = SelectedAnimationFrameSet;
            if (GenerationsLib.Core.ListHelpers.IsInRange(currentFrames, frameID)) currentFrames.RemoveAt(frameID);
            SelectedAnimationFrameSet = currentFrames;
        }

        public void AddFrame(int frameID)
        {
            var currentFrames = SelectedAnimationFrameSet;
            var frame = new EditorAnimation.EditorFrame(AnimationType, SelectedAnimation);
            if (GenerationsLib.Core.ListHelpers.IsInRange(currentFrames, frameID) || currentFrames.Count == 0) currentFrames.Insert(frameID, frame);
            SelectedAnimationFrameSet = currentFrames;
        }

        public void DuplicateFrame(int frameID)
        {
            var currentFrames = SelectedAnimationFrameSet;
            var frame = (EditorAnimation.EditorFrame)currentFrames[frameID].Clone();
            if (GenerationsLib.Core.ListHelpers.IsInRange(currentFrames, frameID)) currentFrames.Insert(frameID, frame);
            SelectedAnimationFrameSet = currentFrames;
        }

        public void RemoveAnimation(int animID)
        {
            var currentAnimations = SelectedAnimationEntries;
            if (GenerationsLib.Core.ListHelpers.IsInRange(currentAnimations, animID)) currentAnimations.RemoveAt(animID);
            SelectedAnimationEntries = currentAnimations;
        }

        public void AddAnimation(int animID)
        {
            var currentAnimations = SelectedAnimationEntries;
            var animation = new EditorAnimation.EditorAnimationInfo(AnimationType, LoadedAnimationFile);
            animation.AnimName = "New Entry";
            animation.Frames = new List<EditorAnimation.EditorFrame>();
            if (GenerationsLib.Core.ListHelpers.IsInRange(currentAnimations, animID) || currentAnimations.Count == 0) currentAnimations.Insert(animID, animation);
            SelectedAnimationEntries = currentAnimations;
        }

        public void DuplicateAnimation(int animID)
        {
            var currentAnimations = SelectedAnimationEntries;
            var animation = currentAnimations[animID];
            if (GenerationsLib.Core.ListHelpers.IsInRange(currentAnimations, animID)) currentAnimations.Insert(animID, animation);
            SelectedAnimationEntries = currentAnimations;
        }


        #endregion
    }
}
