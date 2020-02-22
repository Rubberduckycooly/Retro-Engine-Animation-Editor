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
using System.Windows.Shapes;

namespace AnimationEditor.Pages
{
    /// <summary>
    /// Interaction logic for AddWorkspaceDialog.xaml
    /// </summary>
    public partial class AddWorkspaceDialog : Window
    {
        private int SelectedFormatIndex { get; set; }
        public string SelectedFormat
        {
            get
            {
                string type = "?";
                switch (SelectedFormatIndex)
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
                    default:
                        type = "RSDKv5";
                        break;
                }
                return type;
            }
        }
        public string WorkspaceName
        {
            get
            {
                return WorkspaceNameTextbox.Text;
            }
        }
        public AddWorkspaceDialog()
        {
            InitializeComponent();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void OK_Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var radioButton = (sender != null ? (sender as RadioButton) : null);
            if (sender != null && radioButton != null)
            {
                if (radioButton.Tag != null)
                {
                    int.TryParse(radioButton.Tag.ToString(), out int result);
                    SelectedFormatIndex = result;
                }
            }
        }
    }
}
