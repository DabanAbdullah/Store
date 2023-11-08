using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Store.DataAccess.Repository;
using Store.DataAccess.Repository.IRepository;
using Store.Models;
using Store.Models.VM;
using Store.Utility;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace StoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitofwork;

        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork un)
        {
            _unitofwork = un;

        }

        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int orderid)
        {
           

            
                OrderVM = new()
                {
                    OrderHeader = _unitofwork.OrderHeader.Get(x => x.Id == orderid, includeproperties: "AppUser"),
                    orderDetail = _unitofwork.OrderDetail.GetAll(x => x.Orderheaderid == orderid, includeproperties: "Product")
                };

            if (User.IsInRole(SD.Role_user_cust) || User.IsInRole(SD.Role_user_Com))
            {
                var claimsidentity = (ClaimsIdentity)User.Identity;
                var userID = claimsidentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                var rec = _unitofwork.OrderHeader.Get(x => x.Id == orderid && x.ApplicationUserId == userID, includeproperties: "AppUser");
                if (rec == null)
                {
                    TempData["error"] = "You Dont have Such Order";
                  return  RedirectToAction(nameof(Index));
                }
                
            }
            return View(OrderVM);
        }

        [Authorize(Roles = SD.Role_user_Admin + "," + SD.Role_user_Employee)]
        [HttpPost]
        public IActionResult UpdateOrderDetail()
        {
            var fromdb = _unitofwork.OrderHeader.Get(x => x.Id == OrderVM.OrderHeader.Id, includeproperties: "AppUser");


            fromdb.Name = OrderVM.OrderHeader.Name;
            fromdb.Phonenumber = OrderVM.OrderHeader.Phonenumber;
            fromdb.State = OrderVM.OrderHeader.State;
            fromdb.City = OrderVM.OrderHeader.City;
            fromdb.StreetAdress = OrderVM.OrderHeader.StreetAdress;
            fromdb.Postalcode = OrderVM.OrderHeader.Postalcode;

            if (!string.IsNullOrEmpty(OrderVM.OrderHeader.Carrier))
            {
                fromdb.Carrier = OrderVM.OrderHeader.Carrier;
            }
            if (!string.IsNullOrEmpty(OrderVM.OrderHeader.Trackingnumber))
            {
                fromdb.Trackingnumber = OrderVM.OrderHeader.Trackingnumber;
            }
            _unitofwork.OrderHeader.update(fromdb);

            _unitofwork.save();
            TempData["success"] = "Order Header Updated successfuly";
            return RedirectToAction(nameof(Detail), new { orderid = fromdb.Id });
        }


        
        [HttpPost]
        public IActionResult Pay()
        {


            OrderVM = new()
            {
                OrderHeader = _unitofwork.OrderHeader.Get(x => x.Id == OrderVM.OrderHeader.Id, includeproperties: "AppUser"),
                orderDetail = _unitofwork.OrderDetail.GetAll(x => x.Orderheaderid == OrderVM.OrderHeader.Id, includeproperties: "Product")
            };

            var domian = "https://localhost:7101/";
            var options = new SessionCreateOptions
            {
                SuccessUrl = domian + $"Admin/Order/OrderConfirmation?id={OrderVM.OrderHeader.Id}",
                CancelUrl = domian + $"Admin/Order/Detail?orderid={OrderVM.OrderHeader.Id}",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };


            foreach (var item in OrderVM.orderDetail)
            {
                var sessionLineitem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.price * 100), //20.50 => to make 2050
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title,
                            
                        }
                    },
                    Quantity = item.count


                };
                options.LineItems.Add(sessionLineitem);
            }

            var service = new SessionService();
            Session session = service.Create(options);

            _unitofwork.OrderHeader.updatepaymentId(OrderVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
            _unitofwork.save();
            
            TempData["success"] = "Paid  successfuly";
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
            return RedirectToAction(nameof(OrderConfirmation),new {id=OrderVM.OrderHeader.Id});
        }



        public IActionResult OrderConfirmation(int id)
        {
            var orderheader = _unitofwork.OrderHeader.Get(x => x.Id == id);
            if (orderheader.Paymentstatus == SD.Payment_status_Delayed_payment)
            {
                //order from cutomer

                var service = new SessionService();
                Session session = service.Get(orderheader.SessionId);
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitofwork.OrderHeader.updatepaymentId(id, session.Id, session.PaymentIntentId);
                    _unitofwork.OrderHeader.updateStatus(id, orderheader.Orderstatus, SD.Payment_status_Approved);
                    _unitofwork.save();

                }
            }

           
            return View(id);
        }



        [Authorize(Roles = SD.Role_user_Admin + "," + SD.Role_user_Employee)]

        [HttpPost]
        public IActionResult StartProccessing()
        {

            _unitofwork.OrderHeader.updateStatus(OrderVM.OrderHeader.Id, SD.status_InProccess);
            _unitofwork.save();

            TempData["success"] = "Order Header Updated successfuly";
            return RedirectToAction(nameof(Detail), new { orderid = OrderVM.OrderHeader.Id });
        }

        [Authorize(Roles = SD.Role_user_Admin + "," + SD.Role_user_Employee)]

        [HttpPost]
        public IActionResult CancelOrder()
        {

            var order = _unitofwork.OrderHeader.Get(x => x.Id == OrderVM.OrderHeader.Id);
            if(order.Paymentstatus==SD.Payment_status_Approved ) {

                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = order.Paymentintendid,
                };
                var services = new RefundService();
                Refund refund=services.Create(options);
                _unitofwork.OrderHeader.updateStatus(order.Id, SD.status_Refund);
            }
            else
            {
                _unitofwork.OrderHeader.updateStatus(order.Id, SD.status_Cancelled);
            }


            //_unitofwork.OrderHeader.updateStatus(OrderVM.OrderHeader.Id, SD.status_Cancelled);
            //_unitofwork.save();

            TempData["success"] = "Order Canceled successfuly";
            return RedirectToAction(nameof(Detail), new { orderid = order.Id });
        }

        [HttpPost]
        public IActionResult ShipOrder()
        {

            var order=_unitofwork.OrderHeader.Get(x=>x.Id==OrderVM.OrderHeader.Id);
            order.Trackingnumber = OrderVM.OrderHeader.Trackingnumber;
            order.Orderstatus = SD.status_Shipped;
            order.Shippingdate=DateTime.Now;
            order.Carrier = OrderVM.OrderHeader.Carrier;
            if (order.Paymentstatus == SD.Payment_status_Delayed_payment)
            {
                order.Paymentduedate =DateOnly.FromDateTime( DateTime.Now.AddDays(30));
            }
            _unitofwork.OrderHeader.update(order);
            _unitofwork.save();

            TempData["success"] = "Order Shipped succesfuly";
            return RedirectToAction(nameof(Detail), new { orderid = OrderVM.OrderHeader.Id });
        }



        #region APICalls
        [HttpGet]
        public IActionResult getall(string status)
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var userID = claimsidentity.FindFirst(ClaimTypes.NameIdentifier).Value;


            IEnumerable<OrderHeader> list;
            if (User.IsInRole(SD.Role_user_Admin)|| User.IsInRole(SD.Role_user_Employee))
            {
                list= _unitofwork.OrderHeader.GetAll(includeproperties: "AppUser");
            }
            else
            {
                
                list = _unitofwork.OrderHeader.GetAll(x=>x.ApplicationUserId==userID ,includeproperties: "AppUser");
            }
         
            switch (status)
            {
                case "pending": list = list.Where(x => x.Paymentstatus ==SD.Payment_status_Delayed_payment);
                    break;
                case "inproccess":
                    list = list.Where(x => x.Orderstatus == SD.status_InProccess);
                    break;
                case "completed": list = list.Where(x => x.Orderstatus == SD.status_Shipped);
                    break;
                case "approved": list = list.Where(x => x.Orderstatus == SD.status_Approved);
                    break;
                default: break;
            }
            return Json(new { data = list });
        }


        #endregion
    }
}
