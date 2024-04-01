using Model.DAO;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeRumWebBlog.Areas.Admin.Controllers
{
    public class CommentController : Controller
    {
        // GET: Admin/Comment
        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }
        [HttpPost]
        public ActionResult Create(Comment comment)
        {
            if (ModelState.IsValid)
            {
                new CommentDAO().Insert(comment);
                return RedirectToAction("Index");
            }
            SetViewBag();
            return View();

        }
        public void SetViewBag(long? selectedId = null)
        {
            var dao = new ContentDAO();
            ViewBag.PostID = new SelectList(dao.ListAll(), "Id", "Name", selectedId);
        }
    }
}