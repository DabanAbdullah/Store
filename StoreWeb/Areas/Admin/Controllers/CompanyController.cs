using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Store.DataAccess.Data;
using Store.DataAccess.Repository.IRepository;
using Store.Models;
using Store.Models.VM;
using Store.Utility;
using System.Collections.Generic;


namespace StoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_user_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        private IWebHostEnvironment _webHostEnvironment;    
        public CompanyController(IUnitOfWork db,IWebHostEnvironment webHostEnvironment)
        {
            _unitofwork = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
           // var list = _unitofwork.Company.GetAll();
            return View();
        }


        public IActionResult Upsert(int? id)
        {

            Company obj = new Company();
            if(id==null || id == 0)
            {
                //create
                return View(obj);
            }
            else
            {

                //update
               obj=_unitofwork.Company.Get(x=>x.Id==id);
                return View(obj); 
            }

           
        }

        [HttpPost]
        public IActionResult Upsert(Company obj)
        {
          

            if (ModelState.IsValid)
            {
                string wwwRootPath=_webHostEnvironment.WebRootPath;
               

                if (obj.Id == 0) {
                    _unitofwork.Company.Add(obj);
                }
                else
                {
                    _unitofwork.Company.update(obj);
                }
               
                _unitofwork.save();
                TempData["success"] = "created successfuly";
                return RedirectToAction("Index");
            }
           
               
                return View(obj);   
            
         

        }










        #region APICalls
        [HttpGet]
        public IActionResult getall()
        {
            var list = _unitofwork.Company.GetAll();
            return Json(new { data=list });
        }


        [HttpDelete]
        public IActionResult delete(int? id)
        {
            var rec = _unitofwork.Company.Get(x=>x.Id==id);
            if(rec== null)
            {
                return Json(new {success=false,message="error while deleting"});
            }
            else
            {
              

                _unitofwork.Company.Remove(rec);
                _unitofwork.save();

                return Json(new { success = true, message = "record was deleted" });
            }
           
           
        }
        #endregion

    }
}
