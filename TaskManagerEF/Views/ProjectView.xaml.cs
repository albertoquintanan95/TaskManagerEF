using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using TaskManagerEF.Controllers;
using TaskManagerEF.Models;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using LiveCharts;
using LiveCharts.Defaults;
using System.ComponentModel;
using System.Windows.Media;
using LiveCharts.Wpf;
using System.IO;
using System.DirectoryServices.AccountManagement;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace TaskManagerEF.Views
{
    /// <summary>
    /// Interaction logic for ProjectView.xaml
    /// </summary>
    public partial class ProjectView : UserControl
    {
        public bool codeTriggered = false;
        Project P = new Project();
        Member M = new Member();
        Task T = new Task();

        ActiveDirectoryController ADC = new ActiveDirectoryController();
        AttachmentsController AC = new AttachmentsController();
        CommentsController CC = new CommentsController();
        ManagePanels MP = new ManagePanels();
        MembersController MC = new MembersController();
        ProjectsController PC = new ProjectsController();
        TasksController TC = new TasksController();
        MetroWindow metroWindow = (Application.Current.MainWindow as MetroWindow);

        private double _from;
        private double _to;
        private ChartValues<GanttPoint> _values;
        List<string> startDates = new List<string>();
        List<string> endDates = new List<string>();
        List<string> names = new List<string>();

        public SeriesCollection Series { get; set; }
        public Func<double, string> Formatter { get; set; }
        public string[] Labels { get; set; }

        public ProjectView()
        {
            InitializeComponent();
        }

        //here starts livecharts code

        public double From
        {
            get { return _from; }
            set
            {
                _from = value;
                OnPropertyChanged("From");
            }
        }

        public double To
        {
            get { return _to; }
            set
            {
                _to = value;
                OnPropertyChanged("To");
            }
        }

        private void ResetZoomOnClick(object sender, RoutedEventArgs e)
        {
            From = _values.First().StartPoint;
            To = _values.Last().EndPoint;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //here finishes livecharts

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

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            var PCView = new PCView();
            Content = PCView; //navigate between windows
        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            cbAssignTo.Items.Clear();
            foreach (ProjectMembersView Member in PC.GetMembers(P.idProject))
            {
                cbAssignTo.Items.Add(Member.displayName);
            }

            //blue button on the top of the left grid to add a new task
            AddNewTask.Visibility = Visibility.Visible;
            ProjectGrid.Visibility = Visibility.Hidden;
            TaskGrid.Visibility = Visibility.Hidden; //when the add new task button is clicked, the projectgrid disapears and the task grid comes in    

        }

        private void rtbComment_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //Task comment rich text box
            rtbComment.Foreground = new SolidColorBrush(Colors.Black);
            rtbComment.Document.Blocks.Clear();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //cancel the new task
            txtName.Text = "";
            rtbDescription1.SelectAll();
            rtbDescription1.Selection.Text = "";
            dtpEndDate.Text = "";
            ProjectGrid.Visibility = Visibility.Visible;
            AddNewTask.Visibility = Visibility.Hidden;
            TaskGrid.Visibility = Visibility.Hidden;
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //button add new task on the add task grid 
            TextRange textRange = new TextRange(
                // TextPointer to the start of content in the RichTextBox.
                rtbDescription2.Document.ContentStart,
                // TextPointer to the end of content in the RichTextBox.
                rtbDescription2.Document.ContentEnd
            );

            if (txtName.Text != "" && textRange.Text != "" && dtpEndDate.Text != "")
            {
                TC.AddTask(P, new Task { taskName = txtName.Text, TaskDescription = textRange.Text, endDate = Convert.ToDateTime(dtpEndDate.Text), startDate = DateTime.Now }, MC.SearchMemberByDisplayName(cbAssignTo.SelectedItem.ToString()));

                txtName.Text = "";
                rtbDescription1.SelectAll();
                rtbDescription1.Selection.Text = "";
                dtpEndDate.Text = "";

                var ProjectView = new ProjectView();
                Content = ProjectView;
            }
            else
            {
                await metroWindow.ShowMessageAsync("Atention", "Complete the fields first");
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            AddComment();
        }

        private void lostFocus(object sender, EventArgs e)
        {
            ((Button)sender).Background = Brushes.White;
        }

        private async void TaskLoad(object sender, EventArgs e)
        {
            if (((Button)sender).IsFocused)
            {
                ((Button)sender).Background = Brushes.LightGray;
            }

            ProjectGrid.Visibility = Visibility.Hidden;
            AddNewTask.Visibility = Visibility.Hidden;
            TaskGrid.Visibility = Visibility.Visible;
            rtbDescription.Foreground = new SolidColorBrush(Colors.Black);
            //string task = ((Button)sender).Tag.ToString();

            TaskMembersView Task = TC.LoadTaskInfo(((Button)sender).Tag.ToString());
            T = TC.LoadTask(Task.idTask);
            codeTriggered = true;
            dpEndDate.Text = Convert.ToString(Task.endDate);
            codeTriggered = false;
            tbName.Text = Task.taskName;
            rtbDescription1.Document.Blocks.Clear();
            rtbDescription1.Document.Blocks.Add(new Paragraph(new Run(Task.TaskDescription)));
            if (Task.status == "Completed")
            {
                btnCompleted.IsEnabled = false;
                btnReject.Visibility = Visibility.Visible;
            }
            else if (Task.status != "Completed")
            {
                btnCompleted.IsEnabled = true;
                btnReject.Visibility = Visibility.Hidden;
            }

            cbAssign.Items.Clear();
            List<ProjectMembersView> M = PC.GetMembers(P.idProject);

            foreach (ProjectMembersView member in M)
            {
                cbAssign.Items.Add(member.displayName);
            }

            List<Paragraph> comments = CC.GetTaskComments(Task.idTask);

            rtbCommentHistory.Document.Blocks.Clear();
            foreach (Paragraph comment in comments)
            {
                rtbCommentHistory.Document.Blocks.Add(comment);
            }

            TaskMembersView assignedTo = TC.GetMembers(Task.idTask);
            codeTriggered = true;
            cbAssign.Text = (assignedTo.displayName);
            codeTriggered = false;

            try
            {
                if (!Directory.Exists(@"C:\temp\"))
                {
                    Directory.CreateDirectory(@"C:\temp\");
                }
                lvAttachments.Items.Clear();
                ////find the projec id and save it in a variable so we can use it to insert tasks and comment
                List<string> attachments = AC.GetAttachments(P.idProject, Task.idTask);
                foreach (string attachment in attachments)
                {
                    lvAttachments.Items.Add(attachment);
                }
            }
            catch (Exception ex)
            {
                await metroWindow.ShowMessageAsync("Atention", "Task can not be loaded");
            }
        }

        private async void rtbDescription_LostFocus(object sender, RoutedEventArgs e)
        {
            //button add new task on the add task grid 
            TextRange textRange = new TextRange(
                // TextPointer to the start of content in the RichTextBox.
                rtbDescription.Document.ContentStart,
                // TextPointer to the end of content in the RichTextBox.
                rtbDescription.Document.ContentEnd
            );

            if (PC.UpdateDescription(P, textRange.Text) != null)
            {
                rtbDescription.Document.Blocks.ToString();
                rtbProjectCommentHistory.Document.Blocks.Add(CC.AddProjectComment(P.idProject, "The project description was updated", MC.SearchMember(Environment.UserName).idMember));
            }
            else
            {
                await metroWindow.ShowMessageAsync("Atention", "Description can not be updated");
            }
        }

        private void btnCompleted_Click(object sender, RoutedEventArgs e)
        {
            if (TC.UpdateStatus(T, "Completed") != null)
            {
                List<ProjectMembersView> members = PC.GetMembers(P.idProject);
                TaskMembersView member2 = TC.GetMembers(T.idTask);

                string mails = "";
                string projectName = "";

                foreach (ProjectMembersView member in members)
                {
                    mails += member.email + "; ";
                    projectName = member.projectName;
                }

                //MSOutlookConnector send = new MSOutlookConnector();
                //send.sendingEmail(mails, "APTIV Task Manager - Task Completed", "Good day," + Environment.NewLine + Environment.NewLine + "The user " + member2.displayName + " has completed the task: " + member2.taskName + Environment.NewLine + Environment.NewLine + "If you don't have acces to Aptiv Task Manager, you can download it from: \\\\Dl3v66482\\osapps\\TaskManager\\publish.htm ");

                Member M = MC.SearchMember(Environment.UserName);
                rtbCommentHistory.Document.Blocks.Add(CC.AddTaskComment(T.idTask, "The task has been completed.", M.idMember));

                var ProjectView = new ProjectView();
                Content = ProjectView; //navigate between windows
            }
        }

        private async void searchListResults_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string netID = await ADC.SearchAD(searchListResults.SelectedItem.ToString());

            PrincipalContext context = new PrincipalContext(ContextType.Domain);
            UserPrincipal principal = UserPrincipal.FindByIdentity(context, netID);

            Member M = MC.SearchMember(netID);
            if (M != null)
            {
                if (PC.MemberExist(P.idProject, M.idMember))
                {
                    await metroWindow.ShowMessageAsync("Atention", "User is alredy in the project");
                    txtsearch.Text = "";
                }
                else
                {
                    if (PC.AddProjectMember(P, M))
                    {
                        var ProjectView = new ProjectView();
                        Content = ProjectView; //navigate between windows
                                               //MSOutlookConnector send = new MSOutlookConnector();
                                               //send.sendingEmail(principal.EmailAddress, "APTIV Task Manager - You are part of a new project", "Good day, " + Environment.NewLine + Environment.NewLine + "You are part of the project " + GlobalVariables.projecViewNav + " in the Aptiv Task Manager App." + Environment.NewLine +  Environment.NewLine + "If you don't have it installed yet, please download it from \\\\Dl3v66482\\osapps\\TaskManager\\publish.htm ");
                        rtbProjectCommentHistory.Document.Blocks.Add(CC.AddProjectComment(P.idProject, "The member " + M.displayName + " was added to the project.", MC.SearchMember(Environment.UserName).idMember));

                    }
                    else
                    {
                        await metroWindow.ShowMessageAsync("Atention", "Member can not be added");
                    }
                }

            }
            else
            {
                Member NM = MC.AddNewMember(new Member { firstName = principal.GivenName, lastName = principal.Surname, displayName = principal.DisplayName, email = principal.EmailAddress, netID = principal.SamAccountName });
                if (PC.AddProjectMember(P, NM))
                {
                    //MSOutlookConnector send = new MSOutlookConnector();
                    //send.sendingEmail(principal.EmailAddress, "APTIV Task Manager - You are part of a new project", "Good day, " + Environment.NewLine + Environment.NewLine + "You are part of the project " + GlobalVariables.projecViewNav + " in the Aptiv Task Manager App." + Environment.NewLine + Environment.NewLine + "If you don't have it installed yet, please download it from \\\\Dl3v66482\\osapps\\TaskManager\\publish.htm ");
                    rtbProjectCommentHistory.Document.Blocks.Add(CC.AddProjectComment(P.idProject, "The member " + principal.DisplayName + " was added to the project.", MC.SearchMember(Environment.UserName).idMember));

                    var ProjectView = new ProjectView();
                    Content = ProjectView; //navigate between windows
                }
            }
        }

        private void btnBack2_Click(object sender, RoutedEventArgs e)
        {
            TaskGrid.Visibility = Visibility.Hidden;
            ProjectGrid.Visibility = Visibility.Visible;
        }

        private void btnAddAttachment_Click(object sender, RoutedEventArgs e)
        {
            lbAttachments.Items.Add(AC.AddAttachmet(T.idTask.ToString(), P.idProject).Result);
        }

        private async void lbAttachments_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // OS run test
            Process prc = new Process();
            string[] name = Regex.Split(lbAttachments.SelectedItem.ToString(), "/");
            prc.StartInfo.FileName = @"C:\temp\" + name[0];
            if (!Directory.Exists(prc.StartInfo.FileName))
            {
                try
                {
                    if (!Directory.Exists(@"C:\temp\"))
                    {
                        Directory.CreateDirectory(@"C:\temp\");
                    }
                    ////find the projec id and save it in a variable so we can use it to insert tasks and comments
                    AC.StartProcess(P.idProject);
                }
                catch (Exception ex)
                {
                    await metroWindow.ShowMessageAsync("Warning", ex.Message);
                }
            }
            prc.Start();
        }

        private void txtsearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = txtsearch.Text;
            List<string> s = new List<string>();
            if (search.Length > 4)
            {
                searchListResults.ItemsSource = ADC.SearchContacts(search);   
            }
            
        }


        private void rtbComment_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                rtbComment.SelectAll();
                if (rtbComment.Selection.Text == "\r\n\r\n")
                {
                    rtbComment.Focus();
                }
                if (rtbComment.Selection.Text != "" && rtbComment.Selection.Text != "\r\n\r\n")
                {
                    AddComment();
                }
            }
        }

        private void rtbProjectComment_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                rtbProjectComment.SelectAll();
                if (rtbProjectComment.Selection.Text == "\r\n\r\n")
                {
                    rtbProjectComment.Focus();
                }
                if (rtbProjectComment.Selection.Text != "" && rtbProjectComment.Selection.Text != "\r\n\r\n")
                {
                    AddProjectComment();
                }
            }
        }

        private void rtbProjectComment_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Task comment rich text box
            rtbProjectComment.Foreground = new SolidColorBrush(Colors.Black);
            rtbProjectComment.Document.Blocks.Clear();
            rtbProjectComment.Focus();
        }

        private void AddProjectComment()
        {
            //button to send the comments

            //button add new task on the add task grid 
            TextRange textRange = new TextRange(
                // TextPointer to the start of content in the RichTextBox.
                rtbProjectComment.Document.ContentStart,
                // TextPointer to the end of content in the RichTextBox.
                rtbProjectComment.Document.ContentEnd
            );

            rtbProjectCommentHistory.Document.Blocks.Add(CC.AddProjectComment(P.idProject, textRange.Text, MC.SearchMember(Environment.UserName).idMember));

            rtbProjectComment.SelectAll();
            rtbProjectComment.Selection.Text = "";
            //rtbProjectComment.Document.Blocks.Clear();
            rtbProjectComment.Focus();
        }

        private void btnAttachments_Click(object sender, RoutedEventArgs e)
        {
            flyOutTaskAttachments.IsOpen = true;
        }

        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (Paragraph comment in CC.GetProjectComments(P.idProject))
            {
                rtbProjectCommentHistory.Document.Blocks.Add(comment);
            }
        }

        private async void lvAttachments_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // OS run test
            Process prc = new Process();
            string[] name = Regex.Split(lbAttachments.SelectedItem.ToString(), "/");
            prc.StartInfo.FileName = @"C:\temp\" + name[1];
            if (!Directory.Exists(prc.StartInfo.FileName))
            {
                try
                {
                    if (!Directory.Exists(@"C:\temp\"))
                    {
                        Directory.CreateDirectory(@"C:\temp\");
                    }
                    ////find the projec id and save it in a variable so we can use it to insert tasks and comments
                    AC.StartProcess(P.idProject);

                }
                catch (Exception ex)
                {
                    await metroWindow.ShowMessageAsync("Warning", ex.Message);
                }
            }
            prc.Start();
        }

        private void btnSendPComment_Click(object sender, RoutedEventArgs e)
        {
            rtbProjectComment.SelectAll();
            if (rtbProjectComment.Selection.Text != "" && rtbProjectComment.Selection.Text != "Write a comment...\r\n")
            {
                AddProjectComment();
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

        private void btnReject_Click(object sender, RoutedEventArgs e)
        {
            if (TC.UpdateStatus(T, "On_TIme") != null)
            {
                rtbCommentHistory.Document.Blocks.Add(CC.AddTaskComment(T.idTask, "The user " + MC.SearchMember(Environment.UserName).displayName + " rejected the task.", MC.SearchMember(Environment.UserName).idMember));

                var ProjectView = new ProjectView();
                Content = ProjectView; //navigate between windows
            }
        }

        private async void cbAssign_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!codeTriggered)
            {
                try
                {
                    Member M = MC.SearchMemberByDisplayName(cbAssign.SelectedItem.ToString());
                    if (M.idMember.ToString() != "")
                    {
                        if (TC.GetMembers(T.idTask) != null)
                        {
                            if (TC.DeleteDelegate(T.idTask))
                            {
                                Delegate(T, M);
                            }
                            else
                            {
                                await metroWindow.ShowMessageAsync("Attention", cbAssign.SelectedItem.ToString() + " delegate has been not assigned");
                            }
                        }
                        else
                        {
                            Delegate(T, M);
                        }
                    }
                    else
                    {
                        await metroWindow.ShowMessageAsync("Attention", cbAssign.SelectedItem.ToString() + " is not a member of this project");
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
            }
        }

        private bool Delegate(Task T, Member M)
        {
            try
            {
                TC.InsertDelegate(T, M);
                string delegatecomment = "The task has been assigned to: " + cbAssign.SelectedItem.ToString() + ".";
                rtbCommentHistory.Document.Blocks.Add(CC.AddTaskComment(T.idTask, delegatecomment, MC.SearchMember(Environment.UserName).idMember));
                return true;
            }
            catch
            {
                return false;
            }

        }

        private async void dpEndDate_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (!codeTriggered)
            {
                string date = dpEndDate.Text;
                if (TC.UpdateEndDate(T, Convert.ToDateTime(dpEndDate.SelectedDate)) != null)
                {
                    dpEndDate.Text = dpEndDate.Text;
                    rtbCommentHistory.Document.Blocks.Add(CC.AddTaskComment(T.idTask, "End date has been updated.", MC.SearchMember(Environment.UserName).idMember));

                }
                else
                {
                    await metroWindow.ShowMessageAsync("Attention", "Date can not be updated");
                }
            }
        }

        private async void btnPCompleted_Click(object sender, RoutedEventArgs e)
        {
            if (PC.FinishProject(P) != null)
            {
                rtbCommentHistory.Document.Blocks.Add(CC.AddProjectComment(P.idProject, "The user " + MC.SearchMember(Environment.UserName).displayName + " has marked the project as completed.", MC.SearchMember(Environment.UserName).idMember));

                var ProjectView = new ProjectView();
                this.Content = ProjectView;
            }
            else
            {
                await metroWindow.ShowMessageAsync("Attention", "Project can not be updated");
            }
        }

        private void btnAddAtt_Click(object sender, RoutedEventArgs e)
        {
            lbAttachments.Items.Add(AC.AddAttachmet(T.idTask.ToString(), P.idProject).Result);
        }

        private void btnFlyoutAddAttachment_Click(object sender, RoutedEventArgs e)
        {
            flyOutAttachments.IsOpen = true;
        }

        private void btnAttachment_Click(object sender, RoutedEventArgs e)
        {
            lvAttachments.Items.Add(AC.AddAttachmet(T.idTask.ToString(), P.idProject).Result);
        }

        private async void btnUnfinish_Click(object sender, RoutedEventArgs e)
        {
            if (PC.UnfinishProject(P) != null)
            {
                CC.AddProjectComment(P.idProject, "The user " + MC.SearchMember(Environment.UserName).displayName + " rejected the project.", MC.SearchMember(Environment.UserName).idMember);

                var ProjectView = new ProjectView();
                this.Content = ProjectView;
            }
            else
            {
                await metroWindow.ShowMessageAsync("Attention", "Project can not be updated");
            }
        }

        private void tabPN_Loaded(object sender, RoutedEventArgs e)
        {
            //if (tabPN.IsEnabled)
            //{
            //    List<DOH> partNumbers = new List<DOH>();
            //    DOHMethods doh = new DOHMethods();
            //    foreach (VC PN in vcMet.GetPartNumbers(GlobalVariables.projecViewNav))
            //    {
            //        partNumbers.Add(doh.SearchByPN(PN.plant, PN.material));
            //    }
            //    lvParts.ItemsSource = partNumbers;
            //    lvSummary.ItemsSource = partNumbers;
            //}
        }

        private void tabVI_Loaded(object sender, RoutedEventArgs e)
        {
        //    if (tabVI.IsEnabled)
        //    {
        //        Vendors ov = new Vendors();
        //        Vendors nv = new Vendors();
        //        string old = "";
        //        string newv = "";
        //        foreach (VC vc in vcMet.GetPartNumbers(GlobalVariables.projecViewNav))
        //        {
        //            old = vc.oldVendor;
        //            newv = vc.newVendor;
        //        }
        //        if (old != "" && newv != "")
        //        {
        //            ov = venMet.GetVendors(old);
        //            nv = venMet.GetVendors(newv);

        //            txtVendorNumber.Text = ov.VendorNumber;
        //            txtVendorName.Text = ov.VendorName;
        //            txtDUNS.Text = ov.DUNS;
        //            txtSRT.Text = ov.SRT;
        //            txtCN.Text = ov.Country;
        //            txtPCDes.Text = ov.PCDescription;
        //            txtFixed.Text = ov.Fixed;
        //            txtGRPT.Text = ov.GRPT;
        //            txtPC.Text = ov.PlanningCalendar;
        //            txtLO.Text = ov.LO;
        //            txtOrdering.Text = ov.OrderingSystem;

        //            txtNewVendorNumber.Text = nv.VendorNumber;
        //            txtNewVendorName.Text = nv.VendorName;
        //            txtNewDUNS.Text = nv.DUNS;
        //            txtNewSRT.Text = nv.SRT;
        //            txtNewCN.Text = nv.Country;
        //            txtNewPCDes.Text = nv.PCDescription;
        //            txtNewFixed.Text = nv.Fixed;
        //            txtNewGRPT.Text = nv.GRPT;
        //            txtNewPC.Text = nv.PlanningCalendar;
        //            txtNewLO.Text = nv.LO;
        //            txtNewOrdering.Text = nv.OrderingSystem;
        //        }
        //    }
        }

        private async void rtbDescription1_LostFocus(object sender, RoutedEventArgs e)
        {
            //button add new task on the add task grid 
            TextRange textRange = new TextRange(
                // TextPointer to the start of content in the RichTextBox.
                rtbDescription1.Document.ContentStart,
                // TextPointer to the end of content in the RichTextBox.
                rtbDescription1.Document.ContentEnd
            );

            if (TC.UpdateDescription(T, textRange.Text) != null)
            {
                rtbDescription1.Document.Blocks.ToString();
                rtbCommentHistory.Document.Blocks.Add(CC.AddTaskComment(T.idTask, "Task description has been updated.", MC.SearchMember(Environment.UserName).idMember));

            }
            else
            {
                await metroWindow.ShowMessageAsync("Atention", "Description can not be updated");
            }
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            P = PC.SearchProject(GlobalVariables.projecViewNav);
            M = MC.SearchMember(Environment.UserName);

            GlobalVariables.projectArea = P.area;
            rtbDescription.Document.Blocks.Clear();
            rtbDescription.Document.Blocks.Add(new Paragraph(new Run(P.projectDescription)));

            if (P.area == "SCM")
            {
                tabPN.IsEnabled = true;
                tabVI.IsEnabled = true;
                tabPN.Visibility = Visibility.Visible;
                tabVI.Visibility = Visibility.Visible;
            }

            if (Convert.ToString(P.endDate) != "1/1/0001 12:00:00 AM")
            {
                btnPCompleted.IsEnabled = false;
                btnPCompleted.Visibility = Visibility.Hidden;
                btnUnfinish.Visibility = Visibility.Visible;
            }

            foreach (ProjectMembersView Member in PC.GetMembers(P.idProject))
            {
                lvMembers.Items.Add(Member.displayName);
            }

            //get all the tasks from a project
            tbProjectT.Text = P.projectName;

            ProjectTasksPanel.IsEnabled = true;
            ManagePanels.ClearPanel(ProjectTasksPanel);


            foreach (Button task in TC.GetProjectTasks(P.projectName))
            {
                task.LostFocus += new RoutedEventHandler(lostFocus);
                task.Style = (Style)FindResource("SquareButtonStyle");
                task.Click += new RoutedEventHandler(TaskLoad);
                ProjectTasksPanel.Children.Add(task);
            }

            int aux = 0;
            _values = new ChartValues<GanttPoint>();
            var labels = new List<string>();



            foreach (TaskMembersView TV in TC.LoadTasksInfo(GlobalVariables.projecViewNav))
            {
                labels.Add(TV.taskName);
                if (Convert.ToDateTime(TV.startDate).Ticks < Convert.ToDateTime(TV.endDate).Ticks)
                {
                    _values.Add(new GanttPoint(Convert.ToDateTime(TV.startDate).Ticks, Convert.ToDateTime(TV.endDate).Ticks));

                }
                else
                {
                    _values.Add(new GanttPoint(Convert.ToDateTime(TV.startDate).Ticks, Convert.ToDateTime(TV.endDate).AddDays(1).Ticks));
                }
                aux = 1;
            }

            try
            {
                Series = new SeriesCollection
                   {
                      new RowSeries
                      {
                         Values = _values,
                         DataLabels = true
                      }
                    };
                Formatter = value => new DateTime((long)value).ToString("dd MMM");

                Labels = labels.ToArray();

                ResetZoomOnClick(null, null);

                DataContext = this;
                startDates.ToArray();
                endDates.ToArray();
                names.ToArray();
                if (aux == 0)
                {
                    //ChartGrid.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception)
            {
                //ChartGrid.Visibility = Visibility.Hidden;
            }

            try
            {
                List<string> attachments = AC.GetProjectAttachments(P.idProject);

                foreach (string attachment in attachments)
                {
                    lbAttachments.Items.Add(attachment);
                }
            }
            catch (Exception ex)
            {
                await metroWindow.ShowMessageAsync("Atention", "Project can not be loaded");
            }
        }
    }
}
