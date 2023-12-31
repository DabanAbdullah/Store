﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.DataAccess.Data;
using Store.DataAccess.Repository.IRepository;
using Store.Models;
using Store.Utility;


namespace StoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_user_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        public CategoryController(IUnitOfWork db)
        {
            _unitofwork = db;
        }
        public IActionResult Index()
        {
            var list = _unitofwork.Category.GetAll().ToList();
            return View(list);
        }


        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.CatName == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("CatName", "name and display order can not match");
            }

            if (obj.CatName != null && obj.CatName == "test")
            {
                ModelState.AddModelError("", "this is invalid value");
            }

            if (ModelState.IsValid)
            {
                _unitofwork.Category.Add(obj);
                _unitofwork.save();
                TempData["success"] = "created successfuly";
                return RedirectToAction("Index");
            }
            return View(obj);

        }



        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {

                return NotFound();
            }

            Category? detail = _unitofwork.Category.Get(u => u.CategoryID == id);// only works on primary key
            //Category? detail2 = _dbcontext.catagories.FirstOrDefault(u=>u.CategoryID==id);//works on none primary key
            //Category? detail3 = _dbcontext.catagories.Where(u => u.CategoryID == id).FirstOrDefault();

            if (detail == null) { return NotFound(); }
            return View(detail);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {


            if (ModelState.IsValid)
            {
                _unitofwork.Category.update(obj);
                _unitofwork.save();

                TempData["success"] = "updates successfuly";
                return RedirectToAction("Index");
            }
            return View(obj);

        }





        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {

                return NotFound();
            }

            Category? detail = _unitofwork.Category.Get(u => u.CategoryID == id);// only works on primary key
                                                                                 //   Category? detail2 = _dbcontext.catagories.FirstOrDefault(u => u.CategoryID == id);//works on none primary key
                                                                                 //  Category? detail3 = _dbcontext.catagories.Where(u => u.CategoryID == id).FirstOrDefault();

            if (detail == null) { return NotFound(); }
            return View(detail);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(Category obj)
        {



            if (ModelState.IsValid)
            {
                _unitofwork.Category.Remove(obj);
                _unitofwork.save();
                TempData["success"] = "dleted successfuly";
                return RedirectToAction("Index");
            }
            return View(obj);

        }



    }
}
