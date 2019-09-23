using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using TaskManagerEF.Models;

namespace TaskManagerEF.Controllers
{
    class CommentsController
    {
        TaskManagerEntities_DB _db = new TaskManagerEntities_DB();

        public Paragraph AddTaskComment(int idTask, string comment, int idMember)
        {
            TaskComment Com = new TaskComment { idTask = idTask, idComment = _db.Comments.Add(new Comment { comment1 = comment, date = DateTime.Now }).idComment, idMember = idMember };
            TaskComment TC = _db.TaskComments.Add(Com);
            _db.SaveChanges();

            TaskCommentsView TCV = _db.TaskCommentsViews.Where(x => x.idComment == TC.idComment).FirstOrDefault();

            Paragraph P = new Paragraph();

            P.Inlines.Add(TCV.displayName + " [" + TCV.date + "]");
            P.Inlines.Add(new LineBreak());
            P.Inlines.Add(TCV.comment);

            return P;
        }

        public Paragraph AddProjectComment(int idProject, string comment, int idMember)
        {
            ProjectComment PC = _db.ProjectComments.Add(new ProjectComment { idProject = idProject, idComment = _db.Comments.Add(new Comment { comment1 = comment, date = DateTime.Now }).idComment, idMember = idMember });
            _db.SaveChanges();

            ProjectCommentsView PCV = _db.ProjectCommentsViews.Where(x => x.idComment == PC.idComment).FirstOrDefault();

            Paragraph P = new Paragraph();

            P.Inlines.Add(PCV.displayName + " [" + PCV.date + "]");
            P.Inlines.Add(new LineBreak());
            P.Inlines.Add(PCV.comment);

            return P;
        }

        public List<Paragraph> GetTaskComments(int idTask)
        {
            List<Paragraph> Comments = new List<Paragraph>();

            foreach (TaskCommentsView TCV in _db.TaskCommentsViews.Where(x => x.idTask == idTask).ToList())
            {
                Paragraph P = new Paragraph();

                P.Inlines.Add(TCV.displayName + " [" + TCV.date + "]");
                P.Inlines.Add(new LineBreak());
                P.Inlines.Add(TCV.comment);

                Comments.Add(P);
            }

            return Comments;
        }

        public List<Paragraph> GetProjectComments(int idProject)
        {
            List<Paragraph> Comments = new List<Paragraph>();

            foreach (ProjectCommentsView PCV in _db.ProjectCommentsViews.Where(x => x.idProject == idProject).ToList())
            {
                Paragraph P = new Paragraph();

                P.Inlines.Add(PCV.displayName + " [" + PCV.date + "]");
                P.Inlines.Add(new LineBreak());
                P.Inlines.Add(PCV.comment);

                Comments.Add(P);
            }

            return Comments;
        }
    }
}
