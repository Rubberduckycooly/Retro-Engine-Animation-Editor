using AnimationEditor.ViewModel;
using System;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

namespace AnimationEditor.Pages
{
    /// <summary>
    /// Interaction logic for TextureWindow.xaml
    /// </summary>
    public partial class TextureManager : UserControl
    {
        private MainWindow ParentInstance;
        private List<BitmapImage> SpriteSheets;
        private List<string> Textures { get; set; }
        private int SelectedTextureIndex { get; set; }
        private int SelectedValue { get; set; }
        private BitmapImage CurrentTexture { get; set; }

        public TextureManager()
        {
            InitializeComponent();
            Border.BorderBrush = SystemParameters.WindowGlassBrush;
        }

        public void Startup(MainWindow instance)
        {
            ParentInstance = instance;
            if (ParentInstance != null && ParentInstance.ViewModel != null && ParentInstance.ViewModel.LoadedAnimationFile != null)
            {
                InitializeVarriables();
                UpdateUI();
                SelectedTextureIndex = 0;

                ButtonRemove.IsEnabled = true;
                ButtonAdd.IsEnabled = true;
                ButtonChange.IsEnabled = true;
                ListTextures.IsEnabled = true;
            }
            else
            {
                ButtonRemove.IsEnabled = false;
                ButtonAdd.IsEnabled = false;
                ButtonChange.IsEnabled = false;
                ListTextures.IsEnabled = false;
            }
        }

        public void Shutdown()
        {
            ParentInstance = null;
            ButtonRemove.IsEnabled = true;
            ButtonAdd.IsEnabled = true;
            ButtonChange.IsEnabled = true;
            ListTextures.IsEnabled = true;
        }

        public void InitializeVarriables()
        {
            Textures = new List<string>();
            SpriteSheets = new List<BitmapImage>();
            CurrentTexture = new BitmapImage();
            Textures = ParentInstance.ViewModel.SpriteSheetPaths;
            ListTextures.ItemsSource = Textures;
        }

        public void UpdateUI()
        {
            CurrentTexture = ParentInstance.ViewModel.SpriteSheets[SelectedTextureIndex].Image;
            if (CurrentTexture != null && !ParentInstance.ViewModel.NullSpriteSheetList.Contains(ParentInstance.ViewModel.SpriteSheetPaths[SelectedTextureIndex]))
            {
                SizeText.Text = string.Format("Size: {0} x {1}", CurrentTexture.Width, CurrentTexture.Height);
                ImageTexture.Source = CurrentTexture;
            }
            else
            {
                SizeText.Text = string.Format("Size: {0} x {1}", "NULL", "NULL");
                ImageTexture.Source = null;
            }
            ListTextures.SelectedIndex = SelectedTextureIndex;

        }

        public void RemoveTexture(int index)
        {
            ParentInstance.ViewModel.SpriteSheetPaths.RemoveAt(index);
            ParentInstance.ViewModel.SpriteSheets.RemoveAt(index);

            SelectedTextureIndex = 0;
            if (ParentInstance.ViewModel.CurrentFrame_SpriteSheet == index) ParentInstance.ViewModel.CurrentFrame_SpriteSheet = 0;
            if (ParentInstance.SpriteSheetList.SelectedIndex == index) ParentInstance.SpriteSheetList.SelectedIndex = 0;

            foreach (var list in ParentInstance.ViewModel.LoadedAnimationFile.Animations)
            {
                foreach (var frame in list.Frames)
                {
                    if (frame.SpriteSheet == index)
                    {
                        frame.SpriteSheet = 0;
                    }
                }
            }


            InitializeVarriables();
            UpdateUI();
            ParentInstance.Interfacer.UpdateLoadedAnimationTextureListProperties();
            ListTextures.ItemsSource = null;
            ListTextures.ItemsSource = ParentInstance.ViewModel.SpriteSheetPaths;
            ListTextures.SelectedItem = ParentInstance.ViewModel.SpriteSheetPaths[0];

        }

