using BooksWeb.DataAccess.Repository.IRepository;
using BooksWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace BooksWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Product> products = _unitOfWork._productRepo.GetAll().ToList();
            return View(products);
        }

        public IActionResult CreateProduct()
        {
            return View(); // nothing is passed to the view CreateProduct.cshtml - empty Product model eill be created and set to view
        }

        [HttpPost]
        public IActionResult CreateProduct(Product newProduct)
        {
            try
            {
                checkAndSetModelErrors(newProduct);
                if (newProduct.ISBN.Length > 5)
                {
                    ModelState.AddModelError("", "ISBN Should atleast have 5 characters."); // Model level error 
                }
                if (ModelState.IsValid)
                {
                    _unitOfWork._productRepo.Add(newProduct);
                    _unitOfWork.Save();
                    TempData["success"] = "Successfully created the product !!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("CreateProduct", newProduct);
                }
            }
            catch(Exception ex)
            {
                TempData["error"] = "Error occured while creating the product !! Try again after sometime.";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult ViewProduct(Guid id, string actionType = "get")
        {
            Product? product = _unitOfWork._productRepo.Get(u => u.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            if (actionType.ToLower() == "get")
                return View("ViewProduct", product);
            else if (actionType.ToLower() == "edit")
                return View("EditProduct", product);
            else if (actionType.ToLower() == "delete")
                return View("DeleteConfirmation", product);
            else
                return View("Error");
        }

        [HttpPost]
        public IActionResult UpdateProduct(Product updatableProduct)
        {
            try
            {
                checkAndSetModelErrors(updatableProduct);
                if (updatableProduct.ISBN.Length > 5)
                {
                    ModelState.AddModelError("", "ISBN Should atleast have 5 characters."); // Model level error 
                }
                if (ModelState.IsValid)
                {
                    TempData["success"] = "Successfully updated the product !!";
                    _unitOfWork._productRepo.Update(updatableProduct);
                    _unitOfWork.Save();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("EditProduct", updatableProduct);
                }
            }
            catch(Exception ex)
            {
                TempData["error"] = "Error occured while updating the product !! Try again after sometime.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult DeleteProduct(Product removableProduct)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["success"] = "Successfully deleted the product !!";
                    _unitOfWork._productRepo.Remove(removableProduct);
                    _unitOfWork.Save();
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorr"] = "Error occured while deleting the product !! Try again after sometime.";
                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                TempData["errorr"] = "Error occured while deleting the product !! Try again after sometime.";
                return RedirectToAction("Index");
            }
        }

        private void checkAndSetModelErrors(Product product)
        {
            var properties = product.GetType().GetProperties();
            Console.WriteLine("Total Properties : ", properties);

            for (int i = 0; i < properties.Length; i++)
            {
                var value = properties[i].GetValue(product);
                Console.WriteLine("Property Name : " + properties[i].Name + " Value : " + value + " ---- " + value?.ToString());
                bool isRequiredProperty = Attribute.IsDefined(properties[i], typeof(RequiredAttribute));
                if (isRequiredProperty && (value is null || String.IsNullOrWhiteSpace(value.ToString())))
                {
                    Console.WriteLine("Adding model error for property : " + properties[i].Name);
                    ModelState.AddModelError(properties[i].Name, $"{properties[i].Name} is required");
                }
                else if (value is not null && hasInvalidCharacters(value.ToString()))
                {
                    Console.WriteLine("Adding model error for property : " + properties[i].Name);
                    ModelState.AddModelError("", $"{properties[i].Name} contains invalid characters");
                }
            }
        }

        private bool hasInvalidCharacters(string str)
        {
            Char[] invalidCharactersSet = new Char[] { '<', '>', '\'', '=' };
            foreach (char c in invalidCharactersSet)
            {
                if (str.Contains(c))
                    return true;
            }
            return false;
        }
    }
}
