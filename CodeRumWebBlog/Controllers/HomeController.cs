using Model.DAO;
using System.Web.Mvc;
using System.Web.UI;

namespace CodeRumWebBlog.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [OutputCache(Location = OutputCacheLocation.Client, Duration = 3600 * 24)]
        public ActionResult TopContent()
        {
            var dao = new ContentDAO();
            var top = dao.GetTop3ContentMostView();

            ViewBag.TopContents = top;
            return PartialView("TopContent");
        }
        [OutputCache(Location = OutputCacheLocation.Client, Duration = 3600 * 12)]
        public ActionResult ContentNew()
        {
            var dao = new ContentDAO();
            var top = dao.GetTop3ContentNewest();

            ViewBag.NewContents = top;
            return PartialView("ContentNew");
        }
    }
}