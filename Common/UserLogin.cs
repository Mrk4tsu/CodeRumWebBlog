using System;

namespace CodeRumWebBlog
{
    [Serializable]
    public class UserLogin
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
    }
}