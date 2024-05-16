using BotDetect.Web.UI.Mvc;
using Common;
using Microsoft.Ajax.Utilities;
using Model.DAO;
using Model.Entity;
using Model.ViewModel;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CodeRumWebBlog.Controllers
{
    public class AuthsController : BaseController
    {
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [OutputCache(Duration = 3600, Location = OutputCacheLocation.Client)]
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
                    user.GroupId = "customer";
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
        [HttpGet]
        public ActionResult Login()
        {
            //ViewBag.ReturnUrl = returnUrl;
            return PartialView();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl = "")
        {
            string resultString = "";
            if (ModelState.IsValid)
            {
                var userDAO = new AccountDAO();
                var result = userDAO.Login(model.Username, model.Password);

                if (result)
                {
                    var user = userDAO.GetByUsername(model.Username);

                    var userSession = new UserLogin();
                    userSession.UserName = user.Username;
                    userSession.UserId = user.Id;
                    userSession.GroupId = user.GroupId;

                    var listCredential = userDAO.GetListCredential(model.Username);
                    Session.Add(CommonConstants.SESSION_CREDENTIALS, listCredential);
                    Session.Add(CommonConstants.USER_SESSION, userSession);

                    return Redirect(returnUrl);
                }
                else
                {
                    resultString = "Đăng nhập không thành công.";
                    SetAlert(resultString, "warning");
                    ModelState.AddModelError("", "Đăng nhập không thành công.");
                }

            }
            string results = resultString;
            SetAlert("Đăng nhập không thành công.", "warning");
            return Redirect(returnUrl);
        }
        public ActionResult Logout(string returnUrl)
        {
            Session[Common.CommonConstants.USER_SESSION] = null;
            FormsAuthentication.SignOut();

            return Redirect(returnUrl);
        }
    }
}