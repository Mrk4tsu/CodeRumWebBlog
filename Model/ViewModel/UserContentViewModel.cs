using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class UserContentViewModel
    {
        public Account Account { get; set; }
        public IEnumerable<Content> Contents { get; set; }
    }
}
