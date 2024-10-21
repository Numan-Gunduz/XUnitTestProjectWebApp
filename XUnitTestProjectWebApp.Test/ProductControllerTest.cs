using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUnitTestProjectWebApp.Context;
using XUnitTestProjectWebApp.Models;

namespace XUnitTestProjectWebApp.Test
{
    public class ProductControllerTest
    {
        protected DbContextOptions<ProductContext> _contextOptions { get; private set; }
        public void SetContextOptions(DbContextOptions<ProductContext> contextOptions)
        {
            _contextOptions = contextOptions;
            Seed();
        }
        public void Seed()
        {
            using (ProductContext context = new ProductContext(_contextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.Categories.Add(new Category { CategoryName = "kalemler" });
                context.Categories.Add(new Category { CategoryName = "Defterler" });
                context.SaveChanges();
                context.Products.Add(new Product() { CategoryId=1,ProductName="kalem",ProductPrice=100,ProductStock=120,ProductColor="Mavi"});
                context.Products.Add(new Product() { CategoryId = 1, ProductName = "silgi", ProductPrice = 110, ProductStock = 110, ProductColor = "bordo" });
                context.SaveChanges();

            }
        }
    }
}
