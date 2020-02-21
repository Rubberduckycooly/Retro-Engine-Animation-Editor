using AnimationEditor.Animation.Classes;
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

        //TODO - Migrate all Modes and Colors to this Location

        #region Modes (TO SORT)
        public bool ShowFrameBorder
        {
            get => Instance.MenuViewFrameBorder.IsChecked;
        }
        public bool ShowSolidImageBackground
        {
            get => !Instance.MenuViewTransparentSpriteSheets.IsChecked;
        }
        public bool SetBackgroundColorToMatchSpriteSheet
        {
            get => Instance.MenuViewSetBackgroundToTransparentColor.IsChecked;
        }
        public bool ForceCenterFrame { get; set; } = false;
        private bool ShowHitBox
        {
            get => Instance.ButtonShowFieldHitbox.IsChecked.Value;
        }
        private bool ShowAlignmentLines
        {
            get => Instance.ButtonShowCenter.IsChecked.Value;
        }
        private bool ShowFullFrame
        {
            get => Instance.ViewModel.FullFrameMode;
        }
        private bool isPlaybackInProgress
        {
            get => Instance.isPlaybackEnabled;
        }
        #endregion

        #region Colors
        public Color AlignmentLinesColor
        {
            get { return Instance.AxisColorPicker.SelectedColor.Value; }
            set { Instance.AxisColorPicker.SelectedColor = value; }
        }
        public Color ImageBackground
        {
            get { return Instance.BGColorPicker.SelectedColor.Value; }
            set { Instance.BGColorPicker.SelectedColor = value; }
        }
        public Color HitboxBackground
        {
            get
            {
                return Instance.HitboxColorPicker.SelectedColor.Value;
            }
        }
        public Color FrameBorder = Colors.Black;
        public Color FrameBackground = Colors.Transparent;
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

        #region Loaded Animation Properties
        public void UpdateLoadedAnimationProperties()
        {
            Instance.List.InvalidateProperty(ListBox.ItemsSourceProperty);
            Instance.List.ItemsSource = Instance.ViewModel.SelectedAnimationEntries;
        }
        public void UpdateSelectedSectionProperties()
        {
            Instance.SpeedNUD.Value = Instance.ViewModel.Speed;
            Instance.LoopIndexNUD.Value = Instance.ViewModel.Loop;
            Instance.FlagsSelector.SelectedIndex = (Instance.ViewModel.Flags.HasValue ? Instance.ViewModel.Flags.Value : 0);
            Instance.PlayerID_NUD.Value = Instance.ViewModel.PlayerType;

            Instance.ViewModel.SelectedFrameIndex = -1;
            Instance.FramesList.ItemsSource = null;
            Instance.FramesList.ItemsSource = Instance.ViewModel.AnimationFrameListSource;
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

        #region Current Frame Properties
        private void UpdateSpritesheetProperties()
        {
            Instance.SpriteSheetList.ItemsSource = Instance.ViewModel.SpriteSheetPaths;
            Instance.SpriteSheetList.SelectedIndex = (Instance.ViewModel.CurrentFrame_SpriteSheet.HasValue ? Instance.ViewModel.CurrentFrame_SpriteSheet.Value : 0);

            if (AnimationFramesValid && isFrameIndexValid) Instance.ViewModel.SelectedAnimationFrameSet[Instance.ViewModel.SelectedFrameIndex].SpriteSheet = Instance.ViewModel.CurrentFrame_SpriteSheet.Value;

            if (isSpriteSheetsPathsValid && isSpriteSheetsPathCountNotZero) Instance.SpriteSheetList.SelectedValue = Instance.ViewModel.SpriteSheetPaths[Instance.SpriteSheetList.SelectedIndex];

            if (isFrameIndexValid && AnimationFramesValid)
            {
                if (Instance.ViewModel.SelectedAnimationFrameSet[Instance.ViewModel.SelectedFrameIndex] != null)
                {
                    if (Instance.ViewModel.SelectedAnimationFrameSet[Instance.ViewModel.SelectedFrameIndex].SpriteSheet != Instance.ViewModel.CurrentFrame_SpriteSheet && Instance.ViewModel.CurrentFrame_SpriteSheet != null)
                    {
                        Instance.ViewModel.SelectedAnimationFrameSet[Instance.ViewModel.SelectedFrameIndex].SpriteSheet = Instance.ViewModel.CurrentFrame_SpriteSheet.Value;
                    }
                }
            }


        }
        public void UpdateCurrentFrameMaxMinProperties()
        {
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
        }
        public void UpdateCurrentFrameProperties()
        {
            if (isHitboxesValid)
            {
                Instance.HitBoxComboBox.ItemsSource = Instance.ViewModel.Hitboxes;
                Instance.HitBoxComboBox.SelectedIndex = Instance.ViewModel.SelectedFrameHitboxIndex;

                Instance.HitboxLeftNUD.Value = Instance.ViewModel.SelectedHitboxLeft;
                Instance.HitboxRightNUD.Value = Instance.ViewModel.SelectedHitboxTop;
                Instance.HitboxTopNUD.Value = Instance.ViewModel.SelectedHitboxRight;
                Instance.HitboxBottomNUD.Value = Instance.ViewModel.SelectedHitboxBottom;
            }

            Instance.FrameWidthNUD.Value = Instance.ViewModel.CurrentFrame_Width;
            Instance.FrameHeightNUD.Value = Instance.ViewModel.CurrentFrame_Height;
            Instance.FrameX_NUD.Value = Instance.ViewModel.CurrentFrame_X;
            Instance.FrameY_NUD.Value = Instance.ViewModel.CurrentFrame_Y;
            Instance.PivotX_NUD.Value = Instance.ViewModel.CurrentFrame_PivotX;
            Instance.PivotY_NUD.Value = Instance.ViewModel.CurrentFrame_PivotY;
            Instance.FrameID_NUD.Value = Instance.ViewModel.CurrentFrame_FrameID;
            Instance.Delay_NUD.Value = Instance.ViewModel.CurrentFrame_FrameDuration;
            Instance.HitboxID_NUD.Value = Instance.ViewModel.CurrentFrame_CollisionBox;

            UpdateSpritesheetProperties();
            UpdateCurrentFrameMaxMinProperties();
        }
        public void FixAnimationProperties()
        {
            if (isCurrentSpriteSheetsValid && isCurrentSpriteSheetOutOfRange) Instance.ViewModel.CurrentFrame_SpriteSheet = 0;
        }
        #endregion

        #region Update Controls
        public void UpdateFrameListControls()
        {
            bool invalid = !isFrameIndexValid;
            bool enabled = isFrameSelected;

            Instance.ButtonFrameAdd.IsEnabled = isEntrySelected;
            Instance.ButtonFrameDupe.IsEnabled = enabled;
            Instance.ButtonFrameExport.IsEnabled = enabled;
            Instance.ButtonFrameImport.IsEnabled = enabled;
            Instance.ButtonFrameRemove.IsEnabled = enabled;
            Instance.ButtonFrameLeft.IsEnabled = enabled;
            Instance.ButtonFrameRight.IsEnabled = enabled;
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
            Instance.UnknownNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.HitboxID_NUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);


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
            bool enabled = isAnimationLoaded;
            bool invalid = !isEntryIndexValid;

            Instance.ButtonAnimationAdd.IsEnabled = enabled;
            Instance.ButtonAnimationRemove.IsEnabled = enabled;
            Instance.ButtonAnimationExport.IsEnabled = enabled;
            Instance.ButtonAnimationImport.IsEnabled = enabled;
            Instance.ButtonAnimationDuplicate.IsEnabled = enabled;
            Instance.ButtonAnimationUp.IsEnabled = enabled;
            Instance.ButtonAnimationDown.IsEnabled = enabled;
            Instance.ButtonPlay.IsEnabled = isFrameSelected;
            Instance.PlaybackOptionsButton.IsEnabled = isFrameSelected;
            Instance.AnimationScroller.IsEnabled = enabled;
            Instance.HitboxButton.IsEnabled = enabled;
            Instance.ButtonAnimationRename.IsEnabled = enabled;
            Instance.TextureButton.IsEnabled = enabled;

            Instance.SpeedNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.LoopIndexNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.PlayerID_NUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);

            Instance.FlagsSelector.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.FlagsSelector.Foreground = (invalid ? HideTextBrush : DefaultTextBrush);
            Instance.FlagsSelector.IsHitTestVisible = (invalid ? false : true);
        }
        public void UpdateHitboxControls()
        {
            bool invalid = !isFrameIndexValid;
            bool indexNegative = Instance.HitBoxComboBox.SelectedIndex == -1;

            Instance.HitboxLeftNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.HitboxTopNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.HitboxRightNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.HitboxBottomNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);

            Instance.HitBoxComboBox.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.HitBoxComboBox.Foreground = (invalid ? HideTextBrush : DefaultTextBrush);
            Instance.HitBoxComboBox.IsHitTestVisible = (invalid && !indexNegative ? false : true);
        }
        public void UpdateGeneralControls()
        {
            bool isLoaded = isAnimationLoaded;

            if (isLoaded) Instance.CanvasView.Visibility = Visibility.Visible;
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

            UpdateCanvasBackgroundColor();

            void UpdateCanvasBackgroundColor()
            {
                if (SetBackgroundColorToMatchSpriteSheet && isCurrentSpriteSheetsValid && isSpriteSheetCountNotZero) Instance.CanvasBackground.Background = new SolidColorBrush(Instance.ViewModel.SpriteSheets[Instance.ViewModel.CurrentFrame_SpriteSheet.Value].TransparentColor);
                else if (Instance.BGColorPicker.SelectedColor != null) Instance.CanvasBackground.Background = new SolidColorBrush(Instance.BGColorPicker.SelectedColor.Value);
            }
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
        public void UpdatePlaybackModeElementsEnabledState(bool enabled)
        {
            Instance.ButtonFrameAdd.IsEnabled = enabled;
            Instance.ButtonFrameDupe.IsEnabled = enabled;
            Instance.ButtonFrameExport.IsEnabled = enabled;
            Instance.ButtonFrameImport.IsEnabled = enabled;
            Instance.ButtonFrameRemove.IsEnabled = enabled;
            Instance.ButtonFrameLeft.IsEnabled = enabled;
            Instance.ButtonFrameRight.IsEnabled = enabled;

            Instance.ButtonAnimationAdd.IsEnabled = enabled;
            Instance.ButtonAnimationRemove.IsEnabled = enabled;
            Instance.ButtonAnimationExport.IsEnabled = enabled;
            Instance.ButtonAnimationImport.IsEnabled = enabled;
            Instance.ButtonAnimationDuplicate.IsEnabled = enabled;
            Instance.ButtonAnimationUp.IsEnabled = enabled;
            Instance.ButtonAnimationDown.IsEnabled = enabled;

            Instance.ControlPanel.IsEnabled = enabled;
            Instance.List.IsEnabled = enabled;
            Instance.FramesList.IsEnabled = enabled;

            Instance.AnimationScroller.IsEnabled = enabled;
            Instance.MenuStrip.IsEnabled = enabled;
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
            switch (Instance.AnimationType)
            {
                case EngineType.RSDKvRS: UpdateRSDKvRSLimits(); break;
                case EngineType.RSDKv1: UpdateRSDKv1Limits(); break;
                case EngineType.RSDKv2: UpdateRSDKv2Limits(); break;
                case EngineType.RSDKvB: UpdateRSDKvBLimits(); break;
                case EngineType.RSDKv5: UpdateRSDKv5Limits(); break;
            }

            #region Limits
            void UpdateRSDKvRSLimits()
            {
                SetAnimationRenameVisibilityState(false);
                SetDelayNUDVisibilityState(false);
                SetFrameIDVisibilityState(false);
            }
            void UpdateRSDKv1Limits()
            {
                SetAnimationRenameVisibilityState(false);
                SetDelayNUDVisibilityState(false);
                SetFrameIDVisibilityState(true);
            }
            void UpdateRSDKv2Limits()
            {
                SetAnimationRenameVisibilityState(true);
                SetDelayNUDVisibilityState(false);
                SetFrameIDVisibilityState(true);
            }
            void UpdateRSDKvBLimits()
            {
                SetAnimationRenameVisibilityState(true);
                SetDelayNUDVisibilityState(false);
                SetFrameIDVisibilityState(true);
            }
            void UpdateRSDKv5Limits()
            {
                SetAnimationRenameVisibilityState(true);
                SetDelayNUDVisibilityState(true);
                SetFrameIDVisibilityState(true);
            }
            #endregion

            #region Visibility
            void SetFrameIDVisibilityState(bool isEnabled)
            {
                if (isEnabled) Instance.FrameID_NUD.IsEnabled = true;
                else Instance.FrameID_NUD.IsEnabled = false;
            }
            void SetAnimationRenameVisibilityState(bool isEnabled)
            {
                if (isEnabled) Instance.ButtonAnimationRename.Visibility = Visibility.Visible;
                else Instance.ButtonAnimationRename.Visibility = Visibility.Collapsed;
            }
            void SetDelayNUDVisibilityState(bool isEnabled)
            {
                if (isEnabled) Instance.Delay_NUD.IsEnabled = true;
                else Instance.Delay_NUD.IsEnabled = false;
            }
            #endregion
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

        #region Frame/Animation List Refreshing (Phasing Out)
        public void RefreshAnimationsListFully()
        {
            var temp = Instance.List.SelectedItem;
            Instance.List.ItemsSource = null;
            Instance.List.ItemsSource = Instance.ViewModel.SelectedAnimationEntries;
            Instance.List.UpdateLayout();
            if (Instance.List.Items.Contains(temp)) Instance.List.SelectedItem = temp;
        }
        public void RefreshFramesList()
        {
            if (isAnimationLoaded && isEntryIndexValid)
            {
                var temp = Instance.FramesList.SelectedItem;
                Instance.FramesList.Items.Clear();
                if (Instance.ViewModel.SelectedAnimationIndex < 0) Instance.ViewModel.SelectedAnimationIndex = 0;
                for (int i = 0; i < Instance.ViewModel.LoadedAnimationFile.Animations[Instance.ViewModel.SelectedAnimationIndex].Frames.Count; i++)
                {
                    System.Windows.Controls.Image frame = new System.Windows.Controls.Image();
                    frame.Width = 45;
                    frame.Height = 45;
                    frame.Source = Instance.ViewModel.GetCroppedFrame(i);
                    Instance.FramesList.Items.Add(frame);
                }
                Instance.FramesList.UpdateLayout();
                if (Instance.FramesList.Items.Contains(temp)) Instance.FramesList.SelectedItem = temp;
            }

        }
        public void RefreshCurrentFrame()
        {
            /*
            int selectedIndex = Instance.ViewModel.SelectedFrameIndex;
            if (selectedIndex != -1 && isAnimationLoaded)
            {
                PreventIndexUpdate = true;
                Instance.ViewModel.InvalidateCroppedFrame(selectedIndex);
                System.Windows.Controls.Image frame = new System.Windows.Controls.Image();
                frame.Width = 45;
                frame.Height = 45;
                frame.Source = Instance.ViewModel.GetCroppedFrame(selectedIndex);
                Instance.FramesList.Items.RemoveAt(selectedIndex);
                Instance.FramesList.Items.Insert(selectedIndex, frame);
                Instance.ViewModel.SelectedFrameIndex = selectedIndex;
                Instance.FramesList.SelectedIndex = Instance.ViewModel.SelectedFrameIndex;
                Instance.FramesList.UpdateLayout();
            }*/
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

            if (ShowSolidImageBackground)
            {
                var paint = new SkiaSharp.SKPaint() { Color = SkiaSharp.Views.WPF.WPFExtensions.ToSKColor(ImageBackground) };
                SkiaSharp.SKRect rect;
                if (ShowFullFrame && CurrentSpriteSheet != null) rect = new SkiaSharp.SKRect() { Top = y, Left = x, Size = new SkiaSharp.SKSize(CurrentSpriteSheet.Width, CurrentSpriteSheet.Height) };
                else rect = new SkiaSharp.SKRect() { Top = y, Left = x, Size = new SkiaSharp.SKSize(w, h) };

                canvas.DrawRect(rect, paint);
            }

            canvas.DrawBitmap((ShowFullFrame ? CurrentSpriteSheet : CurrentSpriteSheetFrame), new SkiaSharp.SKPoint(x, y));

            if (ShowFrameBorder) DrawFrameBorder(canvas, bx, by, w, h);

            if (ShowHitBox) DrawHitbox(canvas, hitbox_center_x, hitbox_center_y);


        }

        #endregion
    }
}
