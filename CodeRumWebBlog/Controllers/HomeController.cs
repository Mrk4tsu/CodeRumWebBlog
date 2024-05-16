using Model.DAO;
using System.Configuration;
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
    }
}