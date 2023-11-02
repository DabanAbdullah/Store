using Store.DataAccess.Data;
using Store.DataAccess.Repository.IRepository;
using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>,ICategoryRepository  
    {

        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
       

        public void update(Category obj)
        {
            _db.catagories.Update(obj);
        }
    }
}
