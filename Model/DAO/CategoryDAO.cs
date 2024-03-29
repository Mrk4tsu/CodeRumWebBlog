﻿using Model.Entity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public IEnumerable<Category> ListAll(string searchString, int page, int pageSize)
        {
            IQueryable<Category> model = db.Categories;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(
                    x => x.Name.Contains(searchString) ||
                    x.MetaKeyword.Contains(searchString)||
                    x.MetaTitle.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreateAt).ToPagedList(page, pageSize);
        }
        public Category GetById(long id)
        {
            return db.Categories.Find(id);
        }
        public async Task<long> Insert(Category category)
        {
            category.CreateAt = DateTime.Now;
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
                cate.MetaTitle = category.MetaTitle;
                cate.SeoTitle = category.SeoTitle;
                cate.DisplayOrder = category.DisplayOrder;
                cate.ModifyDate = DateTime.Now;
                cate.Status = category.Status;
                cate.ShowOnHome = category.ShowOnHome;
                cate.ParentId = category.ParentId;
                cate.MetaKeyword = category.MetaKeyword;

                await db.SaveChangesAsync();

                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public async Task<bool> Delete(long id)
        {
            try
            {
                var cate = GetById(id);
                db.Categories.Remove(cate);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
