﻿using System;

namespace CodeRumWebBlog
{
    [Serializable]
    public class UserLogin
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string GroupId { get; set; }
    }
}