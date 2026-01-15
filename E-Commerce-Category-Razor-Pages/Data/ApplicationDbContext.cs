using BooksWeb_Category_Razor_Pages.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BooksWeb_Category_Razor_Pages.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Action",
                    DisplayOrder = 1
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Romance",
                    DisplayOrder = 3
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Suspense Thriller",
                    DisplayOrder = 2
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Spiritual",
                    DisplayOrder = 5
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Horror",
                    DisplayOrder = 4
                }
            );
        }
    }
}
