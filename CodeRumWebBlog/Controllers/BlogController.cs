using Model.DAO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using System.ServiceModel.Syndication;
using Common;
using System.Web;
using System.Linq;

namespace CodeRumWebBlog.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ContentDAO();
            var model = dao.ListAllPagingPublic(searchString, page, pageSize);

            ViewBag.SearchString = searchString;


            int totalRecord = dao.CountAll(); // Đếm tổng số bản ghi
            ViewBag.Total = totalRecord;
            ViewBag.Page = page;

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
        public async Task<ActionResult> Detail(long id, int page = 1, int pageSize = 5)
        {
            var model = await new ContentDAO().ViewDetail(id);

            ViewBag.Tags = new ContentDAO().ListTag(id);
            ViewBag.Comments = new CommentDAO().ListByContent(id, page, pageSize);
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
        [HttpPost]
        public JsonResult AddComment(string comment, long contentId)
        {
            string username = HttpContext.User.Identity.Name;
            var dao = new CommentDAO();
            if (!Request.IsAuthenticated)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập tài khoản!" }, JsonRequestBehavior.AllowGet);
            }
            var lastComment = dao.GetLastCommentByUser(username);
            if (lastComment != null)
            {
                var diffInSeconds = (TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "SE Asia Standard Time") - lastComment.CreateAt.Value).TotalSeconds;
                if (diffInSeconds < 300)
                {
                    return Json(new 
                    { 
                        success = false,
                        message = "Bạn mới bình luận lúc: " + lastComment.CreateAt.Value.ToString("hh:mm") + ". Vui lòng đợi 5 phút để bình luận tiếp!" 
                    }, 
                        JsonRequestBehavior.AllowGet);
                }
            }
            var commentEntity = new Model.Entity.Comment
            {
                Content = comment,
                CreateBy = username,
                CreateAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "SE Asia Standard Time"),
                PostId = contentId
            };

            var result = dao.Insert(commentEntity);
            if (result > 0)
            {
                var user = new AccountDAO().GetByUsername(username);
                var isAuthor = (username == user.Username);

                var author = "";
                if (isAuthor) author = "author";
                else author = "visitor";

                return Json(new
                {
                    success = true,
                    message = "Comment added successfully",
                    avatar = user.Avatar,
                    userName = user.Name,
                    userId = user.Id,
                    createAt = DateTime.Now.ToString("dd MMMM yyyy",
                    new System.Globalization.CultureInfo("vi-VN")),
                    commentId = result,
                    isAuthor = author
                },
                    JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Đã xảy ra lỗi!"
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<JsonResult> DeleteComment(long id)
        {
            var dao = new CommentDAO();
            var result = await dao.DeleteAsync(id);
            if (result)
            {
                return Json(new 
                { 
                    success = true,
                    message = "Comment deleted successfully" 
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new 
                { 
                    success = false, 
                    message = "Error deleting comment" 
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}