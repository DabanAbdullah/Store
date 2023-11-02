using Store.DataAccess.Data;
using Store.DataAccess.Repository.IRepository;
using Store.Models;
using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {

        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void update(Product obj)
        {
          //  _db.Products.Update(obj);
          var rec=_db.Products.FirstOrDefault(x=>x.Id == obj.Id);
            if (rec != null)
            {
                rec.Title = obj.Title;
                rec.Description = obj.Description;
                rec.Category = obj.Category;
                rec.ISBN = obj.ISBN;
                rec.Price = obj.Price;
                rec.Price50 = obj.Price50;  
                rec.ListPrice = obj.ListPrice;
                rec.Author = obj.Author;
                rec.CatID = obj.CatID;  
                if (obj.imageurl != null)
                {
                    rec.imageurl = obj.imageurl;
                }
            }
            
        }

       
    }
}
