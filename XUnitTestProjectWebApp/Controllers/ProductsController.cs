using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using XUnitTestProjectWebApp.Context;
using XUnitTestProjectWebApp.Models;
using XUnitTestProjectWebApp.Repository;

namespace XUnitTestProjectWebApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IRepository<Product> _repository;
        private readonly IValidator<Product> _validator;


        public ProductsController(IRepository<Product> repository, IValidator<Product> validator)
        {
            _repository = repository;
            _validator = validator;
        }



        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetAll());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _repository.GetById((int)id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,ProductName,ProductPrice,ProductStock,ProductColor")] Product product)
        {
            if (ModelState.IsValid)
            {
                 await _repository.Create(product);
               
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _repository.GetById((int)id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Edit(int id, [Bind("ProductID,ProductName,ProductPrice,ProductStock,ProductColor")] Product product)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   _repository.Update(product);
                  
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _repository.GetById((int)id);
           
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _repository.GetById((int)id);
            if (product != null)
            {
                _repository.Delete(product);
            }

      
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            var product = _repository.GetById((int)id).Result;
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
