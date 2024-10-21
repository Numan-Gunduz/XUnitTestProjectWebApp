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
//Genel olarak üç aşamdan oluşutordu
//Arrange
//Act
//Assert
namespace XUnitTestProjectWebApp.Test
{
    public class ProductcontrollerTestWithMSSQLServer : ProductControllerTest
    {
        public ProductcontrollerTestWithMSSQLServer()
        {
            var sqlCon = @"Data Source=PC_4254\SQLEXPRESS01;Initial Catalog=XUnıtTestSimilation;Integrated Security=true;TrustServerCertificate=True;MultipleActiveResultSets=true";
            SetContextOptions(new DbContextOptionsBuilder<ProductContext>().UseSqlServer(sqlCon).Options);

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
                var category = await context.Categories.FindAsync(categoryId);

                context.Categories.Remove(category);
                context.SaveChanges();

            }

            using (var context = new ProductContext(_contextOptions))
            {
                var products = await context.Products.Where(x => x.CategoryId == categoryId).ToListAsync();
                Assert.Empty(products);//kategoriye ait ürünlerin silinip silinmediğini de kontrol etmeyii istiyorum.
            }
        }


        [Fact]
        public async Task UpdateProduct_ExistingProduct_UpdatesProductSuccessfully()
        {
            using (var context = new ProductContext(_contextOptions))
            {
                var mevcutUrun = await context.Products.FirstAsync();
                mevcutUrun.ProductPrice = 300;
                var controller = new ProductsController(context);
                var result = await controller.Edit(mevcutUrun.ProductID, mevcutUrun);
                var redirect = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirect.ActionName);
            }

            using (var context = new ProductContext(_contextOptions))
            {
                var updatedProduct = await context.Products.FirstAsync(p => p.ProductID == 1);
                Assert.Equal(300, updatedProduct.ProductPrice);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task GetProductDetails_ExistingProductId_ReturnsProductDetails(int productId)
        {
            using (var context = new ProductContext(_contextOptions))
            {
                var controller = new ProductsController(context);
                var result = await controller.Details(productId);
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<Product>(viewResult.Model);
                Assert.Equal(productId, model.ProductID);
            }
        }
        [Fact]
        public async Task GetAllProducts_ReturnsAllProducts()
        {
            using (var context = new ProductContext(_contextOptions))
            {
                var controller = new ProductsController(context);
                var result = await controller.Index();
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.Model);//Ürün listesi
                Assert.NotEmpty(model);
                Assert.Equal(2, model.Count());
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task DeleteProduct_ExistingProduct_DeletesProduct(int productId)
        {
            using (var context = new ProductContext(_contextOptions))
            {
                var controller = new ProductsController(context);
                var product = await context.Products.FindAsync(productId);

                if (product != null)
                {
                    Console.WriteLine($"Silinmek üzere olan ürün: ID = {product.ProductID}, Ad = {product.ProductName}");
                }

                Assert.NotNull(product);

                var result = await controller.DeleteConfirmed(productId);
                var deletedProduct = await context.Products.FindAsync(productId);
                Assert.Null(deletedProduct);

                if (deletedProduct == null)
                {
                    Console.WriteLine($"Ürün ID = {productId} başarıyla silindi.");
                }
            }
        }


    }
}
