using AnimationEditor.ViewModel;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xceed.Wpf.Toolkit;
using AnimationEditor.Services;
using AnimationEditor.Classes;
using AnimationEditor.Methods;
using AnimationEditor.Pages;
using AnimationEditor.Extensions;

namespace AnimationEditor.Pages
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Definitions
        public AnimationModel ViewModel => (AnimationModel)this.DataContext;
        public EngineType AnimationType { get => ViewModel.AnimationType; set => ViewModel.AnimationType = value; }
        private bool PreventScrollChange { get; set; } = true;
        public bool isLoadedFully { get; set; } = false;
        public string WindowName { set { this.Title = value; } }
        public string DefaultWindowName { get { return $"RSDK Animation Editor v{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}"; } }

        #endregion

        #region Init
        public MainWindow()
        {
            InitializeComponent();
            InitializeNonDesignTimeComponents();
            InitializeBaseComponents();
            //AutomateMode();
        }

        private void AutomateMode()
        {
            this.Loaded += ProceedToAutomate;
        }

        /*
         * Tested Save/Load 1:1
         * RSDKv5
         * RSDKvB
         */

        private void ProceedToAutomate(object sender, RoutedEventArgs e)
        {
            string inputPath = @"D:\Users\CarJem\Documents\Mania Modding\retrun-1.51\retrun-1.51\Data1\Animations\Sonic.ani";
            string outputPath = @"D:\Users\CarJem\Documents\Mania Modding\retrun-1.51\retrun-1.51\Data1\Animations\Sonic.ani";
            GlobalService.FileHandler.LoadFile(inputPath, EngineType.RSDKvB);
            GlobalService.FileHandler.SaveFileAs(outputPath, EngineType.RSDKvB);
            this.DialogResult = true;
        }

        private void InitializeNonDesignTimeComponents()
        {
            if (App.OpenedExternally)
            {
                ThemeSelector.Visibility = Visibility.Collapsed;
            }
        }
        private void InitializeBaseComponents()
        {
            GlobalService.InputControl = new InputController(this);
            GlobalService.FileHandler = new FileService(this);
            GlobalService.SpriteService = new SpriteService();
            GlobalService.PropertyHandler = new PropertyService(this);
            GlobalService.UIService = new UIService(this);

            DataContext = new AnimationModel();
            List.AllowDrop = true;


            PreventScrollChange = false;
            InitializeColorPickerEvents();
            WindowName = DefaultWindowName;
        }
        #endregion

        #region Menu Items

        private void MenuFileOpen_Click(object sender, RoutedEventArgs e)
        {
            GlobalService.FileHandler.OpenFile();
            GlobalService.PropertyHandler.UpdateControls();
        }

        private void MenuFileSave_Click(object sender, RoutedEventArgs e)
        {
            GlobalService.FileHandler.SaveFile();
            GlobalService.PropertyHandler.UpdateControls();
        }

        private void MenuFileSaveAs_Click(object sender, RoutedEventArgs e)
        {
            GlobalService.FileHandler.SaveFileAs();
            GlobalService.PropertyHandler.UpdateControls();
        }

        private void MenuFileExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuFileOpenFromWorkspaceAddWorkspace_Click(object sender, RoutedEventArgs e)
        {
            GlobalService.FileHandler.SelectNewWorkspaceFolder();
        }

        private void MenuFileOpenFromWorkspaceRemoveMode_Click(object sender, RoutedEventArgs e)
        {
            GlobalService.FileHandler.ToggleWorkspaceRemoveMode(MenuFileOpenFromWorkspaceRemoveMode.IsChecked);
        }

        private void MenuViewTexture_Click(object sender, RoutedEventArgs e)
        {
            TextureManagerMenu.Startup(this);
            TextureManagerPopup.IsOpen = true;
            GlobalService.PropertyHandler.UpdateControls();

        }

        private void MenuViewHitbox_Click(object sender, RoutedEventArgs e)
        {
            HitboxManagerMenu.Startup(this);
            HitboxManagerPopup.IsOpen = true;
            GlobalService.PropertyHandler.UpdateControls();
        }

        private void MenuInfoAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow();
            about.Owner = App.Current.MainWindow;
            about.ShowDialog();
        }

        private void MenuRecentFile_Click(object sender, RoutedEventArgs e)
        {
            GlobalService.FileHandler.RecentDataDirectoryClicked(sender, e);
            GlobalService.PropertyHandler.UpdateControls();
        }

        private void MenuFileOpenRecently_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            GlobalService.FileHandler.RefreshDataDirectories();
            GlobalService.PropertyHandler.UpdateControls();
        }

        private void MenuViewTransparentSpriteSheets_Click(object sender, RoutedEventArgs e)
        {
            GlobalService.PropertyHandler.ShowSolidImageBackground = MenuViewTransparentSpriteSheets.IsChecked;
GlobalService.PropertyHandler.UpdateCanvasVisual();
        }

        private void MenuFileUnloadAnimation_Click(object sender, RoutedEventArgs e)
        {
            GlobalService.FileHandler.UnloadAnimationData();
            GlobalService.PropertyHandler.UpdateControls();
            GlobalService.PropertyHandler.UnloadControls();
        }

        private void MenuViewFullSpriteSheets_Click(object sender, RoutedEventArgs e)
        {
            GlobalService.PropertyHandler.ShowFullFrame = MenuViewFullSpriteSheets.IsChecked;
GlobalService.PropertyHandler.UpdateCanvasVisual();
        }

        private void MenuViewFrameBorder_Click(object sender, RoutedEventArgs e)
        {
            GlobalService.PropertyHandler.ShowFrameBorder = MenuViewFrameBorder.IsChecked;
GlobalService.PropertyHandler.UpdateCanvasVisual();
        }

        private void MenuViewSetBackgroundToTransparentColor_Click(object sender, RoutedEventArgs e)
        {
            GlobalService.PropertyHandler.SetBackgroundColorToMatchSpriteSheet = MenuViewSetBackgroundToTransparentColor.IsChecked;
GlobalService.PropertyHandler.UpdateCanvasVisual();
        }

        private void MenuFileOpenFromWorkspace_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            GlobalService.FileHandler.RefreshWorkspaces();
            GlobalService.PropertyHandler.UpdateControls();
        }

        #endregion

        #region Animation Info Items

        private void ButtonAnimationAdd_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddAnimation(ViewModel.SelectedAnimationIndex);
            GlobalService.PropertyHandler.UpdateControls();
        }

        private void ButtonAnimationUp_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ShiftAnimationUp(ViewModel.SelectedAnimationIndex);
            GlobalService.PropertyHandler.UpdateControls();
        }

        private void ButtonAnimationDown_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ShiftAnimationDown(ViewModel.SelectedAnimationIndex);
            GlobalService.PropertyHandler.UpdateControls();
        }

        private void ButtonAnimationDuplicate_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.DuplicateAnimation(ViewModel.SelectedAnimationIndex);
            GlobalService.PropertyHandler.UpdateControls();
        }

        private void ButtonAnimationRemove_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RemoveAnimation(ViewModel.SelectedAnimationIndex);
            GlobalService.PropertyHandler.UpdateControls();
        }

        private void ButtonAnimationImport_Click(object sender, RoutedEventArgs e)
        {
            GlobalService.FileHandler.ImportAnimation();
            GlobalService.PropertyHandler.UpdateControls();
        }

        private void ButtonAnimationImport_Context(object sender, RoutedEventArgs e)
        {
            ButtonAnimationImport.ContextMenu.IsOpen = true;
        }

        private void ButtonAnimationExport_Context(object sender, RoutedEventArgs e)
        {
            ButtonAnimationExport.ContextMenu.IsOpen = true;
        }

        private void ButtonAnimationExport_Click(object sender, RoutedEventArgs e)
        {
            GlobalService.FileHandler.ExportAnimation();
            GlobalService.PropertyHandler.UpdateControls();
        }

        private void ButtonAnimationRename_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedAnimation != null)
            {
                int tempIndex = ViewModel.SelectedAnimationIndex;
                ViewModel.SelectedAnimationEntries[ViewModel.SelectedAnimationIndex].AnimName = GenerationsLib.WPF.TextPrompt2.ShowDialog("Change Name", "Enter a New Name for the Animation:", ViewModel.SelectedAnimation.AnimName);
                ViewModel.CallPropertyChanged(nameof(ViewModel.SelectedAnimationEntries));
                GlobalService.PropertyHandler.UpdateControls();
                ViewModel.SelectedAnimationIndex = tempIndex;
            }
        }

        #endregion

        #region Frame Items

        private void ButtonFrameAdd_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddFrame((ViewModel.SelectedFrameIndex != -1 ? ViewModel.SelectedFrameIndex : 0));
            GlobalService.PropertyHandler.UpdateControls();
            GlobalService.PropertyHandler.InvalidateSelectionProperties();
        }

        private void ButtonFrameDupe_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.DuplicateFrame(ViewModel.SelectedFrameIndex);
            GlobalService.PropertyHandler.UpdateControls();
            GlobalService.PropertyHandler.InvalidateSelectionProperties();
        }

        private void ButtonFrameLeft_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ShiftFrameLeft(ViewModel.SelectedFrameIndex);
            GlobalService.PropertyHandler.UpdateControls();
            GlobalService.PropertyHandler.InvalidateSelectionProperties();
        }

        private void ButtonFrameRight_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ShiftFrameRight(ViewModel.SelectedFrameIndex);
            GlobalService.PropertyHandler.UpdateControls();
            GlobalService.PropertyHandler.InvalidateSelectionProperties();
        }

        private void ButtonFrameRemove_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RemoveFrame(ViewModel.SelectedFrameIndex);
            GlobalService.PropertyHandler.UpdateControls();
            GlobalService.PropertyHandler.InvalidateSelectionProperties();
        }

        private void ButtonFrameImport_Context(object sender, RoutedEventArgs e)
        {
            ButtonFrameImport.ContextMenu.IsOpen = true;
        }

        private void ButtonFrameExport_Context(object sender, RoutedEventArgs e)
        {
            ButtonFrameExport.ContextMenu.IsOpen = true;
        }

        private void ButtonFrameImport_Click(object sender, RoutedEventArgs e)
        {
            GlobalService.FileHandler.ImportFrame();
            GlobalService.PropertyHandler.UpdateControls();
            GlobalService.PropertyHandler.InvalidateSelectionProperties();
        }

        private void ButtonFrameExport_Click(object sender, RoutedEventArgs e)
        {
            GlobalService.FileHandler.ExportFrame();
            GlobalService.PropertyHandler.UpdateControls();
            GlobalService.PropertyHandler.InvalidateSelectionProperties();
        }

        private void ButtonZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Zoom < 8)
            {
                ViewModel.Zoom = ViewModel.Zoom + 1;
                GlobalService.PropertyHandler.UpdateControls();
            }
        }

        private void ButtonZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Zoom > 1)
            {
                ViewModel.Zoom = ViewModel.Zoom - 1;
                GlobalService.PropertyHandler.UpdateControls();
            }

        }



        #endregion

        #region Frame Viewer Controls

        private void AnimationScroller_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!PreventScrollChange)
            {
                PreventScrollChange = true;
                if (AnimationScroller.Value == 3) GlobalService.PropertyHandler.MoveToAdjacentFrameIndex(false);
                if (AnimationScroller.Value == 1) GlobalService.PropertyHandler.MoveToAdjacentFrameIndex(true);
                AnimationScroller.Value = 2;
                PreventScrollChange = false;
            }

        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonShowCenter_Click(object sender, RoutedEventArgs e)
        {
            GlobalService.PropertyHandler.ShowAlignmentLines = ButtonShowCenter.IsChecked.Value;
            GlobalService.PropertyHandler.UpdateControls();
        }

        private void ButtonPlay_CheckChanged(object sender, RoutedEventArgs e)
        {
            if (ViewModel.LoadedAnimationFile != null && ViewModel.SelectedAnimation != null)
            {
                bool enabled = ButtonPlay.IsChecked.Value && List.SelectedItem != null;
                GlobalService.UIService.TogglePlayback(enabled);
            }
        }

        private void PlaybackOptionsButton_Click(object sender, RoutedEventArgs e)
        {
            PlaybackOptionsContextMenu.IsOpen = true;
        }

        private void ForcePlaybackMenuItem_Click(object sender, RoutedEventArgs e)
        {
            GlobalService.PropertyHandler.isForcePlaybackOn = ForcePlaybackMenuItem.IsChecked;
        }

        private void ForcedPlaybackSpeedNUD_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (GlobalService.PropertyHandler != null) GlobalService.PropertyHandler.ForcePlaybackSpeed = ForcedPlaybackSpeedNUD.Value.Value;
        }

        private void ForcedPlaybackDurationNUD_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (GlobalService.PropertyHandler != null) GlobalService.PropertyHandler.ForcePlaybackDuration = ForcedPlaybackDurationNUD.Value.Value;
        }

        private void ExportAnimationImages_Click(object sender, RoutedEventArgs e)
        {
            GlobalService.FileHandler.ExportAnimationFramesToImages();
            GlobalService.PropertyHandler.UpdateControls();
        }

        private void ButtonShowFieldHitbox_Click(object sender, RoutedEventArgs e)
        {
            GlobalService.PropertyHandler.ShowHitBox = ButtonShowFieldHitbox.IsChecked.Value;
            GlobalService.PropertyHandler.UpdateControls();
        }

        private void ButtonHelp_Click(object sender, RoutedEventArgs e)
        {
            ButtonHelp.ContextMenu.IsOpen = true;
        }

        #endregion

        #region Canvas Controls

        public void InvalidateCanvasSize()
        {
            ViewModel.ViewWidth = 0;
            ViewModel.ViewHeight = 0;
            ViewModel.ViewWidth = CanvasView.ActualWidth;
            ViewModel.ViewHeight = CanvasView.ActualHeight;
            GlobalService.PropertyHandler.UpdateCanvasVisual();
            GlobalService.PropertyHandler.UpdateControls();
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ViewModel.ViewWidth = e.NewSize.Width;
            ViewModel.ViewHeight = e.NewSize.Height;
            GlobalService.PropertyHandler.UpdateCanvasVisual();
            GlobalService.PropertyHandler.UpdateControls();
        }

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            ViewModel.Zoom += (e.Delta / 120) * 0.25;
            GlobalService.PropertyHandler.UpdateCanvasVisual();
            GlobalService.PropertyHandler.UpdateControls();
        }

        #endregion

        #region List Controls

        public void List_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            GlobalService.PropertyHandler.UpdateCanvasVisual();
            GlobalService.PropertyHandler.InvalidateSelectionProperties();
            GlobalService.PropertyHandler.UpdateControls();
        }
        public void FramesList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            GlobalService.PropertyHandler.UpdateCanvasVisual();
            if (!GlobalService.PlaybackService?.IsRunning ?? true) GlobalService.PropertyHandler.UpdateControls();
        }
        private void FramesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            GlobalService.PropertyHandler.UpdateControls();
        }
        private void FramesList_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            GlobalService.PropertyHandler.UpdateControls();
        }

        #endregion

        #region Frame Property Controls
        public void NUD_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (isLoadedFully)
            {
                GlobalService.PropertyHandler.UpdateCanvasVisual();
                GlobalService.PropertyHandler.InvalidateSprite();
                GlobalService.PropertyHandler.UpdateControls();
                GlobalService.UIService.UpdateCurrentFrameValueLimits();
            }
        }
        public void SpriteSheetList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            GlobalService.PropertyHandler.InvalidateSprite();
            GlobalService.PropertyHandler.UpdateControls();
        }
        public void HitBoxComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            GlobalService.PropertyHandler.UpdateControls();
        }
        private void HitBoxComboBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            GlobalService.PropertyHandler.UpdateControls();
        }
        #endregion

        #region Animation Info Controls
        public void SpeedNUD_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (isLoadedFully)
            {
                GlobalService.PropertyHandler.UpdateControls();
            }
        }

        public void LoopIndexNUD_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (isLoadedFully)
            {
                GlobalService.PropertyHandler.UpdateControls();
            }
        }

        public void PlayerID_NUD_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (isLoadedFully)
            {
                GlobalService.PropertyHandler.UpdateControls();
            }
        }

        public void FlagsSelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (isLoadedFully)
            {
                GlobalService.PropertyHandler.UpdateControls();
            }
        }

        public void Unknown_NUD_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (isLoadedFully)
            {
                GlobalService.PropertyHandler.UpdateControls();
            }
        }

        public void DreamcastVer_Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            if (isLoadedFully)
            {
                GlobalService.PropertyHandler.UpdateControls();
            }
        }

        #endregion

        #region Spritesheet/Hitbox Popup Controls

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
            //GlobalService.Interfacer.UpdateUI();
        }

        private void TextureManagerPopup_LostFocus(object sender, RoutedEventArgs e)
        {
            //TextureManagerPopup.IsOpen = false;
            //GlobalService.Interfacer.UpdateUI();
        }

        #endregion

        #region Color Picker Controls

        private void InitializeColorPickerEvents()
        {
            HitboxColorPicker.SelectedColorChanged += ColorPicker_SelectedColorChanged;
            AxisColorPicker.SelectedColorChanged += ColorPicker_SelectedColorChanged;
            BGColorPicker.SelectedColorChanged += ColorPicker_SelectedColorChanged;
            FrameBorderColorPicker.SelectedColorChanged += ColorPicker_SelectedColorChanged;
            FrameBackgroundColorPicker.SelectedColorChanged += ColorPicker_SelectedColorChanged;
        }

        private void ColorLabels_Click(object sender, RoutedEventArgs e)
        {
            if (sender == ButtonBGColorReset) BGColorPicker.SelectedColor = (Color)ColorConverter.ConvertFromString("#303030");
            else if (sender == ButtonAxisColorReset) AxisColorPicker.SelectedColor = (Color)ColorConverter.ConvertFromString("#FFFF0000");
            else if (sender == ButtonHitboxColorReset) HitboxColorPicker.SelectedColor = (Color)ColorConverter.ConvertFromString("#FFE700FF");
            else if (sender == ButtonFrameBorderColorReset) FrameBorderColorPicker.SelectedColor = (Color)ColorConverter.ConvertFromString("Black");
            else if (sender == ButtonFrameBGColorReset) FrameBackgroundColorPicker.SelectedColor = (Color)ColorConverter.ConvertFromString("Transparent");
        }

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (HitboxColorPicker.SelectedColor != null)
            {
                GlobalService.PropertyHandler.HitboxBackground = HitboxColorPicker.SelectedColor.Value;
            }
            if (AxisColorPicker.SelectedColor != null)
            {
                GlobalService.PropertyHandler.AlignmentLinesColor = AxisColorPicker.SelectedColor.Value;
            }
            if (BGColorPicker.SelectedColor != null)
            {
                GlobalService.PropertyHandler.CanvasBackground = BGColorPicker.SelectedColor.Value;
            }
            if (FrameBorderColorPicker.SelectedColor != null)
            {
                GlobalService.PropertyHandler.FrameBorder = FrameBorderColorPicker.SelectedColor.Value;
            }
            if (FrameBackgroundColorPicker.SelectedColor != null)
            {
                GlobalService.PropertyHandler.FrameBackground = FrameBackgroundColorPicker.SelectedColor.Value;
            }
