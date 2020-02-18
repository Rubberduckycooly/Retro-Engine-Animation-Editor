using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnimationEditor.Pages
{
    /// <summary>
    /// Interaction logic for HitboxManager.xaml
    /// </summary>
    public partial class HitboxManager : UserControl
    {
        private MainWindow ParentInstance;
        private List<string> HitboxTypeItems { get; set; }
        private int SelectedIndex { get; set; }
        private string SelectedValue { get; set; }

        private bool TextboxUpdateLock = false;

        public HitboxManager()
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

                if (ParentInstance.ViewModel.LoadedAnimationFile.EngineType == Animation.Classes.EngineType.RSDKv5)
                {
                    ButtonRemove.IsEnabled = true;
                    ButtonAdd.IsEnabled = true;
                    SelectedHitboxTextbox.IsEnabled = true;
                    List.IsEnabled = true;
                }
                else
                {
                    ButtonRemove.IsEnabled = false;
                    ButtonAdd.IsEnabled = false;
                    SelectedHitboxTextbox.IsEnabled = false;
                    List.IsEnabled = false;
                }
            }
            else
            {
                ButtonRemove.IsEnabled = false;
                ButtonAdd.IsEnabled = false;
                SelectedHitboxTextbox.IsEnabled = false;
                List.IsEnabled = false;
            }
        }

        public void Shutdown()
        {
            ParentInstance = null;
            ButtonRemove.IsEnabled = true;
            ButtonAdd.IsEnabled = true;
            SelectedHitboxTextbox.IsEnabled = true;
            List.IsEnabled = true;
        }

        public void InitializeVarriables()
        {
            HitboxTypeItems = ParentInstance.ViewModel.Hitboxes;
            List.ItemsSource = HitboxTypeItems;
            UpdateSelectedHitboxTextbox();
            List.SelectedIndex = SelectedIndex;
        }

        public void UpdateSelectedHitboxTextbox()
        {
            TextboxUpdateLock = true;
            SelectedHitboxTextbox.Text = SelectedValue;
            TextboxUpdateLock = false;
        }

        public void UpdateUI()
        {
            List.ItemsSource = HitboxTypeItems;
            UpdateSelectedHitboxTextbox();
        }

        public void UpdateList()
        {
            int tempIndex = List.SelectedIndex;
            int textboxIndex = SelectedHitboxTextbox.CaretIndex;
            List.ItemsSource = null;
            HitboxTypeItems = ParentInstance.ViewModel.Hitboxes;
            List.ItemsSource = HitboxTypeItems;
            List.SelectedIndex = tempIndex;
            SelectedHitboxTextbox.CaretIndex = textboxIndex;
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedIndex = List.SelectedIndex;
            SelectedValue = List.SelectedValue as string;
            UpdateUI();
        }

        private void SelectedHitboxTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextboxUpdateLock || ParentInstance.ViewModel == null || ParentInstance.ViewModel.LoadedAnimationFile == null) return;
            HitboxTypeItems[SelectedIndex] = SelectedHitboxTextbox.Text;
            ParentInstance.ViewModel.Hitboxes = HitboxTypeItems;
            UpdateList();
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            ParentInstance.ViewModel.Hitboxes.RemoveAt(SelectedIndex);

            SelectedIndex = 0;

            foreach (var list in ParentInstance.ViewModel.LoadedAnimationFile.Animations)
            {
                foreach (var frame in list.Frames)
                {
                    if (frame.CollisionBox == SelectedIndex)
                    {
                        frame.CollisionBox = 0;
                    }
                }
            }

            UpdateList();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            ParentInstance.ViewModel.Hitboxes.Add("New Hitbox Type");

            SelectedIndex = 0;

            UpdateList();
        }
    }
}
