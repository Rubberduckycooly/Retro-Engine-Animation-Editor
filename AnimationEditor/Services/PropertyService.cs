using AnimationEditor.Classes;
using AnimationEditor.Pages;
using AnimationEditor.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NUD = Xceed.Wpf.Toolkit.IntegerUpDown;
using GenerationsLib.WPF;

namespace AnimationEditor.Services
{
    public class PropertyService
    {
        #region On Property Changed Extension

        private void OnPropertyChanged(string entry)
        {
            Instance.ViewModel.CallPropertyChanged(entry);
        }

        #endregion

        #region Definitions

        #region UI
        public bool PreventIndexUpdate { get; set; } = false;
        private MainWindow Instance;
        #endregion

        #region Bitmaps
        public string CurrentSpriteSheetName;

        #endregion

        #region Modes
        public bool ShowFrameBorder
        {
            get
            {
                return Instance?.ViewModel?.ShowFrameBorder ?? false;
            }
            set
            {
                if (Instance?.ViewModel?.ShowFrameBorder != null)
                {
                    Instance.ViewModel.ShowFrameBorder = value;
                }
            }
        }
        public bool ShowSolidImageBackground
        {
            get
            {
                return Instance?.ViewModel?.ShowSolidImageBackground ?? false;
            }
            set
            {
                if (Instance?.ViewModel?.ShowSolidImageBackground != null)
                {
                    Instance.ViewModel.ShowSolidImageBackground = value;
                }
            }
        }
        public bool SetBackgroundColorToMatchSpriteSheet
        {
            get
            {
                return Instance?.ViewModel?.SetBackgroundColorToMatchSpriteSheet ?? false;
            }
            set
            {
                if (Instance?.ViewModel?.SetBackgroundColorToMatchSpriteSheet != null) 
                {
                    Instance.ViewModel.SetBackgroundColorToMatchSpriteSheet = value;
                }
            }
        }
        public bool ForceCenterFrame
        {
            get
            {
                return Instance?.ViewModel?.ForceCenterFrame ?? false;
            }
            set
            {
                if (Instance?.ViewModel?.ForceCenterFrame != null)
                {
                    Instance.ViewModel.ForceCenterFrame = value;
                }
            }
        }
        public bool ShowHitBox
        {
            get
            {
                return Instance?.ViewModel?.ShowHitBox ?? false;
            }
            set
            {
                if (Instance?.ViewModel?.ShowHitBox != null)
                {
                    Instance.ViewModel.ShowHitBox = value;
                }
            }
        }
        public bool ShowAlignmentLines
        {
            get
            {
                return Instance?.ViewModel?.ShowAlignmentLines ?? false;
            }
            set
            {
                if (Instance?.ViewModel?.ShowAlignmentLines != null)
                {
                    Instance.ViewModel.ShowAlignmentLines = value;
                }
            }
        }
        public bool ShowFullFrame
        {
            get
            {
                return Instance?.ViewModel?.ShowFullFrame ?? false;
            }
            set
            {
                if (Instance?.ViewModel?.ShowFullFrame != null)
                {
                    Instance.ViewModel.ShowFullFrame = value;
                }
            }
        }
        public bool isPlaybackEnabled
        {
            get
            {
                return Instance?.ViewModel?.isPlaybackEnabled ?? false;
            }
            set
            {
                if (Instance?.ViewModel?.isPlaybackEnabled != null)
                {
                    Instance.ViewModel.isPlaybackEnabled = value;
                }
            }
        }

        #endregion

        #region Playback Settings
        public bool isForcePlaybackOn { get; set; } = false;
        public int ForcePlaybackDuration { get; set; } = 256;
        public int ForcePlaybackSpeed { get; set; } = 128;
        #endregion

        #region Colors
        public Color AlignmentLinesColor { get; set; } = (Color)ColorConverter.ConvertFromString("#FFFF0000");
        public Color CanvasBackground { get; set; } = (Color)ColorConverter.ConvertFromString("#303030");
        public Color HitboxBackground { get; set; } = (Color)ColorConverter.ConvertFromString("#FFE700FF");
        public Color FrameBorder { get; set; } = Colors.Black;
        public Color FrameBackground { get; set; } = Colors.Transparent;
        #endregion

        #region Opacity
        private double _RefrenceOpacity = 100;
        public double RefrenceOpacity { get => _RefrenceOpacity; set => _RefrenceOpacity = value * 0.01; }
        #endregion

        #region Status

