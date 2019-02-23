using System;
using System.Collections.Generic;
using AnimationEditor.Services;
using RSDKvB;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace AnimationEditor.ViewModels
{
    public class ViewModelvB
    {
        public Animation LoadedAnimationFile;
        public Animation.AnimationEntry _SelectedAnimation;

        public List<Animation.AnimationEntry> Animations { get => GetAnimations(); }
        public List<string> SpriteSheets { get => GetSpriteSheetsList(); }
        public Animation.AnimationEntry SelectedAnimation { get => _SelectedAnimation; set { _SelectedAnimation = value; } }
        public int SelectedAnimationIndex { get; set; }
        public List<Animation.AnimationEntry.Frame> AnimationFrames { get => GetAnimationsFrames(); }
        public int SelectedFrameIndex { get; set; }

        public byte? Loop { get => GetLoopIndex(); set => SetLoopIndex(value); }

        public byte? Speed { get => GetSpeedMultiplyer(); set => SetSpeedMultiplyer(value); }

        public byte? Flags { get => GetRotationFlag(); set => SetRotationFlag(value); }


        public List<Animation.AnimationEntry> GetAnimations()
        {
            if (LoadedAnimationFile != null) return LoadedAnimationFile.Animations;
            else return null;
        }
        public List<string> GetSpriteSheetsList()
        {
            if (LoadedAnimationFile != null) return LoadedAnimationFile.SpriteSheets;
            else return null;
        }
        public List<Animation.AnimationEntry.Frame> GetAnimationsFrames()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1) return LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames;
            else return null;
        }

        public byte? GetLoopIndex()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1) return LoadedAnimationFile.Animations[SelectedAnimationIndex].LoopIndex;
            else return null;
        }
        public byte? GetSpeedMultiplyer()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1) return LoadedAnimationFile.Animations[SelectedAnimationIndex].SpeedMultiplyer;
            else return null;
        }
        public byte? GetRotationFlag()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1) return LoadedAnimationFile.Animations[SelectedAnimationIndex].RotationFlags;
            else return null;
        }

        public void SetLoopIndex(byte? value)
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && value.HasValue) LoadedAnimationFile.Animations[SelectedAnimationIndex].LoopIndex = value.Value;
            else return;
        }
        public void SetSpeedMultiplyer(byte? value)
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && value.HasValue) LoadedAnimationFile.Animations[SelectedAnimationIndex].SpeedMultiplyer = value.Value;
            else return;
        }
        public void SetRotationFlag(byte? value)
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && value.HasValue) LoadedAnimationFile.Animations[SelectedAnimationIndex].RotationFlags = value.Value;
            else return;
        }



        #region Frame Info

        public sbyte? SelectedFramePivotX { get => GetPivotX(); set => SetPivotX(value); }
        public sbyte? SelectedFramePivotY { get => GetPivotY(); set => SetPivotY(value); }
        public byte? SelectedFrameHeight { get => GetHeight(); set => SetHeight(value); }
        public byte? SelectedFrameWidth { get => GetWidth(); set => SetWidth(value); }
        public byte? SelectedFrameLeft { get => GetX(); set => SetX(value); }
        public byte? SelectedFrameTop { get => GetY(); set => SetY(value); }
        public short? SelectedFrameId = null;
        public short? SelectedFrameDuration { get { return GetDelay(); } }
        public byte? CurrentSpriteSheet { get => GetSpriteSheet(); set => SetSpriteSheet(value); }

        #region Get Methods
        public byte? GetSpriteSheet()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1) return LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].SpriteSheet;
            else return null;
        }
        public sbyte? GetPivotX()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1) return LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].PivotX;
            else return null;
        }
        public sbyte? GetPivotY()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1) return LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].PivotY;
            else return null;
        }
        public byte? GetHeight()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1) return LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].Height;
            else return null;
        }
        public byte? GetWidth()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1) return LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].Width;
            else return null;
        }
        public byte? GetX()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1) return LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].X;
            else return null;
        }
        public byte? GetY()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1) return LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].Y;
            else return null;
        }
        public short? GetDelay()
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1) return LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].Delay;
            else return null;
        }
        #endregion
        #region Set Methods
        public void SetSpriteSheet(byte? value)
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1 && value.HasValue) LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].SpriteSheet = value.Value;
            else return;
        }
        public void SetPivotX(sbyte? value)
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1 && value.HasValue) LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].PivotX = value.Value;
            else return;
        }
        public void SetPivotY(sbyte? value)
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1 && value.HasValue) LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].PivotY = value.Value;
            else return;
        }
        public void SetHeight(byte? value)
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1 && value.HasValue) LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].Height = value.Value;
            else return;
        }
        public void SetWidth(byte? value)
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1 && value.HasValue) LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].Width = value.Value;
            else return;
        }
        public void SetX(byte? value)
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1 && value.HasValue) LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].X = value.Value;
            else return;
        }
        public void SetY(byte? value)
        {
            if (LoadedAnimationFile != null && SelectedAnimationIndex != -1 && SelectedFrameIndex != -1 && value.HasValue) LoadedAnimationFile.Animations[SelectedAnimationIndex].Frames[SelectedFrameIndex].Y = value.Value;
            else return;
        }
        #endregion

        #endregion


    }
}
