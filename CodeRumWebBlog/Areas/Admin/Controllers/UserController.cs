using Model.DAO;
using Model.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CodeRumWebBlog.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Admin/User
        public ActionResult Index(string searchString = "", int page = 1, int pageSize = 10)
        {
            var dao = new AccountDAO().ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(dao);
        }
        #region[Create]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(Account account)
        {
            var dao = new AccountDAO();
            if (dao.IsUsenameExist(account.Username))
            {
                SetAlert("Tài khoản đã tồn tại", "error");
                return View(account);
            }
            else if (!dao.IsValidUsername(account.Username))
            {
                SetAlert("Tài khoản ít nhất 6 ký tự, chỉ được sử dụng chữ cái, kí tự '_' và các số", "warning");
                return View(account);
            }
            var encripPassword = account.Password;
            account.Password = encripPassword;

            long id = await dao.InsertAsync(account);
            if (id > 0)
            {
                SetAlert("Thêm User thành công", "sucess");
                return RedirectToAction("Index", "User");
            }
            else
            {
                SetAlert("Thêm User không thành công", "error");
                ModelState.AddModelError("", "Thêm người dùng không thành công");
            }
            return View("Index");
        }
        #endregion
        #region[Edit]
        [HttpGet]
        public ActionResult Edit(long id)
        {
            var user = new AccountDAO().GetById(id);
            return View(user);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(Account account)
        {
            var userDAO = new AccountDAO();
            if (!string.IsNullOrEmpty(account.Password))
            {
                var encryptPassword = account.Password;
                account.Password = encryptPassword;
            }

            var result = await userDAO.Update(account);
            if (result)
            {
                SetAlert("Chỉnh sửa User thành công", "sucess");
                return RedirectToAction("Index", "User");
            }
            else
            {
                ModelState.AddModelError("", "Cập nhật người dùng không thành công");
            }
            return View("Index");
        }
        #endregion
        [HttpDelete]
        public async Task<ActionResult> Delete(long id)
        {
            await new AccountDAO().Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new AccountDAO().ChangeStatus(id);

            return Json(new
            {
                status = result
            });
        }
        [HttpPost]
        public JsonResult AddComment(long id, Comment comment)
        {
            var result = new CommentDAO().Insert(comment);
            return Json(new
            {
                status = result
            });
        }
    }
}