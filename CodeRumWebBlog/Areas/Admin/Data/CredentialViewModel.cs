using System.Collections.Generic;

namespace CodeRumWebBlog.Areas.Admin.Data
{
    public class CredentialViewModel
    {
        public string UserGroupId { get; set; }
        public List<string> RoleIds { get; set; }
    }
}