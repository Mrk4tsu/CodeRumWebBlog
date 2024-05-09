using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CodeRumWebBlog.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        public ActionResult Index(string searchString, int page = 1, int pageSize = 1)
        {
            var dao = new ContentDAO();
            var model = dao.ListAllPaging(searchString, page, pageSize);

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
        public async Task<ActionResult> Tag(string tagId, int page = 1, int pageSize = 1)
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
    }
}