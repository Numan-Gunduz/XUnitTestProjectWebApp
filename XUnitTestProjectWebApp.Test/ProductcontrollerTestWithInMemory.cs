using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUnitTestProjectWebApp.Context;
using XUnitTestProjectWebApp.Controllers;
using XUnitTestProjectWebApp.Models;

namespace XUnitTestProjectWebApp.Test
{
    public class ProductcontrollerTestWithInMemory : ProductControllerTest
    {
        public ProductcontrollerTestWithInMemory()
        {
            SetContextOptions(new DbContextOptionsBuilder<ProductContext>().UseInMemoryDatabase("XUnıtTestInMemory").Options);

        }
        [Fact]
        public async Task Create_ModelValidProduct_ReturnsTedirecToActionWithSaveProduct()
        {
            var newproduct = new Product { ProductName = "klav", ProductPrice = 200, ProductStock = 210, ProductColor = "Mavil" };

            using (var context = new ProductContext(_contextOptions))
            {

                var category = await context.Categories.FirstAsync();
                newproduct.CategoryId = category.CategoryId;
                var controller = new ProductsController(context);
                var result = await controller.Create(newproduct);
                var redirect = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirect.ActionName);


            }
            using (var context = new ProductContext(_contextOptions))
            {
                var product = context.Products.FirstOrDefault(x => x.ProductName == newproduct.ProductName);
                Assert.Equal(newproduct.ProductName, product.ProductName);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task DeleteCategory_ExistCategoryId_DeleteALlProducts(int categoryId)
        {
            using (var context = new ProductContext(_contextOptions))
            {
                var category=await context.Categories.FindAsync(categoryId);

                context.Categories.Remove(category);
                context.SaveChanges();

            }

            using (var context = new ProductContext(_contextOptions))
            {
                var products = await context.Products.Where(x => x.CategoryId == categoryId).ToListAsync();
                Assert.Empty(products);
            }
        }
    }
}
