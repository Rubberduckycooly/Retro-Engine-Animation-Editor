using AnimationEditor.ViewModel;
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
using GenerationsLib.Core;
using AnimationEditor.Classes;
using AnimationEditor.Methods;
using AnimationEditor.Pages;
using RecentFile = AnimationEditor.Classes.Settings.Instance.RecentFile;
using Workspace = AnimationEditor.Classes.Settings.Instance.Workspace;

namespace AnimationEditor.Methods
{
    public class FileHandler
    {
        private MainWindow Instance;
        public System.Collections.Generic.IList<MenuItem> RecentItems;
        public System.Collections.Generic.IList<MenuItem> WorkspaceItems;


        private String[] FilterOpen = new string[] 
        { 
            "RSDKv5 (Sonic Mania) Animation Files|*.bin|", 
            "RSDKvB (Sonic 1 & 2) Animation Files |*.ani|", 
            "RSDKv2 (Sonic CD) Animation Files|*.ani|", 
            "RSDKv1 (Sonic Nexus) Animation Files|*.ani|", 
            "RSDKvRS (Retro-Sonic) Animation Files|*.ani" 
        };

        public FileHandler(MainWindow window)
        {
            Instance = window;
            RecentItems = new List<MenuItem>();
            WorkspaceItems = new List<MenuItem>();
        }

        #region Open File Methods
        public void OpenFile(Workspace workspace = null)
        {
            Instance.Interfacer.PreventIndexUpdate = true;
            UnloadAnimationData();
            var fd = new OpenFileDialog();
            fd.DefaultExt = "*.bin";
            fd.Filter = string.Join("", FilterOpen);
            GenerateShortcuts();
            fd.CustomPlaces.Add(new FileDialogCustomPlace(Services.PathService.GetShortcutsPath()));
            if (workspace != null)
            {
                fd.InitialDirectory = workspace.Path;
                fd.FilterIndex = GetFilterIndexFromTypeString(workspace.DefaultFormat);
            }
            if (fd.ShowDialog() == true)
            {
                AddRecentDataFolder(fd.FileName, fd.FileName, GetTypeStringFromFilterIndex(fd.FilterIndex));
                LoadFile(fd);
            }
            Instance.Interfacer.PreventIndexUpdate = false;

            if (workspace != null)
            {
                Instance.Interfacer.UpdateControls();
                Instance.Interfacer.UpdateCanvasVisual();
            }
        }

        public void OpenFile(string file, EngineType type = EngineType.Invalid)
        {
            Instance.Interfacer.PreventIndexUpdate = true;
            UnloadAnimationData();
            LoadFile(file, type);
            UpdateRecentsDropDown("","","");
            Instance.Interfacer.PreventIndexUpdate = false;
            Instance.Interfacer.UpdateControls();
            Instance.Interfacer.UpdateCanvasVisual();
        }
        #endregion

        #region Load File Methods

        private static string GetFilenameAndFolder(string path)
        {
            string dir = "";
            string pth = Path.GetFileName(path);
            string tmp = path.Replace(pth, "");
            DirectoryInfo di = new DirectoryInfo(tmp);
            dir = di.Name;
            string displayPath = dir + "/" + pth;
            return displayPath;
        }

        public void LoadFile(string filepath, EngineType type = EngineType.Invalid)
        {
            try
            {
                Instance.AnimationType = type;
                Instance.ViewModel.LoadedAnimationFile = new EditorAnimation(Instance.AnimationType);
                Instance.ViewModel.AnimationFilepath = filepath;
                Instance.WindowName = Instance.DefaultWindowName + " - " + GetFilenameAndFolder(filepath);
                Instance.ViewModel.LoadedAnimationFile.LoadFrom((type != EngineType.Invalid ? type : Instance.AnimationType), filepath);
                LoadAnimationTextures(filepath);
            }
            catch (Exception ex)
            {
                RSDKrU.MessageBox.Show(ex.ToString());
                UnloadAnimationData();
            }

        }

        public void LoadFile(OpenFileDialog fd)
        {
            EngineType engineType = GetTypeFromFilterIndex(fd.FilterIndex);
            Instance.AnimationType = engineType;
            LoadFile(fd.FileName, engineType);
        }
        #endregion

