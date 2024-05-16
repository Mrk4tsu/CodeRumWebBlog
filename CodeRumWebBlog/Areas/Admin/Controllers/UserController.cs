using Common;
using Model.DAO;
using Model.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CodeRumWebBlog.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Admin/User
        [HasCredential(RoleId = "VIEW_USER")]
        public ActionResult Index(string searchString = "", int page = 1, int pageSize = 10)
        {
            var dao = new AccountDAO().ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(dao);
        }
        #region[Create]
        [HttpGet]
        [HasCredential(RoleId = "ADD_USER")]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }
        [HttpPost]
        [HasCredential(RoleId = "ADD_USER")]
        public async Task<ActionResult> Create(Account account)
        {
            var dao = new AccountDAO();
            SetViewBag();
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
        [HasCredential(RoleId = "EDIT_USER")]
        public async Task<ActionResult> Edit(long id)
        {
            var user = await new AccountDAO().GetByIdAsync(id);
            SetViewBag(user.GroupId);
            return View(user);
        }
        [HttpPost]
        [HasCredential(RoleId = "EDIT_USER")]
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
            SetViewBag(account.GroupId);
            return View("Index");
        }
        #endregion
        [HttpDelete]
        [HasCredential(RoleId = "DELETE_USER")]
        public async Task<ActionResult> Delete(long id)
        {
            await new AccountDAO().Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [HasCredential(RoleId = "EDIT_USER")]
        public JsonResult ChangeStatus(long id)
        {
            var result = new AccountDAO().ChangeStatus(id);

            return Json(new
            {
                status = result
            });
        }
        [HttpPost]
        public async Task<JsonResult> AddComment(long id, Comment comment)
        {
            var session = (UserLogin)Session[Common.CommonConstants.USER_SESSION];
            comment.CreateBy = session.UserName;
            var result = await new CommentDAO().InsertAsync(comment);
            return Json(new
            {
                status = result
            });
        }
        public void SetViewBag(string selectedId = null)
        {
            var dao = new AccountDAO();
            var roles = dao.ListAllRole();
            ViewBag.RoleId = new SelectList(roles, "Id", "Name", selectedId);
        }
    }
}