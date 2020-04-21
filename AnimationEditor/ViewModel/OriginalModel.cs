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

namespace AnimationEditor.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Properties

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(""));
            }
        }
        protected void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion
        private class DummyHitbox : IHitbox
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }

            public object Clone()
            {
                return new DummyHitbox()
                {
                    Left = Left,
                    Top = Top,
                    Right = Right,
                    Bottom = Bottom
                };
            }

            public void SaveChanges(BinaryWriter writer)
            { }

            public void Read(BinaryReader reader)
            { }

            public void Write(BinaryWriter writer)
            { }
        }

        private string _fileName;
        private EditorAnimation _animationData;
        private EditorAnimation.EditorAnimationInfo _selectedAnimation;
        private SpriteService _spriteService;
        private AnimationService _animService;

        public string PathMod { get; set; }

        #region Animation data

        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Title));
            }
        }

        public string Title => string.IsNullOrEmpty(FileName) ? "RSDK Animation Editor" : $"RSDK Animation Editor - {Path.GetFileName(FileName)}";

        public ObservableCollection<string> Textures { get; private set; }

        public ObservableCollection<EditorAnimation.EditorAnimationInfo> Animations { get; private set; }

        public EditorAnimation AnimationData
        {
            get => _animationData;
            set
            {
                _animationData = value;
                var basePath = Path.GetDirectoryName(_fileName);
                basePath = Path.Combine(basePath, PathMod);

                Textures = new ObservableCollection<string>(_animationData.SpriteSheets);
                Animations = new ObservableCollection<EditorAnimation.EditorAnimationInfo>(_animationData.Animations);
                IsHitboxV3 = _animationData.HitboxTypes == null;
                IsHitboxV5 = !IsHitboxV3;
                if (IsHitboxV3)
                {
                    HitboxEntries = new ObservableCollection<IHitboxEntry>(_animationData.GetHitboxes());
                    HitboxItems = HitboxEntries != null ? new ObservableCollection<string>(
                        HitboxEntries.Select(x => GetHitboxEntryString(x)))
                        : new ObservableCollection<string>();
                }
                else if (IsHitboxV5)
                {
                    HitboxTypes = new ObservableCollection<string>(_animationData.HitboxTypes);
                }
                ValidateHitboxVisibility();

                _animService = new AnimationService(_animationData);
                _animService.OnFrameChanged += OnFrameChanged;
                _spriteService = new SpriteService(_animationData, basePath);

                OnPropertyChanged(nameof(IsAnimationDataLoaded));
                OnPropertyChanged(nameof(Textures));
                OnPropertyChanged(nameof(Animations));
                OnPropertyChanged(nameof(HitboxEntries));
                OnPropertyChanged(nameof(HitboxItems));
            }
        }

        public bool IsAnimationDataLoaded => AnimationData != null;

        #endregion

        #region Animation view view

        private double _viewWidth, _viewHeight, _zoom = 1.0;

        public double ViewWidth
        {
            get => _viewWidth;
            set
            {
                _viewWidth = value;
                OnPropertyChanged(nameof(SpriteLeft));
                OnPropertyChanged(nameof(SpriteTop));
                OnPropertyChanged(nameof(SpriteRight));
                OnPropertyChanged(nameof(SpriteBottom));
                OnPropertyChanged(nameof(SpriteCenter));
            }
        }

        public double ViewHeight
        {
            get => _viewHeight;
            set
            {
                _viewHeight = value;
                OnPropertyChanged(nameof(SpriteLeft));
                OnPropertyChanged(nameof(SpriteTop));
                OnPropertyChanged(nameof(SpriteRight));
                OnPropertyChanged(nameof(SpriteBottom));
                OnPropertyChanged(nameof(SpriteCenter));
            }
        }

        public double Zoom
        {
            get => _zoom;
            set
            {
                _zoom = Math.Max(Math.Min(value, 16), 0.25);
                OnPropertyChanged();
                InvalidateCanvas();
            }
        }

        public BitmapSource Sprite => _spriteService?[SelectedFrameTexture, _animService.CurrentFrame];

        public double SpriteLeft => ViewWidth / 2.0 + _animService?.CurrentFrame?.CenterX ?? 0;
        public double SpriteTop => ViewHeight / 2.0 + _animService?.CurrentFrame?.CenterY ?? 0;
        public double SpriteRight => SpriteLeft + _animService?.CurrentFrame?.Width ?? 0;
        public double SpriteBottom => SpriteTop + _animService?.CurrentFrame?.Height ?? 0;
        public Point SpriteCenter
        {
            get
            {
                var frame = _animService?.CurrentFrame;
                if (frame != null)
                {
                    return new Point((double)-frame.CenterX / frame.Width, (double)-frame.CenterY / frame.Height);
                }
                return new Point(0.5, 0.5);
            }
        }
        public double SpriteScaleX => Zoom;
        public double SpriteScaleY => Zoom;

        public bool IsRunning
        {
            get => _animService?.IsRunning ?? false;
            set
            {
                _animService.IsRunning = value;
                OnPropertyChanged(nameof(IsNotRunning));
            }
        }
        public bool IsNotRunning => !IsRunning;

        #endregion

        #region Current animation properties

        public ObservableCollection<FrameViewModel> AnimationFrames { get; private set; }

        public EditorAnimation.EditorAnimationInfo SelectedAnimation
        {
            get => _selectedAnimation;
            set
            {
                if (_animService == null)
                    return;

                _selectedAnimation = value;
                _animService.Animation = value?.AnimName;

                if (_selectedAnimation != null)
                {
                    ChangeAllFrames();
                }
                else
                {
                    AnimationFrames = null;
                }
                OnPropertyChanged(nameof(AnimationFrames));

                OnPropertyChanged(nameof(IsAnimationSelected));
                OnPropertyChanged(nameof(FramesCount));
                OnPropertyChanged(nameof(Speed));
                OnPropertyChanged(nameof(Loop));
                OnPropertyChanged(nameof(Flags));
            }
        }

        public int SelectedAnimationIndex { get; set; }

        public bool IsFrameSelected => SelectedFrame != null && SelectedAnimation?.Frames.Count() > 0;

        public int FramesCount => SelectedAnimation?.Frames.Count() ?? 0;

        public short Speed
        {
            get => SelectedAnimation != null ? SelectedAnimation.SpeedMultiplyer : (short)0;
            set => SelectedAnimation.SpeedMultiplyer = value;
        }

        public byte Loop
        {
            get => SelectedAnimation?.LoopIndex ?? 0;
            set => SelectedAnimation.LoopIndex = value;
        }

        public byte Flags
        {
            get => SelectedAnimation?.RotationFlags ?? 0;
            set => SelectedAnimation.RotationFlags = value;
        }

        #endregion

        #region Selected frame

        public bool IsAnimationSelected => SelectedAnimation != null;

        public int SelectedFrameIndex
        {
            get => _animService?.FrameIndex ?? 0;
            set
            {
                if (value >= 0)
                {
                    _animService.FrameIndex = value;
                    OnPropertyChanged(nameof(SelectedFrameIndex));
                }
            }
        }

        public EditorAnimation.EditorFrame SelectedFrame => _animService?.CurrentFrame;


        /// <summary>
        /// Get or set the texture for the selected animation
        /// </summary>
        public byte SelectedFrameTexture
        {
            get => SelectedFrame?.SpriteSheet ?? 0;
            set
            {
                if (SelectedFrame != null)
                {
                    SelectedFrame.SpriteSheet = value;
                    _spriteService.InvalidateAll();
                    ChangeAllFrames();
                }
            }
        }

        public byte SelectedFrameHitbox
        {
            get => SelectedFrame?.CollisionBox ?? 0;
            set
            {
                SelectedFrame.CollisionBox = value;
            }
        }

        public short SelectedFrameLeft
        {
            get => SelectedFrame?.X ?? 0;
            set
            {
                SelectedFrame.X = value;
                CurrentFrameChanged();
                InvalidateCanvas();
            }
        }

        public short SelectedFrameTop
        {
            get => SelectedFrame?.Y ?? 0;
            set
            {
                SelectedFrame.Y = value;
                CurrentFrameChanged();
                InvalidateCanvas();
            }
        }

        public short SelectedFrameWidth
        {
            get => SelectedFrame?.Width ?? 0;
            set
            {
                SelectedFrame.Width = value;
                CurrentFrameChanged();
                InvalidateCanvas();
            }
        }

        public short SelectedFrameHeight
        {
            get => SelectedFrame?.Height ?? 0;
            set
            {
                SelectedFrame.Height = value;
                CurrentFrameChanged();
                InvalidateCanvas();
            }
        }

        public short SelectedFramePivotX
        {
            get => SelectedFrame?.PivotX ?? 0;
            set
            {
                SelectedFrame.PivotX = value;
                InvalidateCanvas();
            }
        }

        public short SelectedFramePivotY
        {
            get => SelectedFrame?.PivotY ?? 0;
            set
            {
                SelectedFrame.PivotY = value;
                InvalidateCanvas();
            }
        }

        public ushort SelectedFrameId
        {
            get => (SelectedFrame as EditorAnimation.EditorFrame)?.ID ?? 0;
            set
            {
                if (SelectedFrame is EditorAnimation.EditorFrame frame)
                {
                    frame.ID = value;
                    InvalidateCanvas();
                }
            }
        }

        public short SelectedFrameDuration
        {
            get => (SelectedFrame as EditorAnimation.EditorFrame)?.Delay ?? 0;
            set
            {
                if (SelectedFrame is EditorAnimation.EditorFrame frame)
                {
                    frame.Delay = value;
                    InvalidateCanvas();
                }
            }
        }

        #endregion

        #region Hitbox

        #region Hitbox v3
        private bool _isHitboxV3;
        public bool IsHitboxV3
        {
            get => _isHitboxV3;
            set
            {
                _isHitboxV3 = value;
                ValidateHitboxVisibility();
            }
        }
        public bool IsNotHitboxV3 => !IsHitboxV3;
        public Visibility HitboxV3Visibility => IsHitboxV3 ? Visibility.Visible : Visibility.Collapsed;
        public ObservableCollection<IHitboxEntry> HitboxEntries { get; private set; }
        public ObservableCollection<string> HitboxItems { get; private set; }
        #endregion

        #region Hitbox v5
        private bool _isHitboxV5;
        private int _selectedIndex;
        public bool IsHitboxV5
        {
            get => _isHitboxV5;
            set
            {
                _isHitboxV5 = value;
                ValidateHitboxVisibility();
            }
        }
        public bool IsNotHitboxV5 => !IsHitboxV5;
        public Visibility HitboxV5Visibility => IsHitboxV5 ? Visibility.Visible : Visibility.Collapsed;
        public ObservableCollection<string> HitboxTypes { get; set; }
        public int SelectedHitboxType
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                OnPropertyChanged(nameof(SelectedHitbox));
            }
        }
        public IHitbox SelectedHitbox => SelectedFrame?.GetHitbox(SelectedHitboxType) ?? new DummyHitbox();
        #endregion

        private void ValidateHitboxVisibility()
        {
            OnPropertyChanged(nameof(IsHitboxV3));
            OnPropertyChanged(nameof(IsNotHitboxV3));
            OnPropertyChanged(nameof(HitboxV3Visibility));
            OnPropertyChanged(nameof(HitboxEntries));
            OnPropertyChanged(nameof(HitboxItems));

            OnPropertyChanged(nameof(IsNotHitboxV5));
            OnPropertyChanged(nameof(HitboxV5Visibility));
            OnPropertyChanged(nameof(HitboxTypes));
        }

        #endregion

        #region Methods

        private void InvalidateCanvas()
        {
            OnPropertyChanged(nameof(Sprite));
            OnPropertyChanged(nameof(SpriteLeft));
            OnPropertyChanged(nameof(SpriteTop));
            OnPropertyChanged(nameof(SpriteRight));
            OnPropertyChanged(nameof(SpriteBottom));
            OnPropertyChanged(nameof(SpriteCenter));
            OnPropertyChanged(nameof(SpriteScaleX));
            OnPropertyChanged(nameof(SpriteScaleY));
        }

        public void InvalidateFrameProperties()
        {
            OnPropertyChanged(nameof(IsFrameSelected));
            OnPropertyChanged(nameof(SelectedFrameIndex));
            OnPropertyChanged(nameof(SelectedFrameTexture));
            OnPropertyChanged(nameof(SelectedFrameHitbox));
            OnPropertyChanged(nameof(SelectedFrameLeft));
            OnPropertyChanged(nameof(SelectedFrameTop));
            OnPropertyChanged(nameof(SelectedFrameWidth));
            OnPropertyChanged(nameof(SelectedFrameHeight));
            OnPropertyChanged(nameof(SelectedFramePivotX));
            OnPropertyChanged(nameof(SelectedFramePivotY));
            OnPropertyChanged(nameof(SelectedFrameId));
            OnPropertyChanged(nameof(SelectedFrameDuration));
            OnPropertyChanged(nameof(SelectedHitbox));
        }

        private void OnFrameChanged(AnimationService service)
        {
            InvalidateCanvas();
            InvalidateFrameProperties();
        }

        public bool FileOpen(string fileName)
        {
            if (File.Exists(fileName))
            {
                var ext = Path.GetExtension(fileName);
                using (var fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
                {
                    using (var reader = new BinaryReader(fStream))
                    {
                        FileName = fileName;
                        switch (ext)
                        {
                            case ".ani":
                                PathMod = "..\\sprites";
                                AnimationData = new RSDK3.Animation(reader);
                                break;
                            case ".bin":
                                PathMod = "..";
                                AnimationData = new RSDK5.Animation(reader);
                                return false;
                            default:
                                return false;
                        }
                    }
                }
                return true;
            }
            return false;
        }

        public void FileSave(string fileName = null)
        {
            SaveChanges();

            if (string.IsNullOrWhiteSpace(fileName))
                fileName = FileName;

            using (var fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                using (var writer = new BinaryWriter(fStream))
                {
                    _animationData.SaveChanges(writer);
                }
            }
            FileName = fileName;
        }

        public void AnimationAdd()
        {
            _animationData.Factory(out EditorAnimation.EditorAnimationInfo o);
            Animations.Add(o);
        }
        public void AnimationDuplicate()
        {
            var selectedAnimation = SelectedAnimation;
            if (selectedAnimation != null)
                Animations.Add(selectedAnimation.Clone() as EditorAnimation.EditorAnimationInfo);
        }
        public void AnimationRemove()
        {
            Animations.Remove(SelectedAnimation);
        }
        public void AnimationImport(string fileName)
        {
            if (!File.Exists(fileName)) return;

            using (var fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            {
                using (var reader = new BinaryReader(fStream))
                {
                    _animationData.Factory(out EditorAnimation.EditorAnimationInfo o);
                    o.Read(reader);
                    Animations.Add(o);
                }
            }
        }
        public void AnimationExport(string fileName)
        {
            var selectedAnimation = SelectedAnimation;
            if (selectedAnimation == null) return;

            using (var fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                using (var writer = new BinaryWriter(fStream))
                {
                    selectedAnimation.Write(writer);
                }
            }
        }

        public void FrameAdd()
        {
            _animationData.Factory(out EditorAnimation.EditorFrame frame);
            FrameAdd(frame);
        }
        public void FrameAdd(EditorAnimation.EditorFrame frame, int? insertOnIndex = null)
        {
            var frameVm = new FrameViewModel(_spriteService, frame);
            var list = SelectedAnimation.Frames;
            if (!insertOnIndex.HasValue)
            {
                AnimationFrames.Add(frameVm);
                list.Add(frame);
            }
            else
            {
                AnimationFrames.Insert(insertOnIndex.Value, frameVm);
                list.Insert(insertOnIndex.Value, frame);
            }
            SelectedAnimation.Frames = list;
        }

        public void DupeFrame()
        {
            var selectedFrame = SelectedFrame;
            if (selectedFrame != null)
                FrameAdd(selectedFrame.Clone() as EditorAnimation.EditorFrame, SelectedFrameIndex);
        }

        public void FrameRemove()
        {
            if (SelectedFrameIndex >= 0)
            {
                AnimationFrames.RemoveAt(SelectedFrameIndex);
                var frames = SelectedAnimation.Frames.ToList();
                if (frames.Count > 0)
                {
                    frames.RemoveAt(SelectedFrameIndex);
                    SelectedAnimation.Frames = frames;
                    SelectedFrameIndex = frames.Count - 1;
                    OnPropertyChanged(nameof(IsFrameSelected));
                }
            }
        }

        public void FrameImport(string fileName)
        {
            if (!File.Exists(fileName)) return;
            var selectedAnimation = SelectedAnimation;
            if (selectedAnimation == null) return;

            using (var fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            {
                using (var reader = new BinaryReader(fStream))
                {
                    _animationData.Factory(out EditorAnimation.EditorFrame o);
                    o.Read(reader);
                    FrameAdd(o, SelectedFrameIndex);
                }
            }
        }

        public void FrameExport(string fileName)
        {
            var selectedFrame = SelectedFrame;
            if (selectedFrame == null) return;

            using (var fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                using (var writer = new BinaryWriter(fStream))
                {
                    selectedFrame.Write(writer);
                }
            }
        }

        public void CurrentFrameChanged()
        {
            var curAnim = SelectedAnimation;
            var curFrameIndex = SelectedFrameIndex;
            if (curAnim != null && curFrameIndex >= 0 &&
                curFrameIndex < curAnim.Frames.Count)
            {
                var animationFrames = AnimationFrames;
                var item = animationFrames[curFrameIndex];
                animationFrames.RemoveAt(curFrameIndex);
                animationFrames.Insert(curFrameIndex, item);
                SelectedFrameIndex = curFrameIndex;
                _spriteService.Invalidate(SelectedFrameTexture, item.Frame);
                OnPropertyChanged(nameof(SelectedFrameIndex));
            }
        }

        private void ChangeAllFrames()
        {
            AnimationFrames = new ObservableCollection<FrameViewModel>(
                _selectedAnimation.GetFrames()
                    .Select(x => new FrameViewModel(_spriteService, x)));
            OnPropertyChanged(nameof(AnimationFrames));
        }

        internal static string GetHitboxEntryString(IHitboxEntry entry)
        {
            return entry.Count >= 0 ? GetHitboxString(entry.GetHitbox(0)) : "???";
        }
        internal static string GetHitboxString(IHitbox hb)
        {
            return $"({hb.Left}, {hb.Top}, {hb.Right}, {hb.Bottom})";
        }

        public bool ChangeCurrentAnimationName(string name)
        {
            if (Animations.Any(x => x.AnimName == name))
                return false;

            SelectedAnimation.AnimName = name;
            var index = SelectedAnimationIndex;
            var item = Animations[index];
            Animations.RemoveAt(index);
            Animations.Insert(index, item);
            SelectedAnimationIndex = index;
            OnPropertyChanged(nameof(SelectedAnimationIndex));
            return true;
        }

        public void SaveChanges()
        {
            _animationData.SpriteSheets.Clear();
            _animationData.SpriteSheets.AddRange(Textures);
            _animationData.Animations = Animations.ToList();
            if (IsHitboxV3)
            {
                _animationData.SetHitboxes(HitboxEntries);
            }
            else if (IsHitboxV5)
            {
                _animationData.SetHitboxTypes(HitboxTypes);
            }
        }
        #endregion
    }
}