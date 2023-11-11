using Store.DataAccess.Data;
using Store.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICategoryRepository Category {get; private set;}
        public IProductRepository Product { get; private set;}

        public ICompanyRespository Company { get; private set; }

        public IShoppingcartRepository shoppingcart { get; private set; }


        public IApplicationUserRepository Applcationuser {  get; private set; } 

       public IOrderHeaderRepository OrderHeader { get; private set; }

       public IOrderDetailRepository OrderDetail { get; private set; }

        public IProductImageRepository ProductImage { get; private set; }


        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db) 
        {
            _db = db;
            Category = new CategoryRepository(_db); 
            Product = new ProductRepository(_db);
            Company = new CompanyIRepository(_db);
            shoppingcart = new ShoppingcartRepository(_db);
            Applcationuser=new ApplicationUserRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            OrderDetail = new OrderDetailRepository(_db);
            ProductImage= new ProductImageRepository(_db);
        }
        public void save()
        {
            _db.SaveChanges();  
        }
    }
}
