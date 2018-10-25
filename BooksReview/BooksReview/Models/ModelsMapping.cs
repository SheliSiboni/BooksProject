
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace BooksReview.Models
{
    public class BooksContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Genere> Generes { get; set; }
        public DbSet<Book> Books{ get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}