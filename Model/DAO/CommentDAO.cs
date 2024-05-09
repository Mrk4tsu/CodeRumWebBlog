using Microsoft.SqlServer.Server;
using Model.Entity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class CommentDAO
    {
        public CodeRumDbContext db = null;
        public CommentDAO()
        {
            db = new CodeRumDbContext();
        }

        public IEnumerable<Comment> ListByContent(long contentId, int page, int pageSize)
        {
            var model = (from c in db.Comments
                            where c.PostId == contentId
                            select c);

            return model.OrderByDescending(x => x.CreateAt).ToPagedList(page, pageSize);
        }
        public async Task<Comment> GetByIDAsync(long id)
        {
            return await db.Comments.FindAsync(id);
        }
        public async Task<long> InsertAsync(Comment comment)
        {
            comment.CreateAt = DateTime.Now;
            comment.Status = true;
            db.Comments.Add(comment);

            await db.SaveChangesAsync();
            return comment.Id;
        }
        public async Task UpdateAsync(Comment comment)
        {
            var entity = await db.Comments.FindAsync(comment.Id);
            if (entity != null)
            {
                entity.Content = comment.Content;
                await db.SaveChangesAsync();
            }
        }
    }
}
