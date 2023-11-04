using Store.DataAccess.Data;
using Store.DataAccess.Repository.IRepository;
using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<Applicationuser>,IApplicationUserRepository
    {

        private readonly ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void update(Applicationuser obj)
        {
            //  _db.Products.Update(obj);
            var rec = _db.ApplicationUsers.FirstOrDefault(x => x.Id == obj.Id);
            if (rec != null)
            {
                rec.fullname = obj.fullname;
                rec.Email = obj.Email;
                rec.StreetAdress = obj.StreetAdress;
                rec.state = obj.state;
                rec.PhoneNumber = obj.PhoneNumber;
                rec.postalcode = obj.postalcode;
               
            }
         }
        
    }
}
