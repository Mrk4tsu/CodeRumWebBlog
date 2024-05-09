using Model.DAO;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CodeRumWebBlog.Areas.Admin.Controllers
{
    public class CommentController : Controller
    {
        // GET: Admin/Comment
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            await SetViewBag();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(Comment comment)
        {
            if (ModelState.IsValid)
            {
                await new CommentDAO().InsertAsync(comment);
                return RedirectToAction("Index");
            }
            await SetViewBag();
            return View();

        }
        public async Task SetViewBag(long? selectedId = null)
        {
            var dao = new ContentDAO();
            var post = await dao.ListAll();
            ViewBag.PostID = new SelectList(post, "Id", "Name", selectedId);
        }
    }
}