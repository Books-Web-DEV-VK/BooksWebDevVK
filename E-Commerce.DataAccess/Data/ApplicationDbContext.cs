using BooksWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksWeb.Migrations.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options): base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = Guid.Parse("60592bbc-bc78-4b61-bfb0-925b3a930c23"),
                    Name = "Action",
                    DisplayOrder = 1
                },
                new Category
                {
                    Id = Guid.Parse("ca7b4215-d1ac-4091-a194-2ffee85ed84f"),
                    Name = "Romance",
                    DisplayOrder = 3
                },
                new Category
                {
                    Id = Guid.Parse("8f1a3b4c-5d6e-4f7a-8b9c-0d1e2f3a4b5c"),
                    Name = "Suspense Thriller",
                    DisplayOrder = 2
                },
                new Category
                {
                    Id = Guid.Parse("9a2b4c5d-6e7f-4a8b-9c0d-1e2f3a4b5c6d"),
                    Name = "Spiritual",
                    DisplayOrder = 5
                },
                new Category
                {
                    Id = Guid.Parse("7b3c5d6e-7f8a-4b9c-0d1e-2f3a4b5c6d7e"),
                    Name = "Horror",
                    DisplayOrder = 4
                }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = Guid.Parse("1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d"),
                    Title = "Fortune of Time",
                    Author = "Billy Spark",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus lacinia odio vitae vestibulum vestibulum. Cras venenatis euismod malesuada.",
                    ISBN = "SWD9999001",
                    ListPrice = 99,
                    Price = 90,
                    Price50 = 85,
                    Price100 = 80,
                    CategoryId = Guid.Parse("60592bbc-bc78-4b61-bfb0-925b3a930c23")
                },
                new Product
                {
                    Id = Guid.Parse("2b3c4d5e-6f7a-8b9c-0d1e-2f3a4b5c6d7e"),
                    Title = "Dark Skies",
                    Author = "Nancy Hoover",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus lacinia odio vitae vestibulum vestibulum. Cras venenatis euismod malesuada.",
                    ISBN = "CAW7777001",
                    ListPrice = 40,
                    Price = 30,
                    Price50 = 25,
                    Price100 = 20,
                    CategoryId = Guid.Parse("60592bbc-bc78-4b61-bfb0-925b3a930c23")
                },
                new Product
                {
                    Id = Guid.Parse("3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f"),
                    Title = "Vanish in the Sunset",
                    Author = "Julian Button",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus lacinia odio vitae vestibulum vestibulum. Cras venenatis euismod malesuada.",
                    ISBN = "RITO5555001",
                    ListPrice = 55,
                    Price = 50,
                    Price50 = 40,
                    Price100 = 35, 
                    CategoryId = Guid.Parse("ca7b4215-d1ac-4091-a194-2ffee85ed84f")
                },
                new Product
                {
                    Id = Guid.Parse("4d5e6f7a-8b9c-0d1e-2f3a-4b5c6d7e8f9a"),
                    Title = "Cotton Candy",
                    Author = "Abby Muscles",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus lacinia odio vitae vestibulum vestibulum. Cras venenatis euismod malesuada.",
                    ISBN = "WS3333001",
                    ListPrice = 70,
                    Price = 65,
                    Price50 = 60,
                    Price100 = 55, 
                    CategoryId = Guid.Parse("8f1a3b4c-5d6e-4f7a-8b9c-0d1e2f3a4b5c")
                },
                new Product
                {
                    Id = Guid.Parse("5e6f7a8b-9c0d-1e2f-3a4b-5c6d7e8f9a0b"),
                    Title = "Rock in the Ocean",
                    Author = "Ron Parker",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus lacinia odio vitae vestibulum vestibulum. Cras venenatis euismod malesuada.",
                    ISBN = "SOTJ1111001",
                    ListPrice = 30,
                    Price = 27,
                    Price50 = 25,
                    Price100 = 20,
                    CategoryId = Guid.Parse("9a2b4c5d-6e7f-4a8b-9c0d-1e2f3a4b5c6d")
                }
            );  
        }
    }
}
