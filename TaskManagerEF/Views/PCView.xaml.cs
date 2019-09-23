using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using TaskManagerEF.Controllers;

namespace TaskManagerEF.Views
{
    /// <summary>
    /// Interaction logic for PCView.xaml
    /// Load PC projects 
    /// by Aberto Quintana
    /// Version 1.0
    /// Hours wasted here = 4
    /// </summary>
    public partial class PCView : UserControl
    {
        public PCView()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            var AddProjView = new AddProjectsView();
            Content = AddProjView; //navigate between windows
        }

        protected void Move(object sender, EventArgs e)
        {
            var ProjectView = new ProjectView();
            Content = ProjectView;

            //asign the name of the button clicked to a global variable
            string project = ((Button)sender).Tag.ToString();
            GlobalVariables.projecViewNav = project;
        }

        protected void AddNew(object sender, EventArgs e)
        {
            var AddProject = new AddProjectInformation();
            Content = AddProject;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            txbArea.Text = GlobalVariables.projectArea; //get the area to be consulted from the previous user control
                                                        //And here, we create a projects object to manage the methods and a buttons list to record all the found proects
            ProjectsController PC = new ProjectsController();
           
            //Once the search is performed, now we add an event to each founded button
            foreach (Button btn in PC.SearchProjectsByArea(txbArea.Text))
            {
                PCPanel.Children.Add(btn);
                btn.Click += new RoutedEventHandler(Move);
            }
        }
    }
}
