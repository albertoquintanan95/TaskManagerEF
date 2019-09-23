using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerEF.Models;
namespace TaskManagerEF.Controllers
{
    class MembersController
    {
        TaskManagerEntities_DB _db = new TaskManagerEntities_DB();

        public Member SearchMember(Member M)
        {
            return _db.Members.Where(x => x.displayName == M.displayName).FirstOrDefault();
        }

        public Member SearchMember(string NetID)
        {
            return _db.Members.Where(x => x.netID == NetID).FirstOrDefault();
        }

        public Member SearchMemberByDisplayName(string displayName)
        {
            return _db.Members.Where(x => x.displayName == displayName).FirstOrDefault();
        }

        public bool DeleteMember(Member M)
        {
            _db.Members.Remove(_db.Members.Where(x => x.displayName == M.displayName).FirstOrDefault());
            _db.SaveChanges();
            return true;
        }

        public Member AddNewMember(Member M)
        {
            Member Member = _db.Members.Add(new Member { firstName = M.firstName, lastName = M.lastName, displayName = M.displayName, email = M.email, netID = M.netID });
            _db.SaveChanges();
            return Member;
        }
    }
}
