using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeRumWebBlog.Controllers
{
    public class BaseController : Controller
    {
        protected void SetAlert(string mess, string type)
        {
            TempData["AlertClientMessage"] = mess;
            switch (type)
            {
                case "sucess":
                    TempData["AlertClientType"] = "alert-success";
                    break;
                case "warning":
                    TempData["AlertClientType"] = "alert-warning";
                    break;
                case "error":
                    TempData["AlertClientType"] = "alert-danger";
                    break;
            }
        }
    }
}