using AnimationEditor.ViewModels;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using AnimationEditor.Services;
using System.Windows.Media.Imaging;

namespace AnimationEditor
{
    public class UserInterfacer
    {
        private MainWindow Instance;
        private Brush DefaultBorderBrush;
        private Brush DefaultTextBrush;
        private Brush HideTextBrush;


        public bool PreventIndexUpdate = false;

        public UserInterfacer(MainWindow window)
        {
            Instance = window;
            DefaultBorderBrush = Instance.DefaultBorderBrush;
            DefaultTextBrush = Instance.DefaultTextBrush;
            HideTextBrush = Brushes.Transparent;
        }

        #region UI Updating

        public void UpdateUI(bool frameInfoUpdate = false)
        {
            if (PreventIndexUpdate) return;
            if (Instance.ViewModel != null)
            {
                UpdateList();
                if (frameInfoUpdate) UpdateCurrentFrameInList();
                UpdateInfo();
                UpdateViewerLayout();
                UpdateInvalidState();
                UpdateAnimationTypeLimitations();
            }
            if (!Instance.isPlaybackEnabled) UpdateNormalElements();
            PreventIndexUpdate = false;

        }

        public void UpdateAnimationTypeLimitations()
        {
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

            }

            void UpdateRSDKv1Limits()
            {

            }

            void UpdateRSDKv2Limits()
            {

            }

