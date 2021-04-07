using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewProductsController : Controller
    {
        
        private ApplicationDbContext _db;
        public NewProductsController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.NewProducts.ToList());
        }
        //Create get action method 

        public ActionResult Create()
        {
            return View();
        }

        //Create post action method 

        [HttpPost]
        [ValidateAntiForgeryToken] //for security purpose
        public async Task<IActionResult> Create(NewProducts newProducts)
        {
            if (ModelState.IsValid)
            {
                _db.NewProducts.Add(newProducts);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(newProducts);
        }

        //Get Edit action method 

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newProduct = _db.NewProducts.Find(id);
            if (newProduct == null)
            {
                return NotFound();
            }

            return View(newProduct);
        }

        //post Edit action method 

        [HttpPost]
        [ValidateAntiForgeryToken] //for security purpose
        public async Task<IActionResult> Edit(NewProducts newProducts)
        {
            if (ModelState.IsValid)
            {
                _db.Update(newProducts);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(newProducts);
        }

        //Get Details action method 

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newProduct = _db.NewProducts.Find(id);
            if (newProduct == null)
            {
                return NotFound();
            }

            return View(newProduct);
        }

        //post Details action method 

        [HttpPost]
        [ValidateAntiForgeryToken] //for security purpose
        public IActionResult Details(NewProducts newProducts)
        {
            return RedirectToAction(nameof(Index));

        }

        //Get Delete action method 

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newProduct = _db.NewProducts.Find(id);
            if (newProduct == null)
            {
                return NotFound();
            }

            return View(newProduct);
        }

        //post Delete action method 

        [HttpPost]
        [ValidateAntiForgeryToken] //for security purpose
        public async Task<IActionResult> Delete(int? id, NewProducts newProducts)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (id != newProducts.Id)
            {
                return NotFound();
            }

            var newProduct = _db.NewProducts.Find(id);
            if (newProduct == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Remove(newProduct);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(newProducts);
        }

    }
}

