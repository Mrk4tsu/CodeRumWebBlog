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
        public ActionResult Index(string searchString, int page = 1, int pageSize = 1)
        {
            var dao = new ContentDAO();
            var model = dao.ListAllPaging(searchString, page, pageSize);

            ViewBag.SearchString = searchString;           

            return View(model);
        }
        public ActionResult Tag(string tagId, int page = 1, int pageSize = 1)
        {
            var dao = new ContentDAO();
            var model = dao.ListAllByTag(tagId, page, pageSize);
            int totalRecord = dao.CountAllByTag(tagId); // Đếm tổng số bản ghi

            ViewBag.Total = totalRecord;
            ViewBag.Page = page;

            ViewBag.Tag = new ContentDAO().GetTag(tagId);
            int maxPage = 5;

            int totalPage = (int)Math.Ceiling((double)(totalRecord / (double)pageSize));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;
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
        public async Task<ActionResult> Detail(long id, int page = 1, int pageSize = 5)
        {
            var model = await new ContentDAO().ViewDetail(id);

            ViewBag.Tags = new ContentDAO().ListTag(id);
            ViewBag.Comments = new CommentDAO().ListByContent(id, page, pageSize);
            return View(model);
        }
        [HttpGet]
        public async Task<ActionResult> Edit(long id)
        {
            var dao = new ContentDAO();
            var content = await dao.GetByID(id);
            ViewBag.Content = content.Detail;
            SetViewBag(content.CategoryId);

            return View(content);
        }
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(Content model, string detailContent)
        {
            if (ModelState.IsValid)
            {
                var dao = new ContentDAO();

                model.Detail = detailContent;

                var result = await dao.EditAsyn(model);
                if (result)
                {
                    SetAlert("Chỉnh sửa bài viết thành công", "sucess");
                    return RedirectToAction("Index", "Content");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật bài viết không thành công");
                }
                return View("Index", "Content");
            }
            SetViewBag(model.CategoryId);
            return View();
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(long id)
        {
            await new ContentDAO().DeleteAsyn(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult AddComment(long ContentId, string CommentText)
        {
            var comment = new Comment
            {
                PostId = ContentId,
                Content = CommentText,
                CreateAt = DateTime.Now,
                Status = true
            };

            var commentDao = new CommentDAO();
            commentDao.Insert(comment);

            var commentTime = comment.CreateAt.ToString();

            return Json(new
            {
                success = true,
                commentTime = commentTime,
            });
        }
        [HttpPost]
        public async Task<ActionResult> EditComment(long commentId, string commentText)
        {
            var commentDao = new CommentDAO();
            var comment = await commentDao.GetByID(commentId);
            if (comment != null)
            {
                comment.Content = commentText;
                commentDao.Update(comment);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        public void SetViewBag(long? selectedId = null)
        {
            var dao = new CategoryDAO();
            ViewBag.CategoryID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }
    }
}