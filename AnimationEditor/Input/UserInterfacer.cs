using AnimationEditor.ViewModel;
using AnimationEditor.Classes;
using AnimationEditor.Pages;
using AnimationEditor.Services;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Generic;
using NUD = Xceed.Wpf.Toolkit.IntegerUpDown;

namespace AnimationEditor
{
    public class UserInterfacer
    {
        #region Definitions

        #region UI
        public bool PreventIndexUpdate { get; set; } = false;
        private MainWindow Instance;
        #endregion

        #region Bitmaps
        public SkiaSharp.SKBitmap CurrentSpriteSheet;
        public SkiaSharp.SKBitmap CurrentSpriteSheetFrame;
        public string CurrentSpriteSheetName;

        #endregion

        #region Modes
        public bool ShowFrameBorder { get; set; } = false;
        public bool ShowSolidImageBackground { get; set; } = false;
        public bool SetBackgroundColorToMatchSpriteSheet { get; set; } = false;
        public bool ForceCenterFrame { get; set; } = false;
        public bool ShowHitBox { get; set; } = false;
        public bool ShowAlignmentLines { get; set; } = false;
        public bool ShowFullFrame { get; set; } = false;
        public bool isPlaybackEnabled { get; set; } = false;

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

        #region Brushes
        private Brush DefaultBorderBrush;
        private Brush DefaultTextBrush;
        private Brush HideTextBrush;
        #endregion

        #region Status

        bool isEntrySelected 
        {
           get => Instance.List.SelectedItem != null;
        }
        bool isEntryIndexValid
        {
            get => Instance.ViewModel.SelectedAnimationIndex != -1;
        }
        bool isFrameSelected
        {
            get => Instance.FramesList.SelectedItem != null;
        }
        bool isFrameIndexValid
        {
            get => Instance.ViewModel.SelectedFrameIndex != -1;
        }
        bool AnimationFramesValid
        {
            get => Instance.ViewModel.SelectedAnimationFrameSet != null;
        }
        bool isAnimationLoaded
        {
            get => Instance.ViewModel != null && Instance.ViewModel.LoadedAnimationFile != null;
        }

        bool isHitboxesValid
        {
            get => Instance.ViewModel.Hitboxes != null && Instance.ViewModel.Hitboxes.Count != 0;
        }
        bool isCurrentSpriteSheetsValid
        {
            get => Instance.ViewModel.CurrentFrame_SpriteSheet != null && Instance.ViewModel.SpriteSheets != null;
        }
        bool isCurrentSpriteSheetOutOfRange
        {
            get => Instance.ViewModel.CurrentFrame_SpriteSheet.Value - 1 > Instance.ViewModel.SpriteSheets.Count;
        }
        bool isSpriteSheetsPathsValid
        {
            get => Instance.ViewModel.SpriteSheetPaths != null;
        }
        bool isSpriteSheetCountNotZero
        {
            get => Instance.ViewModel.SpriteSheets.Count > 0;
        }
        bool isSpriteSheetsPathCountNotZero
        {
            get => Instance.ViewModel.SpriteSheetPaths.Count > 0;
        }

        #endregion

        #region Services

        private Methods.PlaybackService PlaybackService;

        #endregion

        #endregion

        #region Init
        public UserInterfacer(MainWindow window)
        {
            Instance = window;

            DefaultBorderBrush = Instance.DefaultBorderBrush;
            DefaultTextBrush = Instance.DefaultTextBrush;
            HideTextBrush = Brushes.Transparent;
        }
        #endregion

        #region Get Loaded Animation Properties
        public void UpdateLoadedAnimationProperties()
        {
            Instance.List.InvalidateProperty(ListBox.ItemsSourceProperty);
            Instance.List.ItemsSource = Instance.ViewModel.SelectedAnimationEntries;
        }
        public void UpdateSelectedSectionProperties(bool justRefreshing = false)
        {
            ToggleAnimationInfoEvents(false);

            Instance.SpeedNUD.Value = Instance.ViewModel.Speed;
            Instance.LoopIndexNUD.Value = Instance.ViewModel.Loop;
            Instance.FlagsSelector.SelectedIndex = (Instance.ViewModel.Flags.HasValue ? Instance.ViewModel.Flags.Value : 0);
            Instance.PlayerID_NUD.Value = Instance.ViewModel.PlayerType;

            if (justRefreshing)
            {
                int lastSelectedFrameIndex = Instance.ViewModel.SelectedFrameIndex;
                Instance.FramesList.ItemsSource = null;
                Instance.FramesList.ItemsSource = Instance.ViewModel.AnimationFrameListSource;
                Instance.FramesList.SelectedIndex = lastSelectedFrameIndex;
            }
            else
            {
                Instance.ViewModel.SelectedFrameIndex = -1;
                Instance.FramesList.ItemsSource = null;
                Instance.FramesList.ItemsSource = Instance.ViewModel.AnimationFrameListSource;
            }

            ToggleAnimationInfoEvents(true);
        }
        public void UpdateLoadedAnimationTextureListProperties()
        {
            if (isAnimationLoaded)
            {
                Instance.SpriteSheetList.ItemsSource = null;
                Instance.SpriteSheetList.ItemsSource = Instance.ViewModel.SpriteSheetPaths;
            }
        }
        #endregion

