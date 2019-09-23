using System.Windows.Controls;
using System.Windows.Media;
using System;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Diagnostics;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using MahApps.Metro.Controls;
using TaskManagerEF.Controllers;
using TaskManagerEF.Models;

namespace TaskManagerEF.Views
{
    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    public partial class TasksView
    {
        Task T = new Task();
        Project P = new Project();
        Member M = new Member();

        TasksController TC = new TasksController();
        ProjectsController PC = new ProjectsController();
        MembersController MC = new MembersController();
        CommentsController CC = new CommentsController();
        AttachmentsController AC = new AttachmentsController();
        MetroWindow metroWindow = (Application.Current.MainWindow as MetroWindow);

        public TasksView()
        {
            InitializeComponent();
        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            TaskGrid.Visibility = Visibility.Hidden; //when the add new task button is clicked, the projectgrid disapears and the task grid comes in 
        }

        private void btnCompleted_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (TC.UpdateStatus(T, "Completed") != null)
            {
                List<ProjectMembersView> Members = PC.GetMembers(P.idProject);

                string mails = "";
                string projectName = "";

                foreach (ProjectMembersView Member in Members)
                {
                    mails += Member.email + "; ";
                    projectName = Member.projectName;
                }

                
                CC.AddTaskComment(T.idTask, "The task has been completed.", M.idMember);

                //MSOutlookConnector send = new MSOutlookConnector();
                //send.sendingEmail(mails, "APTIV Task Manager - Task Completed", "Good day," + Environment.NewLine + Environment.NewLine + "The user " + mem.displayName + " has completed the task" + Environment.NewLine + Environment.NewLine + "If you don't have acces to Aptiv Task Manager, you can download it from: \\\\Dl3v66482\\osapps\\TaskManager\\publish.htm ");
            }
            var TaskView = new TasksView();
            Content = TaskView; //navigate between windows
        }

