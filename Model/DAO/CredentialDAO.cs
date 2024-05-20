using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class CredentialDAO
    {
        public CodeRumDbContext db = null;
        public CredentialDAO()
        {
            db = new CodeRumDbContext();
        }
        public List<Role> ListAllRole()
        {
            var list = db.Roles;
            return list.ToList();
        }
        public IEnumerable<UserGroup> ListAllUserGroup()
        {
            var list = db.UserGroups;
            return list;
        }
        public IEnumerable<Role> GetRoleByUserGroupID(string userGroupId)
        {
            var list = (from a in db.Roles
                        join b in db.Credentials
                        on a.Id equals b.RoleId
                        where b.UserGroupId == userGroupId
                        select new
                        {
                            Id = a.Id,
                            Name = a.Name
                        }).AsEnumerable().Select(x => new Role()
                        {
                            Id = x.Id,
                            Name = x.Name
                        });           
            return list.OrderByDescending(x => x.Name).ToList();
        }
        public UserGroup GetUserGroup(string id)
        {
            return db.UserGroups.Find(id);
        }
    }
}
