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
        public UserInterfacer(MainWindow window)
        {
            Instance = window;
            DefaultBorderBrush = Instance.DelayNUD.BorderBrush;
        }

        #region UI Updating

        public void UpdateUI()
        {
            if (Instance.ViewModel != null)
            {
                UpdateList();
                UpdateFramesList();
                UpdateFrameInfo();
                UpdateAnimatonInfo();
                UpdateFrameInfoEnabledState();
                UpdateImage();
                UpdateDisabledState();
            }

        }

        public void UpdateDisabledState()
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
        }

        public void UpdateFrameInfoInvalidState(bool invalid = true)
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
        }

        public void UpdateAnimationInfoInvalidState(bool invalid = true)
        {
            Instance.SpeedNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.LoopIndexNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.FlagsSelector.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);

            UpdateFrameInfoInvalidState(invalid);
        }

        public void UpdateImage()
        {
            Instance.ImageScale.ScaleX = Instance.ViewModel.SpriteScaleX;
            Instance.ImageScale.ScaleY = Instance.ViewModel.SpriteScaleY;

            Instance.ViewModel.ViewWidth = Instance.CanvasView.ActualWidth;
            Instance.ViewModel.ViewHeight = Instance.CanvasView.ActualHeight;

            System.Windows.Controls.Canvas.SetLeft(Instance.CanvasImage, Instance.ViewModel.SpriteLeft);
            System.Windows.Controls.Canvas.SetTop(Instance.CanvasImage, Instance.ViewModel.SpriteTop);
            System.Windows.Controls.Canvas.SetRight(Instance.CanvasImage, Instance.ViewModel.SpriteRight);
            System.Windows.Controls.Canvas.SetBottom(Instance.CanvasImage, Instance.ViewModel.SpriteBottom);

            if (Instance.ViewModel.CurrentSpriteSheet != null && Instance.ViewModel.LoadedAnimationFile != null)
            {
                Instance.CanvasImage.Source = Instance.ViewModel.SpriteSheets[Instance.ViewModel.CurrentSpriteSheet.Value];
            }
            else
            {
                Instance.CanvasImage.Source = null;
            }

            Instance.Geomotry.Rect = Instance.ViewModel.SpriteFrame;
            Instance.CanvasImage.RenderTransformOrigin = Instance.ViewModel.SpriteCenter;

        }

        public void UpdateList()
        {
            Instance.List.ItemsSource = Instance.ViewModel.Animations;
            Instance.List.UpdateLayout();
        }

        public void UpdateFramesList()
        {
            Instance.FramesList.ItemsSource = Instance.ViewModel.AnimationFrames;
            Instance.FramesList.UpdateLayout();
        }

        public void UpdateFrameInfo()
        {
            Instance.FrameWidthNUD.Value = Instance.ViewModel.SelectedFrameWidth;
            Instance.FrameHeightNUD.Value = Instance.ViewModel.SelectedFrameHeight;
            Instance.FrameLeftNUD.Value = Instance.ViewModel.SelectedFrameLeft;
            Instance.FrameTopNUD.Value = Instance.ViewModel.SelectedFrameTop;
            Instance.PivotXBox.Value = Instance.ViewModel.SelectedFramePivotX;
            Instance.PivotYBox.Value = Instance.ViewModel.SelectedFramePivotY;
            Instance.idNUD.Value = Instance.ViewModel.SelectedFrameId;
            Instance.DelayNUD.Value = Instance.ViewModel.SelectedFrameDuration;


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

            Instance.SpriteSheetList.ItemsSource = Instance.ViewModel.SpriteSheetPaths;
            Instance.SpriteSheetList.SelectedIndex = (Instance.ViewModel.CurrentSpriteSheet.HasValue ? Instance.ViewModel.CurrentSpriteSheet.Value : 0);
        }

        public void UpdateFrameInfoEnabledState()
        {
            Instance.FrameWidthNUD.IsEnabled = Instance.ViewModel.SelectedFrameWidth != null;
            Instance.FrameHeightNUD.IsEnabled = Instance.ViewModel.SelectedFrameHeight != null;
            Instance.FrameLeftNUD.IsEnabled = Instance.ViewModel.SelectedFrameLeft != null;
            Instance.FrameTopNUD.IsEnabled = Instance.ViewModel.SelectedFrameTop != null;
            Instance.PivotXBox.IsEnabled = Instance.ViewModel.SelectedFramePivotX != null;
            Instance.PivotYBox.IsEnabled = Instance.ViewModel.SelectedFramePivotY != null;
            Instance.idNUD.IsEnabled = Instance.ViewModel.SelectedFrameId != null;
            Instance.DelayNUD.IsEnabled = Instance.ViewModel.SelectedFrameDuration != null;

            Instance.SpriteSheetList.IsEnabled = Instance.ViewModel.CurrentSpriteSheet != null;
            Instance.FlagsSelector.IsEnabled = Instance.ViewModel.Flags != null;
            Instance.SpeedNUD.IsEnabled = Instance.ViewModel.Speed != null;
            Instance.LoopIndexNUD.IsEnabled = Instance.ViewModel.Loop != null;

            Instance.HitBoxComboBox.IsEnabled = false;
            Instance.HitBoxComboBox2.IsEnabled = false;

            Instance.HitboxBottomNUD.IsEnabled = false;
            Instance.HitboxTopNUD.IsEnabled = false;
            Instance.HitboxLeftNUD.IsEnabled = false;
            Instance.HitboxRightNUD.IsEnabled = false;
        }

        public void UpdateAnimatonInfo()
        {
            Instance.SpeedNUD.Value = Instance.ViewModel.Speed;
            Instance.LoopIndexNUD.Value = Instance.ViewModel.Loop;
            Instance.FlagsSelector.SelectedIndex = (Instance.ViewModel.Flags.HasValue ? Instance.ViewModel.Flags.Value : 0);
        }

        #endregion
    }
}
