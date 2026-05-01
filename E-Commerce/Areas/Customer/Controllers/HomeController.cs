using BooksWeb.DataAccess.Repository.IRepository;
using BooksWeb.Models;
using BooksWeb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BooksWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork , ILogger<HomeController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var products = _unitOfWork._productRepo.GetAll(null, "Category");
            return View(products);
        }

        [Authorize]
        public IActionResult ViewProduct(Guid id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var existingCart = _unitOfWork._shoppingCartRepo
                                  .Get(sc => sc.ApplicationUserId == userId && sc.ProductId == id, null, true);
            var productDetails = _unitOfWork._productRepo
                                  .Get(p => p.Id==id, includeProperties: "Category");
            if( productDetails == null )
            {
                TempData["error"] = "Product not found!!";
                return RedirectToAction("Index");
            }
            var shoppingCart = new ShoppingCart
            {
                Product = productDetails,
                Count = existingCart != null ? existingCart.Count : 1
            };
            return View(shoppingCart);
        }

        [HttpPost]
        [Authorize(Roles = $"{SD.Role_Company},{SD.Role_Customer}")]
        public IActionResult AddToCart(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            try
            {
                var existingCart = _unitOfWork._shoppingCartRepo
                                      .Get(sc => sc.ApplicationUserId == userId && sc.ProductId == shoppingCart.Product.Id, null, true);
                if (existingCart == null)
                {
                    _unitOfWork._shoppingCartRepo.Add(new ShoppingCart
                    {
                        ApplicationUserId = userId,
                        ProductId = shoppingCart.Product.Id,
                        Count = shoppingCart.Count
                    });
                }
                else
                {
                    existingCart.Count = shoppingCart.Count;
                }
                _unitOfWork.Save();
                TempData["success"] = "Product added to cart successfully!";
                return RedirectToAction("Index", "ShoppingCart");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Some Error occured, Try Again !!";
                return RedirectToAction("Index");
            }
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
