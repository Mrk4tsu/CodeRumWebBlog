using Model.DAO;
using Model.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
namespace CodeRumWebBlog.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        // GET: Admin/Category

        public ActionResult Index(string searchString = "", int page = 1, int pageSize = 10)
        {
            var dao = new CategoryDAO();

            var list = dao.ListAll(searchString, page, pageSize, true);

            ViewBag.SearchString = searchString;
            return View(list);
        }
        public ActionResult HidenCate(string searchString = "", int page = 1, int pageSize = 10)
        {
            var dao = new CategoryDAO();

            var list = dao.ListAll(searchString, page, pageSize, false);

            ViewBag.SearchString = searchString;
            return View(list);
            return View();
        }
        [HttpGet]
        public ActionResult Create() => View();
        [HttpPost]
        public async Task<ActionResult> Create(Category category)
        {
            var session = (UserLogin)Session[Common.CommonConstants.USER_SESSION];
            var dao = new CategoryDAO();
            category.CreateBy = session.UserName;
            long id = await dao.Insert(category);
            if (id > 0)
            {
                SetAlert("Thêm danh mục thành công", "sucess");
                return RedirectToAction("Index", "Category");
            }
            else
            {
                SetAlert("Thêm User không thành công", "error");
                ModelState.AddModelError("", "Thêm người dùng không thành công");
            }
            return View("Index");
        }
        [HttpGet]
        public ActionResult Edit(long id)
        {
            var dao = new CategoryDAO();
            var cate = dao.GetById(id);
            return View(cate);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(Category category)
        {
            var dao = new CategoryDAO();
            var result = await dao.Update(category);
            if (result)
            {
                SetAlert("Chỉnh sửa danh mục thành công", "sucess");
                return RedirectToAction("Index", "Category");
            }
            else
            {
                ModelState.AddModelError("", "Cập nhật danh mục không thành công");
            }
            return View("Index");
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(long id)
        {
            await new CategoryDAO().Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new CategoryDAO().ChangeStatus(id);

            return Json(new
            {
                status = result
            });
        }
    }
}