        #region Get Current Frame Properties
        private void UpdateSpritesheetProperties()
        {
            ToggleFrameSpriteSheetEvents(false);

            Instance.SpriteSheetList.ItemsSource = Instance.ViewModel.SpriteSheetPaths;
            Instance.SpriteSheetList.SelectedIndex = (Instance.ViewModel.CurrentFrame_SpriteSheet.HasValue ? Instance.ViewModel.CurrentFrame_SpriteSheet.Value : 0);

            ToggleFrameSpriteSheetEvents(true);
        }
        public void UpdateCurrentFrameMaxMinProperties(bool toggleEvents = true)
        {
            if (toggleEvents) ToggleFrameNUDEvents(false);
            if (isAnimationLoaded)
            {
                Instance.FrameWidthNUD.Minimum = 0;
                Instance.FrameHeightNUD.Minimum = 0;
                Instance.FrameX_NUD.Minimum = 0;
                Instance.FrameY_NUD.Minimum = 0;

                if (Instance.ViewModel.SpriteSheets != null && Instance.ViewModel.SpriteSheets.Count > 0 && Instance.ViewModel.isCurrentSpriteSheetValid)
                {
                    int SheetHeight = (int)Instance.ViewModel.SpriteSheets[(int)Instance.ViewModel.CurrentFrame_SpriteSheet].Image.Height;
                    int SheetWidth = (int)Instance.ViewModel.SpriteSheets[(int)Instance.ViewModel.CurrentFrame_SpriteSheet].Image.Width;
                    int FrameTop = (int)Instance.ViewModel.CurrentFrame_Y.Value;
                    int FrameHeight = (int)Instance.ViewModel.CurrentFrame_Height.Value;
                    int FrameWidth = (int)Instance.ViewModel.CurrentFrame_Width.Value;
                    int FrameLeft = (int)Instance.ViewModel.CurrentFrame_X.Value;

                    Instance.FrameWidthNUD.Maximum = (FrameLeft + FrameWidth < SheetWidth ? SheetWidth - FrameLeft : 0);
                    Instance.FrameHeightNUD.Maximum = (FrameTop + FrameHeight < SheetHeight ? SheetHeight - FrameTop : 0);
                    Instance.FrameX_NUD.Maximum = SheetWidth - FrameWidth;
                    Instance.FrameY_NUD.Maximum = SheetHeight - FrameHeight;
                }
                else
                {
                    Instance.FrameWidthNUD.Maximum = 0;
                    Instance.FrameHeightNUD.Maximum = 0;
                    Instance.FrameX_NUD.Maximum = 0;
                    Instance.FrameY_NUD.Maximum = 0;
                }
            }
            if (toggleEvents) ToggleFrameNUDEvents(true);
        }
        public void UpdateCurrentFrameProperties()
        {
            ToggleFrameNUDEvents(false);

            UpdateCurrentFrameGeneralProperties(false);
            UpdateCurrentFrameHitboxProperties(false);
            UpdateCurrentFrameMaxMinProperties(false);
            UpdateSpritesheetProperties();

            ToggleFrameNUDEvents(true);

        }
        public void UpdateCurrentFrameGeneralProperties(bool toggleEvents = true)
        {
            if (toggleEvents) ToggleFrameNUDEvents(false);
            Instance.FrameWidthNUD.Value = Instance.ViewModel.CurrentFrame_Width;
            Instance.FrameHeightNUD.Value = Instance.ViewModel.CurrentFrame_Height;
            Instance.FrameX_NUD.Value = Instance.ViewModel.CurrentFrame_X;
            Instance.FrameY_NUD.Value = Instance.ViewModel.CurrentFrame_Y;
            Instance.PivotX_NUD.Value = Instance.ViewModel.CurrentFrame_PivotX;
            Instance.PivotY_NUD.Value = Instance.ViewModel.CurrentFrame_PivotY;
            Instance.FrameID_NUD.Value = Instance.ViewModel.CurrentFrame_FrameID;
            Instance.Delay_NUD.Value = Instance.ViewModel.CurrentFrame_FrameDuration;
            Instance.FrameHitboxID_NUD.Value = Instance.ViewModel.CurrentFrame_CollisionBox;
            if (toggleEvents) ToggleFrameNUDEvents(true);
        }
        public void UpdateCurrentFrameHitboxProperties(bool toggleEvents = true)
        {
            if (toggleEvents) ToggleFrameNUDEvents(false);
            ToggleHitboxEvents(false);

            if (isHitboxesValid)
            {
                Instance.HitBoxComboBox.ItemsSource = Instance.ViewModel.Hitboxes;
                Instance.HitBoxComboBox.SelectedIndex = Instance.ViewModel.SelectedFrameHitboxIndex;

                Instance.HitboxLeftNUD.Value = Instance.ViewModel.SelectedHitboxLeft;
                Instance.HitboxRightNUD.Value = Instance.ViewModel.SelectedHitboxRight;
                Instance.HitboxTopNUD.Value = Instance.ViewModel.SelectedHitboxTop;
                Instance.HitboxBottomNUD.Value = Instance.ViewModel.SelectedHitboxBottom;
            }


            if (toggleEvents) ToggleFrameNUDEvents(true);
            ToggleHitboxEvents(true);
        }
        public void FixAnimationProperties()
        {
            if (isCurrentSpriteSheetsValid && isCurrentSpriteSheetOutOfRange) Instance.ViewModel.CurrentFrame_SpriteSheet = 0;
        }
        #endregion

