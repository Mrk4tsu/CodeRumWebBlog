using CodeRumWebBlog.Areas.Admin.Data;
using Common;
using Model.DAO;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

namespace CodeRumWebBlog.Areas.Admin.Controllers
{
    public class AuthenticateController : Controller
    {
        [HttpGet] 
        public ActionResult Login()
        {
            return View();
        }
        // GET: Authenticate
        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.Client, Duration = int.MaxValue)]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var userDAO = new AccountDAO();
                var result = userDAO.Login(model.UserName, model.Password, true);

                if (result == 1)
                {
                    var user = userDAO.GetByUsername(model.UserName);

                    var userSession = new UserLogin();
                    userSession.UserName = user.Username;
                    userSession.UserId = user.Id;
                    userSession.GroupId = user.GroupId;

                    var listCredential = userDAO.GetListCredential(model.UserName);
                    Session.Add(CommonConstants.SESSION_CREDENTIALS, listCredential);
                    Session.Add(CommonConstants.USER_SESSION, userSession);

                    return RedirectToAction("Index", "Home");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khoá.");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng.");
                }
                else if (result == -3)
                {
                    ModelState.AddModelError("", "Tài khoản của bạn không có quyền đăng nhập.");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập không thành công.");
                }

            }
            return View(model);

        }
        public ActionResult LogOut()
        {
            Session[Common.CommonConstants.USER_SESSION] = null;
            return Redirect("Login");
        }
    }
}