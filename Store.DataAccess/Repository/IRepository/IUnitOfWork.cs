using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; } 

        ICompanyRespository Company { get; }
        IShoppingcartRepository shoppingcart { get; }


        IApplicationUserRepository Applcationuser { get; }

        IOrderHeaderRepository OrderHeader { get; }
        IOrderDetailRepository OrderDetail { get; }

        IProductImageRepository ProductImage { get; }

        void save();
    }
}
