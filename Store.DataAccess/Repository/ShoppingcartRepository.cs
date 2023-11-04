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
    public class ShoppingcartRepository: Repository<Shoppingcart>, IShoppingcartRepository
    {
        private readonly ApplicationDbContext _db;
        public ShoppingcartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void update(Shoppingcart obj)
        {
            _db.Shoopingcart.Update(obj);
        }
    }
}
