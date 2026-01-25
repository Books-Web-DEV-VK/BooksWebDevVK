using System.Diagnostics;
using BooksWeb.DataAccess.Repository.IRepository;
using BooksWeb.Models;
using Microsoft.AspNetCore.Mvc;

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
            var products = _unitOfWork._productRepo.GetAll("Category");
            return View(products);
        }


        public IActionResult ViewProduct(Guid id)
        {
            var productDetails = _unitOfWork._productRepo.Get(p=>p.Id==id, includeProperties: "Category");
            if( productDetails == null )
            {
                TempData["error"] = "Product not found!!";
                return RedirectToAction("Index");
            }
            return View(productDetails);
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
