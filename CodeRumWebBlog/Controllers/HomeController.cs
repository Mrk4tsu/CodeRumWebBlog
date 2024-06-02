using hbehr.recaptcha;
using Model.DAO;
using System.Configuration;
using System.Net;
using System.Web.Mvc;
using System.Web.UI;

namespace CodeRumWebBlog.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //set SEO
            ViewBag.Title = ConfigurationManager.AppSettings["HomeTitle"];
            ViewBag.KeyWords = ConfigurationManager.AppSettings["HomeKeyWord"];
            ViewBag.Descriptions = ConfigurationManager.AppSettings["HomeDescription"];


            return View();
        }
        public ActionResult CategoryCore()
        {
            var dao = new CategoryDAO();
            var model = dao.ListPrimary(8);

            return PartialView("CategoryCore", model);
        }
        public ActionResult CategorySecondary()
        {
            var dao = new CategoryDAO();
            var model = dao.ListSecondary(8);

            return PartialView("CategorySecondary", model);
        }
        public ActionResult TopContent()
        {
            var dao = new ContentDAO();
            var top = dao.GetTop3ContentMostView();

            ViewBag.TopContents = top;
            return PartialView("TopContent");
        }
        public ActionResult ContentNew()
        {
            var dao = new ContentDAO();
            var top = dao.GetTop3ContentNewest();

            ViewBag.NewContents = top;
            return PartialView("ContentNew");
        }

        public ActionResult Captcha()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Confirm()
        {
            string captchaResponse = Request.Form["g-recaptcha-response"];

            if (ReCaptcha.ValidateCaptcha(captchaResponse))
            {
                return View("Captcha");
            }
            // A Bot
            return RedirectToAction("Robot");
        }
    }
}