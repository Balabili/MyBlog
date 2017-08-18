using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Domain;

namespace Blog.Repository
{
    public class EntityContext : DbContext
    {
        public EntityContext() : base("myBlog")
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<ViewHistory> Histories { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>().ToTable("User")
        //        .HasKey(k=>k.Id)
        //        .Property(t=>t.Email).HasColumnName("Email").IsRequired();
        //}
    }
}
