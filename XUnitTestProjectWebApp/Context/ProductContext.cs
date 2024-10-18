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
    }
}

