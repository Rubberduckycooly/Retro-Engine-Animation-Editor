using AnimationEditor.ViewModels;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AnimationEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ViewModelv5 ViewModel => (ViewModelv5)DataContext;

        private UserInterfacer Interfacer;
        public FileHandler Handler;
        public Brush DefaultBorderBrush;
        public Brush DefaultTextBrush;

        private bool PreventScrollChange = true;

        public MainWindow()
        {
            DefaultBorderBrush = (Brush)FindResource("ComboBoxBorder");
            DefaultTextBrush = (Brush)FindResource("NormalText");
            InitializeComponent();
            DataContext = new ViewModelv5();
            List.AllowDrop = true;
            Interfacer = new UserInterfacer(this);
            Handler = new FileHandler(this);
            PreventScrollChange = false;
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
            Interfacer.UpdateUI();
        }

        private void ButtonAnimationUp_Click(object sender, RoutedEventArgs e)
        {
            Interfacer.UpdateUI();
        }

        private void ButtonAnimationDown_Click(object sender, RoutedEventArgs e)
        {
            Interfacer.UpdateUI();
        }

        private void ButtonFrameLeft_Click(object sender, RoutedEventArgs e)
        {
            Interfacer.UpdateUI();
        }

        private void ButtonFrameRight_Click(object sender, RoutedEventArgs e)
        {
            Interfacer.UpdateUI();
        }

        private void ButtonAnimationDuplicate_Click(object sender, RoutedEventArgs e)
        {
            Interfacer.UpdateUI();
        }

        private void ButtonAnimationRemove_Click(object sender, RoutedEventArgs e)
        {
            Interfacer.UpdateUI();
        }

        private void ButtonAnimationImport_Click(object sender, RoutedEventArgs e)
        {
            Interfacer.UpdateUI();
        }

        private void ButtonAnimationExport_Click(object sender, RoutedEventArgs e)
        {
            Interfacer.UpdateUI();
        }

        private void ButtonFrameAdd_Click(object sender, RoutedEventArgs e)
        {
            Interfacer.UpdateUI();
        }

        private void ButtonFrameDupe_Click(object sender, RoutedEventArgs e)
        {
            Interfacer.UpdateUI();
        }

        private void ButtonFrameRemove_Click(object sender, RoutedEventArgs e)
        {
            Interfacer.UpdateUI();
        }

        private void ButtonFrameImport_Click(object sender, RoutedEventArgs e)
        {
            Interfacer.UpdateUI();
        }

        private void ButtonFrameExport_Click(object sender, RoutedEventArgs e)
        {
            Interfacer.UpdateUI();
        }

        private void ButtonZoomIn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SpriteScaleX = ViewModel.SpriteScaleX + 1;
            Interfacer.UpdateUI();
        }

        private void ButtonZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SpriteScaleX != 1)
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
            Interfacer.UpdateUI();
        }

        private void MenuViewTexture_Click(object sender, RoutedEventArgs e)
        {
            TextureManagerMenu.Startup(this);
            TextureManagerPopup.IsOpen = true;
            Interfacer.UpdateUI();

        }

        private void MenuViewHitbox_Click(object sender, RoutedEventArgs e)
        {
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
            HitBoxComboBox.SelectedIndex = -1;
            Interfacer.UpdateUI();
        }

        private void MenuRecentFile_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.MenuItem)
            {
                System.Windows.Controls.MenuItem item = sender as System.Windows.Controls.MenuItem;
                bool result = Int32.TryParse(item.Tag.ToString(), out int index);
                if (result == true) Handler.OpenRecentFile(index-1);
            }
            Interfacer.UpdateUI();

        }

        private void ContextMenu_ContextMenuClosing(object sender, System.Windows.Controls.ContextMenuEventArgs e)
        {
            TextureManagerMenu.Shutdown();
        }

        private void HitboxManagerMenu_DragOver(object sender, DragEventArgs e)
        {

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
    }
}