        EngineType GetTypeFromFilterIndex(int index)
        {
            EngineType type = EngineType.Invalid;
            switch (index - 1)
            {
                case 0:
                    type = EngineType.RSDKv5;
                    break;
                case 1:
                    type = EngineType.RSDKvB;
                    break;
                case 2:
                    type = EngineType.RSDKv2;
                    break;
                case 3:
                    type = EngineType.RSDKv1;
                    break;
                case 4:
                    type = EngineType.RSDKvRS;
                    break;
            }
            return type;
        }

        int GetFilterIndexFromTypeString(string type)
        {
            int index = 0;
            switch (type)
            {
                case "RSDKv5":
                    index = 1;
                    break;
                case "RSDKvB":
                    index = 2;
                    break;
                case "RSDKv2":
                    index = 3;
                    break;
                case "RSDKv1":
                    index = 4;
                    break;
                case "RSDKvRS":
                    index = 5;
                    break;
            }
            return index;
        }

        string GetTypeStringFromFilterIndex(int index)
        {
            string type = "?";
            switch (index - 1)
            {
                case 0:
                    type = "RSDKv5";
                    break;
                case 1:
                    type = "RSDKvB";
                    break;
                case 2:
                    type = "RSDKv2";
                    break;
                case 3:
                    type = "RSDKv1";
                    break;
                case 4:
                    type = "RSDKvRS";
                    break;
            }
            return type;
        }

        #region Save File Methods
        public void SaveFile()
        {
            if (Instance.ViewModel.AnimationFilepath != null) Instance.ViewModel.LoadedAnimationFile.SaveTo(Instance.AnimationType, Instance.ViewModel.AnimationFilepath);
            else SaveFileAs();
        }

        public void SaveFileAs()
        {

            var fd = new SaveFileDialog();
            fd.DefaultExt = "*.bin";
            fd.Filter = string.Join("", FilterOpen);
            GenerateShortcuts();
            fd.CustomPlaces.Add(new FileDialogCustomPlace(Services.PathService.GetShortcutsPath()));
            if (fd.ShowDialog() == true)
            {
                UpdateRecentsDropDown(fd.FileName, fd.FileName, GetTypeStringFromFilterIndex(fd.FilterIndex));

                Instance.AnimationType = GetTypeFromFilterIndex(fd.FilterIndex);
                Instance.ViewModel.LoadedAnimationFile.SaveTo(Instance.AnimationType, fd.FileName);
            }
        }
        #endregion

        #region Unloading / Loading Methods

        public void UnloadAnimationData()
        {
            Instance.List.SelectedIndex = -1;
            Instance.FramesList.SelectedIndex = -1;
            InitlizeSpriteSheets(true);
            Instance.DataContext = new AnimationModel();
            InitlizeSpriteSheets();
            Instance.ViewModel.NullSpriteSheetList.Clear();
            Instance.Interfacer.IntilizePlayback(true);
            Instance.WindowName = Instance.DefaultWindowName;
        }

        public string GetImagePath(string path, string parentDirectory)
        {
            try
            {
                string result = Path.Combine(parentDirectory, Instance.ViewModel.LoadedAnimationFile.PathMod, path.Replace("/", "\\"));
                if (Instance.AnimationType == EngineType.RSDKvRS) result = result.Replace("Characters\\Characters", "Characters"); //Fix for RSDKvRS
                return result;
            }
            catch
            {
                return "<none>";
            }

        }

        public Tuple<BitmapImage, Color> LoadAnimationTexture(string fileName, bool transparent = false)
        {
            if (transparent)
            {
                FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                System.Drawing.Bitmap img = new System.Drawing.Bitmap(fileStream);
                fileStream.Close();
                var color = img.Palette.Entries[0];
                string hex = HexConverter(color);
                img.MakeTransparent(color);
                return new Tuple<BitmapImage, Color>((BitmapImage)BitmapConversion.ToWpfBitmap(img), (Color)ColorConverter.ConvertFromString(hex));
            }
            else
            {
                FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                var img = new System.Windows.Media.Imaging.BitmapImage();
                img.BeginInit();
                img.StreamSource = fileStream;
                img.EndInit();
                return new Tuple<BitmapImage, Color>(img, Colors.Black);
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
                string imagePath = GetImagePath(path, parentDirectory);



                bool result = false;
                if (imagePath == "<none>") result = false;
                else result = File.Exists(imagePath);


                if (result)
                {
                    var normalImage = LoadAnimationTexture(imagePath);
                    var transparentImage = LoadAnimationTexture(imagePath, true);
                    Instance.ViewModel.SpriteSheets.Add(new AnimationModel.Spritesheet(normalImage.Item1, transparentImage.Item1, transparentImage.Item2));
                }
                else
                {
                    Instance.ViewModel.SpriteSheets.Add(new AnimationModel.Spritesheet(new BitmapImage(), new BitmapImage(), true));
                    Instance.ViewModel.NullSpriteSheetList.Add(path);
                }

            }


            foreach(var spritesheet in Instance.ViewModel.SpriteSheets)
            {
                if (!spritesheet.isInvalid) spritesheet.isReady = true;
            }

        }