        bool isAnimationLoaded
        {
            get => Instance.ViewModel != null && Instance.ViewModel.LoadedAnimationFile != null;
        }
        bool isHitboxesValid
        {
            get => Instance.ViewModel.Hitboxes != null && Instance.ViewModel.SelectedHitbox != null && Instance.ViewModel.Hitboxes.Count != 0;
        }
        bool isCurrentSpriteSheetsValid
        {
            get => Instance.ViewModel.CurrentFrame_SpriteSheet != null && Instance.ViewModel.SpriteSheets != null;
        }
        bool isCurrentSpriteSheetOutOfRange
        {
            get => Instance.ViewModel.CurrentFrame_SpriteSheet.Value - 1 > Instance.ViewModel.SpriteSheets.Count;
        }
        bool isSpriteSheetCountNotZero
        {
            get => Instance.ViewModel.SpriteSheets.Count > 0;
        }

        #endregion



        #endregion

        #region Init
        public PropertyService(MainWindow window)
        {
            Instance = window;
        }
        #endregion

        #region Update Properties

        public void InvalidateSelectionProperties()
        {
            UpdateSelectedAnimationProperties();
            UpdateSelectedItemProperties();

            UpdateSelectedFrameProperties();
            UpdateSpriteSheetProperties();

            if (isHitboxesValid) UpdateSelectedHitboxProperties();
        }
        public void InvalidateSprite()
        {
            Instance.ViewModel.InvalidateCroppedFrame(Instance.ViewModel.SelectedFrameIndex);
            UpdateCanvasVisual();
        }

