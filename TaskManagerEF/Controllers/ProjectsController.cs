using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TaskManagerEF.Models;

namespace TaskManagerEF.Controllers
{
    class ProjectsController
    {
        TaskManagerEntities_DB _db = new TaskManagerEntities_DB();

        public Project SearchProject(int idProject)
        {
            return _db.Projects.Find(idProject);
        }

        public Project SearchProject(string projectName)
        {
            return _db.Projects.Where(x => x.projectName == projectName).FirstOrDefault();
        }

        public List<Button> SearchProjects(int idProject)
        {
            List<Button> Btns = new List<Button>();
            foreach (Project P in _db.Projects.Where(x => x.idProject == idProject).ToList())
            {
                Btns.Add(CreateButton(P.projectName, "Images/icons8-task-96.png"));
            }
            return Btns;
        }

        public List<Button> SearchProjects(string projectName)
        {
            List<Button> Btns = new List<Button>();
            foreach (Project P in _db.Projects.Where(x => x.projectName.Contains(projectName)).ToList())
            {
                Btns.Add(CreateButton(P.projectName, "Images/icons8-task-96.png"));
            }
            return Btns;
        }

        public List<Button> SearchProjectsByArea(string projectArea)
        { 
            List<Button> Btns = new List<Button>();
            foreach (Project P in _db.Projects.Where(x => x.area == projectArea).ToList())
            {
                Btns.Add(CreateButton(P.projectName, "Images/icons8-task-96.png"));
            }
            return Btns;
        }

        public Project AddProject(Project P, Member M)
        {
            _db.Projects.Add(P);
            _db.ProjectMembers.Add(new ProjectMember { idProject = P.idProject, idMember = M.idMember });
            _db.SaveChanges();
            return P;
        }

        public bool AddProjectMember(Project P, Member M)//(int idProject, int idMember)
        {           
            if (_db.ProjectMembers.Where(x => x.idProject == P.idProject).Where(x => x.idMember == M.idMember).FirstOrDefault() == null)
            {
                P.ProjectMembers.Add(new ProjectMember { idProject = P.idProject, idMember = M.idMember });
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool MemberExist(int idProject, int idMember)
        {
            if (_db.ProjectMembersViews.Where(x => x.idProject == idProject).Where(x => x.idMember == idMember).FirstOrDefault() == null)
            {
                return false;
            }
            return true;
        }

        public List<ProjectMembersView> GetMembers(int idProject)
        {
            return _db.ProjectMembersViews.Where(x => x.idProject == idProject).ToList();
        }

        public Project UpdateDescription(Project P, string description)
        {
            P.projectDescription = description;
            _db.SaveChanges();
            return P;
        }

        public Project FinishProject(Project P)
        {
            P.endDate = DateTime.Now;
            _db.SaveChanges();
            return P;
        }

        public Project UnfinishProject(Project P)
        {
            P.endDate = null;
            _db.SaveChanges();
            return P;
        }

        public Button CreateButton(string name, string path)
        {
            //for each element in the reader, create a new button inside a wrappanel 
            //definitions
            Button btn = new Button();
            StackPanel spa = new StackPanel();
            TextBlock tb = new TextBlock();
            Image img = new Image();


            //bitmap for the image
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(path, UriKind.Relative);
            bi3.EndInit();

            //image properties
            img.Stretch = Stretch.Fill;
            img.Width = 100;
            img.Height = 100;
            img.HorizontalAlignment = HorizontalAlignment.Center;
            img.Source = bi3;
            img.VerticalAlignment = VerticalAlignment.Top;

            //textbox properties
            tb.Text = name;
            tb.FontSize = 12;
            tb.TextAlignment = TextAlignment.Center;


            //stack panel properties
            spa.Orientation = Orientation.Vertical;
            spa.HorizontalAlignment = HorizontalAlignment.Stretch;
            spa.Children.Add(img);
            spa.Children.Add(tb);

            //button properties
            btn.Width = 150;
            btn.Height = 180;
            btn.Content = spa;
            btn.Background = Brushes.White;
            btn.BorderThickness = new Thickness(0);
            var margin = btn.Margin;
            margin.Left = 5;
            margin.Right = 5;
            margin.Top = 5;
            margin.Bottom = 5;
            btn.Margin = margin;
            btn.Tag = tb.Text;

            return btn;
        }
    }
}
