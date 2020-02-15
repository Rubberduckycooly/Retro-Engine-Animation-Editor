using AnimationEditor.Animation;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using AnimationEditor.Services;
using System.Windows.Media.Imaging;
using AnimationEditor.Pages;
using AnimationEditor.Animation.Classes;

namespace AnimationEditor
{
    public class UserInterfacer
    {
        private MainWindow Instance;


        public UserInterfacer(MainWindow window)
        {
            Instance = window;
            SetOldInitilizationValues();
        }

        #region Old Rendering (Legacy Code in the Process of Phasing Out)
        private Brush DefaultBorderBrush;
        private Brush DefaultTextBrush;
        private Brush HideTextBrush;

        private void SetOldInitilizationValues()
        {
            DefaultBorderBrush = Instance.DefaultBorderBrush;
            DefaultTextBrush = Instance.DefaultTextBrush;
            HideTextBrush = Brushes.Transparent;
        }


        public bool PreventIndexUpdate = false;



        #region UI Updating

        public void UpdateUI(bool frameInfoUpdate = false)
        {
            if (PreventIndexUpdate) return;
            if (Instance.ViewModel != null)
            {
                UpdateList();
                if (frameInfoUpdate) UpdateCurrentFrameInList();
                UpdateInfo();
                Render();
                UpdateInvalidState();
                UpdatePaths();
                UpdateTransparencyColors();
            }
            if (!Instance.isPlaybackEnabled) UpdateNormalElements();
            PreventIndexUpdate = false;
            UpdateAnimationTypeLimitations();
            Instance.CanvasView.InvalidateVisual();
        }

        public void UpdatePaths()
        {
            Instance.SpriteDirectoryLabel.Text = string.Format("Sprite Directory: {0}", (Instance.ViewModel.SpriteDirectory != "" && Instance.ViewModel.SpriteDirectory != null ? Instance.ViewModel.SpriteDirectory : "N/A"));
            Instance.AnimationPathLabel.Text = string.Format("Animation Path: {0}", (Instance.ViewModel.AnimationFilepath != "" && Instance.ViewModel.AnimationFilepath != null ? Instance.ViewModel.AnimationFilepath : "N/A"));
        }

        public void UpdateTransparencyColors()
        {
            if (Instance.MenuViewSetBackgroundToTransparentColor.IsChecked && Instance.ViewModel.SpriteSheets != null)
            {
                if (Instance.ViewModel.SpriteSheets.Count > 0)
                {
                    Instance.CanvasBackground.Background = new SolidColorBrush(Instance.ViewModel.SpriteSheets[Instance.ViewModel.CurrentSpriteSheet.Value].TransparentColor);
                }
            }
            else
            {
                if (Instance.BGColorPicker.SelectedColor != null)
                {
                    Instance.CanvasBackground.Background = new SolidColorBrush(Instance.BGColorPicker.SelectedColor.Value);
                }

            }
        }

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
                Instance.IdentificationNUD.IsEnabled = false;
                Instance.DelayNUD.IsEnabled = false;
                Instance.ButtonAnimationRename.IsEnabled = false;
            }
            void UpdateRSDKv1Limits()
            {
                Instance.IdentificationNUD.IsEnabled = false;
                Instance.DelayNUD.IsEnabled = false;
                Instance.ButtonAnimationRename.IsEnabled = false;
            }

            void UpdateRSDKv2Limits()
            {
                Instance.IdentificationNUD.IsEnabled = false;
                Instance.DelayNUD.IsEnabled = false;
            }

            void UpdateRSDKvBLimits()
            {
                Instance.IdentificationNUD.IsEnabled = false;
                Instance.DelayNUD.IsEnabled = false;
            }

