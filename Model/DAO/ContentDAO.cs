﻿using Common;
using Model.Entity;
using Model.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace Model.DAO
{
    public class ContentDAO
    {
        private CodeRumDbContext db = null;
        public ContentDAO()
        {
            db = new CodeRumDbContext();
        }
        public async Task<IEnumerable<Content>> ListAll()
        {
            return await db.Contents.Where(x => x.Status == true).ToListAsync();
        }
        //Danh sách bài viết đã được duyệt
        public IEnumerable<Content> ListAllPaging(string searchString, int page, int pageSize, bool isLock)
        {

            IQueryable<Content> model = db.Contents;

            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) ||
                x.MetaTitle.Contains(searchString) ||
                x.Tag.Contains(searchString) ||
                x.CreateBy.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreateAt).Where(c => c.Locked == isLock).ToPagedList(page, pageSize);
        }
        //Status = true (Trạng thái Hiện), Locked = false (Trạng thái phê duyệt, cấm)
        public IEnumerable<Content> ListAllPagingPublic(string searchString, int page, int pageSize)
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
            return model.Select(x => x.Content).OrderByDescending(x => x.CreateAt).Where(c => c.Status == true && c.Locked == false).ToPagedList(page, pageSize);
        }
        public IEnumerable<Content> ListAllByTag(string tag, int page, int pageSize)
        {
            var model = (from a in db.Contents
                         join b in db.ContentTags
                         on a.Id equals b.ContentId
                         where b.TagId == tag && a.Status == true
                         select new
                         {
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Image = a.Image,
                             Description = a.Description,
                             CreatedDate = a.CreateAt,
                             CreatedBy = a.CreateBy,
                             ViewCount = a.ViewCount,
                             ID = a.Id

                         }).AsEnumerable().Select(x => new Content()
                         {
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Image = x.Image,
                             Description = x.Description,
                             CreateAt = x.CreatedDate,
                             CreateBy = x.CreatedBy,
                             ViewCount = x.ViewCount,
                             Id = x.ID
                         });
            return model.OrderByDescending(x => x.CreateAt)
                .ToPagedList(page, pageSize);
        }
        public IEnumerable<Content> GetTop3ContentMostView()
        {
            return db.Contents.Where(c => c.Status == true).OrderByDescending(c => c.ViewCount).Take(3).ToList();
        }
        public IEnumerable<Content> GetTop3ContentNewest()
        {
            return db.Contents.Where(c => c.Status == true).OrderByDescending(c => c.CreateAt).Take(3).ToList();
        }
        public IEnumerable<Content> GetRecentPosts()
        {
            return db.Contents.Where(c => c.Status == true && c.Locked == false)
                .OrderByDescending(c => c.CreateAt).ToList();
        }
        public IEnumerable<ContentViewModel> ListByCategoryId(long categoryID, ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {
            totalRecord = db.Contents.Where(x => x.CategoryId == categoryID).Count();
            var model = (from a in db.Contents
                         join b in db.Categories
                         on a.CategoryId equals b.Id
                         where a.CategoryId == categoryID
                         select new
                         {
                             Description = a.Description,
                             CateName = b.Name,
                             CreatedDate = a.CreateAt,
                             CreatedBy = a.CreateBy,
                             ID = a.Id,
                             Images = a.Image,
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Detail = a.Detail,
                             ViewCount = a.ViewCount
                         }).AsEnumerable().Select(x => new ContentViewModel()
                         {
                             Description = x.Description,
                             CateName = x.Name,
                             CreatedDate = x.CreatedDate,
                             CreatedBy = x.CreatedBy,
                             ID = x.ID,
                             Images = x.Images,
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Detail = x.Detail,
                             ViewCount = x.ViewCount
                         });
            model.OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return model.ToPagedList(pageIndex, pageSize);
        }
        public int CountAll()
        {
            return db.Contents.Count();
        }
        public int CountLockContent()
        {
            return (from a in db.Contents
                    where a.Locked == true
                    select a).Count();
        }
        public int CountAllByTag(string tag)
        {
            return (from a in db.Contents
                    join b in db.ContentTags
                    on a.Id equals b.ContentId
                    where b.TagId == tag
                    select a).Count();
        }
        public int GetCommentCountByContentId(long contentId)
        {
            return db.Comments.Count(c => c.PostId == contentId && c.Status == true);
        }
        public async Task<Content> GetByIDAsync(long id)
        {
            return await db.Contents.FindAsync(id);
        }
        public async Task<long> InsertAsync(Content content, HttpPostedFileBase contentImage, string mapPath, string createdBy)
        {
            //Xử lý alias
            if (string.IsNullOrEmpty(content.MetaTitle))
            {
                content.MetaTitle = StringHelper.ToUnsignString(content.Name);
            }

            content.CreateAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "SE Asia Standard Time");
            content.ViewCount = 0;
            content.Status = false;//Ẩn bài viết
            content.Locked = true;//Khóa bài viết

            db.Contents.Add(content);
            await db.SaveChangesAsync();


            var image = FileStoreCommon.SaveUploadedFile(contentImage, mapPath, $"{content.Id}", "content", 50L);
            content.Image = $"/uploads/{createdBy}/images/{image}";

            db.Entry(content).State = EntityState.Modified;
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
                var content = await GetByIDAsync(entity.Id);
                //Xử lý alias
                if (string.IsNullOrEmpty(entity.MetaTitle))
                {
                    content.MetaTitle = StringHelper.ToUnsignString(entity.Name);
                }

                content.Name = entity.Name;
                content.Description = entity.Description;
                content.Image = entity.Image;
                content.CategoryId = entity.CategoryId;
                content.Tag = entity.Tag;
                content.MetaKeyword = entity.MetaKeyword;
                content.Detail = entity.Detail;
                content.Status = content.Status;
                content.ViewCount = content.ViewCount;
                content.ModifyDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "SE Asia Standard Time");

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
                db.Entry(content).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Log the exception here
                return false;
            }
        }
        public async Task<bool> EditAsyn(Content entity, HttpPostedFileBase @base, string createdBy)
        {
            try
            {
                var content = await GetByIDAsync(entity.Id);
                //Xử lý alias
                if (string.IsNullOrEmpty(entity.MetaTitle))
                {
                    content.MetaTitle = StringHelper.ToUnsignString(entity.Name);
                }

                content.Name = entity.Name;
                content.Description = entity.Description;
                if (!string.IsNullOrEmpty(entity.Image))
                {
                    content.Image = FileStoreCommon.SaveUploadedFile(@base, createdBy, $"{content.Id}", "content");
                }
                content.CategoryId = entity.CategoryId;
                content.Tag = entity.Tag;
                content.MetaKeyword = entity.MetaKeyword;
                content.Detail = entity.Detail;
                content.Status = content.Status;
                content.ViewCount = content.ViewCount;
                content.ModifyDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "SE Asia Standard Time");

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
                db.Entry(content).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Log the exception here
                return false;
            }
        }
        public async Task<bool> DeleteAsyn(long id)
        {
            var content = await GetByIDAsync(id);

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
        public async Task<bool> ApproveAsync(long id)
        {
            var content = await GetByIDAsync(id);

            if (content != null)
            {
                content.Locked = false;
                content.Status = true;
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<Content> ViewDetail(long id)
        {
            var content = await GetByIDAsync(id);
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
        public async Task<Tag> GetTagAsync(string id)
        {
            return await db.Tags.FindAsync(id);
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
                             Name = a.Name,
                         }).AsEnumerable().Select(x => new Tag()
                         {
                             Id = x.ID,
                             Name = x.Name
                         });
            return model.ToList();
        }
        public async Task<bool> DeleteByIdUser(long userId)
        {
            var user = await GetByIDAsync(userId);

            if (user != null)
            {
                var list = from a in db.Contents
                           join b in db.Accounts
                           on a.CreateBy equals b.Username
                           where b.Id == userId
                           select a;
                return true;
            }
            return false;
        }
        public bool ChangeStatus(long id)
        {
            var content = db.Contents.Find(id);

            content.Status = !content.Status;
            db.SaveChanges();

            return content.Status;
        }
    }
}
