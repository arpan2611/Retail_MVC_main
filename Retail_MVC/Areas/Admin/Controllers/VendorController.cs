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
    [Authorize(Roles =SD.Role_Admin)]
    public class VendorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public VendorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
           
        }


        public IActionResult Index()
        {
            List<Vendor> vendobj= _unitOfWork.Vendor.GetAll().ToList();
            return View(vendobj);
        }

        public IActionResult Upsert(int? id)
        {
            
            if(id==null || id==0)
            {
                return View(new Vendor());
            }

            else
            {
                Vendor vendobj = _unitOfWork.Vendor.Get(u => u.Id == id);
                return View(vendobj);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Vendor vendobj)
        {
            //VendorVM.Vendor.Id = 0;
            if(ModelState.IsValid)
            {
                if(vendobj.Id == 0)
                {
                    _unitOfWork.Vendor.Add(vendobj);
                }
                else
                {
                    _unitOfWork.Vendor.Update(vendobj);
                }
                
                _unitOfWork.Save();
                return RedirectToAction("Index", "Vendor");
            }
            else
            {
                
                return View(vendobj);
            }
           
        }

       

        public IActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Vendor prodfromdb = _unitOfWork.Vendor.Get(u=>u.Id==id);
            if (prodfromdb == null)
            {
                return NotFound();
            }
            return View(prodfromdb);
        }

        [HttpPost,ActionName("Delete")]
        public IActionResult DelectPost(int id)
        {
            Vendor prodCategory = _unitOfWork.Vendor.Get(u => u.Id == id);
            if (prodCategory == null)
            {
                return NotFound();
            }
            _unitOfWork.Vendor.Remove(prodCategory);
            _unitOfWork.Save();
            return RedirectToAction("Index", "Vendor");
        }


        


    }
}
