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
    public class OrderHeaderRepository : Repository<OrderHeader>,IOrderHeaderRepository  
    {

        private readonly ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
       

        public void update(OrderHeader obj)
        {
            _db.OrderHeaders.Update(obj);
        }

        public void updatepaymentId(int Id, string sessionId, string paymentId)
        {

            var fromDB = _db.OrderHeaders.FirstOrDefault(x => x.Id == Id);
            
            if (!string.IsNullOrEmpty(sessionId))
            {
                fromDB.SessionId = sessionId;

               
            }

            if (!string.IsNullOrEmpty(paymentId))
            {
                fromDB.Paymentintendid = paymentId;
                fromDB.Paymentdate = DateTime.Now;
            }
        }

        public void updateStatus(int Id, string orderStatus, string? paymentStatus = null)
        {
            var fromDB = _db.OrderHeaders.FirstOrDefault(x => x.Id == Id);
            if (fromDB != null)
            {
                fromDB.Orderstatus= orderStatus;
                if (!string.IsNullOrEmpty(paymentStatus))
                {
                    fromDB.Paymentstatus= paymentStatus;
                }
            }
        }
    }
}