        public void ReplaceTexture(int index)
        {
            string parentDirectory = ParentInstance.ViewModel.SpriteDirectory;
            var fd = new OpenFileDialog();
            fd.DefaultExt = "*.gif";
            fd.Filter = "GIF Files | *.gif";
            fd.Title = "Add a Spritesheet...";
            fd.InitialDirectory = parentDirectory;
            if (fd.ShowDialog() == true)
            {
                string selectedImage = fd.FileName;
                if (!fd.FileName.Contains(parentDirectory))
                {
                    MessageBox.Show("You can not add a spritesheet outside of the parent folder of the animation, please use an spritesheet within " + string.Format("{0}", parentDirectory), "Unable to add Spritesheet", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var image = ParentInstance.Handler.LoadAnimationTexture(selectedImage);
                bool widthPowerOf2 = IsPowerOfTwo(Convert.ToUInt16(image.Item1.Width));
                bool heightPowerOf2 = IsPowerOfTwo(Convert.ToUInt16(image.Item1.Height));
                if (!heightPowerOf2 || !widthPowerOf2)
                {
                    var result = MessageBox.Show("Your spritesheet's width and/or height has a value that is not a power of two. It is not recommended that you use a spritesheet with \"non-power-of-two\" sizes, as it will make the sprites look disorted in-game. Do you still wish to continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.No) return;
                }

                string modifiedPath = selectedImage.Replace(parentDirectory, "").Replace("\\", "/");
                if (modifiedPath[0] == '/') modifiedPath = modifiedPath.Remove(0, 1);

                ParentInstance.ViewModel.LoadedAnimationFile.SpriteSheets.RemoveAt(index);
                ParentInstance.ViewModel.LoadedAnimationFile.SpriteSheets.Insert(index, modifiedPath);

                var normalTexture = ParentInstance.Handler.LoadAnimationTexture(selectedImage, false);
                var transparentTexture = ParentInstance.Handler.LoadAnimationTexture(selectedImage, true);



                ParentInstance.ViewModel.SpriteSheets.RemoveAt(index);
                ParentInstance.ViewModel.SpriteSheets.Insert(index, new AnimationModel.Spritesheet(normalTexture.Item1, transparentTexture.Item1, transparentTexture.Item2));
            }

            InitializeVarriables();
            UpdateUI();
            ListTextures.ItemsSource = null;
            ListTextures.ItemsSource = ParentInstance.ViewModel.SpriteSheetPaths;
            ListTextures.SelectedItem = ParentInstance.ViewModel.SpriteSheetPaths[0];
            ParentInstance.TextureManagerPopup.IsOpen = true;
            ParentInstance.Interfacer.UpdateControls();
        }

        public void AddTexture()
        {
            string parentDirectory = ParentInstance.ViewModel.SpriteDirectory;
            var fd = new OpenFileDialog();
            fd.DefaultExt = "*.gif";
            fd.Filter = "GIF Files | *.gif";
            fd.Title = "Add a Spritesheet...";
            fd.InitialDirectory = parentDirectory;
            if (fd.ShowDialog() == true)
            {
                string selectedImage = fd.FileName;
                if (!fd.FileName.Contains(parentDirectory))
                {
                    MessageBox.Show("You can not add a spritesheet outside of the parent folder of the animation, please use an spritesheet within " + string.Format("{0}", parentDirectory), "Unable to add Spritesheet", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var image = ParentInstance.Handler.LoadAnimationTexture(selectedImage);
                var transparentimage = ParentInstance.Handler.LoadAnimationTexture(selectedImage, true);


                bool widthPowerOf2 = IsPowerOfTwo(Convert.ToUInt16(image.Item1.Width));
                bool heightPowerOf2 = IsPowerOfTwo(Convert.ToUInt16(image.Item1.Height));
                if (!heightPowerOf2 || !widthPowerOf2)
                {
                    var result = MessageBox.Show("Your spritesheet's width and/or height has a value that is not a power of two. It is not recommended that you use a spritesheet with \"non-power-of-two\" sizes, as it will make the sprites look disorted in-game. Do you still wish to continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.No) return;
                }

                string modifiedPath = selectedImage.Replace(parentDirectory, "").Replace("\\", "/");
                if (modifiedPath[0] == '/') modifiedPath = modifiedPath.Remove(0, 1);

                ParentInstance.ViewModel.LoadedAnimationFile.SpriteSheets.Add(modifiedPath);
                var sheet = new AnimationModel.Spritesheet(image.Item1, transparentimage.Item1, transparentimage.Item2);
                sheet.isReady = true;
                ParentInstance.ViewModel.SpriteSheets.Add(sheet);
            }

            InitializeVarriables();
            UpdateUI();
            ListTextures.ItemsSource = null;
            ListTextures.ItemsSource = ParentInstance.ViewModel.SpriteSheetPaths;
            ListTextures.SelectedItem = ParentInstance.ViewModel.SpriteSheetPaths[0];
            ParentInstance.TextureManagerPopup.IsOpen = true;
            ParentInstance.Interfacer.UpdateControls();
        }

        private void ListTextures_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ListTextures.SelectedIndex != -1)
            {
                SelectedTextureIndex = ListTextures.SelectedIndex;
                UpdateUI();
            }

        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            int Index = SelectedTextureIndex;
            if (Textures.Count > 1)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to remove this spritesheet? Any frames currently using this spritesheet will be defaulted to the first avaliable spritesheet.", "Remove Spritesheet", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    RemoveTexture(Index);
                }
            }
            else
            {
                MessageBox.Show("You can not remove the base spritesheet, add another spritesheet or replace this one if you wish to remove it.", "Unable to remove Spritesheet", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddTexture();
        }

        bool IsPowerOfTwo(ulong x)
        {
            return (x & (x - 1)) == 0;
        }

        private void ButtonChange_Click(object sender, RoutedEventArgs e)
        {
            int Index = SelectedTextureIndex;            
            if (Textures.Count > 1)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to replace this spritesheet? Any frames currently using this spritesheet will use the replacement.", "Replace Spritesheet", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    ReplaceTexture(Index);
                }
            }
        }
    }
}
