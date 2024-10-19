using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XUnitTestProjectWebApp.Context;
using XUnitTestProjectWebApp.Models;
using XUnitTestProjectWebApp.Repository;

namespace XUnitTestProjectWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsAPIController : ControllerBase
    {
        private readonly IRepository<Product> _repository;


        public ProductsAPIController(IRepository<Product> repository)
        {
            _repository = repository;
        }
        [HttpGet("{a}/{b}")]
        public IActionResult Add(int a,int b)
        {
            return Ok(new Helpers.Helper().add(a,b));
        }
      
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _repository.GetAll();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult>GetProduct(int id)
        {
            var product = await _repository.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPut("{id}")]
        public  IActionResult PutProduct(int id, Product product)
        {
            if (id != product.ProductID)
            {
                return BadRequest();
            }

         _repository.Update(product);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult>PostProduct(Product product)
        {
           await _repository.Create(product);

            return CreatedAtAction("GetProduct", new { id = product.ProductID }, product);
        }

        [HttpDelete("{id}")]
        public async Task< ActionResult<Product>> DeleteProduct(int id)
        {
            var product =  await _repository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
              _repository.Delete(product);


            return NoContent();
        }

        private bool ProductExists(int id)
        {
            Product product = _repository.GetById(id).Result;
            if (product == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
