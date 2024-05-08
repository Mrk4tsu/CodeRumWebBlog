﻿using Common;
using Model.Entity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class ContentDAO
    {
        private CodeRumDbContext db = null;
        public ContentDAO()
        {
            db = new CodeRumDbContext();
        }
        public IEnumerable<Content> ListAll()
        {
            return db.Contents.Where(x => x.Status == true).ToList();
        }
        //public IEnumerable<Content> ListAllPaging(string searchString, int page, int pageSize)
        //{

        //    IQueryable<Content> model = db.Contents;

        //    if (!string.IsNullOrEmpty(searchString))
        //    {
        //        model = model.Where(x => x.Name.Contains(searchString) ||
        //        x.MetaTitle.Contains(searchString) ||
        //        x.Tag.Contains(searchString) ||
        //        x.CreateBy.Contains(searchString));
        //    }
        //    return model.OrderByDescending(x => x.CreateAt).ToPagedList(page, pageSize);
        //}
        public IEnumerable<Content> ListAllPaging(string searchString, int page, int pageSize)
        {

            var model = from content in db.Contents
                        join account in db.Accounts on content.CreateBy equals account.Username
                        select new { Content = content, Account = account };


            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Content.Name.Contains(searchString) ||
                x.Content.Description.Contains(searchString) ||
                x.Content.Tag.Contains(searchString) ||
                x.Content.CreateBy.Contains(searchString) ||
                x.Account.Name.Contains(searchString) ||
                x.Account.Email.Contains(searchString));
            }
            return model.Select(x => x.Content).OrderByDescending(x => x.CreateAt).ToPagedList(page, pageSize);
        }
        public IEnumerable<Content> ListAllByTag(string tag, int page, int pageSize)
        {
            var model = (from a in db.Contents
                         join b in db.ContentTags
                         on a.Id equals b.ContentId
                         where b.TagId == tag
                         select new
                         {
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Image = a.Image,
                             Description = a.Description,
                             CreatedDate = a.CreateAt,
                             CreatedBy = a.CreateBy,
                             ID = a.Id

                         }).AsEnumerable().Select(x => new Content()
                         {
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Image = x.Image,
                             Description = x.Description,
                             CreateAt = x.CreatedDate,
                             CreateBy = x.CreatedBy,
                             Id = x.ID
                         });
            return model.OrderByDescending(x => x.CreateAt).ToPagedList(page, pageSize);
        }
        public int CountAll()
        {
            return db.Contents.Count();
        }
        public int CountAllByTag(string tag)
        {
            return (from a in db.Contents
                    join b in db.ContentTags
                    on a.Id equals b.ContentId
                    where b.TagId == tag
                    select a).Count();
        }
        public async Task<Content> GetByID(long id)
        {
            return await db.Contents.FindAsync(id);
        }
        public async Task<long> InsertAsync(Content content)
        {
            //Xử lý alias
            if (string.IsNullOrEmpty(content.MetaTitle))
            {
                content.MetaTitle = StringHelper.ToUnsignString(content.Name);
            }
            content.CreateAt = DateTime.Now;
            content.ViewCount = 0;

            db.Contents.Add(content);
            await db.SaveChangesAsync();

            //Xử lý tag
            if (!string.IsNullOrEmpty(content.Tag))
            {
                string[] tags = content.Tag.Split(',');
                foreach (var tag in tags)
                {
                    var tagId = StringHelper.ToUnsignString(tag);
                    var existedTag = await this.CheckTag(tagId);

                    //insert to to tag table
                    if (!existedTag)
                    {
                        await this.InsertTag(tagId, tag);
                    }

                    //insert to content tag
                    await this.InsertContentTag(content.Id, tagId);

                }
            }

            return content.Id;
        }
        public async Task<bool> EditAsyn(Content entity)
        {
            try
            {
                var content = await GetByID(entity.Id);
                //Xử lý alias
                if (string.IsNullOrEmpty(entity.MetaTitle))
                {
                    content.MetaTitle = StringHelper.ToUnsignString(entity.Name);
                }
                content.Name = entity.Name;
                content.Description = entity.Description;
                content.Image = entity.Image;
                content.CategoryId = entity.CategoryId;
                content.ViewCount = entity.ViewCount;
                content.TopHot = entity.TopHot;
                content.Tag = entity.Tag;
                content.MetaKeyword = entity.MetaKeyword;
                content.CreateAt = entity.CreateAt;
                content.Detail = entity.Detail;
                content.Status = entity.Status;
                content.CreateBy = entity.CreateBy;
                content.ModifyBy = entity.ModifyBy;
                content.ModifyDate = DateTime.Now;

                //Xử lý tag
                if (!string.IsNullOrEmpty(entity.Tag))
                {
                    this.RemoveAllContentTag(entity.Id);
                    string[] tags = entity.Tag.Split(',');
                    foreach (var tag in tags)
                    {
                        var tagId = StringHelper.ToUnsignString(tag);
                        var existedTag = await this.CheckTag(tagId);

                        //insert to to tag table
                        if (!existedTag)
                        {
                            await this.InsertTag(tagId, tag);
                        }

                        //insert to content tag
                        await this.InsertContentTag(entity.Id, tagId);

                    }
                }

                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception here
                return false;
            }
        }
        public async Task<bool> DeleteAsyn(long id)
        {
            var content = await GetByID(id);

            if (content != null)
            {
                var comments = db.Comments.Where(c => c.Id == content.Id);
                db.Comments.RemoveRange(comments);

                db.Contents.Remove(content);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }
        //public async Task<bool> EditAsyn(Content entity)
        //{
        //    try
        //    {
        //        var content = await GetByID(entity.Id);
        //        //Xử lý alias
        //        if (string.IsNullOrEmpty(entity.MetaTitle))
        //        {
        //            content.MetaTitle = StringHelper.ToUnsignString(entity.Name);
        //        }
        //        content.Name = entity.Name;
        //        content.Description = entity.Description;
        //        content.Image = entity.Image;
        //        content.CategoryId = entity.CategoryId;
        //        content.ViewCount = entity.ViewCount;
        //        content.TopHot = entity.TopHot;
        //        content.Tag = entity.Tag;
        //        content.MetaKeyword = entity.MetaKeyword;
        //        content.CreateAt = entity.CreateAt;
        //        content.Detail = entity.Detail;
        //        content.Status = entity.Status;
        //        content.CreateBy = entity.CreateBy;
        //        content.ModifyBy = entity.ModifyBy;
        //        content.ModifyDate = DateTime.Now;

        //        await db.SaveChangesAsync();

        //        //Xử lý tag
        //        if (!string.IsNullOrEmpty(entity.Tag))
        //        {
        //            this.RemoveAllContentTag(entity.Id);
        //            string[] tags = entity.Tag.Split(',');
        //            foreach (var tag in tags)
        //            {
        //                var tagId = StringHelper.ToUnsignString(tag);
        //                var existedTag = await this.CheckTag(tagId);

        //                //insert to to tag table
        //                if (!existedTag)
        //                {
        //                    await this.InsertTag(tagId, tag);
        //                }

        //                //insert to content tag
        //                await this.InsertContentTag(entity.Id, tagId);

        //            }
        //        }

        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
        public async Task<Content> ViewDetail(long id)
        {
            var content = await GetByID(id);
            content.ViewCount++;
            await db.SaveChangesAsync();

            return content;
        }
        public async Task InsertTag(string id, string name)
        {
            var tag = new Tag();
            tag.Id = id;
            tag.Name = name;
            db.Tags.Add(tag);
            await db.SaveChangesAsync();
        }
        public async Task InsertContentTag(long contentId, string tagId)
        {
            var contentTag = new ContentTag();
            contentTag.ContentId = contentId;
            contentTag.TagId = tagId;
            db.ContentTags.Add(contentTag);
            await db.SaveChangesAsync();
        }
        public void RemoveAllContentTag(long contentId)
        {
            db.ContentTags.RemoveRange(db.ContentTags.Where(x => x.ContentId == contentId));
            db.SaveChanges();
        }
        public async Task<bool> CheckTag(string id)
        {
            return await db.Tags.CountAsync(x => x.Id == id) > 0;
        }
        public Tag GetTag(string id)
        {
            return db.Tags.Find(id);
        }
        public List<Tag> ListTag(long contentId)
        {
            var model = (from a in db.Tags
                         join b in db.ContentTags
                         on a.Id equals b.TagId
                         where b.ContentId == contentId
                         select new
                         {
                             ID = b.TagId,
                             Name = a.Name
                         }).AsEnumerable().Select(x => new Tag()
                         {
                             Id = x.ID,
                             Name = x.Name
                         });
            return model.ToList();
        }
    }
}
