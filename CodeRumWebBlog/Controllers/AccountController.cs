using Model.DAO;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace CodeRumWebBlog.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public async Task<ActionResult> Detail(long id, string name, int page = 1, int pageSize = 5)
        {
            var dao = new AccountDAO();
            var viewModel = await new AccountDAO().DetailAsyn(id);

            var user = await dao.GetById(id);
            

            ViewBag.CountBlog = await new AccountDAO().CountContentsByCreator(viewModel.Account.Username);
            foreach (var content in viewModel.Contents)
            {
                long contentId = content.Id;
                ViewBag.Comments = new CommentDAO().ListByContent(contentId, page, pageSize);
            }

            name = user.Name;
            return View(viewModel);
        }
    }
}