            void UpdateRSDKvBLimits()
            {

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
                Instance.PivotXBox.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
                Instance.PivotYBox.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
                Instance.idNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
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
        public void UpdateViewerLayout()
        {
            if (Instance.ViewModel.LoadedAnimationFile != null && Instance.HitBoxComboBox.SelectedItem != null && Instance.ButtonShowFieldHitbox.IsChecked.Value)
            {
                Instance.HitBoxViewer.Visibility = Visibility.Visible;
                Instance.HitBoxBackground.Visibility = Visibility.Visible;
            }
            else
            {
                Instance.HitBoxViewer.Visibility = Visibility.Hidden;
                Instance.HitBoxBackground.Visibility = Visibility.Hidden;
            }
            Instance.Geomotry.Rect = Instance.ViewModel.SpriteFrame;

            Instance.ViewModel.ViewWidth = Instance.CanvasView.ActualWidth;
            Instance.ViewModel.ViewHeight = Instance.CanvasView.ActualHeight;

            System.Windows.Controls.Canvas.SetLeft(Instance.CanvasImage, Instance.ViewModel.SpriteLeft);
            System.Windows.Controls.Canvas.SetTop(Instance.CanvasImage, Instance.ViewModel.SpriteTop);
            System.Windows.Controls.Canvas.SetRight(Instance.CanvasImage, Instance.ViewModel.SpriteRight);
            System.Windows.Controls.Canvas.SetBottom(Instance.CanvasImage, Instance.ViewModel.SpriteBottom);

            if (Instance.ViewModel.CurrentSpriteSheet != null && Instance.ViewModel.LoadedAnimationFile != null && Instance.ViewModel.SpriteSheets != null)
            {
                if (!Instance.ViewModel.NullSpriteSheetList.Contains(Instance.ViewModel.SpriteSheetPaths[Instance.ViewModel.CurrentSpriteSheet.Value]))
                {
                    Instance.CanvasImage.Source = GetSpriteSheet();
                }
                else
                {
                    Instance.CanvasImage.Source = null;
                }

            }
            else
            {
                Instance.CanvasImage.Source = null;
            }

            Instance.ImageScale.ScaleX = Instance.ViewModel.Zoom;
            Instance.ImageScale.ScaleY = Instance.ViewModel.Zoom;
            Instance.CanvasImage.RenderTransformOrigin = Instance.ViewModel.SpriteCenter;

            System.Windows.Controls.Canvas.SetLeft(Instance.HitBoxViewer, Instance.ViewModel.HitboxLeft);
            System.Windows.Controls.Canvas.SetTop(Instance.HitBoxViewer, Instance.ViewModel.HitboxTop);
            Instance.HitBoxViewer.RenderTransformOrigin = Instance.ViewModel.SpriteCenter;


            if (Instance.MenuViewFrameBorder.IsChecked)
            {
                Instance.BorderMarker.Visibility = Visibility.Visible;
            }
            else
            {
                Instance.BorderMarker.Visibility = Visibility.Hidden;
            }
            System.Windows.Controls.Canvas.SetLeft(Instance.BorderMarker, Instance.ViewModel.BorderLeft);
            System.Windows.Controls.Canvas.SetTop(Instance.BorderMarker, Instance.ViewModel.BorderTop);

            Instance.BorderMarker.RenderTransformOrigin = Instance.ViewModel.SpriteCenter;

            if (Instance.ButtonShowCenter.IsChecked.Value)
            {
                Instance.AxisX.Visibility = Visibility.Visible;
                Instance.AxisY.Visibility = Visibility.Visible;
            }
            else
            {
                Instance.AxisX.Visibility = Visibility.Hidden;
                Instance.AxisY.Visibility = Visibility.Hidden;
            }

            if (Instance.MenuViewSetBackgroundToTransparentColor.IsChecked)
            {
                Instance.CanvasView.Background = new SolidColorBrush(Instance.ViewModel.SpriteSheetTransparentColors[Instance.ViewModel.CurrentSpriteSheet.Value]);                
            }
            else
            {
                if (Instance.BGColorPicker.SelectedColor != null)
                {
                    Instance.CanvasView.Background = new SolidColorBrush(Instance.BGColorPicker.SelectedColor.Value);
                }

            }


            SetHitboxDimensions();
            SetBorderMarkerDimensions();
        }

        public void SetBorderMarkerDimensions()
        {
            double width = Instance.ViewModel.SelectedFrameWidth ?? 0;
            double height = Instance.ViewModel.SelectedFrameHeight ?? 0;

            Instance.BorderMarker.Width = width * Instance.ViewModel.Zoom;
            Instance.BorderMarker.Height = height * Instance.ViewModel.Zoom;
        }

        public void SetHitboxDimensions()
        {
            double FrameX = Instance.ViewModel.SelectedFramePivotX ?? 0 * Instance.ViewModel.Zoom;
            double FrameY = Instance.ViewModel.SelectedFramePivotY ?? 0 * Instance.ViewModel.Zoom;

            double x = (Instance.ViewModel.SelectedHitbox_X < 0 ? -Instance.ViewModel.SelectedHitbox_X : Instance.ViewModel.SelectedHitbox_X) * Instance.ViewModel.Zoom;
            double y = (Instance.ViewModel.SelectedHitbox_Y < 0 ? -Instance.ViewModel.SelectedHitbox_Y : Instance.ViewModel.SelectedHitbox_Y) * Instance.ViewModel.Zoom;
            double width = (Instance.ViewModel.SelectedHitbox_Width < 0 ? -Instance.ViewModel.SelectedHitbox_Width : Instance.ViewModel.SelectedHitbox_Width) * Instance.ViewModel.Zoom;
            double height = (Instance.ViewModel.SelectedHitbox_Height < 0 ? -Instance.ViewModel.SelectedHitbox_Height : Instance.ViewModel.SelectedHitbox_Height) * Instance.ViewModel.Zoom;

            Instance.HitBoxViewer.Width = x + y;
            Instance.HitBoxViewer.Height = width + height;
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

            if (Instance.ViewModel.SelectedAnimationIndex == -1) {
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
                Instance.PivotXBox.Value = Instance.ViewModel.SelectedFramePivotX;
                Instance.PivotYBox.Value = Instance.ViewModel.SelectedFramePivotY;
                Instance.idNUD.Value = Instance.ViewModel.SelectedFrameId;
                Instance.DelayNUD.Value = Instance.ViewModel.SelectedFrameDuration;

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

                Instance.HitboxLeftNUD.Value = Instance.ViewModel.SelectedHitbox_X;
                Instance.HitboxRightNUD.Value = Instance.ViewModel.SelectedHitbox_Y;
                Instance.HitboxTopNUD.Value = Instance.ViewModel.SelectedHitbox_Width;
                Instance.HitboxBottomNUD.Value = Instance.ViewModel.SelectedHitbox_Height;

            }
            void UpdateAnimatonInfo()
            {
                Instance.SpeedNUD.Value = Instance.ViewModel.Speed;
                Instance.LoopIndexNUD.Value = Instance.ViewModel.Loop;
                Instance.FlagsSelector.SelectedIndex = (Instance.ViewModel.Flags.HasValue ? Instance.ViewModel.Flags.Value : 0);
            }
        }
        #endregion

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

        private BitmapImage GetSpriteSheet()
        {
            if (Instance.MenuViewTransparentSpriteSheets.IsChecked)
            {
                var image = Instance.ViewModel.SpriteSheetsWithTransparency[Instance.ViewModel.CurrentSpriteSheet.Value];
                return image;
            }
            else
            {
                var image = Instance.ViewModel.SpriteSheets[Instance.ViewModel.CurrentSpriteSheet.Value];
                return image;
            }

        }

        #endregion
    }
}
