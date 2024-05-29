using Common;
using Model.Entity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Model.DAO
{
    public class CategoryDAO
    {
        private CodeRumDbContext db = null;
        public CategoryDAO()
        {
            db = new CodeRumDbContext();
        }
        public List<Category> ListAll()
        {
            return db.Categories.Where(x => x.Status == true).ToList();
        }
        public IEnumerable<Category> ListAll(string searchString, int page, int pageSize, bool isStatus)
        {
            IQueryable<Category> model = db.Categories;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(
                    x => x.Name.Contains(searchString) ||
                    x.MetaKeyword.Contains(searchString)||
                    x.MetaTitle.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreateAt).Where(c => c.Status == isStatus).ToPagedList(page, pageSize);
        }
        public Category GetById(long? id)
        {
            return db.Categories.Find(id);
        }
        public async Task<long> Insert(Category category)
        {
            category.CreateAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "SE Asia Standard Time");
            //Xử lý alias
            if (string.IsNullOrEmpty(category.MetaTitle))
            {
                category.MetaTitle = StringHelper.ToUnsignString(category.Name);
            }
            db.Categories.Add(category);
            await db.SaveChangesAsync();

            return category.Id;
        }
        public async Task<bool> Update(Category category)
        {
            try
            {
                var cate = GetById(category.Id);
                cate.Name = category.Name;
                if (string.IsNullOrEmpty(category.MetaTitle))
                {
                    cate.MetaTitle = StringHelper.ToUnsignString(category.Name);
                }
                cate.SeoTitle = category.SeoTitle;
                cate.DisplayOrder = category.DisplayOrder;
                cate.ModifyDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "SE Asia Standard Time");
                cate.Status = cate.Status;
                cate.ShowOnHome = category.ShowOnHome;
                cate.ParentId = category.ParentId;
                cate.MetaKeyword = category.MetaKeyword;

                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Delete(long id)
        {
            try
            {
                var cate = GetById(id);

                cate.Status = false;
                cate.ShowOnHome = false;

                //db.Categories.Remove(cate);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> Active(long id)
        {
            try
            {
                var cate = GetById(id);

                cate.Status = !cate.Status;

                //db.Categories.Remove(cate);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool ChangeStatus(long id)
        {
            var cate = db.Categories.Find(id);

            cate.Status = !cate.Status;
            db.SaveChanges();

            return cate.Status;
        }
    }
}
