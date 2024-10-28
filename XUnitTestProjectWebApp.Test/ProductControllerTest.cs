using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XUnitTestProjectWebApp.Controllers;
using XUnitTestProjectWebApp.Models;
using XUnitTestProjectWebApp.Repository;
using Xunit;

namespace XUnitTestProjectWebApp.Test
{
    public class ProductControllerTest
    {
        private readonly Mock<IRepository<Product>> _mockRepo; // Mock repository
        private readonly ProductsController _controller;
        private readonly ProductValidator _validator; // Gerçek validator
        private List<Product> _products;

        public ProductControllerTest()
        {
            // Mock repository'i kur
            _mockRepo = new Mock<IRepository<Product>>();

            // Ürün listesi
            _products = new List<Product>
            {
                new Product { ProductName = "bardak", ProductID = 20, ProductColor = "Yeşil", ProductPrice = 150, ProductStock = 200 },
                new Product { ProductName = "demlik", ProductID = 21, ProductColor = "eflatun", ProductPrice = 300, ProductStock = 150 }
            };

            // Mock repository ayarları
            _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(_products);
            _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((int id) => _products.FirstOrDefault(p => p.ProductID == id));

            // Create, Update, Delete metodları için Returns yerine sadece Callback kullanıldı
            _mockRepo.Setup(repo => repo.Create(It.IsAny<Product>())).Callback((Product product) => _products.Add(product));
            _mockRepo.Setup(repo => repo.Update(It.IsAny<Product>())).Callback((Product product) =>
            {
                var existingProduct = _products.FirstOrDefault(p => p.ProductID == product.ProductID);
                if (existingProduct != null)
                {
                    existingProduct.ProductName = product.ProductName;
                    existingProduct.ProductPrice = product.ProductPrice;
                    existingProduct.ProductStock = product.ProductStock;
                    existingProduct.ProductColor = product.ProductColor;
                }
            });
            _mockRepo.Setup(repo => repo.Delete(It.IsAny<Product>())).Callback((Product product) => _products.Remove(product));

            // Gerçek ProductValidator
            _validator = new ProductValidator();

            // Controller'ı mock repo ve gerçek validator ile oluştur
            _controller = new ProductsController(_mockRepo.Object, _validator);
        }

        [Fact]
        public async Task HataliModelIleBirdenFazlaKuralIhlali_DigerKurallarCalismali_MockIle()
        {
            // Arrange: Hem ürün ismi çok kısa, hem de fiyat negatif olan geçersiz bir ürün oluşturuyoruz.
            var hataliUrun = new Product { ProductName = "", ProductPrice = 10, ProductStock = -5, ProductColor = "Mavi" };

            // Act: Gerçek validator kullanarak validasyonu gerçekleştiriyoruz.
            var validationResult = await _validator.ValidateAsync(hataliUrun);

            // Assert: Validasyon hatalarının döndüğünü kontrol et
            Assert.False(validationResult.IsValid);

            // Hataların sayısını kontrol edelim
            Assert.Equal(3, validationResult.Errors.Count);

            // Hata mesajlarını kontrol edelim
            var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
        
            Assert.Contains("Ürün İsmi Boş Geçilemez", errorMessages);
            Assert.Contains("Ürün İsmi 2-50 karakter arasında olması gerekiyor", errorMessages);
            Assert.DoesNotContain("Ürün fiyatı sıfırdan büyük olmalıdır.", errorMessages);
            Assert.Contains("Ürünün Stok Adedi 0'a eşit veya büyük olmak zorundadır", errorMessages);
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnView()
        {
            // Arrange: Mock repository üzerinden GetAll() metodunu çağırıyoruz.
            var result = await _controller.Index();

            // Assert: Index metodunun ViewResult döndüğünü kontrol ediyoruz.
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnProductList()
        {
            // Arrange: Mock repository üzerinden GetAll() metodunu çağırıyoruz.
            var result = await _controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);

            // Assert: ViewResult'ın modeli ürün listesi olmalı
            var productList = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.Model);
            Assert.Equal(2, productList.Count());
        }

        [Fact]
        public async void Details_IdIsNull_ReturnNotFound()
        {
            // Arrange & Act: ID null olduğunda Details metodunu çağırıyoruz.
            var result = await _controller.Details(null);

            // Assert: Sonucun NotFound olduğunu kontrol ediyoruz.
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void Details_IdIsValid_ReturnProduct()
        {
            // Arrange: Geçerli bir ID ile Details metodunu çağırıyoruz.
            int productId = 20;
            _mockRepo.Setup(repo => repo.GetById(productId)).ReturnsAsync(_products.FirstOrDefault(x => x.ProductID == productId));

            // Act
            var result = await _controller.Details(productId);
            var viewResult = Assert.IsType<ViewResult>(result);
            var product = Assert.IsType<Product>(viewResult.Model);

            // Assert
            Assert.Equal(productId, product.ProductID);
        }

        [Fact]
        public async void Details_ProductNotFound_ReturnNotFound()
        {
            // Arrange: Geçersiz bir ID ile Details metodunu çağırıyoruz.
            int productId = 999; // olmayan bir ürün ID'si
            _mockRepo.Setup(repo => repo.GetById(productId)).ReturnsAsync((Product)null);

            // Act
            var result = await _controller.Details(productId);

            // Assert: Product bulunamadığında NotFound dönüşünü kontrol ediyoruz.
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
