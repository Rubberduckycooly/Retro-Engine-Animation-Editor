using AnimationEditor.ViewModels;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

namespace AnimationEditor
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
            CurrentTexture = ParentInstance.ViewModel.SpriteSheets[SelectedTextureIndex];
            SizeText.Text = string.Format("Size: {0} x {1}", CurrentTexture.Width, CurrentTexture.Height);
            ImageTexture.Source = CurrentTexture;
            ListTextures.SelectedIndex = SelectedTextureIndex;
        }

        public void RemoveTexture(int index)
        {
            ParentInstance.ViewModel.SpriteSheetPaths.RemoveAt(index);
            ParentInstance.ViewModel.SpriteSheets.RemoveAt(index);

            SelectedTextureIndex = 0;
            if (ParentInstance.ViewModel.CurrentSpriteSheet == index) ParentInstance.ViewModel.CurrentSpriteSheet = 0;
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
            ListTextures.ItemsSource = ParentInstance.ViewModel.SpriteSheetPaths;
            ListTextures.SelectedItem = ParentInstance.ViewModel.SpriteSheetPaths[0];

        }

        public void ReplaceTexture(int index)
        {

        }

        public void AddTexture()
        {

        }

        private void ListTextures_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SelectedTextureIndex = ListTextures.SelectedIndex;
            UpdateUI();
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

        }

        private void ButtonChange_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
