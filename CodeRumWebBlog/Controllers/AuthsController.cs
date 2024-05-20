using BotDetect.Web.UI.Mvc;
using Common;
using hbehr.recaptcha;
using Model.DAO;
using Model.Entity;
using Model.ViewModel;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

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
                    user.GroupId = "MEMBER";
                    user.CreateAt = DateTime.Now;
                    user.Avatar = "/uploads/avatar-default.jpg";
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
            //string captchaResponse = Request.Form["g-recaptcha-response"];
            if (ModelState.IsValid)
            {
                var userDAO = new AccountDAO();
                var result = userDAO.SignIn(model.Username, model.Password);
                try
                {
                    if (result)
                    {
                        //var user = userDAO.GetByUsername(model.Username);
                        #region[Session]
                        //var userSession = new UserLogin();
                        //userSession.UserName = user.Username;
                        //userSession.UserId = user.Id;
                        //userSession.GroupId = user.GroupId;

                        //var listCredential = userDAO.GetListCredential(model.Username);
                        //Session.Add(CommonConstants.SESSION_CREDENTIALS, listCredential);
                        //Session.Add(CommonConstants.USER_SESSION, userSession);
                        #endregion
                        #region[Cookies]
                        //// Create the authentication ticket
                        //var authTicket = new FormsAuthenticationTicket(
                        //    1,                             // version
                        //    model.Username,                // user name
                        //    DateTime.Now,                  // created
                        //    DateTime.Now.AddMinutes(20),   // expires
                        //    false,                         // persistent?
                        //    user.GroupId                   // can be used to store roles
                        //);

                        //// Now encrypt the ticket.
                        //string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                        //// Create a cookie and add the encrypted ticket to the
                        //// cookie as data.
                        //var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                        //// Add the cookie to the outgoing cookies collection.
                        //Response.Cookies.Add(authCookie);
                        #endregion
                        //Dặt thời gian hết hạn của vé xác thực dựa trên việc người dùng có tick remember me
                        int timeOut = model.RememberMe ? (40320 * 4) : 1; //5256000 phút = 1 năm

                        //Tạo một vé xác thực Forms mới với username của người dùng
                        var ticket = new FormsAuthenticationTicket(model.Username, model.RememberMe, timeOut);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);

                        cookie.Expires = DateTime.Now.AddMinutes(timeOut);
                        //Đặt HttpOnly = true ngăn cookie được truy cập bởi mã JavaScript
                        cookie.HttpOnly = true;

                        Response.Cookies.Add(cookie);
                        if (Url.IsLocalUrl(returnUrl))
                            return Redirect(returnUrl);
                        else
                            return Redirect("/");
                    }
                    else
                    {
                        SetAlert("Đăng nhập không thành công.", "warning");
                        ModelState.AddModelError("", "Đăng nhập không thành công.");
                    }
                }
                catch
                {
                    return View("Error");
                }
            }
            SetAlert("Đăng nhập không thành công.", "warning");
            return Redirect(returnUrl);
        }
        [Authorize]
        public ActionResult Logout(string returnUrl)
        {
            Session[Common.CommonConstants.USER_SESSION] = null;
            FormsAuthentication.SignOut();

            return Redirect(returnUrl);
        }
    }
}