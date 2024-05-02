using CodeRumWebBlog.Areas.Admin.Data;
using Common;
using Model.DAO;
using System.Web.Mvc;

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
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var userDAO = new AccountDAO();
                var result = userDAO.Login(model.UserName, model.Password);

                if (result == 1)
                {
                    var user = userDAO.GetByUsername(model.UserName);

                    var userSession = new UserLogin();
                    userSession.UserName = user.Username;
                    userSession.UserId = user.Id;
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
    }
}