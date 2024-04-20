using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Retail_MVC.DataAccess.Repository.IRepository;
using Retail_MVC.Models;
using Retail_MVC.Models.ViewModels;
using System.Collections.Generic;
using System.Data;
using Retail_MVC.DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using Retail_MVC.Utility;

namespace Retail_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Vendor)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            List<Product> prodobj= _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            return View(prodobj);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem { Text = u.Name, Value = u.Id.ToString() }),
                Product = new Product()
            };
            if(id==null || id==0)
            {
                return View(productVM);
            }

            else
            {
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            productVM.Product.Id = 0;
            if(ModelState.IsValid)
            {
                string wwwRootPath=_webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName=Guid.NewGuid().ToString()+ Path.GetExtension(file.FileName);
                    string productPath=Path.Combine(wwwRootPath, @"images\product\");

                    if(!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
						var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream=new FileStream(Path.Combine(productPath,fileName),FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }
                if(productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }
                
                _unitOfWork.Save();
                return RedirectToAction("Index", "Product");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }
           
        }

       

        public IActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product prodfromdb = _unitOfWork.Product.Get(u=>u.Id==id);
            if (prodfromdb == null)
            {
                return NotFound();
            }
            return View(prodfromdb);
        }

        [HttpPost,ActionName("Delete")]
        public IActionResult DelectPost(int id)
        {
            Product prodCategory = _unitOfWork.Product.Get(u => u.Id == id);
            if (prodCategory == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(prodCategory);
            _unitOfWork.Save();
            return RedirectToAction("Index", "Product");
        }


        


    }
}
