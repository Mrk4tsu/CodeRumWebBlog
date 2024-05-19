using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeRumWebBlog.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View("Error");
        }
        public ActionResult BadRequest()
        {
            Response.StatusCode = 400;
            return View("BadRequest");
        }
        public ActionResult Unauthorized()
        {
            Response.StatusCode = 401;
            return View("Unauthorized");
        }
        public ActionResult Forbidden()
        {
            Response.StatusCode = 403;
            return View("Forbidden");
        }
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }
        public ActionResult MethodNotAllowed()
        {
            Response.StatusCode = 405;
            return View("MethodNotAllowed");
        }
    }
}