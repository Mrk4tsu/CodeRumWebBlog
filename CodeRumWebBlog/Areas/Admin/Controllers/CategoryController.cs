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
            var dao = new CategoryDAO().ListAll(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(dao);
        }
        [HttpGet]
        public ActionResult Create() => View();
        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.Any, Duration = int.MaxValue)]
        public async Task<ActionResult> Create(Category category)
        {
            var dao = new CategoryDAO();

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