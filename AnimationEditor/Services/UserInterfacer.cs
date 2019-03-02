using AnimationEditor.ViewModels;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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
                Instance.Handler.UpdateRecentsDropDown();
                UpdateList();
                if (frameInfoUpdate) UpdateCurrentFrameInList();
                UpdateInfo();
                UpdateImage();
                UpdateInvalidState();
            }
            PreventIndexUpdate = false;

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
                Instance.HitboxRightNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
                Instance.HitboxTopNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
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

        public void UpdateDisabledState()
        {
            void UpdateFrameInfoEnabledState(bool invalid = false)
            {
                Instance.FrameWidthNUD.IsEnabled = Instance.ViewModel.SelectedFrameWidth != null && invalid == false;
                Instance.FrameHeightNUD.IsEnabled = Instance.ViewModel.SelectedFrameHeight != null && invalid == false;
                Instance.FrameLeftNUD.IsEnabled = Instance.ViewModel.SelectedFrameLeft != null && invalid == false;
                Instance.FrameTopNUD.IsEnabled = Instance.ViewModel.SelectedFrameTop != null && invalid == false;
                Instance.PivotXBox.IsEnabled = Instance.ViewModel.SelectedFramePivotX != null && invalid == false;
                Instance.PivotYBox.IsEnabled = Instance.ViewModel.SelectedFramePivotY != null && invalid == false;
                Instance.idNUD.IsEnabled = Instance.ViewModel.SelectedFrameId != null && invalid == false;
                Instance.DelayNUD.IsEnabled = Instance.ViewModel.SelectedFrameDuration != null && invalid == false;



                Instance.HitBoxComboBox.IsEnabled = false && invalid == false;
                Instance.HitBoxComboBox2.IsEnabled = false && invalid == false;

                Instance.HitboxBottomNUD.IsEnabled = false && invalid == false;
                Instance.HitboxTopNUD.IsEnabled = false && invalid == false;
                Instance.HitboxLeftNUD.IsEnabled = false && invalid == false;
                Instance.HitboxRightNUD.IsEnabled = false && invalid == false;
            }

            void UpdateAnimationInfoEnabledState(bool invalid = false)
            {
                Instance.SpriteSheetList.IsEnabled = Instance.ViewModel.CurrentSpriteSheet != null && invalid == false;
                Instance.FlagsSelector.IsEnabled = Instance.ViewModel.Flags != null && invalid == false;
                Instance.SpeedNUD.IsEnabled = Instance.ViewModel.Speed != null && invalid == false;
                Instance.LoopIndexNUD.IsEnabled = Instance.ViewModel.Loop != null && invalid == false;

                UpdateFrameInfoEnabledState(invalid);
            }
        }

        public void UpdateImage()
        {
            Instance.Geomotry.Rect = Instance.ViewModel.SpriteFrame;

            Instance.ViewModel.ViewWidth = Instance.CanvasView.ActualWidth;
            Instance.ViewModel.ViewHeight = Instance.CanvasView.ActualHeight;

            System.Windows.Controls.Canvas.SetLeft(Instance.CanvasImage, Instance.ViewModel.SpriteLeft);
            System.Windows.Controls.Canvas.SetTop(Instance.CanvasImage, Instance.ViewModel.SpriteTop);
            System.Windows.Controls.Canvas.SetRight(Instance.CanvasImage, Instance.ViewModel.SpriteRight);
            System.Windows.Controls.Canvas.SetBottom(Instance.CanvasImage, Instance.ViewModel.SpriteBottom);

            if (Instance.ViewModel.CurrentSpriteSheet != null && Instance.ViewModel.LoadedAnimationFile != null && Instance.ViewModel.SpriteSheets != null)
            {
                Instance.CanvasImage.Source = Instance.ViewModel.SpriteSheets[Instance.ViewModel.CurrentSpriteSheet.Value];
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
            SetHitboxDimensions();

            if (Instance.ViewModel.SpriteService == null && Instance.ViewModel.LoadedAnimationFile != null) Instance.ViewModel.SpriteService = new AnimationEditor.Services.SpriteService(Instance.ViewModel.LoadedAnimationFile, Instance.ViewModel.AnimationDirectory, Instance);


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

        public void UpdateList()
        {
            Instance.List.ItemsSource = Instance.ViewModel.Animations;
            Instance.List.UpdateLayout();
        }

        public void UpdateFramesList()
        {
            if (Instance.ViewModel.LoadedAnimationFile != null && Instance.ViewModel.SelectedAnimationIndex != -1)
            {
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
            UpdateHitboxInfo();


            void UpdateFrameInfo()
            {
                UpdateHitboxInfo();

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
                Instance.HitboxTopNUD.Value = Instance.ViewModel.SelectedHitbox_Y;
                Instance.HitboxRightNUD.Value = Instance.ViewModel.SelectedHitbox_Width;
                Instance.HitboxBottomNUD.Value = Instance.ViewModel.SelectedHitbox_Height;

            }
            void UpdateAnimatonInfo()
            {
                Instance.SpeedNUD.Value = Instance.ViewModel.Speed;
                Instance.LoopIndexNUD.Value = Instance.ViewModel.Loop;
                Instance.FlagsSelector.SelectedIndex = (Instance.ViewModel.Flags.HasValue ? Instance.ViewModel.Flags.Value : 0);
            }
        }

        public void UpdateFrameIndex(bool subtract = false)
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
                UpdateUI();
            }


        }

        #endregion
    }
}
