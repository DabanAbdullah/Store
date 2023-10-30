using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensibility;
using Store.Models;

namespace Store.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Category> catagories{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryID = 1, CatName = "Action", DisplayOrder = 1 },
                new Category { CategoryID = 2, CatName = "Drama", DisplayOrder = 2 }
                );


        }
    }
}
