using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Retail_MVC.Models;

namespace Retail_MVC.DataAccess.Data
{
    public class ApplicationDbContext:IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }

        public DbSet<Category> categories { get; set; }
        public DbSet<Product> products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "sci-fi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "horror", DisplayOrder = 3 }
                );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id=1,Name="product1",Description="desc1",Price=700.00,  CategoryId=1,ImageUrl="" },
                new Product { Id = 2, Name = "product2", Description = "desc2", Price = 800.00,CategoryId=1, ImageUrl = "" },
                new Product { Id = 3, Name = "product3", Description = "desc3", Price = 900.00 ,  CategoryId =2, ImageUrl = "" }


                );
        }
    }
}
