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

namespace AnimationEditor.Pages
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Definitions
        public AnimationModel ViewModel => (AnimationModel)this.DataContext;

        public UserInterfacer Interfacer;
        public InputController InputControl;
        public FileHandler Handler;
        public Brush DefaultBorderBrush;
        public Brush DefaultTextBrush;
        public EngineType AnimationType { get => ViewModel.AnimationType; set => ViewModel.AnimationType = value; }


        private bool PreventScrollChange { get; set; } = true;

        public bool isLoadedFully { get; set; } = false;
        public string WindowName { set { this.Title = value; } }
        public string DefaultWindowName { get { return $"RSDK Animation Editor v{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}"; } }

        #endregion

        #region Init
        public MainWindow()
        {
            InitializeDesignTimeComponents();
            InitializeComponent();
            InitializeBaseComponents();
        }
        private void InitializeDesignTimeComponents()
        {
            DefaultBorderBrush = (Brush)FindResource("ComboBoxBorder");
            DefaultTextBrush = (Brush)FindResource("NormalText");
        }
        private void InitializeBaseComponents()
        {
            InputControl = new InputController(this);
            DataContext = new AnimationModel();
            List.AllowDrop = true;
            Interfacer = new UserInterfacer(this);
            Handler = new FileHandler(this);
            PreventScrollChange = false;
            InitializeColorPickerEvents();
            WindowName = DefaultWindowName;
        }
        #endregion

        #region Menu Items

        private void MenuFileOpen_Click(object sender, RoutedEventArgs e)
        {
            Handler.OpenFile();
            Interfacer.UpdateControls();
        }

        private void MenuFileSave_Click(object sender, RoutedEventArgs e)
        {
            Handler.SaveFile();
            Interfacer.UpdateControls();
        }

        private void MenuFileSaveAs_Click(object sender, RoutedEventArgs e)
        {
            Handler.SaveFileAs();
            Interfacer.UpdateControls();
        }

        private void MenuFileExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuViewTexture_Click(object sender, RoutedEventArgs e)
        {
            TextureManagerMenu.Startup(this);
            TextureManagerPopup.IsOpen = true;
            Interfacer.UpdateControls();

        }

        private void MenuViewHitbox_Click(object sender, RoutedEventArgs e)
        {
            HitboxManagerMenu.Startup(this);
            HitboxManagerPopup.IsOpen = true;
            Interfacer.UpdateControls();
        }

        private void MenuInfoAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow();
            about.Owner = App.Current.MainWindow;
            about.ShowDialog();
        }

        private void MenuRecentFile_Click(object sender, RoutedEventArgs e)
        {
            Handler.RecentDataDirectoryClicked(sender, e);
            Interfacer.UpdateControls();
        }

        private void MenuFileOpenRecently_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            Handler.RefreshDataDirectories();
            Interfacer.UpdateControls();
        }

        private void MenuViewTransparentSpriteSheets_Click(object sender, RoutedEventArgs e)
        {
            Interfacer.ShowSolidImageBackground = MenuViewTransparentSpriteSheets.IsChecked;
            Interfacer.UpdateCanvasVisual();
        }

        private void MenuFileUnloadAnimation_Click(object sender, RoutedEventArgs e)
        {
            Handler.UnloadAnimationData();
            Interfacer.UpdateControls();
            Interfacer.UnloadControls();
        }

        private void MenuViewFullSpriteSheets_Click(object sender, RoutedEventArgs e)
        {
            Interfacer.ShowFullFrame = MenuViewFullSpriteSheets.IsChecked;
            Interfacer.UpdateCanvasVisual();
        }

        private void MenuViewFrameBorder_Click(object sender, RoutedEventArgs e)
        {
            Interfacer.ShowFrameBorder = MenuViewFrameBorder.IsChecked;
            Interfacer.UpdateCanvasVisual();
        }

        private void MenuViewSetBackgroundToTransparentColor_Click(object sender, RoutedEventArgs e)
        {
            Interfacer.SetBackgroundColorToMatchSpriteSheet = MenuViewSetBackgroundToTransparentColor.IsChecked;
            Interfacer.UpdateCanvasVisual();
        }

        private void MenuViewUseDarkTheme_Checked(object sender, RoutedEventArgs e)
        {
            App.ChangeSkin(Skin.Dark);
            this.Refresh();
            Interfacer.RefreshUIThemes();
        }

        private void MenuViewUseDarkTheme_Unchecked(object sender, RoutedEventArgs e)
        {
            App.ChangeSkin(Skin.Light);
            this.Refresh();
            Interfacer.RefreshUIThemes();
        }

        private void MenuFileOpenFromWorkspace_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            Handler.RefreshWorkspaces();
            Interfacer.UpdateControls();
        }

        #endregion

        #region Animation Info Items

        private void ButtonAnimationAdd_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddAnimation(ViewModel.SelectedAnimationIndex);
            Interfacer.UpdateControls();
        }

        private void ButtonAnimationUp_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ShiftAnimationUp(ViewModel.SelectedAnimationIndex);
            Interfacer.UpdateControls();
        }

        private void ButtonAnimationDown_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ShiftAnimationDown(ViewModel.SelectedAnimationIndex);
            Interfacer.UpdateControls();
        }

        private void ButtonAnimationDuplicate_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.DuplicateAnimation(ViewModel.SelectedAnimationIndex);
            Interfacer.UpdateControls();
        }

        private void ButtonAnimationRemove_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RemoveAnimation(ViewModel.SelectedAnimationIndex);
            Interfacer.UpdateControls();
        }

        private void ButtonAnimationImport_Click(object sender, RoutedEventArgs e)
        {
            Handler.ImportAnimation();
            Interfacer.UpdateControls();
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
            Handler.ExportAnimation();
            Interfacer.UpdateControls();
        }

        private void ButtonAnimationRename_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedAnimation != null)
            {
                int tempIndex = ViewModel.SelectedAnimationIndex;
                ViewModel.SelectedAnimationEntries[ViewModel.SelectedAnimationIndex].AnimName = RSDKrU.TextPrompt2.ShowDialog("Change Name", "Enter a New Name for the Animation:", ViewModel.SelectedAnimation.AnimName);
                List.ItemsSource = null;
                Interfacer.UpdateControls();
                List.SelectedIndex = tempIndex;
            }
        }

        #endregion

        #region Frame Items

        private void ButtonFrameAdd_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddFrame((ViewModel.SelectedFrameIndex != -1 ? ViewModel.SelectedFrameIndex : 0));
            Interfacer.UpdateControls();
            Interfacer.UpdateSelectedSectionProperties();
        }

        private void ButtonFrameDupe_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.DuplicateFrame(ViewModel.SelectedFrameIndex);
            Interfacer.UpdateControls();
            Interfacer.UpdateSelectedSectionProperties();
        }

        private void ButtonFrameLeft_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ShiftFrameLeft(ViewModel.SelectedFrameIndex);
            Interfacer.UpdateControls();
            Interfacer.UpdateSelectedSectionProperties();
        }

        private void ButtonFrameRight_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ShiftFrameRight(ViewModel.SelectedFrameIndex);
            Interfacer.UpdateControls();
            Interfacer.UpdateSelectedSectionProperties();
        }

        private void ButtonFrameRemove_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RemoveFrame(ViewModel.SelectedFrameIndex);
            Interfacer.UpdateControls();
            Interfacer.UpdateSelectedSectionProperties();
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
            Handler.ImportFrame();
            Interfacer.UpdateControls();
            Interfacer.UpdateSelectedSectionProperties();
        }

        private void ButtonFrameExport_Click(object sender, RoutedEventArgs e)
        {
            Handler.ExportFrame();
            Interfacer.UpdateControls();
            Interfacer.UpdateSelectedSectionProperties();
        }

        private void ButtonZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Zoom < 8)
            {
                ViewModel.Zoom = ViewModel.Zoom + 1;
                Interfacer.UpdateControls();
            }
        }

        private void ButtonZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Zoom > 1)
            {
                ViewModel.Zoom = ViewModel.Zoom - 1;
                Interfacer.UpdateControls();
            }

        }



        #endregion

        #region Frame Viewer Controls

        private void AnimationScroller_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!PreventScrollChange)
            {
                PreventScrollChange = true;
                if (AnimationScroller.Value == 3) Interfacer.ScrollFrameIndex(false);
                if (AnimationScroller.Value == 1) Interfacer.ScrollFrameIndex(true);
                AnimationScroller.Value = 2;
                PreventScrollChange = false;
            }

        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonShowCenter_Click(object sender, RoutedEventArgs e)
        {
            Interfacer.ShowAlignmentLines = ButtonShowCenter.IsChecked.Value;
            Interfacer.UpdateControls();
        }

        private void ButtonPlay_CheckChanged(object sender, RoutedEventArgs e)
        {
            if (ViewModel.LoadedAnimationFile != null && ViewModel.SelectedAnimation != null)
            {
                bool enabled = ButtonPlay.IsChecked.Value && List.SelectedItem != null;
                Interfacer.TogglePlayback(enabled);
            }
        }

        private void PlaybackOptionsButton_Click(object sender, RoutedEventArgs e)
        {
            PlaybackOptionsContextMenu.IsOpen = true;
        }

        private void ForcePlaybackMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Interfacer.isForcePlaybackOn = ForcePlaybackMenuItem.IsChecked;
        }

        private void ForcedPlaybackSpeedNUD_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (Interfacer != null) Interfacer.ForcePlaybackSpeed = ForcedPlaybackSpeedNUD.Value.Value;
        }

        private void ForcedPlaybackDurationNUD_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (Interfacer != null) Interfacer.ForcePlaybackDuration = ForcedPlaybackDurationNUD.Value.Value;
        }

        private void ExportAnimationImages_Click(object sender, RoutedEventArgs e)
        {
            Handler.ExportAnimationFramesToImages();
            Interfacer.UpdateControls();
        }

        private void ButtonShowFieldHitbox_Click(object sender, RoutedEventArgs e)
        {
            Interfacer.ShowHitBox = ButtonShowFieldHitbox.IsChecked.Value;
            Interfacer.UpdateControls();
        }

        private void ButtonHelp_Click(object sender, RoutedEventArgs e)
        {
            ButtonHelp.ContextMenu.IsOpen = true;
        }

        #endregion

        #region Canvas Controls

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Interfacer.UpdateControls();
        }

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 1)
            {
                if (ViewModel.Zoom < 8)
                {
                    ViewModel.Zoom = ViewModel.Zoom + 1;
                }
            }
            else
            {
                if (ViewModel.Zoom > 1)
                {
                    ViewModel.Zoom = ViewModel.Zoom - 1;
                }
            }
            Interfacer.UpdateControls();
        }

        #endregion

        #region List Controls
        public void List_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ViewModel.SelectedAnimationIndex = List.SelectedIndex;
            Interfacer.UpdateSelectedSectionProperties(false, true);
            Interfacer.UpdateControls();
        }
        public void FramesList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ViewModel.SelectedFrameIndex = FramesList.SelectedIndex;
            if (!Interfacer.isPlaybackEnabled)
            {
                Interfacer.UpdateControls();
                Interfacer.UpdateCurrentFrameProperties();
            }
        }
        private void FramesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Interfacer.UpdateControls();
        }
        private void FramesList_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            Interfacer.UpdateControls();
        }

        #endregion

        #region Frame Property Controls
        public void NUD_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Console.WriteLine("NUD Event Fired!");
            if (isLoadedFully)
            {
                Interfacer.UpdateFrameNUDValues(sender, e);
                Interfacer.UpdateControls();
                Interfacer.UpdateCurrentFrameMaxMinProperties();
            }
        }
        public void SpriteSheetList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Interfacer.UpdateFrameSpriteSheetValues(sender, e);
            Interfacer.UpdateControls();
        }
        public void HitBoxComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Interfacer.UpdateFrameHitboxValues(sender, e);
            Interfacer.UpdateControls();
            Interfacer.UpdateCurrentFrameHitboxProperties();
        }
        private void HitBoxComboBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Interfacer.UpdateControls();
        }
        #endregion

        #region Animation Info Controls
        public void SpeedNUD_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (isLoadedFully)
            {
                Interfacer.UpdateAnimationInfoNUDValues(sender, e);
                Interfacer.UpdateControls();
            }
        }

        public void LoopIndexNUD_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (isLoadedFully)
            {
                Interfacer.UpdateAnimationInfoNUDValues(sender, e);
                Interfacer.UpdateControls();
            }
        }

        public void PlayerID_NUD_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (isLoadedFully)
            {
                Interfacer.UpdateAnimationInfoNUDValues(sender, e);
                Interfacer.UpdateControls();
            }
        }

        public void FlagsSelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (isLoadedFully)
            {
                Interfacer.UpdateAnimationInfoFlagValues(sender, e);
                Interfacer.UpdateControls();
            }
        }

        public void Unknown_NUD_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (isLoadedFully)
            {
                Interfacer.UpdateAnimationInfoNUDValues(sender, e);
                Interfacer.UpdateControls();
            }
        }

        public void DreamcastVer_Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            if (isLoadedFully)
            {
                Interfacer.UpdateAnimationInfoMiscValues(sender, e);
                Interfacer.UpdateControls();
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
            //Interfacer.UpdateUI();
        }

        private void TextureManagerPopup_LostFocus(object sender, RoutedEventArgs e)
        {
            //TextureManagerPopup.IsOpen = false;
            //Interfacer.UpdateUI();
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
                Interfacer.HitboxBackground = HitboxColorPicker.SelectedColor.Value;
            }
            if (AxisColorPicker.SelectedColor != null)
            {
                Interfacer.AlignmentLinesColor = AxisColorPicker.SelectedColor.Value;
            }
            if (BGColorPicker.SelectedColor != null)
            {
                Interfacer.CanvasBackground = BGColorPicker.SelectedColor.Value;
            }
            if (FrameBorderColorPicker.SelectedColor != null)
            {
                Interfacer.FrameBorder = FrameBorderColorPicker.SelectedColor.Value;
            }
            if (FrameBackgroundColorPicker.SelectedColor != null)
            {
                Interfacer.FrameBackground = FrameBackgroundColorPicker.SelectedColor.Value;
            }
            Interfacer.UpdateCanvasVisual();
        }
        #endregion

        #region Window Controls
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MenuViewUseDarkTheme.IsChecked)
            {
                Classes.Settings.Default.UseDarkTheme = true;
                Classes.Settings.Save();
            }
            else
            {
                Classes.Settings.Default.UseDarkTheme = false;
                Classes.Settings.Save();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            isLoadedFully = true;
            if (Classes.Settings.Default.UseDarkTheme)
            {
                MenuViewUseDarkTheme.IsChecked = true;
            }
        }
        #endregion

        #region Canvas Controls

        private void CanvasView_MouseMove(object sender, MouseEventArgs e)
        {
            if (!Interfacer.isPlaybackEnabled) InputControl.MouseMove(sender, e);
        }

        private void CanvasView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!Interfacer.isPlaybackEnabled) InputControl.MouseDown(sender, e);
        }

        private void CanvasView_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!Interfacer.isPlaybackEnabled) InputControl.MouseUp(sender, e);
        }

        private void CanvasView_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Interfacer.isPlaybackEnabled) InputControl.KeyDown(sender, e);
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

        #region Rendering
        private void CanvasView_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
        {
            if (Interfacer != null) Interfacer.PaintSurface(sender, e);
            
        }





        #endregion

        private void MenuFileOpenFromWorkspaceAddWorkspace_Click(object sender, RoutedEventArgs e)
        {
            Handler.SelectNewWorkspaceFolder();
        }

        private void MenuFileOpenFromWorkspaceRemoveMode_Click(object sender, RoutedEventArgs e)
        {
            Handler.ToggleWorkspaceRemoveMode(MenuFileOpenFromWorkspaceRemoveMode.IsChecked);
        }
    }


}
