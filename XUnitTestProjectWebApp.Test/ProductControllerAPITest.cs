using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUnitTestProjectWebApp.Controllers;
using XUnitTestProjectWebApp.Models;
using XUnitTestProjectWebApp.Repository;

namespace XUnitTestProjectWebApp.Test
{
    public class ProductControllerAPITest
    {

        private readonly Mock<IRepository<Product>> _mockRepo;
        private readonly ProductsAPIController _controller;
        private List<Product> _products;

        public ProductControllerAPITest()
        {
            _mockRepo = new Mock<IRepository<Product>>();
            _controller = new ProductsAPIController(_mockRepo.Object);
            _products = new List<Product>()
            {
                new Product { ProductName = "bardak", ProductID = 20, ProductColor = "Yeşil", ProductPrice = 150, ProductStock = 200 },
                new Product { ProductName = "demlik", ProductID = 21, ProductColor = "eflatun", ProductPrice = 300, ProductStock = 150 }
            };

        }
        [Fact]

        public async void GetProducts_ActionExecutes_ReturnOkResultWithProducts()
        {
            //Veri tabanından veri almak yerine sahte bir depo kullandıum.
            _mockRepo.Setup(x => x.GetAll()).ReturnsAsync(_products);
            var result = await _controller.GetProducts();
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
            Assert.Equal<int>(2,returnProducts.ToList().Count);
        }

    }
}