        #region Set Current Frame Properties
        public void UpdateFrameNUDValues(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            int? nullablevalue = (sender as NUD).Value.Value;
            if (nullablevalue == null) return;
            int value = nullablevalue.Value;

            if (sender == Instance.FrameX_NUD) Instance.ViewModel.CurrentFrame_X = (short)value;
            else if (sender == Instance.FrameY_NUD) Instance.ViewModel.CurrentFrame_Y = (short)value;
            else if(sender == Instance.FrameWidthNUD) Instance.ViewModel.CurrentFrame_Width = (short)value;
            else if (sender == Instance.FrameHeightNUD) Instance.ViewModel.CurrentFrame_Height = (short)value;
            else if (sender == Instance.PivotX_NUD) Instance.ViewModel.CurrentFrame_PivotX = (short)value;
            else if (sender == Instance.PivotY_NUD) Instance.ViewModel.CurrentFrame_PivotY = (short)value;
            else if (sender == Instance.Delay_NUD) Instance.ViewModel.CurrentFrame_FrameDuration = (short)value;
            else if (sender == Instance.FrameID_NUD) Instance.ViewModel.CurrentFrame_FrameID = (ushort)value;
            else if (sender == Instance.FrameHitboxID_NUD) Instance.ViewModel.CurrentFrame_CollisionBox = (byte)value;
            else if (sender == Instance.HitboxLeftNUD) Instance.ViewModel.SelectedHitboxLeft = (short)value;
            else if (sender == Instance.HitboxTopNUD) Instance.ViewModel.SelectedHitboxTop = (short)value;
            else if (sender == Instance.HitboxRightNUD) Instance.ViewModel.SelectedHitboxRight = (short)value;
            else if (sender == Instance.HitboxBottomNUD) Instance.ViewModel.SelectedHitboxBottom = (short)value;

            Instance.ViewModel.InvalidateCroppedFrame(Instance.ViewModel.SelectedFrameIndex);
            UpdateSelectedSectionProperties(true);
        }

        public void UpdateFrameHitboxValues(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int value = (sender as ComboBox).SelectedIndex;

            if (sender == Instance.HitBoxComboBox) Instance.ViewModel.SelectedFrameHitboxIndex = value;
        }

        public void UpdateFrameSpriteSheetValues(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int value = (sender as ComboBox).SelectedIndex;

            if (sender == Instance.SpriteSheetList) Instance.ViewModel.CurrentFrame_SpriteSheet = (byte)value;

            Instance.ViewModel.InvalidateCroppedFrame(Instance.ViewModel.SelectedFrameIndex);
            UpdateSelectedSectionProperties(true);
        }

        #endregion

        #region Set Animation Info Properties
        public void UpdateAnimationInfoNUDValues(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            int? nullablevalue = (sender as NUD).Value.Value;
            if (nullablevalue == null) return;
            int value = nullablevalue.Value;

            if (sender == Instance.SpeedNUD) Instance.ViewModel.Speed = (short)value;
            else if (sender == Instance.LoopIndexNUD) Instance.ViewModel.Loop = (byte)value;
            else if (sender == Instance.PlayerID_NUD) Instance.ViewModel.PlayerType = (byte)value;
            else if (sender == Instance.Unknown_NUD) Instance.ViewModel.Unknown = (byte)value;
        }

        public void UpdateAnimationInfoFlagValues(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int value = (sender as ComboBox).SelectedIndex;

            if (sender == Instance.FlagsSelector) Instance.ViewModel.Flags = (byte)value;
        }

        public void UpdateAnimationInfoMiscValues(object sender, EventArgs e)
        {
            bool? nullablevalue = (sender as CheckBox).IsChecked;
            if (nullablevalue == null) return;
            bool value = nullablevalue.Value;

            if (sender == Instance.DreamcastVer_Checkbox) Instance.ViewModel.DreamcastVer = value;
        }

        #endregion

        #region Property Event Toggling
        public void ToggleFrameNUDEvents(bool isEnabled)
        {
            if (isEnabled)
            {
                Instance.FrameWidthNUD.ValueChanged += Instance.NUD_ValueChanged;
                Instance.FrameHeightNUD.ValueChanged += Instance.NUD_ValueChanged;
                Instance.FrameX_NUD.ValueChanged += Instance.NUD_ValueChanged;
                Instance.FrameY_NUD.ValueChanged += Instance.NUD_ValueChanged;
                Instance.PivotX_NUD.ValueChanged += Instance.NUD_ValueChanged;
                Instance.PivotY_NUD.ValueChanged += Instance.NUD_ValueChanged;
                Instance.FrameID_NUD.ValueChanged += Instance.NUD_ValueChanged;
                Instance.Delay_NUD.ValueChanged += Instance.NUD_ValueChanged;
                Instance.FrameHitboxID_NUD.ValueChanged += Instance.NUD_ValueChanged;

                Instance.HitboxLeftNUD.ValueChanged += Instance.NUD_ValueChanged;
                Instance.HitboxTopNUD.ValueChanged += Instance.NUD_ValueChanged;
                Instance.HitboxRightNUD.ValueChanged += Instance.NUD_ValueChanged;
                Instance.HitboxBottomNUD.ValueChanged += Instance.NUD_ValueChanged;
            }
            else
            {
                Instance.FrameWidthNUD.ValueChanged -= Instance.NUD_ValueChanged;
                Instance.FrameHeightNUD.ValueChanged -= Instance.NUD_ValueChanged;
                Instance.FrameX_NUD.ValueChanged -= Instance.NUD_ValueChanged;
                Instance.FrameY_NUD.ValueChanged -= Instance.NUD_ValueChanged;
                Instance.PivotX_NUD.ValueChanged -= Instance.NUD_ValueChanged;
                Instance.PivotY_NUD.ValueChanged -= Instance.NUD_ValueChanged;
                Instance.FrameID_NUD.ValueChanged -= Instance.NUD_ValueChanged;
                Instance.Delay_NUD.ValueChanged -= Instance.NUD_ValueChanged;
                Instance.FrameHitboxID_NUD.ValueChanged -= Instance.NUD_ValueChanged;

                Instance.HitboxLeftNUD.ValueChanged -= Instance.NUD_ValueChanged;
                Instance.HitboxTopNUD.ValueChanged -= Instance.NUD_ValueChanged;
                Instance.HitboxRightNUD.ValueChanged -= Instance.NUD_ValueChanged;
                Instance.HitboxBottomNUD.ValueChanged -= Instance.NUD_ValueChanged;
            }
        }
        public void ToggleHitboxEvents(bool isEnabled)
        {
            if (isEnabled)
            {
                Instance.HitBoxComboBox.SelectionChanged += Instance.HitBoxComboBox_SelectionChanged;
            }
            else
            {
                Instance.HitBoxComboBox.SelectionChanged -= Instance.HitBoxComboBox_SelectionChanged;
            }
        }
        public void ToggleFrameSpriteSheetEvents(bool isEnabled)
        {
            if (isEnabled)
            {
                Instance.SpriteSheetList.SelectionChanged += Instance.SpriteSheetList_SelectionChanged;
            }
            else
            {
                Instance.SpriteSheetList.SelectionChanged -= Instance.SpriteSheetList_SelectionChanged;
            }
        }
        public void ToggleAnimationInfoEvents(bool isEnabled)
        {
            if (isEnabled)
            {
                Instance.FlagsSelector.SelectionChanged += Instance.FlagsSelector_SelectionChanged;
                Instance.PlayerID_NUD.ValueChanged += Instance.PlayerID_NUD_ValueChanged;
                Instance.SpeedNUD.ValueChanged += Instance.SpeedNUD_ValueChanged;
                Instance.LoopIndexNUD.ValueChanged += Instance.LoopIndexNUD_ValueChanged;
                Instance.Unknown_NUD.ValueChanged += Instance.Unknown_NUD_ValueChanged;
                Instance.DreamcastVer_Checkbox.Checked += Instance.DreamcastVer_Checkbox_Checked;
                Instance.DreamcastVer_Checkbox.Unchecked += Instance.DreamcastVer_Checkbox_Checked;
            }
            else
            {
                Instance.FlagsSelector.SelectionChanged -= Instance.FlagsSelector_SelectionChanged;
                Instance.PlayerID_NUD.ValueChanged -= Instance.PlayerID_NUD_ValueChanged;
                Instance.SpeedNUD.ValueChanged -= Instance.SpeedNUD_ValueChanged;
                Instance.LoopIndexNUD.ValueChanged -= Instance.LoopIndexNUD_ValueChanged;
                Instance.Unknown_NUD.ValueChanged -= Instance.Unknown_NUD_ValueChanged;
                Instance.DreamcastVer_Checkbox.Checked -= Instance.DreamcastVer_Checkbox_Checked;
                Instance.DreamcastVer_Checkbox.Unchecked -= Instance.DreamcastVer_Checkbox_Checked;
            }
        }