GlobalService.PropertyHandler.UpdateCanvasVisual();
        }
        #endregion

        #region Window Controls
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            isLoadedFully = true;
            ThemeSelector.SelectedSkin = Settings.Default.CurrentTheme;
        }
        #endregion

        #region Canvas Controls

        private void CanvasView_MouseMove(object sender, MouseEventArgs e)
        {
            if (!GlobalService.PropertyHandler.isPlaybackEnabled) GlobalService.InputControl.MouseMove(sender, e);
        }

        private void CanvasView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!GlobalService.PropertyHandler.isPlaybackEnabled) GlobalService.InputControl.MouseDown(sender, e);
        }

        private void CanvasView_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!GlobalService.PropertyHandler.isPlaybackEnabled) GlobalService.InputControl.MouseUp(sender, e);
        }

        private void CanvasView_KeyDown(object sender, KeyEventArgs e)
        {
            if (!GlobalService.PropertyHandler.isPlaybackEnabled) GlobalService.InputControl.KeyDown(sender, e);
        }

        private void CanvasView_KeyUp(object sender, KeyEventArgs e)
        {
            KeyboardNavigation.SetDirectionalNavigation(this, KeyboardNavigationMode.Continue);
        }

        private void CanvasView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            KeyboardNavigation.SetControlTabNavigation(this, KeyboardNavigationMode.None);
            CanvasView.Focusable = true;
            CanvasView.Focus();
            Keyboard.Focus(CanvasView);
        }

        #endregion

        #region Unsorted

        private void ThemeSelector_ThemeChanged(object sender, GenerationsLib.WPF.Themes.Skin e)
        {
            App.ChangeSkin(e);
            Classes.Settings.Default.CurrentTheme = e;
            Classes.Settings.Save();

            this.Refresh();
            GlobalService.PropertyHandler.RefreshUIThemes();
            ThemeSelector.UpdateVisual();
        }

        #endregion
    }


}
