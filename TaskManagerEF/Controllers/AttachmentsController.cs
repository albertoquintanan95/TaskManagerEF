using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaskManagerEF.Models;

namespace TaskManagerEF.Controllers
{
    class AttachmentsController
    {
        MetroWindow metroWindow = (Application.Current.MainWindow as MetroWindow);
        TaskManagerEntities_DB _db = new TaskManagerEntities_DB();

        public List<string> GetAttachments(int idProject, int idTask)
        {
            List<string> attachments = new List<string>();

            foreach (Attachment A in _db.Attachments.Where(x => x.idProject == idProject).Where(x => x.idTask == idTask).ToList())
            {
                string tempFile = "";
                tempFile = Path.Combine(@"C:\temp\", A.name);
                File.WriteAllBytes(tempFile/* + ext*/, A.attachment1);
                attachments.Add(A.name);
            }

            return attachments;
        }

        public void StartProcess(int idProject)
        {
            Attachment A = _db.Attachments.Where(x => x.idProject == idProject).FirstOrDefault();
            string tempFile = "";
            tempFile = Path.Combine(@"C:\temp\", A.name);
            File.WriteAllBytes(tempFile/* + ext*/, A.attachment1);
        }

        public async Task<string> AddAttachmet(string idTask, int idProject)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                //Initialize byte array with a null value initially.
                byte[] data = null;

                //Use FileInfo object to get file size.
                FileInfo fInfo = new FileInfo(openFileDialog.FileName);
                long numBytes = fInfo.Length;

                //Open FileStream to read file
                FileStream fStream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);

                //Use BinaryReader to read file stream into byte array.
                BinaryReader br = new BinaryReader(fStream);

                //When you use BinaryReader, you need to supply number of bytes to read from file.
                //In this case we want to read entire file. So supplying total number of bytes.
                data = br.ReadBytes((int)numBytes);

                //Close BinaryReader
                br.Close();

                //Close FileStream
                fStream.Close();

                try
                {
                    if (idTask != null && idTask != "")
                    {
                        Attachment A = _db.Attachments.Add(new Attachment { attachment1 = data, name = fInfo.Name, ext = fInfo.Extension, idProject = idProject, idTask = Convert.ToInt32(idTask) });
                        _db.SaveChanges();
                        return A.name;
                    }
                    else
                    {
                        Attachment A = _db.Attachments.Add(new Attachment { attachment1 = data, name = fInfo.Name, ext = fInfo.Extension, idProject = idProject });
                        _db.SaveChanges();
                        return A.name;
                    }

                }
                catch (Exception ex)
                {
                    await metroWindow.ShowMessageAsync("Warning", ex.Message);
                }
            }
            return "";
        }

        public List<string> GetProjectAttachments(int idProject)
        {
            if (!Directory.Exists(@"C:\temp\"))
            {
                Directory.CreateDirectory(@"C:\temp\");
            }

            List<string> attachments = new List<string>();

            foreach (ProjectAttachment A in _db.ProjectAttachments.Where(x => x.idProject == idProject).ToList())
            {
                string tempFile = "";
                tempFile = Path.Combine(@"C:\temp\", A.name);
                File.WriteAllBytes(tempFile/* + ext*/, A.attachment);
                attachments.Add(A.name);
            }

            return attachments;
        }
    }
}
