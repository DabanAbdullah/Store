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
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        private IWebHostEnvironment _webHostEnvironment;    
        public ProductController(IUnitOfWork db,IWebHostEnvironment webHostEnvironment)
        {
            _unitofwork = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var list = _unitofwork.Product.GetAll(includeproperties:"Category");
            return View(list);
        }


        public IActionResult Upsert(int? id)
        {

            ProductViewModel viewModel = new()
            {
                Product = new Product(),
                Categories = _unitofwork.Category.GetAll().ToList().Select(x => new SelectListItem
                {
                    Text = x.CatName,
                    Value = x.CategoryID.ToString()
                }),
            };
            if(id==null || id == 0)
            {
                //create
                return View(viewModel);
            }
            else
            {

                //update
                viewModel.Product=_unitofwork.Product.Get(x=>x.Id==id);
                return View(viewModel); 
            }

           
        }

        [HttpPost]
        public IActionResult Upsert(ProductViewModel obj,IFormFile? file)
        {
          

            if (ModelState.IsValid)
            {
                string wwwRootPath=_webHostEnvironment.WebRootPath;
                if(file!=null)
                {
                    string filename=Guid.NewGuid().ToString()+Path.GetExtension(file.FileName);
                    string productpath=Path.Combine(wwwRootPath, @"images/products");

                    if (!string.IsNullOrEmpty(obj.Product.imageurl))
                    {
                        var deleteold = Path.Combine(wwwRootPath, obj.Product.imageurl.TrimStart('\\'));
                        if (System.IO.File.Exists(deleteold))
                        {
                            System.IO.File.Delete(deleteold);
                        }

                    }

                    using(var fileStream=new FileStream(Path.Combine(productpath, filename),FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.Product.imageurl = @"\images\products\" + filename;
                }

                if (obj.Product.Id == 0) {
                    _unitofwork.Product.Add(obj.Product);
                }
                else
                {
                    _unitofwork.Product.update(obj.Product);
                }
               
                _unitofwork.save();
                TempData["success"] = "created successfuly";
                return RedirectToAction("Index");
            }
            else
            {
                obj.Categories = _unitofwork.Category.GetAll().ToList().Select(x => new SelectListItem
                {
                    Text = x.CatName,
                    Value = x.CategoryID.ToString()
                });
                return View(obj);   
            }
         

        }










        #region APICalls
        [HttpGet]
        public IActionResult getall()
        {
            var list = _unitofwork.Product.GetAll(includeproperties: "Category");
            return Json(new { data=list });
        }


        [HttpDelete]
        public IActionResult delete(int? id)
        {
            var rec = _unitofwork.Product.Get(x=>x.Id==id);
            if(rec== null)
            {
                return Json(new {success=false,message="error while deleting"});
            }
            else
            {
                if (!string.IsNullOrEmpty(rec.imageurl))
                {
                    var imagedelete = Path.Combine(_webHostEnvironment.WebRootPath, rec.imageurl.TrimStart('\\'));
                    if(System.IO.File.Exists(imagedelete))
                    {
                        System.IO.File.Delete(imagedelete);
                    }
                }

                _unitofwork.Product.Remove(rec);
                _unitofwork.save();

                return Json(new { success = true, message = "record was deleted" });
            }
           
           
        }
        #endregion

    }
}
