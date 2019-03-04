using AnimationEditor.ViewModels;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Linq;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Reflection;
using System.Collections.Specialized;

namespace AnimationEditor
{
    public class FileHandler
    {
        private MainWindow Instance;
        public System.Collections.Generic.IList<MenuItem> RecentItems;
        public FileHandler(MainWindow window)
        {
            Instance = window;
            RecentItems = new List<MenuItem>();
        }


        public void OpenFile()
        {
            Instance.Interfacer.PreventIndexUpdate = true;
            UnloadAnimationData();
            var fd = new OpenFileDialog();
            fd.DefaultExt = "*.bin";
            fd.Filter = "RSDKv5 Animation Files|*.bin|RSDKv2 and RSDKvB Animation Files|*.ani|RSDKv1 Animation Files|*.ani|RSDKvRS Animation Files|*.ani";
            if (fd.ShowDialog() == true)
            {
                AddRecentDataFolder(fd.FileName);
                LoadFile(fd);
            }
            Instance.Interfacer.PreventIndexUpdate = false;
        }

        public void OpenFile(string file)
        {
            Instance.Interfacer.PreventIndexUpdate = true;
            UnloadAnimationData();
            LoadFile(file);
            UpdateRecentsDropDown();
            Instance.Interfacer.PreventIndexUpdate = false;
            Instance.Interfacer.UpdateUI();
        }

        public void LoadFile(string filepath)
        {
            Instance.ViewModel.SpriteSheets = new System.Collections.Generic.List<BitmapImage>();
            Instance.ViewModel.LoadedAnimationFile = new Animation();
            Instance.ViewModel.LoadedAnimationFile.ImportFrom(Instance.AnimationType, filepath);
            LoadAnimationTextures(filepath);
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

            switch (fd.FilterIndex - 1)
            {
                case 0:
                    Instance.AnimationType = EngineType.RSDKv5;
                    break;
                case 1:
                    Instance.AnimationType = EngineType.RSDKv2;
                    break;
                case 2:
                    Instance.AnimationType = EngineType.RSDKv1;
                    break;
                case 3:
                    Instance.AnimationType = EngineType.RSDKvRS;
                    break;
            }

            Instance.ViewModel.LoadedAnimationFile = new Animation();
            Instance.ViewModel.LoadedAnimationFile.ImportFrom(Instance.AnimationType, fd.FileName);
            LoadAnimationTextures(fd.FileName);
        }