        private void Grid_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            M = MC.SearchMember(Environment.UserName);
            LoadTasksAsync();
        }

        private void LoadTasksAsync()
        {
            //get all the tasks from a project
            tbProjectT.Text = "My tasks";
            ProjectTasksPanel.IsEnabled = true;
            ManagePanels.ClearPanel(ProjectTasksPanel);


            List<Button> tasks = TC.GetTasks(Environment.UserName);

            foreach (Button task in tasks)
            {
                task.LostFocus += new RoutedEventHandler(LostFocus);
                task.Style = (Style)FindResource("SquareButtonStyle");
                task.Click += new RoutedEventHandler(TaskLoadAsync);
                ProjectTasksPanel.Children.Add(task);
            }
        }

        private new void LostFocus(object sender, EventArgs e) { ((Button)sender).Background = Brushes.White; }

        private async void TaskLoadAsync(object sender, EventArgs e)
        {
            if (((Button)sender).IsFocused)
            {
                ((Button)sender).Background = Brushes.LightGray;
            }

            Grid.SetColumnSpan(mainBorder, 1);
            TaskGrid.Visibility = Visibility.Visible;
            taskBorder.Visibility = Visibility.Visible;

            TaskMembersView Task = TC.LoadTaskInfo(((Button)sender).Tag.ToString());
            T = TC.LoadTask(Task.idTask);
            P = PC.SearchProject(Task.idProject);
            tbProj.Text = Task.projectName;
            dpEndDate.Text = Task.endDate.ToString();
            tbName.Text = Task.taskName;
            rtbDescription1.Document.Blocks.Clear();
            rtbDescription1.Document.Blocks.Add(new Paragraph(new Run(Task.TaskDescription)));
            //id = task.idTask;
            if (Task.status == "Completed")
            {
                btnCompleted.IsEnabled = false;
            }
            else if (Task.status != "Completed")
            {
                btnCompleted.IsEnabled = true;
            }
            lvDelegates.Items.Clear();
            lvDelegates.Items.Add(Task.displayName);

            List<Paragraph> Comments = CC.GetTaskComments(Task.idTask);

            rtbCommentHistory.Document.Blocks.Clear();
            foreach (Paragraph Comment in Comments)
            {
                rtbCommentHistory.Document.Blocks.Add(Comment);
            }


            try
            {
                if (!Directory.Exists(@"C:\temp\"))
                {
                    Directory.CreateDirectory(@"C:\temp\");
                }
                lvAttachments.Items.Clear();
                ////find the projec id and save it in a variable so we can use it to insert tasks and comment
                List<string> Attachments = AC.GetAttachments(Task.idProject, Task.idTask);
                foreach (string Attachment in Attachments)
                {
                    lvAttachments.Items.Add(Attachment);
                }
            }
            catch (Exception ex)
            {
                await metroWindow.ShowMessageAsync("Atention", ex.Message);
            }
        }

        private void AddComment()
        {
            //button to send the comments

            //button add new task on the add task grid 
            TextRange textRange = new TextRange(
                // TextPointer to the start of content in the RichTextBox.
                rtbComment.Document.ContentStart,
                // TextPointer to the end of content in the RichTextBox.
                rtbComment.Document.ContentEnd
            );

            rtbCommentHistory.Document.Blocks.Add(CC.AddTaskComment(T.idTask, textRange.Text, M.idMember));
            //rtbCommentHistory.Document.Blocks.Add(new Paragraph(new Run(date.ToString() + " | " + textRange.Text)));
            rtbComment.SelectAll();
            rtbComment.Selection.Text = "";
        }

        private void rtbDescription1_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //task description
            rtbDescription1.Foreground = new SolidColorBrush(Colors.Black);
            rtbDescription1.Document.Blocks.Clear(); //activate the description textbox
        }

        private void rtbComment_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //Task comment rich text box
            rtbComment.Foreground = new SolidColorBrush(Colors.Black);
            rtbComment.Document.Blocks.Clear();
        }


        private void btnAttachments_Click(object sender, RoutedEventArgs e)
        {
            flyOutAttachments.IsOpen = true;
        }   

        private void btnBack2_Click(object sender, RoutedEventArgs e)
        {
            TaskGrid.Visibility = Visibility.Hidden;
            taskBorder.Visibility = Visibility.Hidden;
            Grid.SetColumnSpan(mainBorder, 2);
        }

        private void rtbComment_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                rtbComment.SelectAll();
                if (rtbComment.Selection.Text != "" && rtbComment.Selection.Text != "\r\n\r\n")
                {
                    AddComment();
                }
            }
        }

        private void btnSendComment_Click(object sender, RoutedEventArgs e)
        {
            rtbComment.SelectAll();
            if (rtbComment.Selection.Text != "" && rtbComment.Selection.Text != "Write a comment...\r\n")
            {
                AddComment();
            }
        }

        private void btnAddAtt_Click(object sender, RoutedEventArgs e)
        {
            string att = AC.AddAttachmet(T.idTask.ToString(), P.idProject).Result;
            lvAttachments.Items.Add(att);
            CC.AddTaskComment(T.idTask, "The attachment " + att + " has been added.", M.idMember);
        }

        private void btnShowAll_Click(object sender, RoutedEventArgs e)
        {
            //get all the tasks from a project
            tbProjectT.Text = "My tasks";
            ProjectTasksPanel.IsEnabled = true;
            ManagePanels.ClearPanel(ProjectTasksPanel);


            List<Button> Tasks = TC.GetAllTasks(Environment.UserName);

            foreach (Button Task in Tasks)
            {
                Task.LostFocus += new RoutedEventHandler(LostFocus);
                Task.Style = (Style)FindResource("SquareButtonStyle");
                Task.Click += new RoutedEventHandler(TaskLoadAsync);
                ProjectTasksPanel.Children.Add(Task);
            }
            btnHide.Visibility = Visibility.Visible;
            btnShowAll.Visibility = Visibility.Hidden;
        }

        private void btnHide_Click(object sender, RoutedEventArgs e)
        {
            LoadTasksAsync();
            btnHide.Visibility = Visibility.Hidden;
            btnShowAll.Visibility = Visibility.Visible;
        }

        private async void lvAttachments_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // OS run test
            Process prc = new Process();
            prc.StartInfo.FileName = @"C:\temp\" + lvAttachments.SelectedItem.ToString();
            if (!Directory.Exists(prc.StartInfo.FileName))
            {
                try
                {
                    if (!Directory.Exists(@"C:\temp\"))
                    {
                        Directory.CreateDirectory(@"C:\temp\");
                    }
                    AC.StartProcess(P.idProject);
                }
                catch (Exception ex)
                {
                    await metroWindow.ShowMessageAsync("Warning", ex.Message);
                }
            }
            prc.Start();
        }

        private async void LvAttachments_PreviewMouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            // OS run test
            Process prc = new Process();
            prc.StartInfo.FileName = @"C:\temp\" + lvAttachments.SelectedItem.ToString();
            if (!Directory.Exists(prc.StartInfo.FileName))
            {
                try
                {
                    if (!Directory.Exists(@"C:\temp\"))
                    {
                        Directory.CreateDirectory(@"C:\temp\");
                    }
                    AC.StartProcess(P.idProject);
                }
                catch (Exception ex)
                {
                    await metroWindow.ShowMessageAsync("Warning", ex.Message);
                }
            }
            prc.Start();
        }
    }
}