using AnimationEditor.ViewModels;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace AnimationEditor
{
    public class FileHandler
    {
        private MainWindow Instance;
        public FileHandler(MainWindow window)
        {
            Instance = window;
        }


        public void LoadFile()
        {
            UnloadAnimationData();
            var fd = new OpenFileDialog();
            fd.DefaultExt = "*.bin";
            fd.Filter = "RSDKv5 Animation Files|*.bin|RSDKv2 and RSDKvB Animation Files|*.ani|RSDKv1 Animation Files|*.ani|RSDKvRS Animation Files|*.ani";
            if (fd.ShowDialog() == true)
            {

                //RSDKvRS and RSDKv1 don't have rotation flags
                if (fd.FilterIndex - 1 > 1) { Instance.FlagsSelector.IsEnabled = false; }
                if (fd.FilterIndex - 1 < 2) { Instance.FlagsSelector.IsEnabled = true; }

                //For RSDKvRS, RSDKv1 and RSDKv2 & RSDKvB there is no ID and the Delay is always 256, so there is no point to let users change their values
                if (fd.FilterIndex - 1 >= 1) { Instance.DelayNUD.IsEnabled = false; Instance.idNUD.IsEnabled = false; }
                if (fd.FilterIndex - 1 == 3) { Instance.idNUD.IsEnabled = true; Instance.IDLabel.Text = "Player"; }
                else { Instance.IDLabel.Text = "ID"; }
                if (fd.FilterIndex - 1 == 0) { Instance.DelayNUD.IsEnabled = true; Instance.idNUD.IsEnabled = true; }

                Instance.ViewModel.LoadedAnimationFile = new RSDKv5.Animation(new RSDKv5.Reader(fd.FileName));

            }
        }

        public void SaveFile()
        {

        }



        public void SaveFileAs()
        {

        }

        public void UnloadAnimationData()
        {
            Instance.DataContext = new ViewModelv5();
        }

    }
}
