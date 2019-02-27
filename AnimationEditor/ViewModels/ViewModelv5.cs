using System;
using System.Collections.Generic;
using AnimationEditor.Services;
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


namespace AnimationEditor.ViewModels
{
    public class ViewModelv5
    {

        public SpriteService SpriteService;
        public Animation LoadedAnimationFile;
        public List<System.Windows.Media.Imaging.BitmapImage> SpriteSheets { get; set; }
        public Animation.AnimationEntry _SelectedAnimation;

        public List<Animation.AnimationEntry> Animations { get => GetAnimations(); }
        public List<string> SpriteSheetPaths { get => GetSpriteSheetsList(); }
        public Animation.AnimationEntry SelectedAnimation { get => _SelectedAnimation; set { _SelectedAnimation = value; } }
        public int SelectedAnimationIndex { get; set; }

        public double ViewWidth { get; set; }
        public double ViewHeight { get; set; }

        public List<Animation.AnimationEntry.Frame> AnimationFrames { get => GetAnimationsFrames(); }
        public int SelectedFrameIndex { get; set; }
        public byte? Loop { get => GetLoopIndex(); set => SetLoopIndex(value); }
        public short? Speed { get => GetSpeedMultiplyer(); set => SetSpeedMultiplyer(value); }
        public byte? Flags { get => GetRotationFlag(); set => SetRotationFlag(value); }


        public List<Animation.AnimationEntry> GetAnimations()
        {
            if (LoadedAnimationFile != null) return LoadedAnimationFile.Animations;
            else return null;
        }

        public double SpriteLeft => (ViewWidth / 2.0 - SelectedFrameLeft ?? 0);
        public double SpriteTop => (ViewHeight / 2.0 - SelectedFrameTop ?? 0);
        public double SpriteRight => (SpriteLeft + (SelectedFrameWidth) ?? 0);
        public double SpriteBottom => (SpriteTop + (SelectedFrameHeight) ?? 0);
        public Rect SpriteFrame => GetFrame();
        public Point SpriteCenter
        {
            get
            {
                
                if (AnimationFrames != null && SelectedFrameIndex != -1)
                {
                    var frame = AnimationFrames[SelectedFrameIndex];
                    if (frame != null)
                    {
                        if (frame.Width == 0 || frame.Height == 0)
                        {
                            return new Point(0.5, 0.5);
                        }
                        else
                        {
                            return new Point((double)-(frame.PivotX) / (frame.Width), (double)-(frame.PivotY) / frame.Height);
                        }

                    }
                }
                return new Point(0.0, 0.0);
            }
        }
        public double SpriteScaleX { get => Zoom; set => Zoom = value; }
        public double SpriteScaleY { get => Zoom; set => Zoom = value; }

        public double Zoom = 1;

        public Rect GetFrame()
        {
            if (SelectedFrameLeft != null && SelectedFrameTop != null && SelectedFrameWidth != null && SelectedFrameHeight != null) return new Rect(SelectedFrameLeft.Value, SelectedFrameTop.Value, SelectedFrameWidth.Value, SelectedFrameHeight.Value);
            return new Rect(0, 0, 0.5, 0.5);

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



        #region Frame Info

        public short? SelectedFramePivotX { get => GetPivotX(); set => SetPivotX(value); }
        public short? SelectedFramePivotY { get => GetPivotY(); set => SetPivotY(value); }
        public short? SelectedFrameHeight { get => GetHeight(); set => SetHeight(value); }
        public short? SelectedFrameWidth { get => GetWidth(); set => SetWidth(value); }
        public short? SelectedFrameLeft { get => GetX(); set => SetX(value); }
        public short? SelectedFrameTop { get => GetY(); set => SetY(value); }
        public short? SelectedFrameId { get => GetID(); set => SetID(value); }
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
        public short GetID()
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
        public void SetID(short? value)
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

        #endregion


    }
}
