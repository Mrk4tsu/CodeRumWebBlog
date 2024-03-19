using CodeRumWebBlog.Areas.Admin.Data;
using Common;
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
    public class ContentController : BaseController
    {
        // GET: Admin/Content
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ContentDAO();
            var model = dao.ListAllPaging(searchString, page, pageSize);

            ViewBag.SearchString = searchString;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Create(Content model)
        {
            if (ModelState.IsValid)
            {
                await new ContentDAO().InsertAsync(model);
                return RedirectToAction("Index");
            }
            SetViewBag();
            return View();
        }
        public ActionResult Detail(long id)
        {
            var model = new ContentDAO().GetByID(id);

            ViewBag.Tags = new ContentDAO().ListTag(id);
            return View(model);
        }
        public void SetViewBag(long? selectedId = null)
        {
            var dao = new CategoryDAO();
            ViewBag.CategoryID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }
    }
}