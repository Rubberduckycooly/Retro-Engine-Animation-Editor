using AnimationEditor.Animation.Classes;
using AnimationEditor.Pages;
using AnimationEditor.Services;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
            get
            {
                return Instance.AxisColorPicker.SelectedColor.Value;
            }
        }
        public Color ImageBackground
        {
            get
            {
                return Instance.BGColorPicker.SelectedColor.Value;
            }
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
            get => Instance.ViewModel.AnimationFrames != null;
        }
        bool isAnimationLoaded
        {
            get => Instance.ViewModel != null && isAnimationLoaded;
        }

        bool isHitboxesValid
        {
            get => Instance.ViewModel.Hitboxes != null && Instance.ViewModel.Hitboxes.Count != 0;
        }

        bool isSpriteSheetsValid
        {
            get => Instance.ViewModel.SpriteSheetPaths != null && Instance.ViewModel.SpriteSheetPaths.Count > 0;
        }

        #endregion

        #endregion

        #region Init
        public UserInterfacer(MainWindow window)
        {
            Instance = window;
            SetOldInitilizationValues();
        }
        private void SetOldInitilizationValues()
        {
            DefaultBorderBrush = Instance.DefaultBorderBrush;
            DefaultTextBrush = Instance.DefaultTextBrush;
            HideTextBrush = Brushes.Transparent;
        }
        #endregion

        #region Update UI Type Limitations
        public void UpdateAnimationTypeLimitations()
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
                case EngineType.RSDKvRS:
                    UpdateRSDKvRSLimits();
                    break;
                case EngineType.RSDKv1:
                    UpdateRSDKv1Limits();
                    break;
                case EngineType.RSDKv2:
                    UpdateRSDKv2Limits();
                    break;
                case EngineType.RSDKvB:
                    UpdateRSDKvBLimits();
                    break;
                case EngineType.RSDKv5:
                    UpdateRSDKv5Limits();
                    break;


            }

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
        }
        private void SetDelayNUDVisibilityState(bool isEnabled)
        {
            if (isEnabled)
            {
                Instance.DelayNUD.IsEnabled = true;
            }
            else
            {
                Instance.DelayNUD.IsEnabled = false;
            }
        }
        private void SetAnimationRenameVisibilityState(bool isEnabled)
        {
            if (isEnabled)
            {
                Instance.ButtonAnimationRename.IsEnabled = true;
            }
            else
            {
                Instance.ButtonAnimationRename.IsEnabled = false;
            }
        }
        private void SetFrameIDVisibilityState(bool isEnabled)
        {
            if (isEnabled)
            {
                Instance.IdentificationNUD.IsEnabled = true;
            }
            else
            {
                Instance.IdentificationNUD.IsEnabled = false;
            }
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
                if (updateUI) UpdateUI();
            }
        }
        #endregion

        #region Specific Updating

        public void UpdateIOPaths()
        {
            Instance.SpriteDirectoryLabel.Text = string.Format("Sprite Directory: {0}", (Instance.ViewModel.SpriteDirectory != "" && Instance.ViewModel.SpriteDirectory != null ? Instance.ViewModel.SpriteDirectory : "N/A"));
            Instance.AnimationPathLabel.Text = string.Format("Animation Path: {0}", (Instance.ViewModel.AnimationFilepath != "" && Instance.ViewModel.AnimationFilepath != null ? Instance.ViewModel.AnimationFilepath : "N/A"));
        }
        public void UpdateCanvasBackgroundColor()
        {
            if (SetBackgroundColorToMatchSpriteSheet && Instance.ViewModel.SpriteSheets != null)
            {
                if (Instance.ViewModel.SpriteSheets.Count > 0) Instance.CanvasBackground.Background = new SolidColorBrush(Instance.ViewModel.SpriteSheets[Instance.ViewModel.CurrentSpriteSheet.Value].TransparentColor);
            }
            else
            {
                if (Instance.BGColorPicker.SelectedColor != null) Instance.CanvasBackground.Background = new SolidColorBrush(Instance.BGColorPicker.SelectedColor.Value);
            }
        }
        public void UpdateTextureList()
        {
            if (isAnimationLoaded)
            {
                Instance.SpriteSheetList.ItemsSource = null;
                Instance.SpriteSheetList.ItemsSource = Instance.ViewModel.SpriteSheetPaths;
            }
        }

        #endregion

        #region Update Invalid Elements

        public void UpdateHitboxInvalidState(bool invalid = true, bool indexNegative = false)
        {
            Instance.HitboxLeftNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.HitboxTopNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.HitboxRightNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.HitboxBottomNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);

            Instance.HitBoxComboBox.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.HitBoxComboBox.Foreground = (invalid ? HideTextBrush : DefaultTextBrush);
            Instance.HitBoxComboBox.IsHitTestVisible = (invalid && !indexNegative ? false : true);

        }
        public void UpdateFrameInfoInvalidState(bool invalid = true)
        {
            Instance.FrameWidthNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.FrameHeightNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.FrameLeftNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.FrameTopNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.PivotX_NUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.PivotY_NUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.IdentificationNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.DelayNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);

            Instance.SpriteSheetList.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.SpriteSheetList.Foreground = (invalid ? HideTextBrush : DefaultTextBrush);
            Instance.SpriteSheetList.IsHitTestVisible = (invalid ? false : true);

            if (Instance.HitBoxComboBox.SelectedIndex == -1) UpdateHitboxInvalidState(true, true);
            else UpdateHitboxInvalidState(invalid);

        }
        public void UpdateAnimationInfoInvalidState(bool invalid = true)
        {
            Instance.SpeedNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.LoopIndexNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);

            Instance.FlagsSelector.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.FlagsSelector.Foreground = (invalid ? HideTextBrush : DefaultTextBrush);
            Instance.FlagsSelector.IsHitTestVisible = (invalid ? false : true);

            UpdateFrameInfoInvalidState(invalid);
        }
        public void UpdateInvalidState()
        {
            if (isAnimationLoaded && isEntryIndexValid)
            {
                if (isFrameIndexValid) UpdateAnimationInfoInvalidState(false);
                else UpdateFrameInfoInvalidState();
            }
            else UpdateAnimationInfoInvalidState();
        }


        #endregion

        #region Control Updates

        public void UpdateControls()
        {
            if (Instance.MenuViewFullSpriteSheets.IsChecked) Instance.ViewModel.FullFrameMode = true;
            else if (!Instance.MenuViewFullSpriteSheets.IsChecked) Instance.ViewModel.FullFrameMode = false;
            if (Instance.ViewModel.CurrentSpriteSheet != null && Instance.ViewModel.SpriteSheets != null)
            {
                if (Instance.ViewModel.CurrentSpriteSheet.Value - 1 > Instance.ViewModel.SpriteSheets.Count) Instance.ViewModel.CurrentSpriteSheet = 0;
            }
        }

        #endregion

        #region Enabled State Updates
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
        public void UpdateEnabledState()
        {
            if (Instance.ViewModel != null)
            {
                UpdateListEnabledState(isEntrySelected && isAnimationLoaded);
                UpdateFrameListEnabledState(isFrameSelected);
                UpdateMenuStripEnabledState(isAnimationLoaded);
            }
            else
            {
                UpdateListEnabledState(false);
                UpdateFrameListEnabledState(false);
                UpdateListEnabledState(false);
                UpdateMenuStripEnabledState(false);
            }
        }
        private void UpdateFrameListEnabledState(bool enabled)
        {
            bool isEntrySelected = Instance.List.SelectedItem != null;
            Instance.ButtonFrameAdd.IsEnabled = isEntrySelected;
            Instance.ButtonFrameDupe.IsEnabled = enabled;
            Instance.ButtonFrameExport.IsEnabled = enabled;
            Instance.ButtonFrameImport.IsEnabled = enabled;
            Instance.ButtonFrameRemove.IsEnabled = enabled;
            Instance.ButtonFrameLeft.IsEnabled = enabled;
            Instance.ButtonFrameRight.IsEnabled = enabled;
            UpdateZoomInOutEnabledState(enabled);
            Instance.ButtonShowFieldHitbox.IsEnabled = enabled;
            Instance.ButtonShowCenter.IsEnabled = enabled;
        }
        private void UpdateZoomInOutEnabledState(bool enabled)
        {
            if (Instance.ViewModel.SpriteScaleX < 7) Instance.ButtonZoomIn.IsEnabled = enabled;
            else Instance.ButtonZoomIn.IsEnabled = false;
            if (Instance.ViewModel.SpriteScaleX > 1) Instance.ButtonZoomOut.IsEnabled = enabled;
            else Instance.ButtonZoomOut.IsEnabled = false;

        }
        private void UpdateMenuStripEnabledState(bool enabled)
        {
            Instance.MenuFileSave.IsEnabled = enabled;
            Instance.MenuFileSaveAs.IsEnabled = enabled;
            Instance.MenuFileUnloadAnimation.IsEnabled = enabled;
        }
        private void UpdateListEnabledState(bool enabled)
        {
            bool isFrameSelected = Instance.FramesList.SelectedItem != null;
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
        }
        #endregion

        #region Frame/Animation List Refreshing
        public void RefreshAnimationList()
        {
            Instance.List.ItemsSource = Instance.ViewModel.Animations;
            Instance.List.UpdateLayout();

            Instance.FramesCountLabel.Text = Instance.ViewModel.FramesCount.ToString();
            Instance.AnimationsCountLabel.Text = Instance.ViewModel.AnimationsCount.ToString();
            Instance.AllFramesCountLabel.Text = Instance.ViewModel.GetCurrentFrameIndexForAllAnimations().ToString();

            if (Instance.ViewModel.SelectedAnimationIndex == -1)
            {
                Instance.FramesList.Height = 21;
                Instance.FramesList.Visibility = Visibility.Hidden;
                Instance.FakeScrollbar.Visibility = Visibility.Visible;
            }
            else
            {
                Instance.FramesList.Height = Double.NaN;
                Instance.FramesList.Visibility = Visibility.Visible;
                Instance.FakeScrollbar.Visibility = Visibility.Hidden;
            }
        }
        public void RefreshAnimationsListFully()
        {
            var temp = Instance.List.SelectedItem;
            Instance.List.ItemsSource = null;
            Instance.List.ItemsSource = Instance.ViewModel.Animations;
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
                    frame.Source = Instance.ViewModel.GetFrameImage(i);
                    Instance.FramesList.Items.Add(frame);
                }
                Instance.FramesList.UpdateLayout();
                if (Instance.FramesList.Items.Contains(temp)) Instance.FramesList.SelectedItem = temp;
            }

        }
        public void RefreshCurrentFrame()
        {
            int selectedIndex = Instance.ViewModel.SelectedFrameIndex;
            if (selectedIndex != -1 && isAnimationLoaded)
            {
                PreventIndexUpdate = true;
                Instance.ViewModel.InvalidateFrameImage(selectedIndex);
                System.Windows.Controls.Image frame = new System.Windows.Controls.Image();
                frame.Width = 45;
                frame.Height = 45;
                frame.Source = Instance.ViewModel.GetFrameImage(selectedIndex);
                Instance.FramesList.Items.RemoveAt(selectedIndex);
                Instance.FramesList.Items.Insert(selectedIndex, frame);
                Instance.ViewModel.SelectedFrameIndex = selectedIndex;
                Instance.FramesList.SelectedIndex = Instance.ViewModel.SelectedFrameIndex;
                Instance.FramesList.UpdateLayout();
            }
        }
        #endregion

        #region UI Updating

        public void UpdateUI(bool frameInfoUpdate = false)
        {
            if (PreventIndexUpdate) return;
            if (Instance.ViewModel != null)
            {
                RefreshAnimationList();
                if (frameInfoUpdate) RefreshCurrentFrame();
                UpdateValues();
                Render();
                UpdateInvalidState();
                UpdateIOPaths();
                UpdateCanvasBackgroundColor();
                if (!isPlaybackInProgress) UpdateEnabledState();
                PreventIndexUpdate = false;
                UpdateAnimationTypeLimitations();
                Instance.CanvasView.InvalidateVisual();
            }

        }


        #endregion

        #region Values Updating
        public void UpdateValues()
        {
            UpdateAnimatonValues();
            UpdateFrameValues();
            if (isHitboxesValid) UpdateHitboxValues();
        }
        private void UpdateFrameValues()
        {
            if (isHitboxesValid) UpdateHitboxValues();

            Instance.FrameWidthNUD.Value = Instance.ViewModel.SelectedFrameWidth;
            Instance.FrameHeightNUD.Value = Instance.ViewModel.SelectedFrameHeight;
            Instance.FrameLeftNUD.Value = Instance.ViewModel.SelectedFrameLeft;
            Instance.FrameTopNUD.Value = Instance.ViewModel.SelectedFrameTop;
            Instance.PivotX_NUD.Value = Instance.ViewModel.SelectedFramePivotX;
            Instance.PivotY_NUD.Value = Instance.ViewModel.SelectedFramePivotY;
            Instance.IdentificationNUD.Value = Instance.ViewModel.SelectedFrameID;
            Instance.DelayNUD.Value = Instance.ViewModel.SelectedFrameDuration;

            UpdateNUD_Cap_Values();


            Instance.SpriteSheetList.ItemsSource = Instance.ViewModel.SpriteSheetPaths;
            Instance.SpriteSheetList.SelectedIndex = (Instance.ViewModel.CurrentSpriteSheet.HasValue ? Instance.ViewModel.CurrentSpriteSheet.Value : 0);

            if (AnimationFramesValid && isFrameIndexValid) Instance.ViewModel.AnimationFrames[Instance.ViewModel.SelectedFrameIndex].SpriteSheet = Instance.ViewModel.CurrentSpriteSheet.Value;


            if (isSpriteSheetsValid) Instance.SpriteSheetList.SelectedValue = Instance.ViewModel.SpriteSheetPaths[Instance.SpriteSheetList.SelectedIndex];

            if (isFrameIndexValid && AnimationFramesValid)
            {
                if (Instance.ViewModel.AnimationFrames[Instance.ViewModel.SelectedFrameIndex] != null)
                {
                    if (Instance.ViewModel.AnimationFrames[Instance.ViewModel.SelectedFrameIndex].SpriteSheet != Instance.ViewModel.CurrentSpriteSheet && Instance.ViewModel.CurrentSpriteSheet != null)
                    {
                        Instance.ViewModel.AnimationFrames[Instance.ViewModel.SelectedFrameIndex].SpriteSheet = Instance.ViewModel.CurrentSpriteSheet.Value;
                    }
                }
            }


        }
        private void UpdateHitboxValues()
        {
            Instance.HitBoxComboBox.ItemsSource = Instance.ViewModel.CollisionBoxesNames;
            Instance.HitBoxComboBox.SelectedIndex = Instance.ViewModel.SelectedFrameHitboxIndex;

            Instance.HitboxLeftNUD.Value = Instance.ViewModel.SelectedHitboxLeft;
            Instance.HitboxRightNUD.Value = Instance.ViewModel.SelectedHitboxTop;
            Instance.HitboxTopNUD.Value = Instance.ViewModel.SelectedHitboxRight;
            Instance.HitboxBottomNUD.Value = Instance.ViewModel.SelectedHitboxBottom;

        }
        private void UpdateAnimatonValues()
        {
            Instance.SpeedNUD.Value = Instance.ViewModel.Speed;
            Instance.LoopIndexNUD.Value = Instance.ViewModel.Loop;
            Instance.FlagsSelector.SelectedIndex = (Instance.ViewModel.Flags.HasValue ? Instance.ViewModel.Flags.Value : 0);
        }
        public void UpdateNUD_Cap_Values()
        {
            Instance.FrameWidthNUD.Minimum = 0;
            Instance.FrameHeightNUD.Minimum = 0;
            Instance.FrameLeftNUD.Minimum = 0;
            Instance.FrameTopNUD.Minimum = 0;

            if (Instance.ViewModel.SpriteSheets != null && Instance.ViewModel.SpriteSheets.Count > 0 && Instance.ViewModel.isCurrentSpriteSheetValid())
            {
                int SheetHeight = (int)Instance.ViewModel.SpriteSheets[(int)Instance.ViewModel.CurrentSpriteSheet].Image.Height;
                int SheetWidth = (int)Instance.ViewModel.SpriteSheets[(int)Instance.ViewModel.CurrentSpriteSheet].Image.Width;
                int FrameTop = (int)Instance.ViewModel.SelectedFrameTop.Value;
                int FrameHeight = (int)Instance.ViewModel.SelectedFrameHeight.Value;
                int FrameWidth = (int)Instance.ViewModel.SelectedFrameWidth.Value;
                int FrameLeft = (int)Instance.ViewModel.SelectedFrameLeft.Value;

                Instance.FrameWidthNUD.Maximum = (FrameLeft + FrameWidth < SheetWidth ? SheetWidth - FrameLeft : 0);
                Instance.FrameHeightNUD.Maximum = (FrameTop + FrameHeight < SheetHeight ? SheetHeight - FrameTop : 0);
                Instance.FrameLeftNUD.Maximum = SheetWidth - FrameWidth;
                Instance.FrameTopNUD.Maximum = SheetHeight - FrameHeight;
            }
            else
            {
                Instance.FrameWidthNUD.Maximum = 0;
                Instance.FrameHeightNUD.Maximum = 0;
                Instance.FrameLeftNUD.Maximum = 0;
                Instance.FrameTopNUD.Maximum = 0;
            }
        }
        #endregion

        #region Update Bitmaps
        public void UpdateSheetImage()
        {
            if (Instance.ViewModel.SpriteSheets == null || Instance.ViewModel.SpriteSheets.Count == 0) return;


            if (Instance.MenuViewTransparentSpriteSheets.IsChecked)
            {
                var image = Instance.ViewModel.SpriteSheets[Instance.ViewModel.CurrentSpriteSheet.Value];
                if (image.isReady) CurrentSpriteSheet = SkiaSharp.Views.WPF.WPFExtensions.ToSKBitmap(image.TransparentImage);
                else CurrentSpriteSheet = null;
            }
            else
            {
                var image = Instance.ViewModel.SpriteSheets[Instance.ViewModel.CurrentSpriteSheet.Value];
                if (image.isReady) CurrentSpriteSheet = SkiaSharp.Views.WPF.WPFExtensions.ToSKBitmap(image.Image);
                else CurrentSpriteSheet = null;
            }
        }
        public void UpdateFrameImage()
        {
            if (Instance.ViewModel.SpriteSheets == null) return;
            if (CurrentSpriteSheet == null) return;

            double val_x = Instance.ViewModel.SelectedFrameLeft.Value;
            double val_y = Instance.ViewModel.SelectedFrameTop.Value;
            double val_width = Instance.ViewModel.SelectedFrameWidth.Value;
            double val_height = Instance.ViewModel.SelectedFrameHeight.Value;

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

        #region Drawing/Rendering

        public void Render()
        {
            UpdateControls();

            UpdateSheetImage();
            UpdateFrameImage();

            Instance.CanvasView.InvalidateVisual();
            UpdateCanvasBackgroundColor();
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
            float l = center_x - Instance.ViewModel.SelectedHitboxLeft;
            float r = center_x - Instance.ViewModel.SelectedHitboxRight;

            float b = center_y + Instance.ViewModel.SelectedHitboxBottom;
            float t = center_y + Instance.ViewModel.SelectedHitboxTop;

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
            int frame_x = (int)Instance.ViewModel.SelectedFrameLeft;
            int frame_y = (int)Instance.ViewModel.SelectedFrameTop;

            int frame_width = (int)Instance.ViewModel.SelectedFrameWidth;
            int frame_height = (int)Instance.ViewModel.SelectedFrameHeight;

            int frame_center_x = (ForceCenterFrame ? frame_width / 2 : -(int)Instance.ViewModel.SelectedFramePivotX.Value);
            int frame_center_y = (ForceCenterFrame ? frame_height / 2 : -(int)Instance.ViewModel.SelectedFramePivotY.Value);

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