            void UpdateRSDKv5Limits()
            {

            }
        }

        public void UpdateInvalidState()
        {
            if (Instance.ViewModel.LoadedAnimationFile != null && Instance.ViewModel.SelectedAnimationIndex != -1)
            {
                if (Instance.ViewModel.SelectedFrameIndex != -1)
                {
                    UpdateAnimationInfoInvalidState(false);
                }
                else
                {
                    UpdateFrameInfoInvalidState();
                }
            }
            else
            {
                UpdateAnimationInfoInvalidState();
            }

            void UpdateHitboxInvalidState(bool invalid = true, bool indexNegative = false)
            {
                Instance.HitboxLeftNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
                Instance.HitboxTopNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
                Instance.HitboxRightNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
                Instance.HitboxBottomNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);

                Instance.HitBoxComboBox.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
                Instance.HitBoxComboBox.Foreground = (invalid ? HideTextBrush : DefaultTextBrush);
                Instance.HitBoxComboBox.IsHitTestVisible = (invalid && !indexNegative ? false : true);

            }

            void UpdateFrameInfoInvalidState(bool invalid = true)
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

            void UpdateAnimationInfoInvalidState(bool invalid = true)
            {
                Instance.SpeedNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
                Instance.LoopIndexNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);

                Instance.FlagsSelector.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
                Instance.FlagsSelector.Foreground = (invalid ? HideTextBrush : DefaultTextBrush);
                Instance.FlagsSelector.IsHitTestVisible = (invalid ? false : true);

                UpdateFrameInfoInvalidState(invalid);
            }
        }

        public void InvalidateTextureList()
        {
            if (Instance.ViewModel.LoadedAnimationFile != null)
            {
                Instance.SpriteSheetList.ItemsSource = null;
                Instance.SpriteSheetList.ItemsSource = Instance.ViewModel.SpriteSheetPaths;
            }
        }

        #region Image/Hitbox Update Methods

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

        #region Frames/List Update Methods
        public void FullUpdateList()
        {
            var temp = Instance.List.SelectedItem;
            Instance.List.ItemsSource = null;
            Instance.List.ItemsSource = Instance.ViewModel.Animations;
            Instance.List.UpdateLayout();
            if (Instance.List.Items.Contains(temp)) Instance.List.SelectedItem = temp;
        }

        public void UpdateList()
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

        public void UpdateFramesList()
        {
            if (Instance.ViewModel.LoadedAnimationFile != null && Instance.ViewModel.SelectedAnimationIndex != -1)
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

        public void UpdateCurrentFrameInList()
        {
            int selectedIndex = Instance.ViewModel.SelectedFrameIndex;
            if (selectedIndex != -1 && Instance.ViewModel.LoadedAnimationFile != null)
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

        public void UpdateInfo()
        {
            UpdateAnimatonInfo();
            UpdateFrameInfo();
            if (Instance.ViewModel.Hitboxes != null && Instance.ViewModel.Hitboxes.Count != 0) UpdateHitboxInfo();


            void UpdateFrameInfo()
            {
                if (Instance.ViewModel.Hitboxes != null && Instance.ViewModel.Hitboxes.Count != 0) UpdateHitboxInfo();

                Instance.FrameWidthNUD.Value = Instance.ViewModel.SelectedFrameWidth;
                Instance.FrameHeightNUD.Value = Instance.ViewModel.SelectedFrameHeight;
                Instance.FrameLeftNUD.Value = Instance.ViewModel.SelectedFrameLeft;
                Instance.FrameTopNUD.Value = Instance.ViewModel.SelectedFrameTop;
                Instance.PivotX_NUD.Value = Instance.ViewModel.SelectedFramePivotX;
                Instance.PivotY_NUD.Value = Instance.ViewModel.SelectedFramePivotY;
                Instance.IdentificationNUD.Value = Instance.ViewModel.SelectedFrameId;
                Instance.DelayNUD.Value = Instance.ViewModel.SelectedFrameDuration;

                UpdateFrameNUDMaxMin();


                Instance.SpriteSheetList.ItemsSource = Instance.ViewModel.SpriteSheetPaths;
                Instance.SpriteSheetList.SelectedIndex = (Instance.ViewModel.CurrentSpriteSheet.HasValue ? Instance.ViewModel.CurrentSpriteSheet.Value : 0);

                if (Instance.ViewModel.AnimationFrames != null && Instance.ViewModel.SelectedFrameIndex != -1) Instance.ViewModel.AnimationFrames[Instance.ViewModel.SelectedFrameIndex].SpriteSheet = Instance.ViewModel.CurrentSpriteSheet.Value;


                if (Instance.ViewModel.SpriteSheetPaths != null && Instance.ViewModel.SpriteSheetPaths.Count > 0) Instance.SpriteSheetList.SelectedValue = Instance.ViewModel.SpriteSheetPaths[Instance.SpriteSheetList.SelectedIndex];

                if (Instance.ViewModel.SelectedFrameIndex != -1 && Instance.ViewModel.SelectedFrameIndex != null && Instance.ViewModel.AnimationFrames != null)
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
            void UpdateHitboxInfo()
            {
                Instance.HitBoxComboBox.ItemsSource = Instance.ViewModel.CollisionBoxesNames;
                Instance.HitBoxComboBox.SelectedIndex = Instance.ViewModel.SelectedFrameHitboxIndex;

                Instance.HitboxLeftNUD.Value = Instance.ViewModel.SelectedHitboxLeft;
                Instance.HitboxRightNUD.Value = Instance.ViewModel.SelectedHitboxTop;
                Instance.HitboxTopNUD.Value = Instance.ViewModel.SelectedHitboxRight;
                Instance.HitboxBottomNUD.Value = Instance.ViewModel.SelectedHitboxBottom;

            }
            void UpdateAnimatonInfo()
            {
                Instance.SpeedNUD.Value = Instance.ViewModel.Speed;
                Instance.LoopIndexNUD.Value = Instance.ViewModel.Loop;
                Instance.FlagsSelector.SelectedIndex = (Instance.ViewModel.Flags.HasValue ? Instance.ViewModel.Flags.Value : 0);
            }
        }
        #endregion

        public void UpdateFrameNUDMaxMin()
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
        public void UpdateFrameIndex(bool subtract = false, bool updateUI = true)
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

        public void DisablePlaybackModeElements(bool enabled)
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

        public void UpdateNormalElements()
        {
            if (Instance.ViewModel != null)
            {
                UpdateListElements(Instance.ViewModel.LoadedAnimationFile != null);
                UpdateFrameListElements(Instance.FramesList.SelectedItem != null);
                UpdateListElements(Instance.ViewModel.SelectedAnimation != null);
                UpdateMenuStripElements(Instance.ViewModel.LoadedAnimationFile != null);
            }
            else
            {
                UpdateListElements(false);
                UpdateFrameListElements(false);
                UpdateListElements(false);
                UpdateMenuStripElements(false);
            }


            void UpdateListElements(bool enabled)
            {
                Instance.ButtonAnimationAdd.IsEnabled = enabled;
                Instance.ButtonAnimationRemove.IsEnabled = enabled;
                Instance.ButtonAnimationExport.IsEnabled = enabled;
                Instance.ButtonAnimationImport.IsEnabled = enabled;
                Instance.ButtonAnimationDuplicate.IsEnabled = enabled;
                Instance.ButtonAnimationUp.IsEnabled = enabled;
                Instance.ButtonAnimationDown.IsEnabled = enabled;
                Instance.ButtonPlay.IsEnabled = Instance.FramesList.SelectedItem != null;
                Instance.PlaybackOptionsButton.IsEnabled = Instance.FramesList.SelectedItem != null;
                Instance.AnimationScroller.IsEnabled = enabled;
                Instance.HitboxButton.IsEnabled = enabled;
                Instance.ButtonAnimationRename.IsEnabled = enabled;
                Instance.TextureButton.IsEnabled = enabled;
            }

            void UpdateFrameListElements(bool enabled)
            {
                Instance.ButtonFrameAdd.IsEnabled = Instance.List.SelectedItem != null;
                Instance.ButtonFrameDupe.IsEnabled = enabled;
                Instance.ButtonFrameExport.IsEnabled = enabled;
                Instance.ButtonFrameImport.IsEnabled = enabled;
                Instance.ButtonFrameRemove.IsEnabled = enabled;
                Instance.ButtonFrameLeft.IsEnabled = enabled;
                Instance.ButtonFrameRight.IsEnabled = enabled;
                UpdateZoomInOutButtons(enabled);
                Instance.ButtonShowFieldHitbox.IsEnabled = enabled;
                Instance.ButtonShowCenter.IsEnabled = enabled;
            }

            void UpdateZoomInOutButtons(bool enabled)
            {
                if (Instance.ViewModel.SpriteScaleX < 7) Instance.ButtonZoomIn.IsEnabled = enabled;
                else Instance.ButtonZoomIn.IsEnabled = false;
                if (Instance.ViewModel.SpriteScaleX > 1) Instance.ButtonZoomOut.IsEnabled = enabled;
                else Instance.ButtonZoomOut.IsEnabled = false;

            }

            void UpdateMenuStripElements(bool enabled)
            {
                Instance.MenuFileSave.IsEnabled = enabled;
                Instance.MenuFileSaveAs.IsEnabled = enabled;
                Instance.MenuFileUnloadAnimation.IsEnabled = enabled;
            }
        }
        #endregion

        #endregion

        #region New Rendering

        #region Modes
        public bool ShowFrameBorder
        {
            get => Instance.MenuViewFrameBorder.IsChecked;
        }

        public bool ShowSolidImageBackground
        {
            get => !Instance.MenuViewTransparentSpriteSheets.IsChecked;
        }

        public bool ForceCenterFrame = false;

        private bool ShowHitBox
        {
            get => Instance.ButtonShowFieldHitbox.IsChecked.Value;
        }

        private bool RenderRefrenceFrameFirst { get; set; } = false;
        private bool ShowAlignmentLines
        {
            get => Instance.ButtonShowCenter.IsChecked.Value;
        }

        private bool ShowFullFrame
        {
            get => Instance.ViewModel.FullFrameMode;
        }
        #endregion

        #region Bitmaps
        public SkiaSharp.SKBitmap CurrentSpriteSheet;
        public SkiaSharp.SKBitmap CurrentSpriteSheetFrame;
        public string CurrentSpriteSheetName;

        #endregion

        #region Colors
        public Color AlignmentLinesColor = Colors.Red;
        public Color ImgBG = Colors.White;
        public Color RefImgBG = Colors.White;
        public Color FrameBorder = Colors.Black;
        public Color RefFrameBorder = Colors.Black;
        public Color FrameBG = Colors.Transparent;
        public Color RefFrameBG = Colors.Transparent;
        #endregion

        #region Opacity
        private double _RefrenceOpacity = 100;
        public double RefrenceOpacity { get => _RefrenceOpacity; set => _RefrenceOpacity = value * 0.01; }
        #endregion


        #region Get/Set Bitmaps

        public void UpdateSheetImage()
        {
            if (Instance.ViewModel.SpriteSheets == null) return;


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
            UpdateTransparencyColors();
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

                if (RenderRefrenceFrameFirst)
                {
                    Draw(true);
                    Draw();
                }
                else
                {
                    Draw();
                    Draw(true);
                }


                if (ShowAlignmentLines)
                {
                    SkiaSharp.SKPoint x1 = new SkiaSharp.SKPoint(0, height_half);
                    SkiaSharp.SKPoint y1 = new SkiaSharp.SKPoint(width, height_half);
                    SkiaSharp.SKPoint x2 = new SkiaSharp.SKPoint(width_half, 0);
                    SkiaSharp.SKPoint y2 = new SkiaSharp.SKPoint(width_half, height);



                    canvas.DrawLine(x1, y1, new SkiaSharp.SKPaint() { Color = SkiaSharp.Views.WPF.WPFExtensions.ToSKColor(AlignmentLinesColor) });
                    canvas.DrawLine(x2, y2, new SkiaSharp.SKPaint() { Color = SkiaSharp.Views.WPF.WPFExtensions.ToSKColor(AlignmentLinesColor) });
                }


                if (ShowHitBox)
                {
                    float bx = width_half + Instance.ViewModel.SelectedHitboxLeft;
                    float by = height_half + Instance.ViewModel.SelectedHitboxBottom;
                    float h = Instance.ViewModel.SelectedHitboxTop;
                    float w = Instance.ViewModel.SelectedHitboxRight;


                    SkiaSharp.SKPoint x1 = new SkiaSharp.SKPoint(bx, by);
                    SkiaSharp.SKPoint x2 = new SkiaSharp.SKPoint(bx + w, by);
                    SkiaSharp.SKPoint y1 = new SkiaSharp.SKPoint(bx, by + h);
                    SkiaSharp.SKPoint y2 = new SkiaSharp.SKPoint(bx + w, by + h);

                    canvas.DrawLine(x1, x2, new SkiaSharp.SKPaint() { Color = SkiaSharp.Views.WPF.WPFExtensions.ToSKColor(AlignmentLinesColor) });
                    canvas.DrawLine(y1, y2, new SkiaSharp.SKPaint() { Color = SkiaSharp.Views.WPF.WPFExtensions.ToSKColor(AlignmentLinesColor) });
                    canvas.DrawLine(x1, y1, new SkiaSharp.SKPaint() { Color = SkiaSharp.Views.WPF.WPFExtensions.ToSKColor(AlignmentLinesColor) });
                    canvas.DrawLine(x2, y2, new SkiaSharp.SKPaint() { Color = SkiaSharp.Views.WPF.WPFExtensions.ToSKColor(AlignmentLinesColor) });

                    var paint = new SkiaSharp.SKPaint();
                    var transparency = SkiaSharp.Views.WPF.WPFExtensions.ToSKColor(AlignmentLinesColor);
                    paint.Color = transparency;

                    canvas.DrawRect(new SkiaSharp.SKRect() { Top = by, Left = bx, Size = new SkiaSharp.SKSize(w, h) }, paint);
                }
            }

            void Draw(bool isRefrence = false)
            {
                DrawSprite(canvas, width_half, height_half, width, height);
            }
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
                var paint = new SkiaSharp.SKPaint() { Color = SkiaSharp.Views.WPF.WPFExtensions.ToSKColor(ImgBG) };
                SkiaSharp.SKRect rect;
                if (ShowFullFrame && CurrentSpriteSheet != null) rect = new SkiaSharp.SKRect() { Top = y, Left = x, Size = new SkiaSharp.SKSize(CurrentSpriteSheet.Width, CurrentSpriteSheet.Height) };
                else rect = new SkiaSharp.SKRect() { Top = y, Left = x, Size = new SkiaSharp.SKSize(w, h) };

                canvas.DrawRect(rect, paint);
            }

            if (ShowSolidImageBackground)
            {
                var paint = new SkiaSharp.SKPaint() { Color = SkiaSharp.Views.WPF.WPFExtensions.ToSKColor(ImgBG) };
                SkiaSharp.SKRect rect;
                if (ShowFullFrame && CurrentSpriteSheet != null) rect = new SkiaSharp.SKRect() { Top = y, Left = x, Size = new SkiaSharp.SKSize(CurrentSpriteSheet.Width, CurrentSpriteSheet.Height) };
                else rect = new SkiaSharp.SKRect() { Top = y, Left = x, Size = new SkiaSharp.SKSize(w, h) };

                canvas.DrawRect(rect, paint);
            }

            canvas.DrawBitmap((ShowFullFrame ? CurrentSpriteSheet : CurrentSpriteSheetFrame), new SkiaSharp.SKPoint(x, y));

            if (ShowFrameBorder)
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
                var transparency = SkiaSharp.Views.WPF.WPFExtensions.ToSKColor(FrameBG);
                paint.Color = transparency;

                canvas.DrawRect(new SkiaSharp.SKRect() { Top = by, Left = bx, Size = new SkiaSharp.SKSize(w, h) }, paint);
            }


        }

        #endregion

        #endregion
    }
}
