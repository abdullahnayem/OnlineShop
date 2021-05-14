using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;


namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private ApplicationDbContext _db;
        [Obsolete]
        private IHostingEnvironment _he;

        [Obsolete] //eta IHostingEnvironment er jonno
        public ProductController(ApplicationDbContext db, IHostingEnvironment he)
        {
            _db = db;
            _he = he;
        }
        public IActionResult Index()
        {
            return View(_db.Products.Include(c => c.ProductTypes).Include(f => f.SpecialTag).ToList());
        }
        //POST Index Action method
        [HttpPost]

        public IActionResult Index(decimal? lowAmount, decimal? largeAmount)
        {
            var products=_db.Products.Include(c=>c.ProductTypes).Include(c=>c.SpecialTag)
                .Where(c=>c.Price>=lowAmount && c.Price <= largeAmount).ToList();
            if(lowAmount==null ||  largeAmount==null)
            {
                 products = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).ToList();
            }
                return View(products);
        }

        //Get Create method
        public IActionResult Create()
        {
            ViewData["ProductTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["SpecialTagId"] = new SelectList(_db.SpecialTag.ToList(), "Id", "Name");
            return View();
        }

        //Post Create method
        [HttpPost]
        [Obsolete]
        public async Task<IActionResult> Create(Products product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var searchProduct = _db.Products.FirstOrDefault(c => c.Name == product.Name);
                if(searchProduct!=null)
                {
                    ViewData["ProductTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
                    ViewData["SpecialTagId"] = new SelectList(_db.SpecialTag.ToList(), "Id", "Name");
                    ViewBag.message = "This Product is already Exist";
                    return View(product);
                }

                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    product.Image = "Images/" + image.FileName;
                }
                if (image == null)
                {
                    product.Image = "Images/1.png";
                }

                _db.Products.Add(product);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        //Get Edit Action Method



        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _db.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }


            ViewData["ProductTypeId"] = new SelectList(_db.ProductTypes, "Id", "ProductType", products.ProductTypeId);
            ViewData["SpecialTagId"] = new SelectList(_db.SpecialTag, "Id", "Name", products.SpecialTagId);
            return View(products);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Products products, IFormFile image)
        {

            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    products.Image = "Images/" + image.FileName;
                }
                if (image == null)
                {
                    products.Image = "Images/1.png";
                }

                _db.Products.Update(products);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(products);
        }

        //GET Details Action Method

        public ActionResult Details(int? id)
        {

            if(id==null)
            {
                return NotFound();
            }

            var product = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).FirstOrDefault(c => c.Id == id);
            if(product==null)
            {
                return NotFound();
            }
            return View(product);
        }

        //Get Delete Method

        public ActionResult Delete(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c=>c.SpecialTag).Include(c =>c. ProductTypes).Where(c => c.Id == id).FirstOrDefault();
            if(product==null)
            {
                return NotFound();

            }
            return View(product);
        }

        //Get Post Delete Method

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.Products.FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();

            }

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}



//namespace OnlineShop.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    public class ProductController : Controller
//    {
//        private readonly ApplicationDbContext _context;
//        private IWebHostEnvironment _he;
//        private object products;

//        public object FileModel { get; private set; }

//        public ProductController(ApplicationDbContext context, IWebHostEnvironment he)
//        { 
//            _context = context;
//            _he = he;
//        }

//        // GET: Admin/Product
//        public async Task<IActionResult> Index()
//        {
//            var applicationDbContext = _context.Products.Include(p => p.ProductTypes).Include(p => p.SpecialTag);
//            return View(await applicationDbContext.ToListAsync());
//        }

//        // GET: Admin/Product/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var products = await _context.Products
//                .Include(p => p.ProductTypes)
//                .Include(p => p.SpecialTag)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (products == null)
//            {
//                return NotFound();
//            }

//            return View(products);
//        }

//        // GET: Admin/Product/Create
//        public IActionResult Create()
//        {
//            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes.ToList(), "Id", "ProductType");
//            ViewData["SpecialTagId"] = new SelectList(_context.SpecialTag.ToList(), "Id", "Name");
//            return View();
//        }

//        // POST: Admin/Product/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("Id,Name,Price,Image,ProductColor,IsAvailable,ProductTypeId,SpecialTagId")] Products products,IFormFile image)
//        {
//            if (ModelState.IsValid)
//            {
//                if(image!=null)
//                {
//                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
//                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
//                    products.Image ="Images/"+ image.FileName;

//                }
//                if(image == null)
//                {
//                    products.Image = "Images/1.png";
//                }
//                _context.Add(products);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "ProductType", products.ProductTypeId);
//            ViewData["SpecialTagId"] = new SelectList(_context.SpecialTag, "Id", "Id", products.SpecialTagId);
//            return View(products);
//        }

//        // GET: Admin/Product/Edit/5

//        public ActionResult Edit(int? id)
//        {
//            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes.ToList(), "Id", "ProductType");
//            ViewData["SpecialTagId"] = new SelectList(_context.SpecialTag.ToList() ,"Id", "Name");
            
//            return View();
            
//        }

//        //public async Task<IActionResult> Edit(int? id)
//        //{
//        //    if (id == null)
//        //    {
//        //        return NotFound();
//        //    }

//        //    var products = await _context.Products.FindAsync(id);
//        //    if (products == null)
//        //    {
//        //        return NotFound();
//        //    }
            

//        //    ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "ProductType", products.ProductTypeId);
//        //    ViewData["SpecialTagId"] = new SelectList(_context.SpecialTag, "Id", "Name", products.SpecialTagId);
//        //    return View(products);
//        //}

//        //// POST: Admin/Product/Edit/5
//        //// To protect from overposting attacks, enable the specific properties you want to bind to.
//        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        //[HttpPost]
//        //[ValidateAntiForgeryToken]
//        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Image,ProductColor,IsAvailable,ProductTypeId,SpecialTagId")] Products products)
//        //{
//        //    if (id != products.Id)
//        //    {
//        //        return NotFound();
//        //    }

//        //    if (ModelState.IsValid)
//        //    {
//        //        try
//        //        {
//        //            _context.Update(products);
//        //            await _context.SaveChangesAsync();
//        //        }
//        //        catch (DbUpdateConcurrencyException)
//        //        {
//        //            if (!ProductsExists(products.Id))
//        //            {
//        //                return NotFound();
//        //            }
//        //            else
//        //            {
//        //                throw;
//        //            }
//        //        }
//        //        return RedirectToAction(nameof(Index));
//        //    }
//        //    ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "ProductType", products.ProductTypeId);
//        //    ViewData["SpecialTagId"] = new SelectList(_context.SpecialTag, "Id", "Id", products.SpecialTagId);
//        //    return View(products);
//        //}

//        // GET: Admin/Product/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var products = await _context.Products
//                .Include(p => p.ProductTypes)
//                .Include(p => p.SpecialTag)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (products == null)
//            {
//                return NotFound();
//            }

//            return View(products);
//        }

//        // POST: Admin/Product/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var products = await _context.Products.FindAsync(id);
//            _context.Products.Remove(products);
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool ProductsExists(int id)
//        {
//            return _context.Products.Any(e => e.Id == id);
//        }
//    }
//}
