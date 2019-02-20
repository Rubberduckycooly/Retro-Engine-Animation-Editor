using AnimationEditor.ViewModels;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace AnimationEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel ViewModel => (MainViewModel)DataContext;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            List.AllowDrop = true;
        }

        private void MenuFileOpen_Click(object sender, RoutedEventArgs e)
        {
            var fd = new OpenFileDialog();
            fd.DefaultExt = "*.bin";
            fd.Filter = "RSDKv5 Animation Files|*.bin|RSDKv2 and RSDKvB Animation Files|*.ani|RSDKv1 Animation Files|*.ani|RSDKvRS Animation Files|*.ani";
            if (fd.ShowDialog() == true)
            {

                //RSDKvRS and RSDKv1 don't have rotation flags
                if (fd.FilterIndex - 1 > 1) { FlagsSelector.IsEnabled = false; }
                if (fd.FilterIndex - 1 < 2) { FlagsSelector.IsEnabled = true; }

                //For RSDKvRS, RSDKv1 and RSDKv2 & RSDKvB there is no ID and the Delay is always 256, so there is no point to let users change their values
                if (fd.FilterIndex - 1 >= 1) { DelayNUD.IsEnabled = false; idNUD.IsEnabled = false; }
                if (fd.FilterIndex - 1 == 3) { idNUD.IsEnabled = true; IDLabel.Text = "Player"; }
                else { IDLabel.Text = "ID"; }
                if (fd.FilterIndex - 1 == 0) { DelayNUD.IsEnabled = true; idNUD.IsEnabled = true; }

            }
        }

        private void MenuFileSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuFileSaveAs_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuFileExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonAnimationAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonAnimationUp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonAnimationDown_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonFrameLeft_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonFrameRight_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonAnimationDuplicate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonAnimationRemove_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonAnimationImport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonAnimationExport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonFrameAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonFrameDupe_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonFrameRemove_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonFrameImport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonFrameExport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonZoomIn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonZoomOut_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void List_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void MenuViewTexture_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuViewHitbox_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuInfoAbout_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FramesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
