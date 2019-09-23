using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TaskManagerEF.Models;

namespace TaskManagerEF.Controllers
{
    class TasksController
    {
        TaskManagerEntities_DB _db = new TaskManagerEntities_DB();
        CommentsController CC = new CommentsController();

        public TaskMembersView GetMembers(int idTask)
        {
            return _db.TaskMembersViews.Where(x => x.idTask == idTask).FirstOrDefault();
        }

        public List<Button> GetProjectTasks(string ProjectName)
        {
            List<Button> Btns = new List<Button>();
            foreach (TasksView PT in _db.TasksViews.Where(x => x.projectName == ProjectName).ToList())
            {
                Btns.Add(CreateTaskButtons(PT.endDate, PT.status, PT.taskName));
            }
            return Btns;
        }

        public List<Button> GetTasks(string NetID)
        {
            List<Button> Btns = new List<Button>();
            foreach (TaskMembersView TMV in _db.TaskMembersViews.Where(x => x.netID == NetID).Where(x=>x.status != "Completed").ToList())
            {
                Btns.Add(CreateTaskButtons(TMV.endDate, TMV.status, TMV.taskName));
            }
            return Btns;
        }

        public List<Button> GetAllTasks(string NetID)
        {
            List<Button> Btns = new List<Button>();
            foreach (TaskMembersView TMV in _db.TaskMembersViews.Where(x => x.netID == NetID).ToList())
            {
                Btns.Add(CreateTaskButtons(TMV.endDate, TMV.status, TMV.taskName));
            }
            return Btns;
        }

        public TaskMembersView LoadTaskInfo(string TaskName)
        {
            return _db.TaskMembersViews.Where(x => x.taskName == TaskName).FirstOrDefault();
        }

        public Task LoadTask(int idTask)
        {
            return _db.Tasks.Find(idTask);
        }

        public List<TaskMembersView> LoadTasksInfo(string ProjectName)
        {
            return _db.TaskMembersViews.Where(x => x.projectName == ProjectName).ToList();
        }

        public Task UpdateStatus(Task T, string Status)
        {
            T.status = Status;
            _db.SaveChanges();
            return T;
        }

        public Task UpdateEndDate(Task T, DateTime date)
        {
            T.endDate = date;
            _db.SaveChanges();
            return T;
        }

        public Task UpdateDescription(Task T, string Description)
        {
            T.TaskDescription = Description;
            _db.SaveChanges();
            return T;
        }

        public TaskMembersView InsertDelegate(Task T, Member M)
        {
            _db.TaskMembers.Add(new TaskMember { idTask = T.idTask, idMember = M.idMember });
            _db.SaveChanges();
            TaskMembersView TMV = _db.TaskMembersViews.Where(x => x.idTask == T.idTask).FirstOrDefault(); ;
            return TMV;
        }

        public bool DeleteDelegate(int idTask)
        {
            TaskMember TM = _db.TaskMembers.Where(x => x.idTask == idTask).FirstOrDefault();
            _db.TaskMembers.Remove(TM);
            _db.SaveChanges();
            return true;
        }

        public TaskMembersView AddTask(Project P, Task T, Member M)
        {
            Task Task = _db.Tasks.Add(T);
            _db.SaveChanges();
            _db.ProjectTasks.Add(new ProjectTask { idProject = P.idProject, idTask = Task.idTask });
            _db.TaskMembers.Add(new TaskMember { idTask = Task.idTask, idMember = M.idMember });
            _db.SaveChanges();

            CC.AddTaskComment(Task.idTask, "The task was created.", M.idMember);
            CC.AddTaskComment(Task.idTask, "The task has been assigned to: " + M.displayName + ".", M.idMember);

            TaskMembersView TMV = _db.TaskMembersViews.Where(x => x.idTask == Task.idTask).FirstOrDefault();

            return TMV;
        }

        public Button CreateTaskButtons(DateTime? endDate, string status, string taskName)
        {
            //add a new button for each task found on the project
            Button btn = new Button();
            Grid spa = new Grid();
            TextBlock tb = new TextBlock();
            TextBlock tb2 = new TextBlock();
            StackPanel stpa = new StackPanel();


            Image img = new Image();
            BitmapImage bi3 = new BitmapImage();

            //bitmap for the image


            int result = DateTime.Compare(DateTime.Now, Convert.ToDateTime(endDate));

            bi3.BeginInit();
            if (result < 0)
            {
                bi3.UriSource = new Uri("images/icons8-realtime-48.png", UriKind.Relative);
            }
            else if (result == 0)
            {
                bi3.UriSource = new Uri("images/icons8-realtime-48.png", UriKind.Relative);
            }
            else
            {
                bi3.UriSource = new Uri("images/icons8-cancel-48.png", UriKind.Relative);
            }
            if (status == "Completed")
            {
                bi3.UriSource = new Uri("images/icons8-ok-48.png", UriKind.Relative);
            }
            bi3.EndInit();

            //image properties
            img.Stretch = Stretch.Fill;
            img.Width = 20;
            img.Height = 20;
            img.HorizontalAlignment = HorizontalAlignment.Right;
            img.Source = bi3;
            img.VerticalAlignment = VerticalAlignment.Top;

            //textbox properties
            tb.Text = taskName;
            tb.FontSize = 16;
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.TextAlignment = TextAlignment.Center;

            //textbox properties
            tb2.Text = Convert.ToDateTime(endDate).ToShortDateString();
            tb2.FontSize = 14;
            tb2.HorizontalAlignment = HorizontalAlignment.Right;
            tb2.VerticalAlignment = VerticalAlignment.Center;
            tb2.TextAlignment = TextAlignment.Center;

            //stack panel properties
            stpa.HorizontalAlignment = HorizontalAlignment.Right;
            stpa.Orientation = Orientation.Horizontal;
            stpa.Children.Add(tb2);
            stpa.Children.Add(img);

            //stack panel properties
            spa.HorizontalAlignment = HorizontalAlignment.Stretch;

            spa.Children.Add(tb);
            spa.Children.Add(stpa);


            //button properties

            btn.HorizontalAlignment = HorizontalAlignment.Stretch;
            btn.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            btn.Height = 45;
            btn.Content = spa;
            btn.Background = Brushes.White;
            btn.BorderThickness = new Thickness(0, 0, 0, 2);
            btn.BorderBrush = Brushes.LightGray;
            btn.Tag = taskName;
            return btn;
        }
    }
}
