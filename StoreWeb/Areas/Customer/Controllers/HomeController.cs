using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.DataAccess.Repository;
using Store.DataAccess.Repository.IRepository;
using Store.Models;
using Store.Utility;
using System.Diagnostics;
using System.Security.Claims;

namespace StoreWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitofwork)
        {
            _logger = logger;
            _unitOfWork = unitofwork;
        }

        public IActionResult Index()
        {
          

            IEnumerable<Product> productlist = _unitOfWork.Product.GetAll(includeproperties: "Category,ProductImages").ToList();
            return View(productlist);
        }

        public IActionResult detail(int id)
        {
            

                Shoppingcart cart = new Shoppingcart()
                {
                    product = _unitOfWork.Product.Get(x => x.Id == id, includeproperties: "Category,ProductImages"),
                    count=1,
                    productId = id,
                    
                    

                };

               
                if(cart.product != null)
                {
                    return View(cart);
                }
                else
                {
                    return NotFound();

                }
               
           
        }

        [HttpPost]
        [Authorize]
        public IActionResult detail(Shoppingcart obj)
        {
            var claimsidentity=(ClaimsIdentity)User.Identity;
            var userID = claimsidentity.FindFirst(ClaimTypes.NameIdentifier).Value ;
            obj.ApplicationUserId = userID;

            var rec=_unitOfWork.shoppingcart.Get(x=>x.ApplicationUserId== userID&&x.productId==obj.productId);
            if(rec != null)
            {
                rec.count += obj.count;
                _unitOfWork.shoppingcart.update(rec);
                _unitOfWork.save();
            }
            else
            {
                _unitOfWork.shoppingcart.Add(obj);
                _unitOfWork.save();
                HttpContext.Session.SetInt32(SD.Sessioncart, _unitOfWork.shoppingcart.GetAll(x => x.ApplicationUserId == userID).Count());

            }



            TempData["success"] = "Shopping cart updated";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
