using Model.DAO;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeRumWebBlog.Areas.Admin.Controllers
{
    public class CredentialController : Controller
    {
        public ActionResult Index()
        {
            var dao = new CredentialDAO();
            var list  = dao.ListAllUserGroup();
            return View(list);
        }
        public ActionResult UserGroupDetail(string userGroupId)
        {
            var dao = new CredentialDAO();
            var model = dao.GetRoleByUserGroupID(userGroupId);


            var listrole = dao.ListAllRole();
            List<Role> roles = new List<Role>();
            try
            {
                foreach (var role in listrole)
                {
                    if (role.Name != model.First().Name)
                    {
                        roles.Add(role);
                    }
                }
            }
            catch { }
            ViewBag.Role = roles;
            ViewBag.UserGroup = dao.GetUserGroup(userGroupId);
            return View(model);
        }
    }
}