        #endregion

        #region Update Controls
        public void UpdateFrameListControls()
        {
            bool invalid = !isFrameIndexValid;
            bool enabled = isFrameSelected;
            bool noPlayback = !isPlaybackEnabled;

            Instance.ButtonFrameAdd.IsEnabled = isEntrySelected && noPlayback;
            Instance.ButtonFrameDupe.IsEnabled = enabled && noPlayback;
            Instance.ButtonFrameExport.IsEnabled = enabled && noPlayback;
            Instance.ButtonFrameImport.IsEnabled = enabled && noPlayback;
            Instance.ButtonFrameRemove.IsEnabled = enabled && noPlayback;
            Instance.ButtonFrameLeft.IsEnabled = enabled && noPlayback;
            Instance.ButtonFrameRight.IsEnabled = enabled && noPlayback;
            Instance.ButtonShowFieldHitbox.IsEnabled = enabled;
            Instance.ButtonShowCenter.IsEnabled = enabled;
            UpdateZoomInOutEnabledState();

            Instance.FrameWidthNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.FrameHeightNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.FrameX_NUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.FrameY_NUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.PivotX_NUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.PivotY_NUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.FrameID_NUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.Delay_NUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.FrameHitboxID_NUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);

            Instance.FrameWidthNUD.IsEnabled = enabled;
            Instance.FrameHeightNUD.IsEnabled = enabled;
            Instance.FrameX_NUD.IsEnabled = enabled;
            Instance.FrameY_NUD.IsEnabled = enabled;
            Instance.PivotX_NUD.IsEnabled = enabled;
            Instance.PivotY_NUD.IsEnabled = enabled;

            Instance.SpriteSheetList.IsEnabled = enabled;
            Instance.SpriteSheetList.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.SpriteSheetList.Foreground = (invalid ? HideTextBrush : DefaultTextBrush);
            Instance.SpriteSheetList.IsHitTestVisible = (invalid ? false : true);

            UpdateHitboxControls();

            void UpdateZoomInOutEnabledState()
            {
                if (Instance.ViewModel.Zoom < 7) Instance.ButtonZoomIn.IsEnabled = enabled;
                else Instance.ButtonZoomIn.IsEnabled = false;
                if (Instance.ViewModel.Zoom > 1) Instance.ButtonZoomOut.IsEnabled = enabled;
                else Instance.ButtonZoomOut.IsEnabled = false;
            }
        }
        public void UpdateAnimationInfoControls()
        {
            bool isLoaded = isAnimationLoaded;
            bool isSelected = isEntrySelected;
            bool invalid = !isEntryIndexValid;
            bool noPlayback = !isPlaybackEnabled;

            Instance.ButtonAnimationAdd.IsEnabled = isLoaded && noPlayback;
            Instance.ButtonAnimationRemove.IsEnabled = isSelected && noPlayback;
            Instance.ButtonAnimationExport.IsEnabled = isSelected && noPlayback;
            Instance.ButtonAnimationImport.IsEnabled = isLoaded && noPlayback;
            Instance.ButtonAnimationDuplicate.IsEnabled = isSelected && noPlayback;
            Instance.ButtonAnimationUp.IsEnabled = isSelected && noPlayback;
            Instance.ButtonAnimationDown.IsEnabled = isSelected && noPlayback;
            Instance.AnimationScroller.IsEnabled = isLoaded && noPlayback;

            Instance.SpeedNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.LoopIndexNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.PlayerID_NUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);

