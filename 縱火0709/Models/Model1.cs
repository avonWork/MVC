using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace 縱火0709.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserType> UserTypes { get; set; }
        public virtual DbSet<Premission> Premissions { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Hobbys> Hobbys { get; set; }
        public virtual DbSet<ImgClass> ImgClasses { get; set; }
        public virtual DbSet<FileClass> FileClasses { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Experience> Experience { get; set; }

        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<Reply> Reply { get; set; }

        public virtual DbSet<Contact> Contacts { get; set; }
    }
}