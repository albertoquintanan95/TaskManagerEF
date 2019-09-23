using System.Windows;
using System.Windows.Controls;

namespace TaskManagerEF.Views
{
    /// <summary>
    /// Interaction logic for AddProjectsView.xaml
    /// </summary>
    public partial class AddProjectsView : UserControl
    {
        

        public AddProjectsView()
        {
            InitializeComponent();
        }

        private void btnPC_Click(object sender, RoutedEventArgs e)
        {
            GlobalVariables.projectArea = "PC";
            var PCView = new PCView();
            Content = PCView; //navigate between windows
        }

        private void btnSCM_Click(object sender, RoutedEventArgs e)
        {
            GlobalVariables.projectArea = "SCM";
            var PCView = new PCView();
            Content = PCView; //navigate between windows
        }
    }
}
