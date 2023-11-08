using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.DataAccess;
using Store.DataAccess.Repository.IRepository;
using Store.Models;
using Store.Utility;
using System.Security.Claims;
namespace StoreWeb.Areas.ViewComponents
{
    public class ShoppingcartViewComponent: ViewComponent
    {

        private readonly IUnitOfWork _unitOfWork;
        public ShoppingcartViewComponent(IUnitOfWork un)
        {
            _unitOfWork = un;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var cliam = claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
            if (cliam != null)
            {
                if (HttpContext.Session.Get(SD.Sessioncart) != null)
                {
                 return View(HttpContext.Session.GetInt32(SD.Sessioncart));
                }
                else
                {
                    HttpContext.Session.SetInt32(SD.Sessioncart, _unitOfWork.shoppingcart.GetAll(x => x.ApplicationUserId == cliam.Value).Count());
                    return View(HttpContext.Session.GetInt32(SD.Sessioncart));
                }
           
                
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
        }
    }
}