        public void LoadAnimationTextures(string filename)
        {
            foreach (string path in Instance.ViewModel.SpriteSheetPaths)
            {
                string animationDirectory = Path.GetDirectoryName(filename);
                Instance.ViewModel.AnimationDirectory = animationDirectory;
                string imagePath = Path.Combine(Directory.GetParent(animationDirectory).FullName, Instance.ViewModel.LoadedAnimationFile.pathmod, path);
                if (File.Exists(imagePath))
                {
                    Instance.ViewModel.SpriteSheets.Add(LoadAnimationTexture(imagePath));
                }
                else
                {
                    Instance.ViewModel.SpriteSheets.Add(new BitmapImage());
                    Instance.ViewModel.NullSpriteSheetList.Add(path);
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
            var fd = new SaveFileDialog();
            fd.DefaultExt = "*.bin";
            fd.Filter = "RSDKv5 Animation Files|*.bin|RSDKv2 and RSDKvB Animation Files|*.ani|RSDKv1 Animation Files|*.ani|RSDKvRS Animation Files|*.ani";
            if (fd.ShowDialog() == true)
            {
                UpdateRecentsDropDown(fd.FileName);

                switch (fd.FilterIndex - 1)
                {
                    case 0:
                        Instance.AnimationType = EngineType.RSDKv5;
                        break;
                    case 1:
                        Instance.AnimationType = EngineType.RSDKv2;
                        break;
                    case 2:
                        Instance.AnimationType = EngineType.RSDKv1;
                        break;
                    case 3:
                        Instance.AnimationType = EngineType.RSDKvRS;
                        break;
                }
                Instance.ViewModel.LoadedAnimationFile.ExportTo(Instance.AnimationType, fd.FileName);
            }
        }

        public void UnloadAnimationData()
        {
            Instance.List.SelectedIndex = -1;
            Instance.FramesList.SelectedIndex = -1;
            if (Instance.ViewModel.SpriteSheets != null) Instance.ViewModel.SpriteSheets.Clear();
            Instance.DataContext = new MainViewModel();
            Instance.ViewModel.SpriteSheets = new System.Collections.Generic.List<BitmapImage>();
            Instance.ViewModel.NullSpriteSheetList.Clear();
        }

        #region Recent Files (Lifted from Maniac Editor)

        public void UpdateRecentsDropDown(string itemToAdd = "")
        {
            if (itemToAdd != "") AddRecentDataFolder(itemToAdd);
            RefreshDataDirectories(Properties.Settings.Default.RecentFiles);
        }

        public void RecentDataDirectoryClicked(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as System.Windows.Controls.MenuItem;
            string dataDirectory = menuItem.Tag.ToString();
            var dataDirectories = Properties.Settings.Default.RecentFiles;
            if (File.Exists(dataDirectory))
            {
                AddRecentDataFolder(dataDirectory);
                OpenFile(dataDirectory);
            }
            else
            {
                RSDKrU.MessageBox.Show($"The specified File {dataDirectory} is not valid.",
                                "Invalid Annimation File!",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                dataDirectories.Remove(dataDirectory);
                RefreshDataDirectories(dataDirectories);

            }
            Properties.Settings.Default.Save();
        }

        public void RefreshDataDirectories(System.Collections.Specialized.StringCollection recentDataDirectories)
        {
            if (Properties.Settings.Default.RecentFiles?.Count > 0)
            {
                Instance.NoRecentFiles.Visibility = Visibility.Collapsed;
                CleanUpRecentList();

                var startRecentItems = Instance.MenuFileOpenRecently.Items.IndexOf(Instance.NoRecentFiles);

                foreach (var dataDirectory in recentDataDirectories)
                {
                    RecentItems.Add(CreateDataDirectoryMenuLink(dataDirectory));
                }



                foreach (MenuItem menuItem in RecentItems.Reverse())
                {
                    Instance.MenuFileOpenRecently.Items.Insert(startRecentItems, menuItem);
                }
            }
            else
            {
                Instance.NoRecentFiles.Visibility = Visibility.Visible;
            }



        }

        private MenuItem CreateDataDirectoryMenuLink(string target)
        {
            MenuItem newItem = new MenuItem();
            newItem.Header = target;
            newItem.InputGestureText = GetRecentItemFileVersion(target);
            newItem.Tag = target;
            newItem.Click += RecentDataDirectoryClicked;
            return newItem;
        }

        private string GetRecentItemFileVersion(string target)
        {
            byte[] Header = null;
            using (FileStream fs = new FileStream(target, FileMode.Open, FileAccess.Read))
            {
                Header = new byte[4];
                fs.Read(Header, 0, (int)4);
            }

            byte[] RSDKv5 = new byte[] { (byte)'S', (byte)'P', (byte)'R', (byte)'\0' };
            byte[] RSDKv1 = new byte[] { (byte)'\0', (byte)'\0', (byte)'\0', (byte)'\0' };
            
            // WIP 
            byte[] RSDKvRS = new byte[] { (byte)'\0', (byte)'\0', (byte)'\0', (byte)'\0' };
            byte[] RSDKv2 = new byte[] { (byte)'\0', (byte)'\0', (byte)'\0', (byte)'\0' };
            byte[] RSDKvB = new byte[] { (byte)'\0', (byte)'\0', (byte)'\0', (byte)'\0' };




            if (HeaderMatches(Header, RSDKv5))
            {
                return "RSDKv5";
            }
            else if (HeaderMatches(Header, RSDKv1))
            {
                return "RSDKv1";
            }
            else
            {
                return "?";
            }
        }

        private bool HeaderMatches(byte[] target, byte[] format) {
            for (int i = 0; i < 4; i++)
            {
                string targetItem = target[i].ToString();
                string formatItem = format[i].ToString();
                if (targetItem == formatItem) continue;
                else return false;
            }
            return true;
        }

        private void CleanUpRecentList()
        {
            foreach (var menuItem in RecentItems)
            {
                menuItem.Click -= RecentDataDirectoryClicked;
                Instance.MenuFileOpenRecently.Items.Remove(menuItem);
            }

            List<string> ItemsForRemoval = new List<string>();

            for (int i = 0; i < Properties.Settings.Default.RecentFiles.Count; i++)
            {
                if (File.Exists(Properties.Settings.Default.RecentFiles[i])) continue;
                else ItemsForRemoval.Add(Properties.Settings.Default.RecentFiles[i]);
            }
            foreach(string item in ItemsForRemoval)
            {
                Properties.Settings.Default.RecentFiles.Remove(item);
            }

            RecentItems.Clear();
        }

        public void AddRecentDataFolder(string dataDirectory)
        {
            try
            {
                var mySettings = Properties.Settings.Default;
                var dataDirectories = mySettings.RecentFiles;

                if (dataDirectories == null)
                {
                    dataDirectories = new System.Collections.Specialized.StringCollection();
                    mySettings.RecentFiles = dataDirectories;
                }

                if (dataDirectories.Contains(dataDirectory))
                {
                    dataDirectories.Remove(dataDirectory);
                }

                if (dataDirectories.Count >= 10)
                {
                    for (int i = 9; i < dataDirectories.Count; i++)
                    {
                        dataDirectories.RemoveAt(i);
                    }
                }

                dataDirectories.Insert(0, dataDirectory);

                mySettings.Save();

                RefreshDataDirectories(dataDirectories);


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Failed to add data folder to recent list: " + ex);
            }
        }

        #endregion

    }

}
