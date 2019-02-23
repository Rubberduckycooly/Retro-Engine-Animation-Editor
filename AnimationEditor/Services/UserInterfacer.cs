using AnimationEditor.ViewModels;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace AnimationEditor
{
    public class UserInterfacer
    {
        private MainWindow Instance;
        public UserInterfacer(MainWindow window)
        {
            Instance = window;
        }

        #region UI Updating

        public void UpdateUI()
        {
            UpdateList();
            UpdateFramesList();
            UpdateFrameInfo();
            UpdateAnimatonInfo();
            UpdateFrameInfoEnabledState();
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

            Instance.SpriteSheetList.ItemsSource = Instance.ViewModel.SpriteSheets;
            Instance.SpriteSheetList.SelectedIndex = (Instance.ViewModel.CurrentSpriteSheet.HasValue ? Instance.ViewModel.CurrentSpriteSheet.Value : -1);
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
            Instance.FlagsSelector.SelectedIndex = (Instance.ViewModel.Flags.HasValue ? Instance.ViewModel.Flags.Value : -1);
        }

        #endregion
    }
}
