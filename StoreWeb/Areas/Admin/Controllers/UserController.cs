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
        private readonly ApplicationDbContext db;

        private readonly UserManager<IdentityUser> usermanager;
        public UserController(ApplicationDbContext _db, UserManager<IdentityUser> _usermanager)
        {
            db = _db;
            usermanager= _usermanager;
        }
        public IActionResult Index()
        {
           
            return View();
        }


        public IActionResult UserRoleManagement(string Id)
        {
         Applicationuser  appuser1 = db.ApplicationUsers.Include(x => x.Company).Where(x=>x.Id==Id).FirstOrDefault();


            if(appuser1 != null)
            {

                var roles = db.Roles.ToList();
                var userrole = db.UserRoles.ToList();

                var roleid = userrole.FirstOrDefault(x => x.UserId == appuser1.Id).RoleId;
                appuser1.rolename = roles.FirstOrDefault(x => x.Id == roleid).Name;

                UserRoleManageVM vm = new UserRoleManageVM()
                {
                    appuser = appuser1,
                    rolelist=roles.Select(i=>new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Name
                    }),

                     companlist = db.Companies.ToList().Select(i => new SelectListItem
                     {
                         Text = i.Name,
                         Value = i.Id.ToString()
                     })

                };

                return View(vm);

            }

           

            return View();
        }


        [HttpPost]
        public IActionResult UserRoleManagement(UserRoleManageVM model)
        {
            var RoleId = db.UserRoles.FirstOrDefault(x => x.UserId == model.appuser.Id).RoleId;
            var oldname = db.Roles.FirstOrDefault(x => x.Id == RoleId).Name;
            if(!(model.appuser.rolename==oldname))
            {
                Applicationuser appuser = db.ApplicationUsers.FirstOrDefault(x => x.Id == model.appuser.Id);
                if (model.appuser.rolename == SD.Role_user_Com)
                {
                    appuser.companyId = model.appuser.companyId;
                }
                if(oldname==SD.Role_user_Com)
                {
                    appuser.companyId = null;
                }


                db.SaveChanges();
                usermanager.RemoveFromRoleAsync(appuser, oldname).GetAwaiter().GetResult();
                usermanager.AddToRoleAsync(appuser, model.appuser.rolename).GetAwaiter().GetResult();
                TempData["success"] = "Role Updated";
              
            }



            return RedirectToAction("Index");
        }



        #region APICalls
        [HttpGet]
        public IActionResult getall()
        {
            List <Applicationuser> appuser = db.ApplicationUsers.Include(x=>x.Company).ToList();
            var roles = db.Roles.ToList();
            var userrole = db.UserRoles.ToList();
            foreach(Applicationuser row in appuser)
            {
                var roleid = userrole.FirstOrDefault(x => x.UserId == row.Id).RoleId;
                row.rolename = roles.FirstOrDefault(x => x.Id == roleid).Name;
             
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
            var rec = db.ApplicationUsers.FirstOrDefault(x => x.Id == id);
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


                db.SaveChanges();

            }


            return Json(new { success = true, message = status });
         }
           
           
        }
        #endregion

    
}
