using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using XUnitTestProjectWebApp.Models;

namespace XUnitTestProjectWebApp.Context
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade); // Kategori silinirse ona bağlı ürünleri,n silinmesini istediğimdenc dolayı cascede kullandım

            //modelBuilder.Entity<Category>().HasData(
            //    new Category { CategoryId = 1, CategoryName = "Switchler" },
            //    new Category { CategoryId = 2, CategoryName = "Routerlar" }
            //);
        }
    }
}
