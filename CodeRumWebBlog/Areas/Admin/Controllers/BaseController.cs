using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CodeRumWebBlog.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var session = Session[CommonConstants.USER_SESSION];
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new
                    {
                        controller = "Authenticate",
                        action = "Login",
                    }));
            }
            base.OnActionExecuted(filterContext);
        }
        protected void SetAlert(string mess, string type)
        {
            TempData["AlertMessage"] = mess;
            switch (type)
            {
                case "sucess":
                    TempData["AlertType"] = "alert-success";
                    break;
                case "warning":
                    TempData["AlertType"] = "alert-warning";
                    break;
                case "error":
                    TempData["AlertType"] = "alert-danger";
                    break;
            }
        }
    }
}