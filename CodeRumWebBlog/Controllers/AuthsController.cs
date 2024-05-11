using BotDetect.Web.UI.Mvc;
using Common;
using Model.DAO;
using Model.Entity;
using Model.ViewModel;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CodeRumWebBlog.Controllers
{
    public class AuthsController : Controller
    {
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [CaptchaValidation("CaptchaCode", "registerCapcha", "Mã xác nhận không chính xác!")]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new AccountDAO();
                if (dao.IsUsenameExist(model.Username))
                {
                    ModelState.AddModelError("", "Username đã tồn tại");
                }
                else if (dao.IsEmailExist(model.Email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }
                else
                {
                    var user = new Account();
                    user.Username = model.Username;
                    user.Password = model.Password;
                    user.Email = model.Email;
                    user.Name = model.Name;
                    user.RoleId = "customer";
                    user.CreateAt = DateTime.Now;
                    user.Status = true;

                    var result = await dao.InsertAsync(user);
                    if (result > 0)
                    {
                        ViewBag.Success = "Tạo tài khoản thành công!";
                        model = new RegisterModel();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng ký tài khoản không thành công!");
                    }
                }
            }
            return View(model);
        }
        // GET: Auths
        public ActionResult Login()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userDAO = new AccountDAO();
                var result = userDAO.Login(model.Username, model.Password);

                if (result == 1)
                {
                    var user = userDAO.GetByUsername(model.Username);

                    var userSession = new UserLogin();
                    userSession.UserName = user.Username;
                    userSession.UserId = user.Id;
                    userSession.Avatar = user.Avatar;
                    userSession.Name = user.Name;
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
            return PartialView("Login", model);
        }
        public ActionResult Logout()
        {
            Session[Common.CommonConstants.USER_SESSION] = null;
            return Redirect("/");
        }
    }
}