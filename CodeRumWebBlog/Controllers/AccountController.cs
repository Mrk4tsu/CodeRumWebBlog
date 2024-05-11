using Model.DAO;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CodeRumWebBlog.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.Client, Duration = int.MaxValue, VaryByParam = "id")]
        public async Task<ActionResult> Detail(long id, string name, int page = 1, int pageSize = 5)
        {
            var dao = new AccountDAO();
            var viewModel = await new AccountDAO().DetailAsyn(id);

            var user = await dao.GetByIdAsync(id);
            

            ViewBag.CountBlog = await new AccountDAO().CountContentsByCreator(viewModel.Account.Username);
            foreach (var content in viewModel.Contents)
            {
                long contentId = content.Id;
                ViewBag.Comments = new CommentDAO().ListByContent(contentId, page, pageSize);
            }

            name = user.Name;
            return View(viewModel);
        }
        public void SetViewBag(string selectedId = null)
        {
            var dao = new AccountDAO();
            var roles = dao.ListAllRole();
            ViewBag.RoleId = new SelectList(roles, "Id", "Name", selectedId);
        }
    }
}