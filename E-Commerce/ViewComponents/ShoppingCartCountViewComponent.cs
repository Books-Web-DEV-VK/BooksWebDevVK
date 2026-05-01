using BooksWeb.DataAccess.Repository.IRepository;
using BooksWeb.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BooksWeb.ViewComponents
{
    public class ShoppingCartCountViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartCountViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (claimsIdentity != null)
            {
                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork._shoppingCartRepo.GetAll(sc => sc.ApplicationUserId == userId).ToList().Count);
                return View(HttpContext.Session.GetInt32(SD.SessionCart));
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
        }
    }
}
