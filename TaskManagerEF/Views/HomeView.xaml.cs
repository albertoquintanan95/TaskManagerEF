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
using TaskManagerEF.Controllers;
using TaskManagerEF.Models;
using System.DirectoryServices.AccountManagement;

namespace TaskManagerEF.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        MembersController MC = new MembersController();

        public HomeView()
        {
            InitializeComponent();
        }

        private void PrincipalGrid_Loaded(object sender, RoutedEventArgs e)
        {
            //Declaring the domain context to use the active directory
            PrincipalContext context = new PrincipalContext(ContextType.Domain);
            UserPrincipal UP = UserPrincipal.FindByIdentity(context, Environment.UserName);

            //We generate a new 'Member' instance, and asign the founded AD member 
            Member M = new Member { firstName = UP.GivenName, lastName = UP.Surname, displayName = UP.DisplayName, email = UP.DisplayName, netID = UP.SamAccountName };

            //Here we validate if the member exist in our database, if it doesn't exist, add it
            if (MC.SearchMember(M) == null)
            {
                MC.AddNewMember(M);
            }

            Welcome.Text = "Welcome to ATM " + M.displayName;
        }

        //Event when GoBack is clicked
        private void Go_Back(object sender, RoutedEventArgs e)
        {
            //Main panel will hide and projectPanel will show itself, the searchbox will be cleaned
            MainPanel.Visibility = Visibility.Visible;
            projectsPanel.Visibility = Visibility.Hidden;
            txtSearch.Text = "";
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addProjInfo = new AddProjectInformation();
            Content = addProjInfo; //navigate between user controls
        }

        private void txtSearch_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Search();
            }
        }

        private void txtSearch_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            txtSearch.Text = ""; //clean the search texbox
        }

        /// Function to navigate when dynamic buttons are created
        protected void Move(object sender, EventArgs e)
        {
            var ProjectView = new ProjectView();
            Content = ProjectView;

            //Assign the name of the button clicked to a global variable
            string project = ((Button)sender).Tag.ToString();
            GlobalVariables.projecViewNav = project;
        }

        //In this method we create a new project object and call the search method to look for projects
        private void Search()
        {
            //At first, we hide the main grid and enable the projectPanel, where the found projects will display
            MainPanel.Visibility = Visibility.Hidden;
            projectsPanel.IsEnabled = true;
            projectsPanel.Visibility = Visibility.Visible;
            //Calling this function, we clean the projects panel, so each time a new search is made, the panel will not have 
            //the results from the previous searches
            ManagePanels.ClearPanel(projectsPanel);

            //And here, we create a projects object to manage the methods and a buttons list to record all the found proects
            ProjectsController PC = new ProjectsController();
            List<Button> btns = PC.SearchProjects(txtSearch.Text);

            //Create the go back button from the project class
            Button btnGoBack = PC.CreateButton("Go Back", "Images/icons8-go-back-96.png");
            btnGoBack.Click += new RoutedEventHandler(Go_Back);
            projectsPanel.Children.Add(btnGoBack);

            if (btns != null)
            {
                //Once the search is performed, now we add an event to each founded button
                foreach (Button btn in btns)
                {
                    projectsPanel.Children.Add(btn);
                    btn.Click += new RoutedEventHandler(Move);
                }
            }
            else
            {
                var home = new HomeView();
                Content = home;
            }
        }
    }
}
