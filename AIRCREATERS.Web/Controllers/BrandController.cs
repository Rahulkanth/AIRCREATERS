using AIRCREATERS.Web.Data;
using AIRCREATERS.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AIRCREATERS.Web.Controllers
{
    public class BrandController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BrandController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]

        public IActionResult Index()
        {
            List<Brand> brands = _dbContext.Brand.ToList();

            return View(brands);

        }

        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Create(Brand brand)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;

            var file = HttpContext.Request.Form.Files;

            if(file.Count > 0)
            {
                string newFileName = Guid.NewGuid().ToString();

                var upload = Path.Combine(webRootPath, @"images\brand");

                var extension = Path.GetExtension(file[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(upload, newFileName + extension), FileMode.Create)) 
                {
                    file[0].CopyTo(fileStream);
                }


                brand.BrandLogo = @"\images\brand\" + newFileName + extension;
            
            }
            if (ModelState.IsValid)
            {
                _dbContext.Brand.Add(brand);
                _dbContext.SaveChanges();

                TempData["success"] = "Record Created Successfully";

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]

        public IActionResult Details(Guid id)
        {
             Brand brand = _dbContext.Brand.FirstOrDefault(x => x.Id == id);

            return View(brand);
        }

        [HttpGet]

        public IActionResult Edit(Guid id)
        {
            Brand brand = _dbContext.Brand.FirstOrDefault(x => x.Id == id);

            return View(brand);
        }

        [HttpPost]

        public IActionResult Edit(Brand brand)
        {

            string webRootPath = _webHostEnvironment.WebRootPath;

            var file = HttpContext.Request.Form.Files;

            if (file.Count > 0)
            {
                string newFileName = Guid.NewGuid().ToString();

                var upload = Path.Combine(webRootPath, @"images\brand");

                var extension = Path.GetExtension(file[0].FileName);

                //delete old images

                var objfromDb = _dbContext.Brand.AsNoTracking().FirstOrDefault(x => x.Id == brand.Id);

                if(objfromDb.BrandLogo != null)
                {
                    var oldImagepath = Path.Combine(webRootPath, objfromDb.BrandLogo.Trim('\\'));


                    if (System.IO.File.Exists(oldImagepath))
                    {
                        System.IO.File.Delete(oldImagepath);
                    }
                }


                using (var fileStream = new FileStream(Path.Combine(upload, newFileName + extension), FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                }


                brand.BrandLogo = @"\images\brand\" + newFileName + extension;

            }


            if (ModelState.IsValid)
            {
                var objfromDb = _dbContext.Brand.AsNoTracking().FirstOrDefault(x => x.Id == brand.Id);

                objfromDb.Name = brand.Name;
                objfromDb.Establishyear = brand.Establishyear;

                if(brand.BrandLogo !=null)
                {
                    objfromDb.BrandLogo = brand.BrandLogo;
                }


                _dbContext.Brand.Update(objfromDb);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));

            }

            return View();
        }

        [HttpGet]

        public IActionResult Delete(Guid id)
        {
            Brand brand = _dbContext.Brand.FirstOrDefault(x => x.Id == id);

            return View(brand);
        }

        [HttpPost]

        public IActionResult Delete(Brand brand)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;

            if(string.IsNullOrEmpty(brand.BrandLogo))
            {
                var objfromDb = _dbContext.Brand.AsNoTracking().FirstOrDefault(x => x.Id == brand.Id);

                if (objfromDb.BrandLogo != null)
                {
                    var oldImagepath = Path.Combine(webRootPath, objfromDb.BrandLogo.Trim('\\'));


                    if (System.IO.File.Exists(oldImagepath))
                    {
                        System.IO.File.Delete(oldImagepath);
                    }
                }
            }

            _dbContext.Brand.Remove(brand);
            _dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
