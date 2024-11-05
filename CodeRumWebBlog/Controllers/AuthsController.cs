using BotDetect.Web.UI.Mvc;
using Common;
using hbehr.recaptcha;
using Model.DAO;
using Model.Entity;
using Model.ViewModel;
using System;
using System.Net;
using System.Net.Mail;
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
            bool status = false;
            string message = "";

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
                    user.Status = false;
                    user.CodeActive = Guid.NewGuid().ToString();

                    var result = await dao.InsertAsync(user);
                    if (result > 0)
                    {
                        sendEmail(user.Email, user.CodeActive);
                        message = "Đăng ký được thực hiện thành công. Liên kết kích hoạt tài khoản đã được gửi đến id email của bạn: " + user.Email;
                        status = true;
                        model = new RegisterModel();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng ký tài khoản không thành công!");
                        message = "Đăng ký tài khoản không thành công!"; 
                    }
                }
            }
            ViewBag.Message = message;
            ViewBag.Status = status;
            return View(model);
        }
        [HttpGet]
        public ActionResult Verify(string id)
        {
            bool status = false;
            var dao = new AccountDAO();
            var user = dao.GetByCodeActive(id);
            if (user != null)
            {
                dao.ChangeStatus(user.Id);
                status = true;
            }
            else ViewBag.Message = "Yêu cầu không hợp lệ!";

            ViewBag.Status = status;
            return View();
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
            string message = "";
            //string captchaResponse = Request.Form["g-recaptcha-response"];
            if (ModelState.IsValid)
            {
                var userDAO = new AccountDAO();
                var result = userDAO.SignIn(model.Username, model.Password);
                try
                {
                    switch (result)
                    {
                        case 1:
                            //Dặt thời gian hết hạn của vé xác thực dựa trên việc người dùng có tick remember me
                            int timeOut = model.RememberMe ? (40320 * 60) : 1; //5256000 phút = 1 năm

                            //Tạo một vé xác thực Forms mới với username của người dùng
                            var ticket = new FormsAuthenticationTicket(model.Username, model.RememberMe, timeOut);
                            string encrypted = FormsAuthentication.Encrypt(ticket);
                            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);

                            cookie.Expires = DateTime.Now.AddMinutes(timeOut);
                            //Đặt HttpOnly = true ngăn cookie được truy cập bởi mã JavaScript
                            cookie.HttpOnly = true;

                            Response.Cookies.Add(cookie);
                            try
                            {
                                return Redirect(returnUrl);
                            }
                            catch (Exception)
                            {
                                return View(model);
                            }
                        case -1:
                            message = "Vui lòng kiểm tra tài khoản và mật khẩu.";
                            break;
                        case -2:
                            message = "Tài khoản của bạn đang bị tạm khóa.";
                            break;
                        case -3:
                            message = "Vui lòng kiểm tra tài khoản và mật khẩu.";
                            break;
                        case -4:
                            message = "Vui lòng nhập đầy đủ để tiến hành đăng nhập.";
                            break;
                        default:
                            message = "Đăng nhập thành công.";
                            break;
                    }
                }
                catch
                {
                    return View("Error");
                }
            }
            SetAlert(message, "warning");
            return Redirect(returnUrl);
        }
        [Authorize]
        public ActionResult Logout(string returnUrl)
        {
            Session[Common.CommonConstants.USER_SESSION] = null;
            FormsAuthentication.SignOut();

            return Redirect(returnUrl);
        }
        public ActionResult Detail()
        {
            return View();
        }
        [NonAction]
        public void sendEmail(string email, string active)
        {
            var verifyUrl = $"/active/{active}";
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("thang.ndu.63cntt@ntu.edu.vn", "MrKatsu");
            var toEmail = new MailAddress(email);

            var fromEmailPassword = "**********"; // Replace with actual password
            string subject = "Tài khoản của bạn đã được tạo thành công!";

            //string body = "<br/><br/>Chúng tôi vui mừng thông báo với bạn rằng tài khoản để tham gia Coderum Blog của bạn được tạo thành công. Vui lòng nhấp vào liên kết bên dưới để xác minh tài khoản của bạn" +
            //" <br/><br/><a href='" + link + "'>" + "Xác minh tài khoản" + "</a> ";
            string body = StringHelper.EmailBody(link, active);
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })smtp.Send(message);
        }

    }
}
