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
    public class CompanyIRepository : Repository<Company>, ICompanyRespository
    {

        private readonly ApplicationDbContext _db;
        public CompanyIRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void update(Company obj)
        {
            _db.Companies.Update(obj);
        }
    }
}
