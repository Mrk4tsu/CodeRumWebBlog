using Common;
using Model.Entity;
using PagedList;
using System;
using System.Collections.Generic;
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
        public IEnumerable<Content> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Content> model = db.Contents;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) ||
                x.MetaTitle.Contains(searchString)||
                x.Tag.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreateAt).ToPagedList(page, pageSize);
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
        public Content GetByID(long id)
        {
            return db.Contents.Find(id);
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
                    var existedTag = this.CheckTag(tagId);

                    //insert to to tag table
                    if (!existedTag)
                    {
                        this.InsertTag(tagId, tag);
                    }

                    //insert to content tag
                    this.InsertContentTag(content.Id, tagId);

                }
            }

            return content.Id;
        }
        public void InsertTag(string id, string name)
        {
            var tag = new Tag();
            tag.Id = id;
            tag.Name = name;
            db.Tags.Add(tag);
            db.SaveChanges();
        }
        public void InsertContentTag(long contentId, string tagId)
        {
            var contentTag = new ContentTag();
            contentTag.ContentId = contentId;
            contentTag.TagId = tagId;
            db.ContentTags.Add(contentTag);
            db.SaveChanges();
        }
        public bool CheckTag(string id)
        {
            return db.Tags.Count(x => x.Id == id) > 0;
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
