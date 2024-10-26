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
                var validator = new ProductValidator();

                var controller = new ProductsController(context, validator);
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
                var products = await context.Products.Where(x => x.CategoryId == categoryId).ToListAsync();//bu listenin boş gelmesini bekliyorum fakat InMemory ilişkisel değilli!1
                Assert.NotEmpty(products);
            }
        }
        [Fact]
        public async Task HataliModelIleBirdenFazlaKuralIhlali_DigerKurallarCalismali()
        {
            // Arrange: Hem ürün ismi çok kısa, hem de fiyat negatif olan geçersiz bir ürün oluşturuyoruz.
            var hataliUrun = new Product { ProductName = "a", ProductPrice = -10, ProductStock = 50, ProductColor = "Mavi" };

            using (var context = new ProductContext(_contextOptions))
            {
                var kategori = await context.Categories.FirstAsync();
                hataliUrun.CategoryId = kategori.CategoryId;

                var validator = new ProductValidator();
                var validationResult = await validator.ValidateAsync(hataliUrun);

                // Assert: ModelState'in geçersiz olduğunu ve her iki hatanın da döndüğünü kontrol edelim.
                Assert.False(validationResult.IsValid);

                // Hata mesajlarını kontrol edelim
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();

                // İki hata mesajı da döndü mü?
                Assert.Contains("Ürün İsmi 2-50 karakter arasında olması gerekiyor", errorMessages);
                Assert.Contains("Ürün fiyatı sıfırdan büyük olmalıdır.", errorMessages);
            }
        }

    }
}
