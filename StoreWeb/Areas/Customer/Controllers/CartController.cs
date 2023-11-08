using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Store.DataAccess.Repository.IRepository;
using Store.Models;
using Store.Models.VM;
using Store.Utility;
using Stripe.Checkout;
using System.Security.Claims;

namespace StoreWeb.Areas.Customer.Controllers
{

    [Authorize]
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unofwork,IEmailSender em)
        {
            _unitOfWork = unofwork;
            _emailSender = em;
        }

        public IActionResult Index()
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var userID = claimsidentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new ShoppingCartVM()
            {
                shoppingcartlist = _unitOfWork.shoppingcart.GetAll(x => x.ApplicationUserId == userID, includeproperties: "product"),
                orderheader = new(),

            };

            foreach (Shoppingcart sp in ShoppingCartVM.shoppingcartlist)
            {
                double price = getpricebyquantity(sp);
                sp.price = price;
                ShoppingCartVM.orderheader.Total = ShoppingCartVM.orderheader.Total + (price * sp.count);
            }

            return View(ShoppingCartVM);
        }

        public IActionResult plus(int cardId)
        {
            var fromdb = _unitOfWork.shoppingcart.Get(x => x.Id == cardId);
            fromdb.count += 1;
            _unitOfWork.shoppingcart.update(fromdb);
            _unitOfWork.save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult minus(int cardId)
        {
            var fromdb = _unitOfWork.shoppingcart.Get(x => x.Id == cardId);
            if (fromdb.count > 1)
            {
                fromdb.count -= 1;
                _unitOfWork.shoppingcart.update(fromdb);
                _unitOfWork.save();
            }
            return RedirectToAction(nameof(Index));
        }


        public IActionResult remove(int cardId)
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var userID = claimsidentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var fromdb = _unitOfWork.shoppingcart.Get(x => x.Id == cardId, tracked:true); ;

            _unitOfWork.shoppingcart.Remove(fromdb);
            _unitOfWork.save();

            HttpContext.Session.SetInt32(SD.Sessioncart, _unitOfWork.shoppingcart.GetAll(x => x.ApplicationUserId == userID).Count());

            return RedirectToAction(nameof(Index));
        }

        public IActionResult summary()
        {

            var claimsidentity = (ClaimsIdentity)User.Identity;
            var userID = claimsidentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new ShoppingCartVM()
            {
                shoppingcartlist = _unitOfWork.shoppingcart.GetAll(x => x.ApplicationUserId == userID, includeproperties: "product"),
                orderheader = new(),

            };

            ShoppingCartVM.orderheader.AppUser = _unitOfWork.Applcationuser.Get(x => x.Id == userID);
            ShoppingCartVM.orderheader.City = ShoppingCartVM.orderheader.AppUser.city;
            ShoppingCartVM.orderheader.State = ShoppingCartVM.orderheader.AppUser.state;
            ShoppingCartVM.orderheader.Name = ShoppingCartVM.orderheader.AppUser.fullname;
            ShoppingCartVM.orderheader.Phonenumber = ShoppingCartVM.orderheader.AppUser.PhoneNumber;
            ShoppingCartVM.orderheader.StreetAdress = ShoppingCartVM.orderheader.AppUser.StreetAdress;
            ShoppingCartVM.orderheader.Postalcode = ShoppingCartVM.orderheader.AppUser.postalcode;



            foreach (Shoppingcart sp in ShoppingCartVM.shoppingcartlist)
            {
                double price = getpricebyquantity(sp);
                sp.price = price;
                ShoppingCartVM.orderheader.Total = ShoppingCartVM.orderheader.Total + (price * sp.count);
            }


            return View(ShoppingCartVM);
        }



        [HttpPost]
        [ActionName("summary")]

        public IActionResult summaryPost()
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var userID = claimsidentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM.shoppingcartlist = _unitOfWork.shoppingcart.GetAll(x => x.ApplicationUserId == userID, includeproperties: "product");

            ShoppingCartVM.orderheader.ApplicationUserId = userID;
            ShoppingCartVM.orderheader.Orderdate = DateTime.Now;


            Applicationuser AppUser = _unitOfWork.Applcationuser.Get(x => x.Id == userID);
            foreach (Shoppingcart sp in ShoppingCartVM.shoppingcartlist)
            {
                double price = getpricebyquantity(sp);
                sp.price = price;
                ShoppingCartVM.orderheader.Total = ShoppingCartVM.orderheader.Total + (price * sp.count);
            }

            if (AppUser.companyId.GetValueOrDefault() == 0)
            {
                //regular customer account we need to capture payment

                ShoppingCartVM.orderheader.Paymentstatus = SD.Payment_status_Pending;
                ShoppingCartVM.orderheader.Orderstatus = SD.status_Pending;

            }
            else
            {
                ShoppingCartVM.orderheader.Paymentstatus = SD.Payment_status_Delayed_payment;
                ShoppingCartVM.orderheader.Orderstatus = SD.status_Approved;
                //company account
            }

            _unitOfWork.OrderHeader.Add(ShoppingCartVM.orderheader);
            _unitOfWork.save();

            foreach (var item in ShoppingCartVM.shoppingcartlist)
            {
                OrderDetail detail = new OrderDetail()
                {
                    Orderheaderid = ShoppingCartVM.orderheader.Id,
                    Productid = item.productId,
                    count = item.count,
                    price = item.price,
                };
                _unitOfWork.OrderDetail.Add(detail);
                _unitOfWork.save();

            }

            if (AppUser.companyId.GetValueOrDefault() == 0)
            {
                //regular customer account we need to capture payment
                //strip payment
                var domian = "https://localhost:7101/";
                var options = new SessionCreateOptions
                {
                    SuccessUrl = domian + $"Customer/cart/OrderConfirmation?id={ShoppingCartVM.orderheader.Id}",
                    CancelUrl = domian + $"Customer/cart/Index",
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                };


                foreach(var item in ShoppingCartVM.shoppingcartlist){
                    var sessionLineitem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.price * 100), //20.50 => to make 2050
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.product.Title
                            }
                        },
                        Quantity = item.count


                    };
                    options.LineItems.Add(sessionLineitem);
                }

                var service = new SessionService();
                Session session= service.Create(options);
                _unitOfWork.OrderHeader.updatepaymentId(ShoppingCartVM.orderheader.Id, session.Id, session.PaymentIntentId);
                _unitOfWork.save();
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
                 
            }

            return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartVM.orderheader.Id });
        }


        public IActionResult OrderConfirmation(int id)
        {
            var orderheader = _unitOfWork.OrderHeader.Get(x => x.Id == id,includeproperties: "AppUser");
            if (orderheader.Paymentstatus != SD.Payment_status_Delayed_payment)
            {
                //order from cutomer

                var service=new SessionService();
                Session session = service.Get(orderheader.SessionId);
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.updatepaymentId(id, session.Id, session.PaymentIntentId);
                    _unitOfWork.OrderHeader.updateStatus(id, SD.status_Approved, SD.Payment_status_Approved);
                    _unitOfWork.save();

                }
            }

            _emailSender.SendEmailAsync(orderheader.AppUser.Email, "you have made purchase", "hello dear customer thank you for your purchase order ID= " + orderheader.Id);


            List<Shoppingcart> cart=_unitOfWork.shoppingcart.GetAll(x=>x.ApplicationUserId==orderheader.ApplicationUserId).ToList();
            _unitOfWork.shoppingcart.RemoveRange(cart);
            _unitOfWork.save();
            HttpContext.Session.SetInt32(SD.Sessioncart, _unitOfWork.shoppingcart.GetAll(x => x.ApplicationUserId == orderheader.ApplicationUserId).Count());



            return View(id);
        }

        double getpricebyquantity(Shoppingcart item)
        {
            if (item.count == 1)
            {
                return item.product.ListPrice;
            }
            else
            {
                if (item.count <= 50)
                {
                    return item.product.Price;
                }
                else
                {
                    return item.product.Price50;
                }
            }
        }
    }
}