            Instance.SpeedNUD.IsEnabled = isSelected && noPlayback;
            Instance.LoopIndexNUD.IsEnabled = isSelected && noPlayback;


            Instance.FlagsSelector.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.FlagsSelector.Foreground = (invalid ? HideTextBrush : DefaultTextBrush);
            Instance.FlagsSelector.IsHitTestVisible = (invalid ? false : true);
        }
        public void UpdateHitboxControls()
        {
            bool invalid = !isFrameIndexValid;
            bool enabled = isFrameSelected;
            bool indexNegative = Instance.HitBoxComboBox.SelectedIndex == -1;

            Instance.HitboxLeftNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.HitboxTopNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.HitboxRightNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.HitboxBottomNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);

            Instance.HitboxLeftNUD.IsEnabled = enabled;
            Instance.HitboxTopNUD.IsEnabled = enabled;
            Instance.HitboxRightNUD.IsEnabled = enabled;
            Instance.HitboxBottomNUD.IsEnabled = enabled;

            Instance.HitBoxComboBox.IsEnabled = enabled;
            Instance.HitBoxComboBox.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.HitBoxComboBox.Foreground = (invalid ? HideTextBrush : DefaultTextBrush);
            Instance.HitBoxComboBox.IsHitTestVisible = (invalid && !indexNegative ? false : true);
        }
        public void UpdateGeneralControls()
        {
            bool isLoaded = isAnimationLoaded;
            bool noPlayback = !isPlaybackEnabled;

            if (isLoaded)
            {
                Instance.FramesList.Visibility = Visibility.Visible;
                Instance.FakeScrollbar.Visibility = Visibility.Collapsed;
            }
            else
            {
                Instance.FramesList.Visibility = Visibility.Collapsed;
                Instance.FakeScrollbar.Visibility = Visibility.Visible;
            }

            Instance.ButtonPlay.IsEnabled = isFrameSelected;
            Instance.PlaybackOptionsButton.IsEnabled = isFrameSelected;
            Instance.HitboxButton.IsEnabled = isLoaded;
            Instance.TextureButton.IsEnabled = isLoaded;

            Instance.ControlPanel.IsEnabled = noPlayback;
            Instance.List.IsEnabled = noPlayback;
            Instance.FramesList.IsEnabled = noPlayback;

            Instance.MenuStrip.IsEnabled = noPlayback;

            if (isLoaded && isFrameSelected) Instance.CanvasView.Visibility = Visibility.Visible;
            else Instance.CanvasView.Visibility = Visibility.Collapsed;

            Instance.MenuFileSave.IsEnabled = isLoaded;
            Instance.MenuFileSaveAs.IsEnabled = isLoaded;
            Instance.MenuFileUnloadAnimation.IsEnabled = isLoaded;

            Instance.SpriteDirectoryLabel.Text = string.Format("Sprite Directory: {0}", (Instance.ViewModel.SpriteDirectory != "" && Instance.ViewModel.SpriteDirectory != null ? Instance.ViewModel.SpriteDirectory : "N/A"));
            Instance.AnimationPathLabel.Text = string.Format("Animation Path: {0}", (Instance.ViewModel.AnimationFilepath != "" && Instance.ViewModel.AnimationFilepath != null ? Instance.ViewModel.AnimationFilepath : "N/A"));

            Instance.SelectedAnimationIndexLabel.Text = Instance.ViewModel.SelectedAnimationIndex.ToString();
            Instance.SelectedFrameIndexLabel.Text = Instance.ViewModel.SelectedFrameIndex.ToString();
            Instance.FramesCountLabel.Text = Instance.ViewModel.FramesCount.ToString();
            Instance.AnimationsCountLabel.Text = Instance.ViewModel.AnimationsCount.ToString();
            Instance.AllFramesCountLabel.Text = Instance.ViewModel.GetCurrentFrameIndexForAllAnimations().ToString();
        }
        public void UpdateControls()
        {
            UpdateLoadedAnimationProperties();

            UpdateAnimationInfoControls();
            UpdateFrameListControls();
            UpdateGeneralControls();
            UpdateTypeLimitations();

            UpdateCanvasVisual();
            Instance.CanvasView.InvalidateVisual();
        }
        public void UpdateTypeLimitations()
        {
            /*
            - RSDKv5 is the only one that has “ID” and the only one with an editable “delay” value
            - RSDKvRS and RSDKv1 don’t have editable names
            - RSDKvRS and RSDKv1 always has to have 3 spritesheets no less no more
            (The dreamcast version of RSDKvRS only allows 2 spritesheets)
            - RSDKvRS has a “playerType” value that determines what players moveset tp give
            */
            var currentType = Instance.AnimationType;
            bool allowRenaming = (currentType != EngineType.RSDKv1 && currentType != EngineType.RSDKvRS);
            bool allowFlags = (currentType != EngineType.RSDKv1 && currentType != EngineType.RSDKvRS);
            bool allowDelayEditing = (currentType == EngineType.RSDKv5);
            bool allowHitboxIdentificationEditing = (currentType != EngineType.RSDKv5 && currentType != EngineType.RSDKvRS);
            bool allowFrameIdentificationEditing = (currentType == EngineType.RSDKv5);
            bool allowPlayerIdentificationEditing = (currentType == EngineType.RSDKvRS);
            bool allowDreamcastToggle = (currentType == EngineType.RSDKvRS);
            bool allowUnknownValueEditing = (currentType == EngineType.RSDKvRS);

            SetAnimationRenameVisibilityState(allowRenaming);
            SetFlagsVisibilityState(allowFlags);
            SetDelayNUDVisibilityState(allowDelayEditing);
            SetHitboxIDVisibilityState(allowHitboxIdentificationEditing);
            SetFrameIDVisibilityState(allowFrameIdentificationEditing);
            SetPlayerIDVisibilityState(allowPlayerIdentificationEditing);
            SetDreamcastVersionVisibilityState(allowDreamcastToggle);
            SetUnknownNUDVisibilityState(allowUnknownValueEditing);

            #region Visibility
            void SetFrameIDVisibilityState(bool isEnabled)
            {
                if (isEnabled)
                {
                    Instance.FrameID_NUD.IsEnabled = isAnimationLoaded && isFrameSelected;
                    Instance.FrameID_Label.IsEnabled = true;
                    Instance.FrameID_Section.Visibility = Visibility.Visible;
                }
                else
                {
                    Instance.FrameID_NUD.IsEnabled = false;
                    Instance.FrameID_Label.IsEnabled = false;
                    Instance.FrameID_Section.Visibility = Visibility.Collapsed;
                }
            }
            void SetFlagsVisibilityState(bool isEnabled)
            {
                if (isEnabled)
                {
                    Instance.FlagsSelector.IsEnabled = isAnimationLoaded && isEntrySelected;
                    Instance.FlagsSelector_Label.IsEnabled = true;
                }
                else
                {
                    Instance.FlagsSelector.IsEnabled = false;
                    Instance.FlagsSelector_Label.IsEnabled = false;
                }
            }
            void SetHitboxIDVisibilityState(bool isEnabled)
            {
                if (isEnabled)
                {
                    Instance.FrameHitboxID_NUD.IsEnabled = isAnimationLoaded && isFrameSelected;
                    Instance.FrameHitboxID_Label.IsEnabled = true;
                    Instance.FrameHitboxID_Section.Visibility = Visibility.Visible;
                }
                else
                {
                    Instance.FrameHitboxID_NUD.IsEnabled = false;
                    Instance.FrameHitboxID_Label.IsEnabled = false;
                    Instance.FrameHitboxID_Section.Visibility = Visibility.Collapsed;
                }
            }
            void SetPlayerIDVisibilityState(bool isEnabled)
            {
                if (isEnabled)
                {
                    Instance.PlayerID_NUD.IsEnabled = isAnimationLoaded && isEntrySelected;
                    Instance.PlayerID_Label.IsEnabled = true;
                    Instance.PlayerID_Section.Visibility = Visibility.Visible;
                }
                else
                {
                    Instance.PlayerID_NUD.IsEnabled = false;
                    Instance.PlayerID_Label.IsEnabled = false;
                    Instance.PlayerID_Section.Visibility = Visibility.Collapsed;
                }
            }
            void SetAnimationRenameVisibilityState(bool isEnabled)
            {
                if (isEnabled)
                {

                    Instance.ButtonAnimationRename.IsEnabled = isAnimationLoaded && isEntrySelected && !isPlaybackEnabled;
                    Instance.ButtonAnimationRename.Visibility = Visibility.Visible;
                }
                else
                {
                    Instance.ButtonAnimationRename.IsEnabled = false;
                    Instance.ButtonAnimationRename.Visibility = Visibility.Collapsed;
                }
            }
            void SetDelayNUDVisibilityState(bool isEnabled)
            {
                if (isEnabled)
                {
                    Instance.Delay_NUD.IsEnabled = isAnimationLoaded && isFrameSelected;
                    Instance.Delay_Label.IsEnabled = true;
                }
                else
                {
                    Instance.Delay_NUD.IsEnabled = false;
                    Instance.Delay_Label.IsEnabled = false;
                }
            }
            void SetUnknownNUDVisibilityState(bool isEnabled)
            {
                if (isEnabled)
                {
                    Instance.Unknown_NUD.IsEnabled = isAnimationLoaded && isEntrySelected;
                    Instance.Unknown_Label.IsEnabled = true;
                    Instance.Unknown_Section.Visibility = Visibility.Visible;
                }
                else
                {
                    Instance.Unknown_NUD.IsEnabled = false;
                    Instance.Unknown_Label.IsEnabled = false;
                    Instance.Unknown_Section.Visibility = Visibility.Collapsed;
                }
            }
            void SetDreamcastVersionVisibilityState(bool isEnabled)
            {
                if (isEnabled)
                {
                    Instance.DreamcastVer_Checkbox.IsEnabled = isAnimationLoaded && isEntrySelected;
                    Instance.DreamcastVer_Label.IsEnabled = true;
                    Instance.DreamcastVer_Section.Visibility = Visibility.Visible;
                }
                else
                {
                    Instance.DreamcastVer_Checkbox.IsEnabled = false;
                    Instance.DreamcastVer_Label.IsEnabled = false;
                    Instance.DreamcastVer_Section.Visibility = Visibility.Collapsed;
                }
            }
            #endregion
        }
        #endregion

        #region Theme Updating

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

        #endregion

        #region Playback Triggers

        public void TogglePlayback(bool enabled)
        {
            if (PlaybackService == null) IntilizePlayback();

            if (enabled)
            {
                isPlaybackEnabled = true;
                PlaybackService.AnimationData = Instance.ViewModel.LoadedAnimationFile;
                PlaybackService.Animation = Instance.ViewModel.SelectedAnimation.AnimName;
                PlaybackService.IsRunning = true;
                UpdateControls();
            }
            else
            {
                isPlaybackEnabled = false;
                PlaybackService.IsRunning = false;
                UpdateControls();
            }
        }

        public void IntilizePlayback(bool kill = false)
        {
            if (kill)
            {
                PlaybackService = null;
            }
            else
            {
                PlaybackService = new Methods.PlaybackService(Instance.ViewModel.LoadedAnimationFile, Instance);
                PlaybackService.OnFrameChanged += PlaybackService_OnFrameChanged;
            }
        }

        private void PlaybackService_OnFrameChanged(Methods.PlaybackService obj)
        {
            Application.Current.Dispatcher.Invoke(new Action(() => UpdatePlaybackIndex(obj.FrameIndex)));
        }

        private void UpdatePlaybackIndex(int frameIndex)
        {
            Instance.FramesList.SelectedIndex = frameIndex;
            UpdateCanvasVisual();
        }

        #endregion

        #region Index Scrolling
        public void ScrollFrameIndex(bool subtract = false, bool updateUI = true)
        {
            if (Instance.FramesList.Items != null && Instance.FramesList.Items.Count > 0)
            {
                if (subtract)
                {
                    if (Instance.FramesList.SelectedIndex - 1 > -1) Instance.FramesList.SelectedIndex--;
                }
                else
                {
                    if (Instance.FramesList.SelectedIndex + 1 < Instance.FramesList.Items.Count) Instance.FramesList.SelectedIndex++;
                    else Instance.FramesList.SelectedIndex = (Instance.LoopIndexNUD.Value != null ? Instance.LoopIndexNUD.Value.Value : 0);
                }
                Instance.FramesList.ScrollIntoView(Instance.FramesList.SelectedItem);
                if (updateUI) UpdateControls();
            }
        }
        #endregion

        #region Drawing/Rendering

        #region Update Bitmaps
        public void UpdateSheetImage()
        {
            if (Instance.ViewModel.SpriteSheets == null || Instance.ViewModel.SpriteSheets.Count == 0) return;


            if (Instance.MenuViewTransparentSpriteSheets.IsChecked)
            {
                var image = Instance.ViewModel.SpriteSheets[Instance.ViewModel.CurrentFrame_SpriteSheet.Value];
                if (image.isReady) CurrentSpriteSheet = SkiaSharp.Views.WPF.WPFExtensions.ToSKBitmap(image.TransparentImage);
                else CurrentSpriteSheet = null;
            }
            else
            {
                var image = Instance.ViewModel.SpriteSheets[Instance.ViewModel.CurrentFrame_SpriteSheet.Value];
                if (image.isReady) CurrentSpriteSheet = SkiaSharp.Views.WPF.WPFExtensions.ToSKBitmap(image.Image);
                else CurrentSpriteSheet = null;
            }
        }
        public void UpdateFrameImage()
        {
            if (Instance.ViewModel.SpriteSheets == null) return;
            if (CurrentSpriteSheet == null) return;

            double val_x = Instance.ViewModel.CurrentFrame_X.Value;
            double val_y = Instance.ViewModel.CurrentFrame_Y.Value;
            double val_width = Instance.ViewModel.CurrentFrame_Width.Value;
            double val_height = Instance.ViewModel.CurrentFrame_Height.Value;

            if (val_width != 0 && val_height != 0)
            {
                try
                {
                    System.Drawing.Bitmap sourceImage = SkiaSharp.Views.Desktop.Extensions.ToBitmap(CurrentSpriteSheet);
                    System.Drawing.Bitmap croppedImg = (System.Drawing.Bitmap)BitmapExtensions.CropImage(sourceImage, new System.Drawing.Rectangle((int)val_x, (int)val_y, (int)val_width, (int)val_height));
                    BitmapImage croppedBitmapImage = (BitmapImage)BitmapExtensions.ToWpfBitmap(croppedImg);
                    CurrentSpriteSheetFrame = SkiaSharp.Views.WPF.WPFExtensions.ToSKBitmap(croppedBitmapImage);

                    sourceImage.Dispose();
                    sourceImage = null;

                    croppedImg.Dispose();
                    croppedImg = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        #endregion
        public void UpdateCanvasVisual()
        {
            FixAnimationProperties();

            UpdateSheetImage();
            UpdateFrameImage();

            UpdateCanvasBackgroundColor();
            Instance.CanvasView.InvalidateVisual();
        }
        public void PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
        {
            var info = e.Info;
            var canvas = e.Surface.Canvas;
            float Zoom = (float)Instance.ViewModel.Zoom;

            canvas.Scale(Zoom);

            float width = info.Width / Zoom;
            float height = info.Height / Zoom;

            float width_half = width / 2;
            float height_half = height / 2;

            canvas.Clear(SkiaSharp.SKColors.Transparent);

            if (CurrentSpriteSheet != null || CurrentSpriteSheetFrame != null)
            {
                DrawSprite(canvas, width_half, height_half, width, height);
                if (ShowAlignmentLines) DrawAlignmentLines(canvas, width_half, height_half, width, height);
            }
        }
        public void DrawAlignmentLines(SkiaSharp.SKCanvas canvas, float width_half, float height_half, float width, float height)
        {
            SkiaSharp.SKPoint x1 = new SkiaSharp.SKPoint(0, height_half);
            SkiaSharp.SKPoint y1 = new SkiaSharp.SKPoint(width, height_half);
            SkiaSharp.SKPoint x2 = new SkiaSharp.SKPoint(width_half, 0);
            SkiaSharp.SKPoint y2 = new SkiaSharp.SKPoint(width_half, height);

            canvas.DrawLine(x1, y1, new SkiaSharp.SKPaint() { Color = SkiaSharp.Views.WPF.WPFExtensions.ToSKColor(AlignmentLinesColor) });
            canvas.DrawLine(x2, y2, new SkiaSharp.SKPaint() { Color = SkiaSharp.Views.WPF.WPFExtensions.ToSKColor(AlignmentLinesColor) });
        }
        public void DrawFrameBorder(SkiaSharp.SKCanvas canvas, float bx, float by, float w, float h)
        {
            SkiaSharp.SKPoint x1 = new SkiaSharp.SKPoint(bx, by);
            SkiaSharp.SKPoint x2 = new SkiaSharp.SKPoint(bx + w, by);
            SkiaSharp.SKPoint y1 = new SkiaSharp.SKPoint(bx, by + h);
            SkiaSharp.SKPoint y2 = new SkiaSharp.SKPoint(bx + w, by + h);

            canvas.DrawLine(x1, x2, new SkiaSharp.SKPaint() { Color = SkiaSharp.Views.WPF.WPFExtensions.ToSKColor(FrameBorder) });
            canvas.DrawLine(y1, y2, new SkiaSharp.SKPaint() { Color = SkiaSharp.Views.WPF.WPFExtensions.ToSKColor(FrameBorder) });
            canvas.DrawLine(x1, y1, new SkiaSharp.SKPaint() { Color = SkiaSharp.Views.WPF.WPFExtensions.ToSKColor(FrameBorder) });
            canvas.DrawLine(x2, y2, new SkiaSharp.SKPaint() { Color = SkiaSharp.Views.WPF.WPFExtensions.ToSKColor(FrameBorder) });

            var paint = new SkiaSharp.SKPaint();
            var transparency = SkiaSharp.Views.WPF.WPFExtensions.ToSKColor(FrameBackground);
            paint.Color = transparency;

            canvas.DrawRect(new SkiaSharp.SKRect() { Top = by, Left = bx, Size = new SkiaSharp.SKSize(w, h) }, paint);
        }
        public void DrawHitbox(SkiaSharp.SKCanvas canvas, float center_x, float center_y)
        {
            float l = center_x - Instance.ViewModel.SelectedHitboxLeft.Value;
            float r = center_x - Instance.ViewModel.SelectedHitboxRight.Value;

            float b = center_y + Instance.ViewModel.SelectedHitboxBottom.Value;
            float t = center_y + Instance.ViewModel.SelectedHitboxTop.Value;

            var paint = new SkiaSharp.SKPaint();
            paint.Color = SkiaSharp.Views.WPF.WPFExtensions.ToSKColor(HitboxBackground);

            var SemiOpacity = GenerationsLib.WPF.ColorExt.ToSWMColor(System.Drawing.Color.FromArgb(128, GenerationsLib.WPF.ColorExt.ToSDColor(HitboxBackground)));
            var paint2 = new SkiaSharp.SKPaint();
            paint2.Color = SkiaSharp.Views.WPF.WPFExtensions.ToSKColor(SemiOpacity);


            SkiaSharp.SKPoint x1 = new SkiaSharp.SKPoint(l, t);
            SkiaSharp.SKPoint y1 = new SkiaSharp.SKPoint(r, t);
            SkiaSharp.SKPoint x2 = new SkiaSharp.SKPoint(l, b);
            SkiaSharp.SKPoint y2 = new SkiaSharp.SKPoint(r, b);

            canvas.DrawLine(x1, y1, paint);
            canvas.DrawLine(x2, y2, paint);
            canvas.DrawLine(x1, x2, paint);
            canvas.DrawLine(y1, y2, paint);

            canvas.DrawRect(new SkiaSharp.SKRect() { Top = t, Left = l, Bottom = b, Right = r }, paint2);
        }
        private void DrawSprite(SkiaSharp.SKCanvas canvas, float width_half, float height_half, float width, float height)
        {
            if (CurrentSpriteSheetFrame == null || CurrentSpriteSheet == null) return;
            int frame_x = (int)Instance.ViewModel.CurrentFrame_X;
            int frame_y = (int)Instance.ViewModel.CurrentFrame_Y;

            int frame_width = (int)Instance.ViewModel.CurrentFrame_Width;
            int frame_height = (int)Instance.ViewModel.CurrentFrame_Height;

            int frame_center_x = (ForceCenterFrame ? frame_width / 2 : -(int)Instance.ViewModel.CurrentFrame_PivotX.Value);
            int frame_center_y = (ForceCenterFrame ? frame_height / 2 : -(int)Instance.ViewModel.CurrentFrame_PivotY.Value);

            float img_center_x = width_half - frame_center_x;
            float img_center_y = height_half - frame_center_y;

            float hitbox_center_x = width_half;
            float hitbox_center_y = height_half;

            float img_full_center_x = width_half - frame_x - frame_center_x;
            float img_full_center_y = height_half - frame_y - frame_center_y;

            float img_full_border_center_x = width_half - frame_center_x;
            float img_full_border_center_y = height_half - frame_center_y;

            float x;
            float y;
            float w;
            float h;


            float bx;
            float by;



            if (ShowFullFrame)
            {
                x = img_full_center_x;
                y = img_full_center_y;
                w = frame_width;
                h = frame_height;

                bx = img_center_x;
                by = img_center_y;
            }
            else
            {
                x = img_center_x;
                y = img_center_y;
                w = frame_width;
                h = frame_height;

                bx = x;
                by = y;
            }

            canvas.DrawBitmap((ShowFullFrame ? CurrentSpriteSheet : CurrentSpriteSheetFrame), new SkiaSharp.SKPoint(x, y));

            if (ShowFrameBorder) DrawFrameBorder(canvas, bx, by, w, h);

            if (ShowHitBox) DrawHitbox(canvas, hitbox_center_x, hitbox_center_y);


        }
        public void UpdateCanvasBackgroundColor()
        {
            if (SetBackgroundColorToMatchSpriteSheet && isCurrentSpriteSheetsValid && isSpriteSheetCountNotZero) Instance.CanvasBackground.Background = new SolidColorBrush(Instance.ViewModel.SpriteSheets[Instance.ViewModel.CurrentFrame_SpriteSheet.Value].TransparentColor);
            else if (Instance.BGColorPicker.SelectedColor != null) Instance.CanvasBackground.Background = new SolidColorBrush(CanvasBackground);
        }

        #endregion
    }
}
