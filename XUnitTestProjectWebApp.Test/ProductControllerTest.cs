using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUnitTestProjectWebApp.Context;
using XUnitTestProjectWebApp.Controllers;
using XUnitTestProjectWebApp.Models;
using XUnitTestProjectWebApp.Repository;

namespace XUnitTestProjectWebApp.Test
{


    public class ProductControllerTest
    {

        private readonly Mock<IRepository<Product>> _mockRepo;
        private readonly ProductsController _controller;
        private List<Product> _products;


        public ProductControllerTest()
        {
            _mockRepo = new Mock<IRepository<Product>>();
            _controller = new ProductsController(_mockRepo.Object);
            _products = new List<Product>()
            {
                new Product { ProductName = "bardak", ProductID = 20, ProductColor = "Yeşil", ProductPrice = 150, ProductStock = 200 },
                new Product { ProductName = "demlik", ProductID = 21, ProductColor = "eflatun", ProductPrice = 300, ProductStock = 150 }
            };
        }


        //product listeminin geriye bir view döndüğünü test etmek için kullanıldı
        [Fact]
        public async void Index_ActionExecutes_ReturnView()
        {
            _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(_products);
            var result = await _controller.Index();
            Assert.IsType<ViewResult>(result);// ındex metotdumuzun dönüş türünün viewresult olup olmadığı test edildi
        }


        //product listemizin geriye data döndürdüğünden emin olabilmek için

        [Fact]
        public async void Index_ActionExecutes_ReturnProductList()
        {
            _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(_products);//ındexin içerisindeki gettall çalışınca fake olarak veri tabanına bağlanmak yerine yukarıda gösterilen dataları çekmeisnmi sağladık 
            var result = await _controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);// ındex metotdumuzun dönüş türünün viewresult olup olmadığı test edildi
            var productList = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.Model);

            Assert.Equal<int>(2, productList.Count());// yukarıda iki tane ürün tanımlandığı için cevanın true olmasını bekliyoruz.
        }
      
        [Fact]
        public async void Details_IdIsNull_ReturnNotFound()
        {
            var result = await _controller.Details(null);
            Assert.IsType<NotFoundResult>(result); // ID null olduğunda NotFound dönüşünü test eder
        }

        [Fact]
        public async void Details_IdIsValid_ReturnProduct()
        {
            int productId = 20;
            _mockRepo.Setup(repo => repo.GetById(productId)).ReturnsAsync(_products.FirstOrDefault(x => x.ProductID == productId));

            var result = await _controller.Details(productId);
            var viewResult = Assert.IsType<ViewResult>(result); 
            var product = Assert.IsType<Product>(viewResult.Model);

            Assert.Equal(productId, product.ProductID); 
        }

        [Fact]
        public async void Details_ProductNotFound_ReturnNotFound()
        {
            int productId = 999; // olmayan bir ürün ID'si
            _mockRepo.Setup(repo => repo.GetById(productId)).ReturnsAsync((Product)null);

            var result = await _controller.Details(productId);
            Assert.IsType<NotFoundResult>(result); // Product bulunamadığında NotFound dönüşünü test eder
        }
    }


}