        public void LoadNewAnimationTexture(string imagePath)
        {
            var normalImage = LoadAnimationTexture(imagePath);
            var transparentImage = LoadAnimationTexture(imagePath, true);
            Instance.ViewModel.SpriteSheets.Add(new AnimationModel.Spritesheet(normalImage.Item1, transparentImage.Item1, transparentImage.Item2));
        }

        public void InitlizeSpriteSheets(bool clearMode = false)
        {
            if (clearMode)
            {
                if (Instance.ViewModel.SpriteSheets != null) Instance.ViewModel.SpriteSheets.Clear();
            }
            else
            {
                Instance.ViewModel.SpriteSheets = new System.Collections.Generic.List<AnimationModel.Spritesheet>();

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
                    var img = Instance.ViewModel.GetCroppedFrame(i);
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
                var importAnim = new EditorAnimation.EditorAnimationInfo(EngineType.RSDKv5, Instance.ViewModel.LoadedAnimationFile);
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
                var importFrame = new EditorAnimation.EditorFrame(EngineType.RSDKv5, Instance.ViewModel.SelectedAnimation);
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

        public void UpdateRecentsDropDown(string name, string filepath, string format)
        {
            if (name != "" && filepath != "" && format != "") AddRecentDataFolder(filepath, name, format);
            RefreshDataDirectories();
        }

        public void RecentDataDirectoryClicked(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as System.Windows.Controls.MenuItem;
            RecentFile dataDirectory = menuItem.Tag as RecentFile;
            var dataDirectories = AnimationEditor.Classes.Settings.Default.RecentFiles;
            if (File.Exists(dataDirectory.FilePath))
            {
                EngineType type = GetInputGestureTextEngineType(dataDirectory.Format);
                AddRecentDataFolder(dataDirectory.FilePath, dataDirectory.Name, dataDirectory.Format);
                OpenFile(dataDirectory.FilePath, type);
            }
            else
            {
                RSDKrU.MessageBox.Show($"The specified File {dataDirectory} is not valid.",
                                "Invalid Annimation File!",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                dataDirectories.Remove(dataDirectory);
                RefreshDataDirectories();

            }
            AnimationEditor.Classes.Settings.Save();
        }

        public void RefreshDataDirectories()
        {
            List<AnimationEditor.Classes.Settings.Instance.RecentFile> recentDataDirectories = AnimationEditor.Classes.Settings.Default.RecentFiles;
            if (AnimationEditor.Classes.Settings.Default.RecentFiles?.Count > 0)
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

        private MenuItem CreateDataDirectoryMenuLink(RecentFile file)
        {
            MenuItem newItem = new MenuItem();
            newItem.Header = file.FilePath;
            newItem.InputGestureText = file.Format;
            newItem.Tag = file;
            newItem.Click += RecentDataDirectoryClicked;
            return newItem;
        }

        private EngineType GetInputGestureTextEngineType(string gesture)
        {
            switch (gesture)
            {
                case "RSDKv5":
                    return EngineType.RSDKv5;
                case "RSDKvB":
                    return EngineType.RSDKvB;
                case "RSDKv2":
                    return EngineType.RSDKv2;
                case "RSDKv1":
                    return EngineType.RSDKv1;
                case "RSDKvRS":
                    return EngineType.RSDKvRS;
                case "?":
                    return DetermineVersionManually();
                default:
                    return EngineType.Invalid;
            }

            EngineType DetermineVersionManually()
            {
                MessageBoxResult result = RSDKrU.MessageBox.ShowYesNoCancel("Which version is this animation file for?", "Help Me!", "RSDKv2/RSDKvB (CD/1/1)", "RSDKv1 (Sonic Nexus)", "RSDKvRS (Retro Sonic)");
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        MessageBoxResult result2 = RSDKrU.MessageBox.ShowYesNo("Which version is this animation file for?", "Help Me!", "RSDKv2 (Sonic CD)", "RSDKvB (Sonic 1/2 2013)");
                        if (result2 == MessageBoxResult.Yes)
                            return EngineType.RSDKv2;
                        else return EngineType.RSDKvB;
                    case MessageBoxResult.No:
                        return EngineType.RSDKv1;
                    case MessageBoxResult.Cancel:
                        return EngineType.RSDKvRS;
                    default:
                        return EngineType.Invalid;
                }
            }
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
            //else
            //{
                //if (Instance.AnimationType == EngineType.RSDKvB)
                //{
                //    return "RSDKvB";
                //}
                //else if (Instance.AnimationType == EngineType.RSDKv2)
                //{
                //    return "RSDKv2";
                //}
                //else if (Instance.AnimationType == EngineType.RSDKvRS)
                //{
                //    return "RSDKvRS";
                //}
                else return "?";

            //}
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

            for (int i = 0; i < AnimationEditor.Classes.Settings.Default.RecentFiles.Count; i++)
            {
                if (File.Exists(AnimationEditor.Classes.Settings.Default.RecentFiles[i].FilePath)) continue;
                else ItemsForRemoval.Add(AnimationEditor.Classes.Settings.Default.RecentFiles[i].FilePath);
            }
            foreach(string item in ItemsForRemoval)
            {
                AnimationEditor.Classes.Settings.Default.RecentFiles.RemoveAll(x => x.FilePath == item);
            }

            RecentItems.Clear();
        }
        public void AddRecentDataFolder(string dataDirectory, string name, string format)
        {
            try
            {

                if (AnimationEditor.Classes.Settings.Default.RecentFiles == null)
                {
                    AnimationEditor.Classes.Settings.Default.RecentFiles = new List<AnimationEditor.Classes.Settings.Instance.RecentFile>();
                }

                if (AnimationEditor.Classes.Settings.Default.RecentFiles.Exists(x => x.FilePath == dataDirectory))
                {
                    AnimationEditor.Classes.Settings.Default.RecentFiles.RemoveAll(x => x.FilePath == dataDirectory);
                }

                if (AnimationEditor.Classes.Settings.Default.RecentFiles.Count >= 10)
                {
                    for (int i = 9; i < AnimationEditor.Classes.Settings.Default.RecentFiles.Count; i++)
                    {
                        AnimationEditor.Classes.Settings.Default.RecentFiles.RemoveAt(i);
                    }
                }

                AnimationEditor.Classes.Settings.Instance.RecentFile recentFile = new Settings.Instance.RecentFile(name, dataDirectory, format);

                AnimationEditor.Classes.Settings.Default.RecentFiles.Insert(0, recentFile);

                AnimationEditor.Classes.Settings.Save();
                AnimationEditor.Classes.Settings.Reload();

                RefreshDataDirectories();


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Failed to add data folder to recent list: " + ex);
            }
        }

        #endregion

        #region Saved Workspaces

        private bool IsWorkspaceOnRemoveMode { get; set; } = false;
        public void WorkspaceFolderClicked(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as System.Windows.Controls.MenuItem;
            Workspace directory = menuItem.Tag as Workspace;

            if (IsWorkspaceOnRemoveMode)
            {
                AnimationEditor.Classes.Settings.Default.Workspaces.Remove(directory);
                CleanUpWorkspaceList();
                RefreshWorkspaces();
            }
            else
            {
                OpenFile(directory);
            }

            AnimationEditor.Classes.Settings.Save();
        }
        public void RefreshWorkspaces()
        {
            List<Workspace> recentDataDirectories = AnimationEditor.Classes.Settings.Default.Workspaces;
            if (AnimationEditor.Classes.Settings.Default.Workspaces?.Count > 0)
            {
                Instance.NoRecentFiles2.Visibility = Visibility.Collapsed;
                CleanUpWorkspaceList();

                var startRecentItems = Instance.MenuFileOpenFromWorkspace.Items.IndexOf(Instance.NoRecentFiles2);

                foreach (var dataDirectory in recentDataDirectories)
                {
                    WorkspaceItems.Add(CreateWorkSpaceMenuLink(dataDirectory));
                }



                foreach (MenuItem menuItem in WorkspaceItems.Reverse())
                {
                    Instance.MenuFileOpenFromWorkspace.Items.Insert(startRecentItems, menuItem);
                }
            }
            else
            {
                Instance.NoRecentFiles2.Visibility = Visibility.Visible;
            }
        }
        private MenuItem CreateWorkSpaceMenuLink(Workspace directory)
        {
            MenuItem newItem = new MenuItem();
            newItem.Header = directory.Name;
            newItem.InputGestureText = directory.DefaultFormat;
            newItem.Tag = directory;
            newItem.StaysOpenOnClick = IsWorkspaceOnRemoveMode;
            newItem.Click += WorkspaceFolderClicked;
            return newItem;
        }
        private void CleanUpWorkspaceList()
        {
            foreach (var menuItem in WorkspaceItems)
            {
                menuItem.Click -= WorkspaceFolderClicked;
                Instance.MenuFileOpenFromWorkspace.Items.Remove(menuItem);
            }

            WorkspaceItems.Clear();
        }
        public void AddWorkspaceFolder(Workspace workspace)
        {
            try
            {
                if (AnimationEditor.Classes.Settings.Default.Workspaces == null)
                {
                    AnimationEditor.Classes.Settings.Default.Workspaces = new List<Workspace>();
                }

                AnimationEditor.Classes.Settings.Default.Workspaces.Insert(0, workspace);

                AnimationEditor.Classes.Settings.Save();
                AnimationEditor.Classes.Settings.Reload();

                RefreshWorkspaces();


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Failed to add data folder to recent list: " + ex);
            }
        }
        public void SelectNewWorkspaceFolder()
        {
            GenerationsLib.Core.FolderSelectDialog fsd = new GenerationsLib.Core.FolderSelectDialog();
            if (fsd.ShowDialog())
            {
                AddWorkspaceDialog awd = new AddWorkspaceDialog();
                awd.Owner = Instance;
                if (awd.ShowDialog() == true)
                {
                    Workspace workspace = new Workspace(awd.WorkspaceName, fsd.FileName, awd.SelectedFormat);
                    AddWorkspaceFolder(workspace);
                }

            }
        }
        public void ToggleWorkspaceRemoveMode(bool isEnabled)
        {
            if (isEnabled)
            {
                IsWorkspaceOnRemoveMode = true;
                Instance.MenuFileOpenFromWorkspaceAddWorkspace.IsEnabled = false;
                var textblock = new TextBlock();
                string text = "(Click the workspaces you want to remove)" + Environment.NewLine + "(Click Here to Exit Remove Mode)";
                textblock.TextWrapping = TextWrapping.Wrap;
                textblock.Text = text;
                Instance.MenuFileOpenFromWorkspaceRemoveMode.Header = textblock;
                foreach (var entry in WorkspaceItems)
                {
                    entry.StaysOpenOnClick = true;
                }
            }
            else
            {
                IsWorkspaceOnRemoveMode = false;
                Instance.MenuFileOpenFromWorkspaceAddWorkspace.IsEnabled = true;
                Instance.MenuFileOpenFromWorkspaceRemoveMode.Header = "Remove Workspaces...";
                foreach (var entry in WorkspaceItems)
                {
                    entry.StaysOpenOnClick = false;
                }
            }
        }

        public void GenerateShortcuts()
        {
            string directory = Services.PathService.GetShortcutsPath();
            foreach (var entry in Directory.EnumerateFiles(directory))
            {
                File.Delete(entry);
            }
            foreach (var workspace in Classes.Settings.Default.Workspaces)
            {
                var wsh = new IWshRuntimeLibrary.IWshShell_Class();
                IWshRuntimeLibrary.IWshShortcut shortcut = wsh.CreateShortcut(directory + string.Format("\\{0}.lnk", workspace.Name)) as IWshRuntimeLibrary.IWshShortcut;
                shortcut.TargetPath = workspace.Path;
                shortcut.Save();
            }
        }

        #endregion

    }

}
