using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeRumWebBlog.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            var dao = new AccountDAO();
            var percent = dao.CalculatePercentageOfActiveAccounts();
            ViewBag.Percent = percent;
            return View();
        }
    }
}