using Model.Entity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Model.DAO
{
    public abstract class BaseDAO<T> where T : class, ISearchable
    {
        protected CodeRumDbContext db = null;
        public BaseDAO()
        {
            db = new CodeRumDbContext();
        }
        public IEnumerable<T> ListAllByPaging(string searchString, int page, int pageSize)
        {
            IQueryable<T> model = db.Set<T>();
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(
                    x => x.MetaKeyword.Contains(searchString) ||
                    x.MetaTitle.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreateAt).ToPagedList(page, pageSize);
        }
    }
    public interface ISearchable
    {
        string MetaKeyword { get; set; }
        string MetaTitle { get; set; }
        DateTime CreateAt { get; set; }
    }
}
