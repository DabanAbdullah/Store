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
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
       
        
       
        void update(OrderHeader obj);

        void updateStatus(int Id,string orderStatus,string?paymentStatus=null);

        void updatepaymentId(int Id, string sessionId, string paymentId);


    }
}
