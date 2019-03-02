using AnimationEditor.ViewModels;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace AnimationEditor
{
    public class FileHandler
    {
        private MainWindow Instance;
        public FileHandler(MainWindow window)
        {
            Instance = window;
        }


        public void OpenFile()
        {
            UnloadAnimationData();
            var fd = new OpenFileDialog();
            fd.DefaultExt = "*.bin";
            fd.Filter = "RSDKv5 Animation Files|*.bin|RSDKv2 and RSDKvB Animation Files|*.ani|RSDKv1 Animation Files|*.ani|RSDKvRS Animation Files|*.ani";
            if (fd.ShowDialog() == true)
            {
                UpdateRecentsDropDown(fd.FileName);
                LoadFile(fd);
            }
        }

        public void OpenRecentFile(int index)
        {
            if (Properties.Settings.Default.RecentFiles != null)
            {
                if (Properties.Settings.Default.RecentFiles[index] != null)
                {
                    if (File.Exists(Properties.Settings.Default.RecentFiles[index])) LoadFile(Properties.Settings.Default.RecentFiles[index].ToString());
                }
            }
            UpdateRecentsDropDown();
        }

        public void LoadFile(string filepath)
        {
            Instance.ViewModel.SpriteSheets = new System.Collections.Generic.List<BitmapImage>();
            Instance.ViewModel.LoadedAnimationFile = new RSDKv5.Animation(new RSDKv5.Reader(filepath));
            foreach (string path in Instance.ViewModel.SpriteSheetPaths)
            {
                string animationDirectory = Path.GetDirectoryName(filepath);
                Instance.ViewModel.AnimationDirectory = animationDirectory;
                string imagePath = Path.Combine(Directory.GetParent(animationDirectory).FullName, path);
                if (File.Exists(imagePath))
                {
                    Instance.ViewModel.SpriteSheets.Add(LoadAnimationTexture(imagePath));
                }
                else
                {
                    Instance.ViewModel.SpriteSheets.Add(new BitmapImage());
                }

            }
        }

        public void LoadFile(OpenFileDialog fd)
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
            foreach (string path in Instance.ViewModel.SpriteSheetPaths)
            {
                string animationDirectory = Path.GetDirectoryName(fd.FileName);
                Instance.ViewModel.AnimationDirectory = animationDirectory;
                string imagePath = Path.Combine(Directory.GetParent(animationDirectory).FullName, path);
                if (File.Exists(imagePath))
                {
                    Instance.ViewModel.SpriteSheets.Add(LoadAnimationTexture(imagePath));
                }
                else
                {
                    Instance.ViewModel.SpriteSheets.Add(new BitmapImage());
                }

            }
        }

        public BitmapImage LoadAnimationTexture(string fileName)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            var img = new System.Windows.Media.Imaging.BitmapImage();
            img.BeginInit();
            img.StreamSource = fileStream;
            img.EndInit();
            return img;
        }

        public void SaveFile()
        {

        }



        public void SaveFileAs()
        {

        }

        public void UpdateRecentsDropDown(string itemToAdd = "")
        {
            if (Properties.Settings.Default.RecentFiles == null) Properties.Settings.Default.RecentFiles = new System.Collections.Specialized.StringCollection();
            if (itemToAdd != "")
            {
                if (Properties.Settings.Default.RecentFiles.Contains(itemToAdd))
                {
                    Properties.Settings.Default.RecentFiles.Remove(itemToAdd);
                    Properties.Settings.Default.RecentFiles.Add(itemToAdd);
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.RecentFiles.Add(itemToAdd);
                    Properties.Settings.Default.Save();
                }
            }

            if (Properties.Settings.Default.RecentFiles.Count >= 1) { Instance.MenuRecentFile1.Header = String.Format("1: {0}", Properties.Settings.Default.RecentFiles[0].ToString()); Instance.MenuRecentFile1.IsEnabled = true; Instance.MenuRecentFile1.Visibility = Visibility.Visible; }
            else { Instance.MenuRecentFile1.Header = "1: N/A"; Instance.MenuRecentFile1.IsEnabled = false; Instance.MenuRecentFile1.Visibility = Visibility.Collapsed; }
            if (Properties.Settings.Default.RecentFiles.Count >= 2) { Instance.MenuRecentFile2.Header = String.Format("2: {0}", Properties.Settings.Default.RecentFiles[1].ToString()); Instance.MenuRecentFile2.IsEnabled = true; Instance.MenuRecentFile2.Visibility = Visibility.Visible; }
            else { Instance.MenuRecentFile2.Header = "2: N/A"; Instance.MenuRecentFile2.IsEnabled = false; Instance.MenuRecentFile2.Visibility = Visibility.Collapsed; }
            if (Properties.Settings.Default.RecentFiles.Count >= 3) { Instance.MenuRecentFile3.Header = String.Format("3: {0}", Properties.Settings.Default.RecentFiles[2].ToString()); Instance.MenuRecentFile3.IsEnabled = true; Instance.MenuRecentFile3.Visibility = Visibility.Visible; }
            else { Instance.MenuRecentFile3.Header = "3: N/A"; Instance.MenuRecentFile3.IsEnabled = false; Instance.MenuRecentFile3.Visibility = Visibility.Collapsed; }
            if (Properties.Settings.Default.RecentFiles.Count >= 4) { Instance.MenuRecentFile4.Header = String.Format("4: {0}", Properties.Settings.Default.RecentFiles[3].ToString()); Instance.MenuRecentFile4.IsEnabled = true; Instance.MenuRecentFile4.Visibility = Visibility.Visible; }
            else { Instance.MenuRecentFile4.Header = "4: N/A"; Instance.MenuRecentFile4.IsEnabled = false; Instance.MenuRecentFile4.Visibility = Visibility.Collapsed; }
            if (Properties.Settings.Default.RecentFiles.Count >= 5) { Instance.MenuRecentFile5.Header = String.Format("5: {0}", Properties.Settings.Default.RecentFiles[4].ToString()); Instance.MenuRecentFile5.IsEnabled = true; Instance.MenuRecentFile5.Visibility = Visibility.Visible; }
            else { Instance.MenuRecentFile5.Header = "5: N/A"; Instance.MenuRecentFile5.IsEnabled = false; Instance.MenuRecentFile5.Visibility = Visibility.Collapsed; }
            if (Properties.Settings.Default.RecentFiles.Count >= 6) { Instance.MenuRecentFile6.Header = String.Format("6: {0}", Properties.Settings.Default.RecentFiles[5].ToString()); Instance.MenuRecentFile6.IsEnabled = true; Instance.MenuRecentFile6.Visibility = Visibility.Visible; }
            else { Instance.MenuRecentFile6.Header = "6: N/A"; Instance.MenuRecentFile6.IsEnabled = false; Instance.MenuRecentFile6.Visibility = Visibility.Collapsed; }
            if (Properties.Settings.Default.RecentFiles.Count >= 7) { Instance.MenuRecentFile7.Header = String.Format("7: {0}", Properties.Settings.Default.RecentFiles[6].ToString()); Instance.MenuRecentFile7.IsEnabled = true; Instance.MenuRecentFile7.Visibility = Visibility.Visible; }
            else { Instance.MenuRecentFile7.Header = "7: N/A"; Instance.MenuRecentFile7.IsEnabled = false; Instance.MenuRecentFile7.Visibility = Visibility.Collapsed; }
            if (Properties.Settings.Default.RecentFiles.Count >= 8) { Instance.MenuRecentFile8.Header = String.Format("8: {0}", Properties.Settings.Default.RecentFiles[7].ToString()); Instance.MenuRecentFile8.IsEnabled = true; Instance.MenuRecentFile8.Visibility = Visibility.Visible; }
            else { Instance.MenuRecentFile8.Header = "8: N/A"; Instance.MenuRecentFile8.IsEnabled = false; Instance.MenuRecentFile8.Visibility = Visibility.Collapsed; }
            if (Properties.Settings.Default.RecentFiles.Count >= 9) { Instance.MenuRecentFile9.Header = String.Format("9: {0}", Properties.Settings.Default.RecentFiles[8].ToString()); Instance.MenuRecentFile9.IsEnabled = true; Instance.MenuRecentFile9.Visibility = Visibility.Visible; }
            else { Instance.MenuRecentFile9.Header = "9: N/A"; Instance.MenuRecentFile9.IsEnabled = false; Instance.MenuRecentFile9.Visibility = Visibility.Collapsed; }
            if (Properties.Settings.Default.RecentFiles.Count >= 10) { Instance.MenuRecentFile10.Header = String.Format("10: {0}", Properties.Settings.Default.RecentFiles[9].ToString()); Instance.MenuRecentFile10.IsEnabled = true; Instance.MenuRecentFile10.Visibility = Visibility.Visible; }
            else { Instance.MenuRecentFile10.Header = "10: N/A"; Instance.MenuRecentFile10.IsEnabled = false; Instance.MenuRecentFile10.Visibility = Visibility.Collapsed; }

        }

        public void UnloadAnimationData()
        {
            Instance.List.SelectedIndex = -1;
            Instance.FramesList.SelectedIndex = -1;
            if (Instance.ViewModel.SpriteSheets != null) Instance.ViewModel.SpriteSheets.Clear();
            Instance.DataContext = new ViewModelv5();
            Instance.ViewModel.SpriteSheets = new System.Collections.Generic.List<BitmapImage>();

        }

    }
}
