
using Microsoft.EntityFrameworkCore;
using Store.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Utility;
using Microsoft.AspNetCore.Identity;
using Store.Models;

namespace Store.DataAccess.DBInitializer
{
    public class DBInitializer : IDBInitializer
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext DbContext;

            public DBInitializer(UserManager<IdentityUser> usrmngr,
              RoleManager<IdentityRole> idntmngr,
              ApplicationDbContext dbcntxt)
            {
            userManager= usrmngr;
            _roleManager = idntmngr;
            DbContext= dbcntxt;
            }

        public  void  Initialize()
        {
            //migration if not applied

            //add role if not added

            //create admin users

            try
            {

                if (DbContext.Database.GetPendingMigrations().Count() > 0) {

                    DbContext.Database.Migrate();
                }

               
            }
            catch(Exception ex) { 
            
            }

            if (!_roleManager.RoleExistsAsync(SD.Role_user_cust).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_user_cust)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_user_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_user_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_user_Com)).GetAwaiter().GetResult();

                userManager.CreateAsync(new Applicationuser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    fullname = "Admin",
                    state = "Bavaria",
                    city = "Nuremberg",
                    postalcode = "90431",
                    PhoneNumber = "00491786636042",
                    EmailConfirmed = true

                }, "admin123*").GetAwaiter().GetResult();

                Applicationuser appuser = DbContext.ApplicationUsers.FirstOrDefault(x => x.Email == "admin@gmail.com");
                userManager.AddToRoleAsync(appuser, SD.Role_user_Admin).GetAwaiter().GetResult();
            }

            return;
        }
    }
}
