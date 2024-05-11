using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class ContentCategoryDAO
    {
        public CodeRumDbContext db = null;
        public ContentCategoryDAO() {
            db = new CodeRumDbContext();
        }
        public IEnumerable<ContentCategory> ListAllPaging()
        {
            return null;
        }
    }
}
