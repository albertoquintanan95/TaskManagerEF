using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Windows.Input;

namespace TaskManagerEF.Views
{
    /// <summary>
    /// Interaction logic for VendorChangesUpdate.xaml
    /// </summary>
    public partial class VendorChangesUpdate : UserControl
    {
        //public string vendorName = "";
        //public List<info> information = new List<info>();
        //VCMethods vc = new VCMethods();

        public class info
        {
            public string plant { get; set; }
            public string part { get; set; }
            public string agreement { get; set; }
            public string item { get; set; }
            public string vendor { get; set; }
        }

        public VendorChangesUpdate()
        {
            InitializeComponent();
        }

        private void btnPaste_Click(object sender, RoutedEventArgs e)
        {
            //Cursor = Cursors.Wait;

            //BackgroundWorker worker = new BackgroundWorker();
            //worker.WorkerReportsProgress = true;
            //worker.DoWork += worker_DoWork;
            //worker.ProgressChanged += worker_ProgressChanged;

            //worker.RunWorkerAsync();
            //ErrorLayer.Visibility = Visibility.Visible;
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //this.Dispatcher.Invoke(() =>
            //{
            //    try
            //    {
            //        List<string> vendors = new List<string>();
            //        string value = Clipboard.GetText();
            //        string[] line = Regex.Split(value, "\n");
            //        //MessageBox.Show(value);
            //        List<info> errors = new List<info>();
            //        //string vendor = "1066049";

            //        foreach (string word in line)
            //        {
            //            try
            //            {
            //                string[] cell = Regex.Split(word, "\t");

            //                string[] cell2 = Regex.Split(cell[3], "\r");

            //                string qry301 = "SELECT * FROM dbo.VendorChanges WHERE plant = @plant and material = @material and agreement = @agreement and item = @item";

            //                SqlCommand my301Command = new SqlCommand(qry301, ConnectionModule.myConnection);
            //                my301Command.Parameters.Add("@plant", System.Data.SqlDbType.VarChar).Value = Convert.ToString(cell[0]);
            //                my301Command.Parameters.Add("@material", System.Data.SqlDbType.VarChar).Value = Convert.ToString(cell[1]);
            //                my301Command.Parameters.Add("@agreement", System.Data.SqlDbType.VarChar).Value = Convert.ToString(cell[2]);
            //                my301Command.Parameters.Add("@item", System.Data.SqlDbType.VarChar).Value = Convert.ToString(cell2[0]);

            //                ConnectionModule.Conect();

            //                using (SqlDataReader o303Reader = my301Command.ExecuteReader())
            //                {
            //                    //start the reader and create a button for each element in the database, then add them to a wrappanel
            //                    while (o303Reader.Read())
            //                    {

            //                        if (cell != null && cell[0] != "" && cell[1] != "" && cell[2] != "" && cell2[0] != "")
            //                        {
            //                            vendors.Add(o303Reader["vendor"].ToString());
            //                        }
            //                    }
            //                    ConnectionModule.Disconect();
            //                }

            //            }
            //            catch (Exception)
            //            {
            //                //MessageBox.Show(ex.Message);
            //            }

            //        }

            //        int indicator = 0;

            //        var result =
            //         vendors.GroupBy(i => i)
            //            .Select(g => new { i = g.Key, count = g.Count() })
            //            .OrderByDescending(x => x.count)
            //            .TakeWhile(x =>
            //            {
            //                if (x.count == indicator || indicator == 0)
            //                {
            //                    indicator = x.count;
            //                    return true;
            //                }
            //                return false;
            //            })
            //        .Select(x => x.i);


            //        string resultado = result.First();
            //        foreach (string word in line)
            //        {
            //            try
            //            {
            //                string[] cell = Regex.Split(word, "\t");

            //                string[] cell2 = Regex.Split(cell[3], "\r");

            //                string qry301 = "SELECT * FROM dbo.VendorChanges WHERE plant = @plant and material = @material and agreement = @agreement and item = @item";

            //                SqlCommand my301Command = new SqlCommand(qry301, ConnectionModule.myConnection);
            //                my301Command.Parameters.Add("@plant", System.Data.SqlDbType.VarChar).Value = Convert.ToString(cell[0]);
            //                my301Command.Parameters.Add("@material", System.Data.SqlDbType.VarChar).Value = Convert.ToString(cell[1]);
            //                my301Command.Parameters.Add("@agreement", System.Data.SqlDbType.VarChar).Value = Convert.ToString(cell[2]);
            //                my301Command.Parameters.Add("@item", System.Data.SqlDbType.VarChar).Value = Convert.ToString(cell2[0]);

            //                ConnectionModule.Conect();

            //                using (SqlDataReader o303Reader = my301Command.ExecuteReader())
            //                {
            //                    //start the reader and create a button for each element in the database, then add them to a wrappanel
            //                    while (o303Reader.Read())
            //                    {
            //                        if (o303Reader["vendor"].ToString() == resultado)
            //                        {
            //                            if (cell != null && cell[0] != "" && cell[1] != "" && cell[2] != "" && cell2[0] != "")
            //                            {
            //                                information.Add(new info() { plant = cell[0], part = cell[1], agreement = cell[2], item = cell2[0], vendor = o303Reader["vendorName"].ToString() });
            //                                vendorName = o303Reader["vendorName"].ToString();
            //                            }
            //                            else
            //                            {
            //                                errors.Add(new info() { plant = cell[0], part = cell[1], agreement = cell[2], item = cell2[0], vendor = o303Reader["vendorName"].ToString() });
            //                            }
            //                        }
            //                        else
            //                        {
            //                            errors.Add(new info() { plant = cell[0], part = cell[1], agreement = cell[2], item = cell2[0], vendor = o303Reader["vendorName"].ToString() });
            //                        }
            //                    }
            //                    ConnectionModule.Disconect();
            //                }

            //            }
            //            catch(Exception ex)
            //            {
            //                ConnectionModule.Disconect();
            //                //MessageBox.Show(ex.Message);
            //             }

            //        }

            //        string qry305 = "SELECT * FROM dbo.VendorChanges WHERE vendor = @vendor";
            //        int bol = 0;

            //        SqlCommand my305Command = new SqlCommand(qry305, ConnectionModule.myConnection);
            //        my305Command.Parameters.Add("@vendor", System.Data.SqlDbType.VarChar).Value = Convert.ToString(result.First());

            //        ConnectionModule.Conect();

            //        using (SqlDataReader o305Reader = my305Command.ExecuteReader())
            //        {
            //            if (o305Reader.Read())
            //            {
            //                //start the reader and create a button for each element in the database, then add them to a wrappanel
            //                txtVendorNumber.Text = o305Reader["vendor"].ToString();
            //                txtVendorName.Text = o305Reader["vendorName"].ToString();
            //                txtDUNS.Text = o305Reader["DUNS"].ToString();
            //                txtCN.Text = o305Reader["CN"].ToString();
            //                txtSRT.Text = o305Reader["SRT"].ToString();
            //                txtFrom.Text = Convert.ToDateTime(o305Reader["from"]).ToShortDateString();
            //                txtTo.Text = Convert.ToDateTime(o305Reader["to"]).ToShortDateString();
            //                txtPCDes.Text = o305Reader["PCDesc"].ToString();
            //                txtFixed.Text = o305Reader["newFixed"].ToString();
            //                txtGRPT.Text = o305Reader["GRPT"].ToString();
            //                txtPC.Text = o305Reader["planningCalendar"].ToString();
            //                txtLO.Text = o305Reader["LOImplemented"].ToString();
            //                txtOrdering.Text = o305Reader["orderingSystem"].ToString();

            //            }
            //        }

            //        ConnectionModule.Disconect();
            //        lvParts.ItemsSource = information;
            //        lvErrorParts.ItemsSource = errors;

            //        string qry307 = "SELECT * FROM dbo.oldVendors WHERE vendor = @vendor";

            //        SqlCommand my307Command = new SqlCommand(qry307, ConnectionModule.myConnection);
            //        my307Command.Parameters.Add("@vendor", System.Data.SqlDbType.VarChar).Value = Convert.ToString(result.First());

            //        ConnectionModule.Conect();

            //        using (SqlDataReader o307Reader = my307Command.ExecuteReader())
            //        {
            //            while (o307Reader.Read())
            //            {
            //                bol = 1;
            //            }
            //        }
            //        ConnectionModule.Disconect();


            //        if (bol == 0)
            //        {
            //            string qry306 = "INSERT INTO dbo.oldVendors (vendor,vendorName,DUNS,SRT,vfrom,vto,PTF,newPTF,newFixed,GRPT,planningCalendar,LOImplemented,orderingSystem)VALUES(@vendor,@vendorName,@DUNS,@SRT,@from,@to,@PTF,@newPTF,@newFixed,@GRPT,@planningCalendar,@LOImplemented,@orderingSystem)";
            //            SqlCommand my306Command = new SqlCommand(qry306, ConnectionModule.myConnection);

            //            my306Command.Parameters.Add("@vendor", System.Data.SqlDbType.VarChar).Value = Convert.ToString(txtVendorNumber.Text);
            //            my306Command.Parameters.Add("@vendorName", System.Data.SqlDbType.VarChar).Value = Convert.ToString(txtVendorName.Text);
            //            my306Command.Parameters.Add("@DUNS", System.Data.SqlDbType.VarChar).Value = Convert.ToString(txtDUNS.Text);
            //            my306Command.Parameters.Add("@SRT", System.Data.SqlDbType.VarChar).Value = Convert.ToString(txtSRT.Text);
            //            my306Command.Parameters.Add("@from", System.Data.SqlDbType.Date).Value = Convert.ToDateTime(txtFrom.Text).ToShortDateString();
            //            my306Command.Parameters.Add("@to", System.Data.SqlDbType.Date).Value = Convert.ToDateTime(txtTo.Text).ToShortDateString();
            //            my306Command.Parameters.Add("@PTF", System.Data.SqlDbType.VarChar).Value = Convert.ToString("");
            //            my306Command.Parameters.Add("@newPTF", System.Data.SqlDbType.VarChar).Value = Convert.ToString("");
            //            my306Command.Parameters.Add("@newFixed", System.Data.SqlDbType.VarChar).Value = Convert.ToString(txtFixed.Text);
            //            my306Command.Parameters.Add("@GRPT", System.Data.SqlDbType.VarChar).Value = Convert.ToString(txtGRPT.Text);
            //            my306Command.Parameters.Add("@planningCalendar", System.Data.SqlDbType.VarChar).Value = Convert.ToString(txtPC.Text);
            //            my306Command.Parameters.Add("@LOImplemented", System.Data.SqlDbType.VarChar).Value = Convert.ToString(txtLO.Text);
            //            my306Command.Parameters.Add("@orderingSystem", System.Data.SqlDbType.VarChar).Value = Convert.ToString(txtOrdering.Text);

            //            ConnectionModule.Conect();
            //            my306Command.ExecuteNonQuery();
            //            Cursor = Cursors.Arrow;
            //            ConnectionModule.Disconect();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //    Cursor = Cursors.Arrow;
            //});
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            MessageBox.Show(e.ProgressPercentage.ToString());
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {

            //string eMemberID = "";
            //string qry101 = "SELECT * FROM dbo.Members WHERE netID = @net";
            //SqlCommand my101Command = new SqlCommand(qry101, ConnectionModule.myConnection);
            //my101Command.Parameters.Add("@net", System.Data.SqlDbType.VarChar).Value = Convert.ToString(Environment.UserName);
            //ConnectionModule.Conect();
            //using (SqlDataReader c101Reader = my101Command.ExecuteReader())
            //{
            //    while (c101Reader.Read())
            //    {
            //        eMemberID = c101Reader["idMember"].ToString();
            //    }
            //    ConnectionModule.Disconect();
            //}

            //string projectName = "Vendor change " + vendorName + " to " + txtNewVendorName.Text;
            //GlobalVariables.projecViewNav = projectName;

            //foreach (info pn in information)
            //{
            //    await vc.SetPartNumbers(pn.plant, pn.part, projectName, vendorName, txtNewVendorName.Text);
            //}

            //string description = "Vendor change from " + vendorName + " to " + txtNewVendorName.Text;

            //string qry102 = "INSERT INTO dbo.Projects (projectName,projectDescription,startDate,area) values (@name,@description,@startDate,@area);SELECT SCOPE_IDENTITY();";
            //SqlCommand my102Command = new SqlCommand(qry102, ConnectionModule.myConnection);
            //// create parameters to avoid sql injection
            //my102Command.Parameters.Add("@name", System.Data.SqlDbType.VarChar).Value = Convert.ToString(projectName);
            //my102Command.Parameters.Add("@description", System.Data.SqlDbType.VarChar).Value = Convert.ToString(description);
            //my102Command.Parameters.Add("@startDate", System.Data.SqlDbType.Date).Value = Convert.ToDateTime(DateTime.Now);
            //my102Command.Parameters.Add("@area", System.Data.SqlDbType.VarChar).Value = Convert.ToString("SCM");
            //try
            //{
            //    //connect to the database, excecute the query and delete the information from the fields
            //    ConnectionModule.Conect();

            //    int ident = Convert.ToInt32(my102Command.ExecuteScalar());


            //    //now we insert it into the projectTask table to asosiate the task with the project, the project id comes from the load of the project grid
            //    //a task only can be asosiated with the project where you click add project
            //    string qry103 = "INSERT INTO dbo.ProjectMembers (idProject,idMember) VALUES (@idP,@idM);";

            //    SqlCommand my103Command = new SqlCommand(qry103, ConnectionModule.myConnection);

            //    my103Command.Parameters.Add("@idP", System.Data.SqlDbType.Int).Value = Convert.ToInt32(ident);
            //    my103Command.Parameters.Add("@idM", System.Data.SqlDbType.Int).Value = Convert.ToInt32(eMemberID);
            //    my103Command.ExecuteNonQuery();
            //}
            //catch (SqlException ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
            //ConnectionModule.Disconect();

            //var ProjectView = new ProjectView();
            //Content = ProjectView;
        }

        private void txtNewVendorNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            //string vendor = txtNewVendorNumber.Text;
            //if (vendor.Length < 7)
            //{
            //    txtNewVendorName.Text = "";
            //    txtNewDUNS.Text = "";
            //    txtNewSRT.Text = "";
            //    txtNewCN.Text = "";
            //    txtNewPCDes.Text = "";
            //    txtNewFixed.Text = "";
            //    txtNewGRPT.Text = "";
            //    txtNewPC.Text = "";
            //    txtNewLO.Text = "";
            //    txtNewOrdering.Text = "";
            //}

            //if (vendor.Length > 6)
            //{
            //    string qry305 = "SELECT * FROM dbo.VendorChanges WHERE vendor = @vendor";

            //    SqlCommand my305Command = new SqlCommand(qry305, ConnectionModule.myConnection);
            //    my305Command.Parameters.Add("@vendor", System.Data.SqlDbType.VarChar).Value = Convert.ToString(vendor);

            //    ConnectionModule.Conect();

            //    using (SqlDataReader o305Reader = my305Command.ExecuteReader())
            //    {
            //        if (o305Reader.Read())
            //        {
            //            //start the reader and create a button for each element in the database, then add them to a wrappanel
            //            txtNewVendorName.Text = o305Reader["vendorName"].ToString();
            //            txtNewDUNS.Text = o305Reader["DUNS"].ToString();
            //            txtNewSRT.Text = o305Reader["SRT"].ToString();
            //            txtNewPCDes.Text = o305Reader["PCDesc"].ToString();
            //            txtNewCN.Text = o305Reader["CN"].ToString();
            //            txtNewFixed.Text = o305Reader["newFixed"].ToString();
            //            txtNewGRPT.Text = o305Reader["GRPT"].ToString();
            //            txtNewPC.Text = o305Reader["planningCalendar"].ToString();
            //            txtNewLO.Text = o305Reader["LOImplemented"].ToString();
            //            txtNewOrdering.Text = o305Reader["orderingSystem"].ToString();

            //        }
            //    }

            //    ConnectionModule.Disconect();
            //}
        }

        private void btnContinuar_Click(object sender, RoutedEventArgs e)
        {
            PasteLayer.Visibility = Visibility.Hidden;
            ErrorLayer.Visibility = Visibility.Hidden;
            VendorsLayer.Visibility = Visibility.Visible;
        }
    }
}
