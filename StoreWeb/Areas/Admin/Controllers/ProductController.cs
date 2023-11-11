using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
                viewModel.Product=_unitofwork.Product.Get(x=>x.Id==id,includeproperties: "ProductImages");//must match dbcontext but model i think
                
                return View(viewModel); 
            }

           
        }





        public IActionResult DeleteImage(int ImageID)
        {
            var imagetobedeleted = _unitofwork.ProductImage.Get(x => x.Id == ImageID);
            var ProductId = imagetobedeleted.ProductId;
            if(imagetobedeleted != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string finalpath = Path.Combine(wwwRootPath, imagetobedeleted.ImageUrl.TrimStart('\\'));
                if(System.IO.File.Exists(finalpath)) {
                    System.IO.File.Delete(finalpath);
                }
                _unitofwork.ProductImage.Remove(imagetobedeleted);
                _unitofwork.save();
            }
            TempData["success"] = "image was deleted";
            return RedirectToAction(nameof(Upsert), new { Id = ProductId });
        }


        [HttpPost]
        public IActionResult Upsert(ProductViewModel obj,List<IFormFile>? files)
        {
          

            if (ModelState.IsValid)
            {
                if (obj.Product.Id == 0)
                {
                    _unitofwork.Product.Add(obj.Product);
                }
                else
                {
                    _unitofwork.Product.update(obj.Product);
                }

                _unitofwork.save();
                string wwwRootPath=_webHostEnvironment.WebRootPath;
                if(files!=null)
                {
                    
                    foreach (var file in files)
                    {
                        string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productpath = @"images\product\product-" + obj.Product.Id;
                        string finalpath = Path.Combine(wwwRootPath, productpath);

                        if (!Directory.Exists(finalpath))
                        {
                            Directory.CreateDirectory(finalpath);
                        }

                        using (var fileStream = new FileStream(Path.Combine(finalpath, filename), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        productimage pr = new productimage()
                        {
                            ImageUrl =@"\"+ productpath +@"\"+ filename,
                            ProductId = obj.Product.Id,
                        };
                        




                        if(obj.Product.ProductImages == null)
                        obj.Product.ProductImages = new List<productimage>();


                        obj.Product.ProductImages.Add(pr);

                       
                      

                      


                    }
                   _unitofwork.Product.update(obj.Product);
                   _unitofwork.save();




                   
                 
                }

               
                TempData["success"] = "product created/updated successfuly";
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

                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string productpath = @"images\product\product-" + id;
                string finalpath = Path.Combine(wwwRootPath, productpath);

                if (Directory.Exists(finalpath))
                {
                    string[]files= Directory.GetFiles(finalpath);
                    foreach (string file in files)
                    {
                        System.IO.File.Delete(file);
                    }
                    Directory.Delete(finalpath);
                }
                var list = _unitofwork.ProductImage.GetAll(x => x.ProductId == id);
                _unitofwork.ProductImage.RemoveRange(list);
                _unitofwork.save();

                //if (!string.IsNullOrEmpty(rec.imageurl))
                //{
                //    var imagedelete = Path.Combine(_webHostEnvironment.WebRootPath, rec.imageurl.TrimStart('\\'));
                //    if(System.IO.File.Exists(imagedelete))
                //    {
                //        System.IO.File.Delete(imagedelete);
                //    }
                //}

                _unitofwork.Product.Remove(rec);
                _unitofwork.save();

                return Json(new { success = true, message = "record was deleted" });
            }
           
           
        }
        #endregion

    }
}
