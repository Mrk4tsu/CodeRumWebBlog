using Common;
using Model.DAO;
using Model.Entity;
using System;
using System.Drawing;
using System.IO;
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
            var model = dao.ListAllPaging(searchString, page, pageSize, false);

            ViewBag.SearchString = searchString;

            return View(model);
        }
        public async Task<ActionResult> Tag(string tagId, int page = 1, int pageSize = 10)
        {
            var dao = new ContentDAO();
            var model = dao.ListAllByTag(tagId, page, pageSize);
            int totalRecord = dao.CountAllByTag(tagId); // Đếm tổng số bản ghi

            ViewBag.Total = totalRecord;
            ViewBag.Page = page;

            ViewBag.Tag = await new ContentDAO().GetTagAsync(tagId);
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
        #region[Danh sách bài chưa duyệt]
        public ActionResult DisApproveContent(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ContentDAO();
            var model = dao.ListAllPaging(searchString, page, pageSize, true);

            ViewBag.SearchString = searchString;

            return View(model);
        }
        [HttpDelete]
        public async Task<ActionResult> ApproveContent(long id)
        {
            var dao = new ContentDAO();

            var content = await dao.GetByIDAsync(id);

            await dao.ApproveAsync(id);
            var serviceRss = new RssFeedService();
            serviceRss.CreateRssFeed();
            return RedirectToAction("Index");
        }
        #endregion
        #region[Tạo mới]
        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Create(Content model, HttpPostedFileBase Image)
        {

            if (ModelState.IsValid)
            {
                model.CreateBy = session().UserName;
                await new ContentDAO().InsertAsync(content: model, contentImage: Image, mapPath: mapPath(), createdBy: session().UserName);

                SetAlert("Bài viết của bạn đã được tạo, vui lòng chờ quản trị duyệt bài để hiển thị", "sucess");
                return RedirectToAction("Index");
            }
            SetViewBag();
            return View();
        }
        #endregion
        public async Task<ActionResult> Detail(long id, int page = 1, int pageSize = 5)
        {
            var model = await new ContentDAO().ViewDetail(id);

            ViewBag.Tags = new ContentDAO().ListTag(id);
            ViewBag.Comments = new CommentDAO().ListByContent(id, page, pageSize);
            return View(model);
        }
        #region[Chỉnh sửa]
        [HttpGet]
        public async Task<ActionResult> Edit(long id)
        {
            var dao = new ContentDAO();
            var content = await dao.GetByIDAsync(id);
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
        #endregion

        [HttpDelete]
        public async Task<ActionResult> Delete(long id)
        {
            await new ContentDAO().DeleteAsyn(id);
            return RedirectToAction("Index");
        }
        #region[Bình luận]
        [HttpPost]
        public async Task<ActionResult> AddComment(long ContentId, string CommentText)
        {
            var comment = new Comment
            {
                PostId = ContentId,
                Content = CommentText,
                CreateAt = DateTime.Now,
                CreateBy = session().UserName,
                Status = true
            };

            var commentDao = new CommentDAO();
            await commentDao.InsertAsync(comment);

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
            var comment = await commentDao.GetByIDAsync(commentId);
            if (comment != null)
            {
                comment.Content = commentText;
                await commentDao.UpdateAsync(comment);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        #endregion
        public void SetViewBag(long? selectedId = null)
        {
            var dao = new CategoryDAO();
            ViewBag.CategoryID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }
        public JsonResult ChangeStatus(long id)
        {
            var result = new ContentDAO().ChangeStatus(id);

            return Json(new
            {
                status = result
            });
        }
        [NonAction]
        public string SaveUploadedFile(HttpPostedFileBase file, string subFolder, string fail)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var directoryPath = Server.MapPath($"~/uploads/{subFolder}/images");

                // Tạo thư mục nếu không tồn tại
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var filePath = Path.Combine(directoryPath, fileName);

                // Lưu tệp lên máy chủ
                file.SaveAs(filePath);

                return fileName;
            }

            return fail;
        }
    }
}