using Microsoft.AspNetCore.Mvc;
using Retail_MVC.DataAccess.Data;
using Retail_MVC.Models;
using Retail_MVC.DataAccess.Repository.IRepository;
using Retail_MVC.DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Retail_MVC.Utility;

namespace Retail_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin+","+SD.Role_Vendor)]
    
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var objCategoryModel = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryModel);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                return RedirectToAction("Index", "Category");
            }

            return View();
        }

        public IActionResult Edit(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category objectCategory = _unitOfWork.Category.Get(u => u.Id == id);
            if (objectCategory == null)
            {
                return NotFound();
            }
            return View(objectCategory);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                return RedirectToAction("Index", "Category");
            }

            return View();
        }

        public IActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category objectCategory = _unitOfWork.Category.Get(u => u.Id == id);
            if (objectCategory == null)
            {
                return NotFound();
            }
            return View(objectCategory);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int id)
        {
            Category objectCategory = _unitOfWork.Category.Get(u => u.Id == id);
            if (objectCategory == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(objectCategory);
            _unitOfWork.Save();
            return RedirectToAction("Index", "Category");
        }
    }
}
