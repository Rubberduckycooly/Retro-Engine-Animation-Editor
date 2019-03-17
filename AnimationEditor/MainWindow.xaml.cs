using AnimationEditor.ViewModels;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xceed.Wpf.Toolkit;
using AnimationEditor.Services;

namespace AnimationEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel ViewModel => (MainViewModel)DataContext;

        public UserInterfacer Interfacer;
        public FileHandler Handler;
        public Brush DefaultBorderBrush;
        public Brush DefaultTextBrush;
        public EngineType AnimationType = EngineType.RSDKv5;
        public static int AnimationIndex { get; set; }

        private PlaybackService PlaybackService;

        public bool isPlaybackEnabled = false;
        private bool PreventScrollChange = true;
        public static bool isForcePlaybackOn = false;
        public static int ForcePlaybackDuration = 256;
        public static int ForcePlaybackSpeed = 128;

        public MainWindow()
        {
            DefaultBorderBrush = (Brush)FindResource("ComboBoxBorder");
            DefaultTextBrush = (Brush)FindResource("NormalText");
            InitializeComponent();
            DataContext = new MainViewModel();
            List.AllowDrop = true;
            Interfacer = new UserInterfacer(this);
            Handler = new FileHandler(this);
            PreventScrollChange = false;
            HitboxColorPicker.SelectedColorChanged += ColorPicker_SelectedColorChanged;
        }

        private void PlaybackService_OnFrameChanged(PlaybackService obj)
        {
            Application.Current.Dispatcher.Invoke(new Action(() => UpdatePlaybackIndex(obj.FrameIndex)));

        }

        private void UpdatePlaybackIndex(int frameIndex)
        {
            FramesList.SelectedIndex = frameIndex;
            Interfacer.UpdateImage();
        }

        private void MenuFileOpen_Click(object sender, RoutedEventArgs e)
        {
            Handler.OpenFile();
            Interfacer.UpdateUI();
        }

        private void MenuFileSave_Click(object sender, RoutedEventArgs e)
        {
            Handler.SaveFile();
            Interfacer.UpdateUI();
        }

        private void MenuFileSaveAs_Click(object sender, RoutedEventArgs e)
        {
            Handler.SaveFileAs();
            Interfacer.UpdateUI();
        }

        private void MenuFileExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonAnimationAdd_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddAnimation(ViewModel.SelectedAnimationIndex);
            Interfacer.FullUpdateList();
            Interfacer.UpdateUI();
        }

        private void ButtonAnimationUp_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ShiftAnimationUp(ViewModel.SelectedAnimationIndex);
            Interfacer.FullUpdateList();
            Interfacer.UpdateUI();
        }

        private void ButtonAnimationDown_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ShiftAnimationDown(ViewModel.SelectedAnimationIndex);
            Interfacer.FullUpdateList();
            Interfacer.UpdateUI();
        }

        private void ButtonFrameLeft_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ShiftFrameLeft(ViewModel.SelectedFrameIndex);
            Interfacer.UpdateFramesList();
            Interfacer.UpdateUI(true);
        }

        private void ButtonFrameRight_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ShiftFrameRight(ViewModel.SelectedFrameIndex);
            Interfacer.UpdateFramesList();
            Interfacer.UpdateUI(true);
        }

        private void ButtonAnimationDuplicate_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.DuplicateAnimation(ViewModel.SelectedAnimationIndex);
            Interfacer.FullUpdateList();
            Interfacer.UpdateUI();
        }

        private void ButtonAnimationRemove_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RemoveAnimation(ViewModel.SelectedAnimationIndex);
            Interfacer.FullUpdateList();
            Interfacer.UpdateUI();
        }

        private void ButtonAnimationImport_Click(object sender, RoutedEventArgs e)
        {
            ButtonAnimationImport.ContextMenu.IsOpen = true;
            //Handler.ImportAnimation();
            //Interfacer.UpdateUI();
        }

        private void ButtonAnimationExport_Click(object sender, RoutedEventArgs e)
        {
            ButtonAnimationExport.ContextMenu.IsOpen = true;
            //Handler.ExportAnimation();
            //Interfacer.UpdateUI();
        }

        private void ButtonFrameAdd_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddFrame((ViewModel.SelectedFrameIndex != -1 ? ViewModel.SelectedFrameIndex : 0));
            Interfacer.UpdateFramesList();
            Interfacer.UpdateUI(true);
        }

        private void ButtonFrameDupe_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.DuplicateFrame(ViewModel.SelectedFrameIndex);
            Interfacer.UpdateFramesList();
            Interfacer.UpdateUI(true);
        }

        private void ButtonFrameRemove_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RemoveFrame(ViewModel.SelectedFrameIndex);
            Interfacer.UpdateFramesList();
            Interfacer.UpdateUI(true);
        }

        private void ButtonFrameImport_Click(object sender, RoutedEventArgs e)
        {
            ButtonFrameImport.ContextMenu.IsOpen = true;
            //Handler.ImportFrame();
            //Interfacer.UpdateUI();
        }

        private void ButtonFrameExport_Click(object sender, RoutedEventArgs e)
        {
            ButtonFrameExport.ContextMenu.IsOpen = true;
            //Handler.ExportFrame();
            //Interfacer.UpdateUI();
        }

        private void ButtonZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SpriteScaleX < 8)
            {
                ViewModel.SpriteScaleX = ViewModel.SpriteScaleX + 1;
                Interfacer.UpdateUI();
            }
        }

        private void ButtonZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SpriteScaleX > 1)
            {
                ViewModel.SpriteScaleX = ViewModel.SpriteScaleX - 1;
                Interfacer.UpdateUI();
            }

        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Interfacer.UpdateUI();
        }

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 1)
            {
                if (ViewModel.SpriteScaleX < 8)
                {
                    ViewModel.SpriteScaleX = ViewModel.SpriteScaleX + 1;
                }
            }
            else
            {
                if (ViewModel.SpriteScaleX > 1)
                {
                    ViewModel.SpriteScaleX = ViewModel.SpriteScaleX - 1;
                }
            }
            Interfacer.UpdateUI();
        }

        private void List_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ViewModel.SelectedAnimation != null)
            {
                ViewModel.Animations[ViewModel.SelectedAnimationIndex].AnimName = RSDKrU.TextPrompt2.ShowDialog("Change Name", "Enter a New Name for the Annimation:", ViewModel.SelectedAnimation.AnimName);
                List.ItemsSource = null;
                Interfacer.UpdateUI();
            }

        }

        private void MenuViewTexture_Click(object sender, RoutedEventArgs e)
        {
            TextureManagerMenu.Startup(this);
            TextureManagerPopup.IsOpen = true;
            Interfacer.UpdateUI();

        }

        private void MenuViewHitbox_Click(object sender, RoutedEventArgs e)
        {
            HitboxManagerMenu.Startup(this);
            HitboxManagerPopup.IsOpen = true;
            Interfacer.UpdateUI();
        }

        private void MenuInfoAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow();
            about.Owner = App.Current.MainWindow;
            about.ShowDialog();
        }

        private void FramesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Interfacer.UpdateUI();
        }

        private void NUD_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Interfacer.UpdateUI(true);
        }

        private void List_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Interfacer.UpdateFramesList();
            Interfacer.UpdateUI();
        }

        private void FramesList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (!isPlaybackEnabled)
            Interfacer.UpdateUI();
        }

        private void SpriteSheetList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Interfacer.UpdateUI();
        }

        private void HitBoxComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Interfacer.UpdateUI();
        }

        private void FramesList_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            Interfacer.UpdateUI();
        }

        private void HitBoxComboBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Interfacer.UpdateUI();
        }

        private void MenuRecentFile_Click(object sender, RoutedEventArgs e)
        {
            Handler.RecentDataDirectoryClicked(sender, e);
            Interfacer.UpdateUI();

        }

        private void ContextMenu_ContextMenuClosing(object sender, System.Windows.Controls.ContextMenuEventArgs e)
        {
            TextureManagerMenu.Shutdown();
        }

        private void HitboxManagerMenu_DragOver(object sender, DragEventArgs e)
        {
            HitboxManagerMenu.Shutdown();
        }

        private void HitboxManagerPopup_LostFocus(object sender, RoutedEventArgs e)
        {
            //HitboxManagerPopup.IsOpen = false;
            //Interfacer.UpdateUI();
        }

        private void TextureManagerPopup_LostFocus(object sender, RoutedEventArgs e)
        {
            //TextureManagerPopup.IsOpen = false;
            //Interfacer.UpdateUI();
        }

        private void AnimationScroller_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!PreventScrollChange)
            {
                PreventScrollChange = true;
                if (AnimationScroller.Value == 3) Interfacer.UpdateFrameIndex(false);
                if (AnimationScroller.Value == 1) Interfacer.UpdateFrameIndex(true);
                AnimationScroller.Value = 2;
                PreventScrollChange = false;
            }

        }

        private void HitboxColorBox_Click(object sender, RoutedEventArgs e)
        {
            HitboxColorPicker.ColorMode = ColorMode.ColorCanvas;
            HitboxColorPicker.ShowStandardColors = false;
            HitboxColorPicker.IsOpen = true;
        }

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (HitboxColorPicker.SelectedColor != null)
            {
                HitBoxViewer.BorderBrush = new SolidColorBrush(HitboxColorPicker.SelectedColor.Value);
                HitBoxBackground.Background = new SolidColorBrush(HitboxColorPicker.SelectedColor.Value);
                HitboxColorBox.Background = new SolidColorBrush(HitboxColorPicker.SelectedColor.Value);
            }
        }

        private void MenuFileOpenRecently_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            Handler.RefreshDataDirectories(Properties.Settings.Default.RecentFiles);
            Interfacer.UpdateUI();
        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {

        }


        #region Playback Triggers

        public void TogglePlayback(bool enabled)
        {
            if (PlaybackService == null) IntilizePlayback();

            if (enabled)
            {
                isPlaybackEnabled = true;
                PlaybackService.AnimationData = ViewModel.LoadedAnimationFile;
                PlaybackService.Animation = ViewModel.SelectedAnimation.AnimName;
                PlaybackService.IsRunning = true;
            }
            else
            {
                isPlaybackEnabled = false;
                PlaybackService.IsRunning = false;
                Interfacer.UpdateUI();
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
                PlaybackService = new PlaybackService(ViewModel.LoadedAnimationFile, this);
                PlaybackService.OnFrameChanged += PlaybackService_OnFrameChanged;
            }

        }

        #endregion

        private void ButtonPlay_CheckChanged(object sender, RoutedEventArgs e)
        {
            if (ViewModel.LoadedAnimationFile != null && ViewModel.SelectedAnimation != null)
            {
                bool enabled = ButtonPlay.IsChecked.Value && List.SelectedItem != null;
                Interfacer.DisablePlaybackModeElements(!enabled);
                TogglePlayback(enabled);
            }
        }

        private void MenuViewTransparentSpriteSheets_Click(object sender, RoutedEventArgs e)
        {
            Interfacer.UpdateImage();
        }

        private void MenuFileUnloadAnimation_Click(object sender, RoutedEventArgs e)
        {
            Handler.UnloadAnimationData();
            FramesList.Items.Clear();
            Interfacer.UpdateUI();
        }

        private void PlaybackOptionsButton_Click(object sender, RoutedEventArgs e)
        {
            PlaybackOptionsContextMenu.IsOpen = true;
        }

        private void ForcePlaybackMenuItem_Click(object sender, RoutedEventArgs e)
        {
            isForcePlaybackOn = ForcePlaybackMenuItem.IsChecked;
        }

        private void ForcedPlaybackSpeedNUD_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ForcePlaybackSpeed = ForcedPlaybackSpeedNUD.Value.Value;
        }

        private void ForcedPlaybackDurationNUD_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ForcePlaybackDuration = ForcedPlaybackDurationNUD.Value.Value;
        }

        private void ExportAnimationImages_Click(object sender, RoutedEventArgs e)
        {
            Handler.ExportAnimationFramesToImages();
        }

        private void ButtonShowFieldHitbox_Click(object sender, RoutedEventArgs e)
        {
            Interfacer.UpdateUI();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MenuViewUseDarkTheme.IsChecked)
            {
                Properties.Settings.Default.UseDarkTheme = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.UseDarkTheme = false;
                Properties.Settings.Default.Save();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.UseDarkTheme)
            {
                MenuViewUseDarkTheme.IsChecked = true;
            }
        }
    }
}
