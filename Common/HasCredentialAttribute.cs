using CodeRumWebBlog;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Common
{
    public class HasCredentialAttribute : AuthorizeAttribute
    {
        public string RoleId { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //var isAuthorized = base.AuthorizeCore(httpContext);
            //if (!isAuthorized) return false;

            var session = (UserLogin)HttpContext.Current.Session[CommonConstants.USER_SESSION];
            if (session == null) return false;

            List<string> privilegeLevels = this.GetCredentialByLoggedInUser(session.UserName);

            if (privilegeLevels.Contains(this.RoleId) || session.GroupId == CommonConstants.ADMIN_GROUP) return true;
            else return false;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Areas/Admin/Views/Shared/Error401.cshtml"
            };
        }
        private List<string> GetCredentialByLoggedInUser(string username)
        {
            var credentail =  (List<string>)HttpContext.Current.Session[CommonConstants.SESSION_CREDENTIALS];
            return credentail;
        }
    }
}
