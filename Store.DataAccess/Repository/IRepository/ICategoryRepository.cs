using Store.DataAccess.Data;
using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
       
        
       
        void update(Category obj);
      
       
    }
}
