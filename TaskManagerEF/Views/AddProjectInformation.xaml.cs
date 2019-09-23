using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using TaskManagerEF.Controllers;
using TaskManagerEF.Models;

namespace TaskManagerEF.Views
{
    /// <summary>
    /// Interaction logic for AddProjectInformation.xaml
    /// </summary>
    public partial class AddProjectInformation : UserControl
    {
        ProjectsController PC = new ProjectsController();
        MembersController MC = new MembersController();
        CommentsController CC = new CommentsController();
        MetroWindow metroWindow = (Application.Current.MainWindow as MetroWindow);

        public AddProjectInformation()
        {
            InitializeComponent();
        }

        private async void btnSend_Click(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(
               // TextPointer to the start of content in the RichTextBox.
               rtbDescription.Document.ContentStart,
               // TextPointer to the end of content in the RichTextBox.
               rtbDescription.Document.ContentEnd
           );


            if (textRange.Text != "" && textRange.Text != "\r\n" && txtName.Text != "" && cbArea.Text != "")
            {
                Member M = MC.SearchMember(Environment.UserName);

                Project P = PC.AddProject(new Project { projectName = txtName.Text, projectDescription = textRange.Text, startDate = DateTime.Now, area = cbArea.Text }, M);
                CC.AddProjectComment(P.idProject, "The project was created. ", M.idMember);

                GlobalVariables.projecViewNav = P.projectName;
                txtName.Text = "";
                rtbDescription.SelectAll();
                rtbDescription.Selection.Text = "";
                cbArea.Text = "";

                var ProjectView = new ProjectView();
                this.Content = ProjectView;
            }
            else
            {
                await metroWindow.ShowMessageAsync("Attention", "Fill all the fields first");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            cbArea.Text = "";
            txtName.Text = "";
            rtbDescription.SelectAll();
            rtbDescription.Selection.Text = "";

            //delete everything and go back to add projects view
            var Home = new HomeView();
            this.Content = Home;
        }
    }
}