        public void UpdateSelectedItemProperties()
        {
            int LastSelectedAnimationIndex = Instance.ViewModel.SelectedAnimationIndex;

            OnPropertyChanged(nameof(Instance.ViewModel.SelectedAnimationEntries));

            
            if (isAnimationLoaded)
            {
                Instance.ViewModel.SelectedAnimationIndex = Instance.ViewModel.GetIndexWithinRange(Instance.ViewModel.SelectedAnimationEntries.Count, LastSelectedAnimationIndex);
            }
            else
            {
                Instance.ViewModel.SelectedAnimationIndex = -1;
            }
            

            OnPropertyChanged(nameof(Instance.ViewModel.SelectedAnimation));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedAnimationIndex));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedFrameIndex));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedAnimationEntries));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedAnimationFrameSet));
        }
        public void UpdateSpriteSheetProperties()
        {
            OnPropertyChanged(nameof(Instance.ViewModel.SpriteSheetPaths));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_SpriteSheet));
        }
        public void UpdateSelectedHitboxProperties()
        {
            OnPropertyChanged(nameof(Instance.ViewModel.Hitboxes));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedFrameHitboxIndex));

            OnPropertyChanged(nameof(Instance.ViewModel.SelectedHitboxLeft));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedHitboxRight));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedHitboxTop));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedHitboxBottom));
        }
        public void UpdateSelectedFrameProperties()
        {
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_Width));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_Height));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_X));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_Y));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_PivotX));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_PivotY));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_FrameID));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_FrameDuration));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_CollisionBox));
        }
        public void UpdateSelectedAnimationProperties()
        {
            OnPropertyChanged(nameof(Instance.ViewModel.Speed));
            OnPropertyChanged(nameof(Instance.ViewModel.Loop));
            OnPropertyChanged(nameof(Instance.ViewModel.PlayerType));
            OnPropertyChanged(nameof(Instance.ViewModel.Flags));
        }

        #endregion

        #region Methods
        public void UpdateControls()
        {
            InvalidateSelectionProperties();

            GlobalService.UIService.UpdateIndexStatusVisual();
            GlobalService.UIService.UpdateEntrySections();

            GlobalService.UIService.UpdateFrameSections();
            GlobalService.UIService.UpdateGeneralSections();
            GlobalService.UIService.UpdateTypeLimitationsSections();

            UpdateCanvasVisual();
        }
        public void UnloadControls()
        {
            //GlobalService.UIService.ToggleFrameSectionEvents(false);
            //GlobalService.UIService.ToggleHitboxSectionEvents(false);
            //GlobalService.UIService.ToggleEntrySectionEvents(false);
            //GlobalService.UIService.ToggleListSelectionEvents(false);

            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_Width));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_Height));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_X));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_Y));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_PivotX));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_PivotY));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_FrameID));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_FrameDuration));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_CollisionBox));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_SpriteSheet));
            OnPropertyChanged(nameof(Instance.ViewModel.Hitboxes));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedFrameHitboxIndex));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedHitboxLeft));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedHitboxRight));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedHitboxTop));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedHitboxBottom));

            OnPropertyChanged(nameof(Instance.ViewModel.Speed));
            OnPropertyChanged(nameof(Instance.ViewModel.Loop));
            OnPropertyChanged(nameof(Instance.ViewModel.PlayerType));
            OnPropertyChanged(nameof(Instance.ViewModel.Flags));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedAnimationEntries));

            OnPropertyChanged(nameof(Instance.ViewModel.SelectedAnimationIndex));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedFrameIndex));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedAnimationFrameSet));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedAnimationEntries));

            OnPropertyChanged(nameof(Instance.ViewModel.SpriteSheetPaths));
        }
        public void MoveToAdjacentFrameIndex(bool subtract = false, bool updateUI = true)
        {
            if (Instance.ViewModel.SelectedAnimationFrameSet != null && Instance.ViewModel.SelectedAnimationFrameSet.Count > 0)
            {
                if (subtract)
                {
                    if (Instance.ViewModel.SelectedFrameIndex - 1 > -1) Instance.ViewModel.SelectedFrameIndex--;
                }
                else
                {
                    if (Instance.ViewModel.SelectedFrameIndex + 1 < Instance.FramesList.Items.Count) Instance.ViewModel.SelectedFrameIndex++;
                    else Instance.ViewModel.SelectedFrameIndex = (Instance.ViewModel.Loop != null ? Instance.ViewModel.Loop.Value : 0);
                }
                Instance.FramesList.ScrollIntoView(Instance.FramesList.SelectedItem);
                if (updateUI)
                {
                    InvalidateSelectionProperties();
                    UpdateControls();
                }
            }
        }
        public void RefreshUIThemes()
        {
            /*
            Instance.ImportAnimationContext.Style = Application.Current.FindResource("DefaultContextMenuStyle") as Style;
            Instance.ExportAnimationContext.Style = Application.Current.FindResource("DefaultContextMenuStyle") as Style;
            Instance.ImportFrameContext.Style = Application.Current.FindResource("DefaultContextMenuStyle") as Style;
            Instance.ExportFrameContext.Style = Application.Current.FindResource("DefaultContextMenuStyle") as Style;
            Instance.HelpContext.Style = Application.Current.FindResource("DefaultContextMenuStyle") as Style;
            Instance.PlaybackOptionsContextMenu.Style = Application.Current.FindResource("DefaultContextMenuStyle") as Style;

            Instance.ImportAnimationContext.Refresh(); 
            Instance.ExportAnimationContext.Items.Refresh();
            Instance.ImportFrameContext.Items.Refresh();
            Instance.ExportFrameContext.Items.Refresh();
            Instance.HelpContext.Items.Refresh();
            Instance.PlaybackOptionsContextMenu.Items.Refresh();
            */
        }
        public void FixAnimationProperties()
        {
            //if (isCurrentSpriteSheetsValid && isCurrentSpriteSheetOutOfRange) Instance.ViewModel.CurrentFrame_SpriteSheet = 0;
        }

        #endregion

        #region Drawing/Rendering

        public void UpdateCanvasVisual()
        {         
            UpdateCanvasColors();
            UpdateCanvasVisibility();
            Instance.ViewModel.InvalidateCanvas();
        }

        public void UpdateCanvasVisibility()
        {
            if (ShowFrameBorder) Instance.BorderMarker.Visibility = Visibility.Visible;
            else Instance.BorderMarker.Visibility = Visibility.Collapsed;

            if (ShowHitBox) Instance.HitBoxViewer.Visibility = Visibility.Visible;
            else Instance.HitBoxViewer.Visibility = Visibility.Collapsed;

            if (ShowHitBox) Instance.HitBoxBackground.Visibility = Visibility.Visible;
            else Instance.HitBoxBackground.Visibility = Visibility.Collapsed;

            if (ShowAlignmentLines)
            {
                Instance.AxisX.Visibility = Visibility.Visible;
                Instance.AxisY.Visibility = Visibility.Visible;
            }
            else
            {
                Instance.AxisX.Visibility = Visibility.Collapsed;
                Instance.AxisY.Visibility = Visibility.Collapsed;
            }
        }

        public void UpdateCanvasColors()
        {
            if (SetBackgroundColorToMatchSpriteSheet && isCurrentSpriteSheetsValid && isSpriteSheetCountNotZero) Instance.CanvasBackground.Background = new SolidColorBrush(Instance.ViewModel.SpriteSheets[Instance.ViewModel.CurrentFrame_SpriteSheet.Value].TransparentColor);
            else if (Instance.BGColorPicker.SelectedColor != null) Instance.CanvasBackground.Background = new SolidColorBrush(CanvasBackground);

            Instance.HitBoxViewer.BorderBrush = new SolidColorBrush(HitboxBackground);
            Instance.HitBoxBackground.Background = new SolidColorBrush(GenerationsLib.WPF.ColorExt.ToSWMColor(System.Drawing.Color.FromArgb(128, GenerationsLib.WPF.ColorExt.ToSDColor(HitboxBackground))));

            Instance.BorderMarker.BorderBrush = new SolidColorBrush(FrameBorder);
            Instance.BorderMarkerBackground.Background = new SolidColorBrush(FrameBackground);

            Instance.AxisX.Background = new SolidColorBrush(AlignmentLinesColor);
            Instance.AxisY.Background = new SolidColorBrush(AlignmentLinesColor);
        }

        #endregion
    }
}
