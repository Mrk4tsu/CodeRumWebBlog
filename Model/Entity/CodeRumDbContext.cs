using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Model.Entity
{
    public partial class CodeRumDbContext : DbContext
    {
        public CodeRumDbContext()
            : base("name=CodeRumDbContext")
        {
        }

        public virtual DbSet<About> Abouts { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Content> Contents { get; set; }
        public virtual DbSet<ContentTag> ContentTags { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<MenuType> MenuTypes { get; set; }
        public virtual DbSet<Slide> Slides { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<SystemConfig> SystemConfigs { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<About>()
                .Property(e => e.MetaTitle)
                .IsUnicode(false);

            modelBuilder.Entity<About>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<About>()
                .Property(e => e.ModifyBy)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.ModifyBy)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .Property(e => e.MetaTitle)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .Property(e => e.SeoTitle)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .Property(e => e.ModifyBy)
                .IsUnicode(false);

            modelBuilder.Entity<Comment>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Comment>()
                .Property(e => e.ModifyBy)
                .IsUnicode(false);

            modelBuilder.Entity<Content>()
                .Property(e => e.MetaTitle)
                .IsUnicode(false);

            modelBuilder.Entity<Content>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Content>()
                .Property(e => e.ModifyBy)
                .IsUnicode(false);

            modelBuilder.Entity<SystemConfig>()
                .Property(e => e.Id)
                .IsUnicode(false);

            modelBuilder.Entity<SystemConfig>()
                .Property(e => e.Type)
                .IsUnicode(false);
        }
    }
}
