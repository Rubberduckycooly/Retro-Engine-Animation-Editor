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

        #region Open File Methods
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
            Instance.Interfacer.UpdateViewerLayout();
        }
        #endregion

        #region Load File Methods
        public void LoadFile(string filepath)
        {
            Instance.ViewModel.LoadedAnimationFile = new Animation();
            Instance.ViewModel.AnimationFilepath = filepath;
            Instance.ViewModel.LoadedAnimationFile.ImportFrom(Instance.AnimationType, filepath);
            LoadAnimationTextures(filepath);
        }

        public void LoadFile(OpenFileDialog fd)
        {
            //RSDKvRS and RSDKv1 don't have rotation flags
            if (fd.FilterIndex - 1 > 1) { Instance.FlagsSelector.IsEnabled = false; }
            if (fd.FilterIndex - 1 < 2) { Instance.FlagsSelector.IsEnabled = true; }

            //For RSDKvRS, RSDKv1 and RSDKv2 & RSDKvB there is no ID and the Delay is always 256, so there is no point to let users change their values
            if (fd.FilterIndex - 1 >= 1) { Instance.DelayNUD.IsEnabled = false; Instance.IdentificationNUD.IsEnabled = false; }
            if (fd.FilterIndex - 1 == 3) { Instance.IdentificationNUD.IsEnabled = true; Instance.IDLabel.Text = "Player"; }
            else { Instance.IDLabel.Text = "ID"; }
            if (fd.FilterIndex - 1 == 0) { Instance.DelayNUD.IsEnabled = true; Instance.IdentificationNUD.IsEnabled = true; }

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

            LoadFile(fd.FileName);
        }
        #endregion

        #region Save File Methods
        public void SaveFile()
        {
            if (Instance.ViewModel.AnimationFilepath != null) Instance.ViewModel.LoadedAnimationFile.ExportTo(Instance.AnimationType, Instance.ViewModel.AnimationFilepath);
            else SaveFileAs();
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
        #endregion

        #region Unloading / Loading Methods

        public void UnloadAnimationData()
        {
            Instance.List.SelectedIndex = -1;
            Instance.FramesList.SelectedIndex = -1;
            InitlizeSpriteSheets(true);
            Instance.DataContext = new MainViewModel();
            InitlizeSpriteSheets();
            Instance.ViewModel.NullSpriteSheetList.Clear();
            Instance.IntilizePlayback(true);
        }

        public BitmapImage LoadAnimationTexture(string fileName, bool transparent = false)
        {
            if (transparent)
            {
                FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                System.Drawing.Bitmap img = new System.Drawing.Bitmap(fileStream);
                fileStream.Close();
                var color = img.Palette.Entries[0];
                string hex = HexConverter(color);
                Instance.ViewModel.SpriteSheetTransparentColors.Add((Color)ColorConverter.ConvertFromString(hex));
                img.MakeTransparent(color);
                return (BitmapImage)BitmapConversion.ToWpfBitmap(img);
            }
            else
            {
                FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                var img = new System.Windows.Media.Imaging.BitmapImage();
                img.BeginInit();
                img.StreamSource = fileStream;
                img.EndInit();
                return img;
            }

        }

        private static String HexConverter(System.Drawing.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        public void LoadAnimationTextures(string filename)
        {
            if (Instance.ViewModel.SpriteSheets == null) InitlizeSpriteSheets(true);
            foreach (string path in Instance.ViewModel.SpriteSheetPaths)
            {
                string animationDirectory = Path.GetDirectoryName(filename);
                Instance.ViewModel.AnimationDirectory = animationDirectory;
                int subDirectoryCount = path.Split('/').Length - 1;
                string parentDirectory = animationDirectory;
                for (int i = 0; i < subDirectoryCount; i++)
                {
                    parentDirectory = Directory.GetParent(parentDirectory).FullName;
                }
                Instance.ViewModel.SpriteDirectory = parentDirectory;
                string imagePath = Path.Combine(parentDirectory, Instance.ViewModel.LoadedAnimationFile.pathmod, path.Replace("/", "\\"));
                imagePath = imagePath.Replace("Characters\\Characters", "Characters"); //Fix for RSDKvRS
                if (File.Exists(imagePath))
                {
                    Instance.ViewModel.SpriteSheets.Add(LoadAnimationTexture(imagePath));
                    Instance.ViewModel.SpriteSheetsWithTransparency.Add(LoadAnimationTexture(imagePath, true));
                }
                else
                {
                    Instance.ViewModel.SpriteSheets.Add(new BitmapImage());
                    Instance.ViewModel.SpriteSheetTransparentColors.Add((Color)ColorConverter.ConvertFromString("#303030"));
                    Instance.ViewModel.SpriteSheetsWithTransparency.Add(new BitmapImage());
                    Instance.ViewModel.NullSpriteSheetList.Add(path);
                    Instance.ViewModel.NullSpriteSheetList.Add(path);
                }

            }
        }

        public void InitlizeSpriteSheets(bool clearMode = false)
        {
            if (clearMode)
            {
                if (Instance.ViewModel.SpriteSheets != null) Instance.ViewModel.SpriteSheets.Clear();
                if (Instance.ViewModel.SpriteSheetsWithTransparency != null) Instance.ViewModel.SpriteSheetsWithTransparency.Clear();
                if (Instance.ViewModel.SpriteSheetTransparentColors != null) Instance.ViewModel.SpriteSheetTransparentColors.Clear();
            }
            else
            {
                Instance.ViewModel.SpriteSheets = new System.Collections.Generic.List<BitmapImage>();
                Instance.ViewModel.SpriteSheetsWithTransparency = new System.Collections.Generic.List<BitmapImage>();
                Instance.ViewModel.SpriteSheetTransparentColors = new List<Color>();

            }
        }
        #endregion

        #region Import / Export Methods

        public void ExportAnimationFramesToImages()
        {
            if (Instance.ViewModel.LoadedAnimationFile == null || Instance.ViewModel.SelectedAnimation == null) return;
            var fd = new FolderSelectDialog();
            fd.Title = "Select A Folder to Save the Images to...";
            if (fd.ShowDialog() == true)
            {
                string placeToSave = fd.FileName;
                for (int i = 0; i < Instance.ViewModel.LoadedAnimationFile.Animations[Instance.ViewModel.SelectedAnimationIndex].Frames.Count; i++)
                {
                    var img = Instance.ViewModel.GetFrameImage(i);
                    string fileName = Path.GetFileNameWithoutExtension(Instance.ViewModel.AnimationFilepath) + string.Format("_{0}_{1}.png", Instance.ViewModel.SelectedAnimationIndex, i);
                    string filePath = Path.Combine(placeToSave, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        BitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(img));
                        encoder.Save(fileStream);
                    }
                }
            }
        }

        public void ImportAnimation()
        {
            if (Instance.ViewModel.LoadedAnimationFile == null || Instance.ViewModel.SelectedAnimation == null) return;
            var fd = new OpenFileDialog();
            fd.DefaultExt = "*.anim";
            fd.Filter = "RSDK Animation Files|*.anim";
            if (fd.ShowDialog() == true)
            {
                var importAnim = new Animation.AnimationEntry();
                importAnim.ImportFrom(EngineType.RSDKv5, fd.FileName);
                Instance.ViewModel.LoadedAnimationFile.Animations.Add(importAnim);
            }

        }

        public void ExportAnimation()
        {
            if (Instance.ViewModel.LoadedAnimationFile == null || Instance.ViewModel.SelectedAnimation == null) return;
            var fd = new SaveFileDialog();
            fd.DefaultExt = "*.anim";
            fd.Filter = "RSDK Animation Files|*.anim";
            if (fd.ShowDialog() == true)
            {
                Instance.ViewModel.LoadedAnimationFile.Animations[Instance.ViewModel.SelectedAnimationIndex].ExportTo(EngineType.RSDKv5, fd.FileName);
            }
        }

        public void ImportFrame()
        {
            if (Instance.ViewModel.LoadedAnimationFile == null || Instance.ViewModel.SelectedAnimation == null) return;
            var fd = new OpenFileDialog();
            fd.DefaultExt = "*.frame";
            fd.Filter = "RSDK Frame Files|*.frame";
            if (fd.ShowDialog() == true)
            {
                var importFrame = new Animation.Frame();
                importFrame.ImportFrom(EngineType.RSDKv5, fd.FileName);
                Instance.ViewModel.LoadedAnimationFile.Animations[Instance.ViewModel.SelectedAnimationIndex].Frames.Add(importFrame); 
            }
        }

        public void ExportFrame()
        {
            if (Instance.ViewModel.LoadedAnimationFile == null || Instance.FramesList.SelectedItem == null) return;
            var fd = new SaveFileDialog();
            fd.DefaultExt = "*.frame";
            fd.Filter = "RSDK Frame Files|*.frame";
            if (fd.ShowDialog() == true)
            {
                Instance.ViewModel.LoadedAnimationFile.Animations[Instance.ViewModel.SelectedAnimationIndex].Frames[Instance.ViewModel.SelectedFrameIndex].ExportTo(EngineType.RSDKv5, fd.FileName);
            }
        }



        #endregion

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
