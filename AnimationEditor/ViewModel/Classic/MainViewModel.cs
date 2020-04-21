using AnimationEditor.Services;
using AnimationEditor.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace AnimationEditor.ViewModels
{
    public class MainViewModel : Xe.Tools.Wpf.BaseNotifyPropertyChanged
    {

        private string _fileName;
        private EditorAnimation _animationData;
        private EditorAnimation.EditorAnimationInfo _selectedAnimation;
        private SpriteService _spriteService;
        private AnimationService _animService;

        public string PathMod { get; set; } = "";

        private string _AnimationDirectory;
        private string _SpriteDirectory; public string AnimationDirectory
        {
            get
            {
                return _AnimationDirectory;
            }
            set
            {
                _AnimationDirectory = value;
                OnPropertyChanged("AnimationDirectory");
            }
        }
        public string SpriteDirectory
        {
            get
            {
                return _SpriteDirectory;
            }
            set
            {
                _SpriteDirectory = value;
                OnPropertyChanged("SpriteDirectory");
            }
        }

        #region Animation data
        public ObservableCollection<string> Hitboxes
        {
            get
            {
                if (IsAnimationDataLoaded) return new ObservableCollection<string>(AnimationData.CollisionBoxes);
                else return new ObservableCollection<string>();
            }
            set
            {
                if (IsAnimationDataLoaded) AnimationData.CollisionBoxes = value.ToList();
                OnPropertyChanged("Hitboxes");
            }
        }
        public ObservableCollection<string> RetroHitboxStrings
        {
            get
            {
                if (IsAnimationDataLoaded)
                {
                    List<string> output = new List<string>();
                    for (int i = 1; i <= AnimationData.RetroCollisionBoxes.Count; i++)
                    {
                        output.Add(string.Format("Collision Set #{0}", i));
                    }
                    return new ObservableCollection<string>(output);
                }
                else return new ObservableCollection<string>(new List<string>());
            }
        }
        public ObservableCollection<EditorAnimation.EditorRetroHitBox> RetroHitboxes
        {
            get
            {
                if (IsAnimationDataLoaded) return new ObservableCollection<EditorAnimation.EditorRetroHitBox>(AnimationData.RetroCollisionBoxes);
                else return new ObservableCollection<EditorAnimation.EditorRetroHitBox>();
            }
            set
            {
                if (IsAnimationDataLoaded) AnimationData.RetroCollisionBoxes = value.ToList();
                OnPropertyChanged("RetroHitboxes");
            }
        }

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

        public EngineType AnimationType
        {
            get
            {
                return AnimationData?.EngineType ?? EngineType.Invalid;
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

                _animService = new AnimationService(_animationData);
                _animService.OnFrameChanged += OnFrameChanged;
                _spriteService = new SpriteService(_animationData, basePath);

                OnPropertyChanged(nameof(IsAnimationDataLoaded));
                OnPropertyChanged(nameof(Textures));
                OnPropertyChanged(nameof(Animations));
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

        public double SpriteLeft => ViewWidth / 2.0 + _animService?.CurrentFrame?.PivotX ?? 0;
        public double SpriteTop => ViewHeight / 2.0 + _animService?.CurrentFrame?.PivotY ?? 0;
        public double SpriteRight => SpriteLeft + _animService?.CurrentFrame?.Width ?? 0;
        public double SpriteBottom => SpriteTop + _animService?.CurrentFrame?.Height ?? 0;
        public Point SpriteCenter
        {
            get
            {
                var frame = _animService?.CurrentFrame;
                if (frame != null)
                {
                    return new Point((double)-frame.PivotX / frame.Width, (double)-frame.PivotY / frame.Height);
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

        public int Speed
        {
            get => SelectedAnimation != null ? SelectedAnimation.SpeedMultiplyer : 0;
            set => SelectedAnimation.SpeedMultiplyer = (byte)value;
        }

        public int Loop
        {
            get => SelectedAnimation?.LoopIndex ?? 0;
            set => SelectedAnimation.LoopIndex = (byte)value;
        }

        public int Flags
        {
            get => SelectedAnimation?.RotationFlags ?? 0;
            set => SelectedAnimation.RotationFlags = (byte)value;
        }

        #endregion

        #region Current Spritesheets
        public List<Spritesheet> SpriteSheets { get; set; } = new List<Spritesheet>();
        public List<string> SpriteSheetPaths
        {
            get
            {
                if (IsAnimationDataLoaded) return AnimationData.SpriteSheets;
                else return null;
            }
        }
        public List<string> NullSpriteSheetList { get; set; } = new List<string>();
        public class Spritesheet
        {
            public System.Windows.Media.Imaging.BitmapImage Image;
            public System.Windows.Media.Imaging.BitmapImage TransparentImage;
            public System.Windows.Media.Color TransparentColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#303030");

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
            get
            {
                return SelectedFrame?.ID ?? 0;
            }
            set
            {
                SelectedFrame.ID = value;
                InvalidateCanvas();
            }
        }

        public short SelectedFrameDuration
        {
            get
            {
                return SelectedFrame?.Delay ?? 0;
            }
            set
            {
                SelectedFrame.Delay = value;
                InvalidateCanvas();
            }
        }

        #endregion

        /*
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
        public ObservableCollection<EditorAnimation.EditorRetroHitBox> HitboxEntries { get; private set; }
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
        public EditorAnimation.EditorHitbox SelectedHitbox => SelectedFrame?.HitBoxes[_selectedIndex] ?? new EditorAnimation.EditorHitbox();
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
        */

        #region Hitbox Info
        public int _SelectedFrameHitboxIndex = 0; 
        public int SelectedFrameHitboxIndex
        {
            get
            {
                return _SelectedFrameHitboxIndex;
            }
            set
            {
                _SelectedFrameHitboxIndex = value;
                OnPropertyChanged("SelectedFrameHitboxIndex");
            }
        }
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
                OnPropertyChanged("SelectedHitboxLeft");
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
                OnPropertyChanged("SelectedHitboxTop");
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
                OnPropertyChanged("SelectedHitboxRight");
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
                OnPropertyChanged("SelectedHitboxBottom");
            }
        }

        #region RSDKv5 Hitboxes
        public short? SelectedHitboxLeft_v5
        {
            get
            {
                if (IsFrameSelected && SelectedFrameIndex != -1 && SelectedFrame.HitBoxes.Count > 0) return SelectedFrame.HitBoxes[SelectedFrameHitboxIndex].Left;
                else return 0;
            }
            set
            {
                if (IsFrameSelected && SelectedFrameHitboxIndex != -1 && value.HasValue)
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
                if (IsFrameSelected && SelectedFrameHitboxIndex != -1 && SelectedFrame.HitBoxes.Count > 0) return SelectedFrame.HitBoxes[SelectedFrameHitboxIndex].Top;
                else return 0;
            }
            set
            {
                if (IsFrameSelected && SelectedFrameHitboxIndex != -1 && value.HasValue)
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
                if (IsFrameSelected && SelectedFrameHitboxIndex != -1 && SelectedFrame.HitBoxes.Count > 0) return SelectedFrame.HitBoxes[SelectedFrameHitboxIndex].Right;
                else return 0;
            }
            set
            {
                if (IsFrameSelected && SelectedFrameHitboxIndex != -1 && value.HasValue)
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
                if (IsFrameSelected && SelectedFrameHitboxIndex != -1 && SelectedFrame.HitBoxes.Count > 0) return SelectedFrame.HitBoxes[SelectedFrameHitboxIndex].Bottom;
                else return 0;
            }
            set
            {
                if (IsFrameSelected && SelectedFrameHitboxIndex != -1 && value.HasValue)
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
                int index = (int)SelectedFrame.CollisionBox;
                if (IsFrameSelected && SelectedFrameHitboxIndex != -1 && RetroHitboxes.Count() - 1 >= index)
                {
                    return RetroHitboxes[index].Hitboxes[SelectedFrameHitboxIndex].Left;
                }
                else return 0;
            }
            set
            {
                int index = (int)SelectedFrame.CollisionBox;
                if (IsFrameSelected && SelectedFrameHitboxIndex != -1 && value.HasValue && RetroHitboxes.Count() - 1 >= index)
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
                int index = (int)SelectedFrame.CollisionBox;
                if (IsFrameSelected && SelectedFrameHitboxIndex != -1 && RetroHitboxes.Count() - 1 >= index)
                {
                    return RetroHitboxes[index].Hitboxes[SelectedFrameHitboxIndex].Top;
                }
                else return 0;
            }
            set
            {
                int index = (int)SelectedFrame.CollisionBox;
                if (IsFrameSelected && SelectedFrameHitboxIndex != -1 && value.HasValue && RetroHitboxes.Count() - 1 >= index)
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
                int index = (int)SelectedFrame.CollisionBox;
                if (IsFrameSelected && SelectedFrameHitboxIndex != -1 && RetroHitboxes.Count() - 1 >= index)
                {
                    return RetroHitboxes[index].Hitboxes[SelectedFrameHitboxIndex].Right;
                }
                else return 0;
            }
            set
            {
                int index = (int)SelectedFrame.CollisionBox;
                if (IsFrameSelected && SelectedFrameHitboxIndex != -1 && value.HasValue && RetroHitboxes.Count() - 1 >= index)
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
                int index = (int)SelectedFrame.CollisionBox;
                if (IsFrameSelected && SelectedFrameHitboxIndex != -1 && RetroHitboxes.Count() - 1 >= index)
                {
                    return RetroHitboxes[index].Hitboxes[SelectedFrameHitboxIndex].Bottom;
                }
                else return 0;
            }
            set
            {
                int index = (int)SelectedFrame.CollisionBox;
                if (IsFrameSelected && SelectedFrameHitboxIndex != -1 && value.HasValue && RetroHitboxes.Count() - 1 >= index)
                {
                    RetroHitboxes[index].Hitboxes[SelectedFrameHitboxIndex].Bottom = (sbyte)value.Value;
                }
                else return;
            }
        }
        #endregion

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
            OnPropertyChanged(nameof(SelectedFrameHitbox));
            OnPropertyChanged(nameof(SelectedFrameHitboxIndex));
        }

        private void OnFrameChanged(AnimationService service)
        {
            InvalidateCanvas();
            InvalidateFrameProperties();
        }

        public bool FileOpen(string fileName)
        {
            return false; 
            /*
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
            */
        }

        public void FileSave(string fileName = null)
        {
            /*
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
            */
        }





        public void CurrentFrameChanged()
        {
            var curAnim = SelectedAnimation;
            var curFrameIndex = SelectedFrameIndex;
            if (curAnim != null && curFrameIndex >= 0 &&
                curFrameIndex < curAnim.Frames.Count())
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
                _selectedAnimation.Frames.Select(x => new FrameViewModel(_spriteService, x)));
            OnPropertyChanged(nameof(AnimationFrames));
        }

        internal static string GetHitboxEntryString(EditorAnimation.EditorRetroHitBox entry)
        {
            return entry.Hitboxes.Length >= 0 ? GetHitboxString(entry.Hitboxes[0]) : "???";
        }
        internal static string GetHitboxString(EditorAnimation.EditorRetroHitBox.HitboxInfo hb)
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
            /*
            _animationData.SpriteSheets.Clear();
            _animationData.SpriteSheets.AddRange(Textures);
            _animationData.Animations = Animations;
            if (IsHitboxV3)
            {
                _animationData.git(HitboxEntries);
            }
            else if (IsHitboxV5)
            {
                _animationData.CollisionBoxes = HitboxTypes.ToList();
            }
            */
        }
        #endregion

        #region Frame Management
        public void ShiftFrameRight()
        {

        }

        public void ShiftFrameLeft()
        {

        }

        public void ShiftAnimationUp()
        {

        }

        public void ShiftAnimationDown()
        {

        }
        public void AddAnimation(int? insertOnIndex = null)
        {
            //_animationData.Factory(out IAnimationEntry o);
            Animations.Add(new EditorAnimation.EditorAnimationInfo(AnimationData.EngineType, AnimationData));
        }
        public void DuplicateAnimation()
        {
            
            var selectedAnimation = SelectedAnimation;
            if (selectedAnimation != null)
                Animations.Add(selectedAnimation.Clone() as EditorAnimation.EditorAnimationInfo);
            
        }
        public void RemoveAnimation()
        {
            Animations.Remove(SelectedAnimation);
        }
        public void AnimationImport(string fileName)
        {
            /*
            if (!File.Exists(fileName)) return;

            using (var fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            {
                using (var reader = new BinaryReader(fStream))
                {
                    _animationData.Factory(out IAnimationEntry o);
                    o.Read(reader);
                    Animations.Add(o);
                }
            }
            */
        }
        public void AnimationExport(string fileName)
        {
            /*
            var selectedAnimation = SelectedAnimation;
            if (selectedAnimation == null) return;

            using (var fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                using (var writer = new BinaryWriter(fStream))
                {
                    selectedAnimation.Write(writer);
                }
            }
            */
        }

        public void AddFrame(int? insertOnIndex = null)
        {
            //_animationData.Factory(out IFrame frame);
            FrameAdd(new EditorAnimation.EditorFrame(), insertOnIndex);
        }
        public void FrameAdd(EditorAnimation.EditorFrame frame, int? insertOnIndex = null)
        {
            var frameVm = new FrameViewModel(_spriteService, frame);
            var list = SelectedAnimation.Frames.ToList();
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

        public void DuplicateFrame()
        {
            var selectedFrame = SelectedFrame;
            if (selectedFrame != null)
                FrameAdd(selectedFrame.Clone() as EditorAnimation.EditorFrame, SelectedFrameIndex);
        }

        public void RemoveFrame()
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
            /*
            if (!File.Exists(fileName)) return;
            var selectedAnimation = SelectedAnimation;
            if (selectedAnimation == null) return;

            using (var fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            {
                using (var reader = new BinaryReader(fStream))
                {
                    _animationData.Factory(out IFrame o);
                    o.Read(reader);
                    FrameAdd(o, SelectedFrameIndex);
                }
            }
            */
        }

        public void FrameExport(string fileName)
        {
            /*
            var selectedFrame = SelectedFrame;
            if (selectedFrame == null) return;

            using (var fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                using (var writer = new BinaryWriter(fStream))
                {
                    selectedFrame.Write(writer);
                }
            }
            */
        }


        #endregion
    }
}
