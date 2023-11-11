using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using Store.DataAccess.Data;
using Store.DataAccess.Repository.IRepository;
using Store.Models;
using Store.Models.VM;
using Store.Utility;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;


namespace StoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_user_Admin)]
    public class UserController : Controller
    {
        private readonly IUnitOfWork db;

        private readonly UserManager<IdentityUser> usermanager;
        private readonly RoleManager<IdentityRole> rolemanager;
        public UserController(IUnitOfWork _db, UserManager<IdentityUser> _usermanager, RoleManager<IdentityRole> _rolemanager)
        {
            db = _db;
            usermanager = _usermanager;
            rolemanager = _rolemanager;
        }
        public IActionResult Index()
        {
           
            return View();
        }


        public IActionResult UserRoleManagement(string Id)
        {
            Applicationuser appuser1 = db.Applcationuser.Get(x => x.Id == Id,includeproperties: "Company");


            if(appuser1 != null)
            {

                var roles = rolemanager.Roles.ToList();
              
                UserRoleManageVM vm = new UserRoleManageVM()
                {
                    appuser = appuser1,
                    rolelist=roles.Select(i=>new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Name
                    }),

                     companlist = db.Company.GetAll().Select(i => new SelectListItem
                     {
                         Text = i.Name,
                         Value = i.Id.ToString()
                     })

                };

                vm.appuser.rolename = usermanager.GetRolesAsync(db.Applcationuser.Get(x => x.Id == Id))
                    .GetAwaiter().GetResult().FirstOrDefault();

                return View(vm);

            }

           

            return View();
        }


        [HttpPost]
        public IActionResult UserRoleManagement(UserRoleManageVM model)
        {
            
            var oldrole= usermanager.GetRolesAsync(db.Applcationuser.Get(x => x.Id == model.appuser.Id))
                    .GetAwaiter().GetResult().FirstOrDefault();

            Applicationuser appuser = db.Applcationuser.Get(x => x.Id == model.appuser.Id);
            if (!(model.appuser.rolename== oldrole))
            {
               
                if (model.appuser.rolename == SD.Role_user_Com)
                {
                    appuser.companyId = model.appuser.companyId;
                }
                if(oldrole==SD.Role_user_Com)
                {
                    appuser.companyId = null;
                }

                db.Applcationuser.update(appuser);
                db.save();
                usermanager.RemoveFromRoleAsync(appuser, oldrole).GetAwaiter().GetResult();
                usermanager.AddToRoleAsync(appuser, model.appuser.rolename).GetAwaiter().GetResult();
                TempData["success"] = "Role Updated";
              
            }
            else
            {
                if(oldrole==SD.Role_user_Com && appuser.companyId != model.appuser.companyId)
                {
                    appuser.companyId = model.appuser.companyId;
                    db.Applcationuser.update(appuser);
                    db.save();
                    TempData["success"] = "Company Updated";
                }
            }



            return RedirectToAction("Index");
        }



        #region APICalls
        [HttpGet]
        public IActionResult getall()
        {
            List <Applicationuser> appuser = db.Applcationuser.GetAll(includeproperties: "Company").ToList();
            var roles = rolemanager.Roles.ToList();
          
            foreach(Applicationuser row in appuser)
            {
               
                row.rolename = usermanager.GetRolesAsync(row).GetAwaiter().GetResult().FirstOrDefault();
             
                if (row.Company == null)
                {
                    row.Company = new Company() { Name = "" };
                }

            }
            return Json(new { data= appuser });
        }


        [HttpPost]
        public IActionResult LockUnlock([FromBody]string id)
        {
            var rec = db.Applcationuser.Get(x => x.Id == id);
            string status = "";
            if (rec == null){
                return Json(new { success = true, message = "error while lockunlock user no such record" });
            }
            else
            {
                if (rec.LockoutEnd != null && rec.LockoutEnd > DateTime.Now)
                {
                    rec.LockoutEnd = DateTime.Now;
                    status = "record unlocked";
                }
                else
                {
                    rec.LockoutEnd = DateTime.Now.AddYears(100);
                    status = "record was locked for 100 years";
                }

                db.Applcationuser.update(rec);
                db.save();

            }


            return Json(new { success = true, message = status });
         }
           
           
        }
        #endregion

    
}
