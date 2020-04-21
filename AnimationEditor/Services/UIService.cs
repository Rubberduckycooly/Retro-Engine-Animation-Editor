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
    public class UIService
    {
        #region On Property Changed Extension

        private void OnPropertyChanged(string entry)
        {
            Instance.ViewModel.CallPropertyChanged(entry);
        }

        #endregion

        #region Definitions

        #region Status

        bool isEntrySelected
        {
            get => Instance.List.SelectedItem != null;
        }
        bool isFrameSelected
        {
            get => Instance.FramesList.SelectedItem != null;
        }
        bool isAnimationLoaded
        {
            get => Instance.ViewModel != null && Instance.ViewModel.LoadedAnimationFile != null;
        }

        #endregion

        #region Brushes

        private Brush DefaultBorderBrush
        {
            get
            {
                return ColorExt.GetSCBResource(Instance, "ComboBoxBorder");
            }
        }
        private Brush DefaultTextBrush
        {
            get
            {
                return ColorExt.GetSCBResource(Instance, "NormalText");
            }
        }
        private Brush HideTextBrush
        {
            get
            {
                return System.Windows.Media.Brushes.Transparent;
            }
        }

        #endregion

        #region Misc
        private MainWindow Instance { get; set; }

        #endregion

        #endregion

        #region Init
        public UIService(MainWindow window)
        {
            Instance = window;
        }
        #endregion

        #region Control Update
        public void UpdateFrameSections()
        {
            UpdateFrameValuesEnabledState();
            UpdateFrameControlsEnabledState();
            UpdateFrameValuesVisual();
            UpdateHitboxSections();
        }
        public void UpdateEntrySections()
        {
            UpdateAnimationValuesVisual();
            UpdateAnimationValuesEnabledState();
            UpdateAnimationControlsEnabledState();
        }
        public void UpdateHitboxSections()
        {
            UpdateHitboxValuesVisual();
            UpdateHitboxValuesEnabledState();
        }
        public void UpdateGeneralSections()
        {
            UpdateGeneralControlsVisual();
            UpdateGeneralControlsEnabledState();
            UpdateIndexStatusVisual();
        }

        #endregion

        #region Update Enabled Status
        public void UpdateFrameValuesEnabledState()
        {
            bool enabled = isFrameSelected;
            bool invalid = !(isFrameSelected && isAnimationLoaded);
            bool noPlayback = !GlobalService.PropertyHandler.isPlaybackEnabled;

            Instance.FrameWidthNUD.IsEnabled = enabled;
            Instance.FrameHeightNUD.IsEnabled = enabled;
            Instance.FrameX_NUD.IsEnabled = enabled;
            Instance.FrameY_NUD.IsEnabled = enabled;
            Instance.PivotX_NUD.IsEnabled = enabled;
            Instance.PivotY_NUD.IsEnabled = enabled;

            Instance.SpriteSheetList.IsEnabled = enabled;
            Instance.SpriteSheetList.IsHitTestVisible = (invalid ? false : true);
        }
        public void UpdateFrameControlsEnabledState()
        {
            bool enabled = isFrameSelected;
            bool invalid = !(isFrameSelected && isAnimationLoaded);
            bool noPlayback = !GlobalService.PropertyHandler.isPlaybackEnabled;

            Instance.ButtonFrameAdd.IsEnabled = isEntrySelected && noPlayback;
            Instance.ButtonFrameDupe.IsEnabled = enabled && noPlayback;
            Instance.ButtonFrameExport.IsEnabled = enabled && noPlayback;
            Instance.ButtonFrameImport.IsEnabled = enabled && noPlayback;
            Instance.ButtonFrameRemove.IsEnabled = enabled && noPlayback;
            Instance.ButtonFrameLeft.IsEnabled = enabled && noPlayback;
            Instance.ButtonFrameRight.IsEnabled = enabled && noPlayback;
            Instance.ButtonShowFieldHitbox.IsEnabled = enabled;
            Instance.ButtonShowCenter.IsEnabled = enabled;

            if (Instance.ViewModel.Zoom < 7) Instance.ButtonZoomIn.IsEnabled = enabled;
            else Instance.ButtonZoomIn.IsEnabled = false;
            if (Instance.ViewModel.Zoom > 1) Instance.ButtonZoomOut.IsEnabled = enabled;
            else Instance.ButtonZoomOut.IsEnabled = false;
        }
        public void UpdateAnimationValuesEnabledState()
        {
            bool isSelected = isEntrySelected;
            bool invalid = !(isEntrySelected && isAnimationLoaded);
            bool noPlayback = !GlobalService.PropertyHandler.isPlaybackEnabled;

            Instance.SpeedNUD.IsEnabled = isSelected && noPlayback;
            Instance.LoopIndexNUD.IsEnabled = isSelected && noPlayback;
            Instance.FlagsSelector.IsHitTestVisible = (invalid ? false : true);
        }
        public void UpdateAnimationControlsEnabledState()
        {
            bool isLoaded = isAnimationLoaded;
            bool isSelected = isEntrySelected;
            bool invalid = !(isEntrySelected && isAnimationLoaded);
            bool noPlayback = !GlobalService.PropertyHandler.isPlaybackEnabled;

            Instance.ButtonAnimationAdd.IsEnabled = isLoaded && noPlayback;
            Instance.ButtonAnimationRemove.IsEnabled = isSelected && noPlayback;
            Instance.ButtonAnimationExport.IsEnabled = isSelected && noPlayback;
            Instance.ButtonAnimationImport.IsEnabled = isLoaded && noPlayback;
            Instance.ButtonAnimationDuplicate.IsEnabled = isSelected && noPlayback;
            Instance.ButtonAnimationUp.IsEnabled = isSelected && noPlayback;
            Instance.ButtonAnimationDown.IsEnabled = isSelected && noPlayback;
            Instance.AnimationScroller.IsEnabled = isLoaded && noPlayback;
        }
        public void UpdateHitboxValuesEnabledState()
        {
            bool isValid = isFrameSelected;

            Instance.HitboxLeftNUD.IsEnabled = isValid;
            Instance.HitboxTopNUD.IsEnabled = isValid;
            Instance.HitboxRightNUD.IsEnabled = isValid;
            Instance.HitboxBottomNUD.IsEnabled = isValid;

            Instance.HitBoxComboBox.IsEnabled = isValid;
            Instance.HitBoxComboBox.IsHitTestVisible = (isValid ? false : true);
        }
        public void UpdateGeneralControlsEnabledState()
        {
            bool isLoaded = isAnimationLoaded;
            bool noPlayback = !GlobalService.PropertyHandler.isPlaybackEnabled;

            Instance.ButtonPlay.IsEnabled = isFrameSelected;
            Instance.PlaybackOptionsButton.IsEnabled = isFrameSelected;
            Instance.HitboxButton.IsEnabled = isLoaded;
            Instance.TextureButton.IsEnabled = isLoaded;

            Instance.ControlPanel.IsEnabled = noPlayback;
            Instance.List.IsEnabled = noPlayback;
            Instance.FramesList.IsEnabled = noPlayback;

            Instance.MenuStrip.IsEnabled = noPlayback;

            Instance.MenuFileSave.IsEnabled = isLoaded;
            Instance.MenuFileSaveAs.IsEnabled = isLoaded;
            Instance.MenuFileUnloadAnimation.IsEnabled = isLoaded;
        }

        #endregion

        #region Update Value Limits

        public void UpdateCurrentFrameValueLimits()
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

        #endregion

        #region Update Visuals

        public void UpdateFrameValuesVisual()
        {
            bool invalid = !(isFrameSelected && isAnimationLoaded);

            Instance.FrameWidthNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.FrameHeightNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.FrameX_NUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.FrameY_NUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.PivotX_NUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.PivotY_NUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.FrameID_NUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.Delay_NUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.FrameHitboxID_NUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);

            Instance.SpriteSheetList.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.SpriteSheetList.Foreground = (invalid ? HideTextBrush : DefaultTextBrush);
        }
        public void UpdateAnimationValuesVisual()
        {
            bool invalid = !(isEntrySelected && isAnimationLoaded);

            Instance.SpeedNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.LoopIndexNUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.PlayerID_NUD.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);

            Instance.FlagsSelector.BorderBrush = (invalid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.FlagsSelector.Foreground = (invalid ? HideTextBrush : DefaultTextBrush);
        }
        public void UpdateHitboxValuesVisual()
        {
            bool isValid = isFrameSelected;

            Instance.HitboxLeftNUD.BorderBrush = (isValid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.HitboxTopNUD.BorderBrush = (isValid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.HitboxRightNUD.BorderBrush = (isValid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.HitboxBottomNUD.BorderBrush = (isValid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.HitBoxComboBox.BorderBrush = (isValid ? System.Windows.Media.Brushes.Red : DefaultBorderBrush);
            Instance.HitBoxComboBox.Foreground = (isValid ? HideTextBrush : DefaultTextBrush);
        }
        public void UpdateGeneralControlsVisual()
        {
            bool isLoaded = isAnimationLoaded;

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

            if (isLoaded && isFrameSelected) Instance.CanvasView.Visibility = Visibility.Visible;
            else Instance.CanvasView.Visibility = Visibility.Collapsed;

            Instance.SpriteDirectoryLabel.Text = string.Format("Sprite Directory: {0}", (Instance.ViewModel.SpriteDirectory != "" && Instance.ViewModel.SpriteDirectory != null ? Instance.ViewModel.SpriteDirectory : "N/A"));
            Instance.AnimationPathLabel.Text = string.Format("Animation Path: {0}", (Instance.ViewModel.AnimationFilepath != "" && Instance.ViewModel.AnimationFilepath != null ? Instance.ViewModel.AnimationFilepath : "N/A"));
        }
        public void UpdateIndexStatusVisual()
        {
            Instance.SelectedAnimationIndexLabel.Text = Instance.ViewModel.SelectedAnimationIndex.ToString();
            Instance.SelectedFrameIndexLabel.Text = Instance.ViewModel.SelectedFrameIndex.ToString();
            Instance.FramesCountLabel.Text = Instance.ViewModel.FramesCount.ToString();
            Instance.AnimationsCountLabel.Text = Instance.ViewModel.AnimationsCount.ToString();
            Instance.SelectedCombinedFrameIndexLabel.Text = Instance.ViewModel.GetCurrentFrameIndexForAllAnimations().ToString();
            Instance.AllFramesCountLabel.Text = Instance.ViewModel.GetTotalFrameCount().ToString();
        }

        #endregion

        #region Update Type Limitations

        public void UpdateTypeLimitationsSections()
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


        }
        private void SetFrameIDVisibilityState(bool isEnabled)
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
        private void SetFlagsVisibilityState(bool isEnabled)
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
        private void SetHitboxIDVisibilityState(bool isEnabled)
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
        private void SetPlayerIDVisibilityState(bool isEnabled)
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
        private void SetAnimationRenameVisibilityState(bool isEnabled)
        {
            if (isEnabled)
            {

                Instance.ButtonAnimationRename.IsEnabled = isAnimationLoaded && isEntrySelected && !GlobalService.PropertyHandler.isPlaybackEnabled;
                Instance.ButtonAnimationRename.Visibility = Visibility.Visible;
            }
            else
            {
                Instance.ButtonAnimationRename.IsEnabled = false;
                Instance.ButtonAnimationRename.Visibility = Visibility.Collapsed;
            }
        }
        private void SetDelayNUDVisibilityState(bool isEnabled)
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
        private void SetUnknownNUDVisibilityState(bool isEnabled)
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
        private void SetDreamcastVersionVisibilityState(bool isEnabled)
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

        #region Playback Triggers

        public void TogglePlayback(bool enabled)
        {
            if (GlobalService.PlaybackService == null) IntilizePlayback();

            if (enabled)
            {
                GlobalService.PropertyHandler.isPlaybackEnabled = true;
                GlobalService.PlaybackService.AnimationData = Instance.ViewModel.LoadedAnimationFile;
                GlobalService.PlaybackService.Animation = Instance.ViewModel.SelectedAnimation.AnimName;
                GlobalService.PlaybackService.IsRunning = true;
                GlobalService.PropertyHandler.UpdateControls();
            }
            else
            {
                GlobalService.PropertyHandler.isPlaybackEnabled = false;
                GlobalService.PlaybackService.IsRunning = false;
                GlobalService.PropertyHandler.UpdateControls();
            }
        }

        public void IntilizePlayback(bool kill = false)
        {
            if (kill)
            {
                GlobalService.PlaybackService = null;
            }
            else
            {
                GlobalService.PlaybackService = new Services.PlaybackService(Instance.ViewModel.LoadedAnimationFile, Instance);
                GlobalService.PlaybackService.OnFrameChanged += PlaybackService_OnFrameChanged;
            }
        }

        private void PlaybackService_OnFrameChanged(Services.PlaybackService obj)
        {
            Application.Current.Dispatcher.Invoke(new Action(() => UpdatePlaybackIndex(obj.FrameIndex)));
        }

        private void UpdatePlaybackIndex(int frameIndex)
        {
            Instance.ViewModel.SelectedFrameIndex = frameIndex;
            GlobalService.PropertyHandler.UpdateCanvasVisual();
        }

        #endregion
    }
}
