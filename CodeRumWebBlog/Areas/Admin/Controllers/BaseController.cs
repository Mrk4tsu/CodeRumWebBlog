using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeRumWebBlog.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
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