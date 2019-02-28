using AnimationEditor.ViewModels;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AnimationEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ViewModelv5 ViewModel => (ViewModelv5)DataContext;

        private UserInterfacer Interfacer;
        private FileHandler Handler;
        public Brush DefaultBorderBrush;
        public Brush DefaultTextBrush;

        public MainWindow()
        {
            DefaultBorderBrush = (Brush)FindResource("ComboBoxBorder");
            DefaultTextBrush = (Brush)FindResource("NormalText");
            InitializeComponent();
            DataContext = new ViewModelv5();

            List.AllowDrop = true;
            Interfacer = new UserInterfacer(this);
            Handler = new FileHandler(this);
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
            Interfacer.UpdateUI();
        }

        private void MenuViewHitbox_Click(object sender, RoutedEventArgs e)
        {
            Interfacer.UpdateUI();
        }

        private void MenuInfoAbout_Click(object sender, RoutedEventArgs e)
        {
            Interfacer.UpdateUI();
        }

        private void FramesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Interfacer.UpdateUI();
        }

        private void NUD_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Interfacer.UpdateUI();
        }

        private void List_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
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
    }
}
