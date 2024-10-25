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
//Genel olarak üç aşamdan oluşuyordu
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
        public async Task GecerliModelUrunOlustur_KayitBasariliVeYonlendirmeYapildi()
        {
            var yeniUrun = new Product { ProductName = "klav", ProductPrice = 200, ProductStock = 210, ProductColor = "Mavil" };

            using (var context = new ProductContext(_contextOptions))
            {

                var kategori = await context.Categories.FirstAsync();
                yeniUrun.CategoryId = kategori.CategoryId;
                var validator = new ProductValidator();
                var controller = new ProductsController(context, validator);
                var result = await controller.Create(yeniUrun);
                var redirect = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirect.ActionName);


            }
            using (var context = new ProductContext(_contextOptions))
            {
                var urun = context.Products.FirstOrDefault(x => x.ProductName == yeniUrun.ProductName);
                Assert.Equal(yeniUrun.ProductName, urun.ProductName);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task KategoriSil_VarolanKategoriIdyeAitTumUrunleriSil(int kategoriId)
        {
            using (var context = new ProductContext(_contextOptions))
            {
                var kategori = await context.Categories.FindAsync(kategoriId);

                context.Categories.Remove(kategori);
                context.SaveChanges();

            }

            using (var context = new ProductContext(_contextOptions))
            {
                var urunler = await context.Products.Where(x => x.CategoryId == kategoriId).ToListAsync();
                Assert.Empty(urunler);//kategoriye ait ürünlerin silinip silinmediğini de kontrol etmeyi istiyorum.
            }
        }


        [Fact]
        public async Task VarolanUrunGuncelle_BasariliGuncelleme()
        {
            using (var context = new ProductContext(_contextOptions))
            {
                var mevcutUrun = await context.Products.FirstAsync();
                mevcutUrun.ProductPrice = 300;
                var validator = new ProductValidator();

                var controller = new ProductsController(context, validator);
                var result = await controller.Edit(mevcutUrun.ProductID, mevcutUrun);
                var redirect = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirect.ActionName);
            }

            using (var context = new ProductContext(_contextOptions))
            {
                var guncellenmisUrun = await context.Products.FirstAsync(p => p.ProductID == 1);
                Assert.Equal(300, guncellenmisUrun.ProductPrice);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task UrunDetaylariniGetir_VarolanUrunId_UrunDetaylariniDoner(int urunId)
        {
            using (var context = new ProductContext(_contextOptions))
            {
                var validator = new ProductValidator();

                var controller = new ProductsController(context, validator);
                var result = await controller.Details(urunId);
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<Product>(viewResult.Model);
                Assert.Equal(urunId, model.ProductID);
            }
        }
        [Fact]
        public async Task TumUrunleriGetir_TumUrunleriDoner()
        {
            using (var context = new ProductContext(_contextOptions))
            {
                var validator = new ProductValidator();

                var controller = new ProductsController(context, validator);
                var result = await controller.Index();
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.Model);//Ürün listesi
                Assert.NotEmpty(model);
                Assert.Equal(2, model.Count());
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task UrunSil_VarolanUrun_BasariliSilme(int urunId)
        {
            using (var context = new ProductContext(_contextOptions))
            {
                var validator = new ProductValidator();

                var controller = new ProductsController(context, validator);
                var urun = await context.Products.FindAsync(urunId);

                if (urun != null)
                {
                    Console.WriteLine($"Silinmek üzere olan ürün: ID = {urun.ProductID}, Ad = {urun.ProductName}");
                }

                Assert.NotNull(urun);

                var result = await controller.DeleteConfirmed(urunId);
                var silinmisUrun = await context.Products.FindAsync(urunId);
                Assert.Null(silinmisUrun);

                if (silinmisUrun == null)
                {
                    Console.WriteLine($"Ürün ID = {urunId} başarıyla silindi.");
                }
            }
        }
        [Fact]
        public async Task HataliModelIleUrunOlustur_GecersizModel_HataDonmeli()
        {
            // Arrange: Hatalı bir ürün oluşturuyoruz.
            var hataliUrun = new Product { ProductName = "aaa", ProductPrice = 10, ProductStock = 50, ProductColor = "Mavi" };

            using (var context = new ProductContext(_contextOptions))
            {
                var kategori = await context.Categories.FirstAsync();
                hataliUrun.CategoryId = kategori.CategoryId;

                // Doğrulayıcıyı testler için manuel olarak oluşturuyoruz.
                var validator = new ProductValidator();
                var controller = new ProductsController(context, validator);

                // Act: Ürünü oluşturma denemesi yapıyoruz.
                var result = await controller.Create(hataliUrun);

                // Assert: ViewResult döndüğünü ve ModelState'in geçersiz olduğunu kontrol ediyoruz.
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.True(controller.ModelState.IsValid);
            }
        }


        [Fact]
        public async Task HataliModelIleKisaIsimliUrunOlustur_ValidasyonHatasiDonmeli()
        {
            // Arrange: Geçersiz bir ürün oluşturuyoruz (ad çok kısa)
            var hataliUrun = new Product { ProductName = "a", ProductPrice = 200, ProductStock = 50, ProductColor = "Mavi" };

            using (var context = new ProductContext(_contextOptions))
            {
                var kategori = await context.Categories.FirstAsync();
                hataliUrun.CategoryId = kategori.CategoryId;

                var validator = new ProductValidator();
                var controller = new ProductsController(context, validator);

                // Act: Ürünü oluşturmayı deniyoruz
                var result = await controller.Create(hataliUrun);

                // Assert: Hatalı bir ViewResult dönmeli ve ModelState geçersiz olmalı
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.False(controller.ModelState.IsValid);
            }
        }

        [Fact]
        public async Task HataliModelIleNegatifFiyatliUrunOlustur_ValidasyonHatasiDonmeli()
        {
            // Arrange: Geçersiz bir ürün oluşturuyoruz (negatif fiyat)
            var hataliUrun = new Product { ProductName = "Geçersiz Ürün", ProductPrice = -10, ProductStock = 50, ProductColor = "Mavi" };

            using (var context = new ProductContext(_contextOptions))
            {
                var kategori = await context.Categories.FirstAsync();
                hataliUrun.CategoryId = kategori.CategoryId;

                var validator = new ProductValidator();
                var controller = new ProductsController(context, validator);

                // Act: Ürünü oluşturmayı deniyoruz
                var result = await controller.Create(hataliUrun);

                // Assert: Hatalı bir ViewResult dönmeli ve ModelState geçersiz olmalı
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.False(controller.ModelState.IsValid);
            }
        }

        [Fact]
        public async Task CreateYontemindeGecersizUrunEklenmesi_DigerTestleriEtkilememelidir()
        {
            // Arrange: Önce hatalı bir ürün oluşturalım
            var hataliUrun = new Product { ProductName = "a", ProductPrice = 200, ProductStock = 50, ProductColor = "Mavi" };

            using (var context = new ProductContext(_contextOptions))
            {
                var kategori = await context.Categories.FirstAsync();
                hataliUrun.CategoryId = kategori.CategoryId;

                var validator = new ProductValidator();
                var controller = new ProductsController(context, validator);

                // Act: Hatalı ürünü oluşturmayı deniyoruz
                var invalidResult = await controller.Create(hataliUrun);
                var invalidViewResult = Assert.IsType<ViewResult>(invalidResult);
                Assert.False(controller.ModelState.IsValid);
            }

            // Arrange: Şimdi geçerli bir ürün oluşturalım
            var gecerliUrun = new Product { ProductName = "Router", ProductPrice = 300, ProductStock = 210, ProductColor = "Siyah" };

            using (var context = new ProductContext(_contextOptions))
            {
                var kategori = await context.Categories.FirstAsync();
                gecerliUrun.CategoryId = kategori.CategoryId;

                var validator = new ProductValidator();
                var controller = new ProductsController(context, validator);

                // Act: Geçerli ürünü oluşturmayı deniyoruz
                var validResult = await controller.Create(gecerliUrun);
                var redirect = Assert.IsType<RedirectToActionResult>(validResult);

                // Assert: Ürün başarılı bir şekilde eklendiği için Index'e yönlendirilmesi bekleniyor
                Assert.Equal("Index", redirect.ActionName);
            }
        }
        [Fact]
        public async Task GecerliModelIleUrunOlustur_BasariliSekildeUrunOlusturulmalidir()
        {
            // Arrange: Geçerli bir ürün oluşturuyoruz.
            var gecerliUrun = new Product { ProductName = "Valid Product", ProductPrice = 200, ProductStock = 50, ProductColor = "Mavi" };

            using (var context = new ProductContext(_contextOptions))
            {
                var kategori = await context.Categories.FirstAsync();
                gecerliUrun.CategoryId = kategori.CategoryId;

                var validator = new ProductValidator();
                var controller = new ProductsController(context, validator);

                // Act: Ürünü oluşturmaya çalışıyoruz.
                var result = await controller.Create(gecerliUrun);

                // Assert: RedirectToActionResult dönmeli ve ModelState geçerli olmalı.
                var redirect = Assert.IsType<RedirectToActionResult>(result);
                Assert.True(controller.ModelState.IsValid);

                // Ayrıca, veritabanında ürünün doğru şekilde kaydedilip kaydedilmediğini kontrol ediyoruz.
                var kaydedilenUrun = await context.Products.FirstOrDefaultAsync(p => p.ProductName == gecerliUrun.ProductName);
                Assert.NotNull(kaydedilenUrun);
            }
        }

    }
}
