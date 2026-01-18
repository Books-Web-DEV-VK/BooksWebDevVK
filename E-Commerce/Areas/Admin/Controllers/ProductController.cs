using BooksWeb.DataAccess.Repository.IRepository;
using BooksWeb.Models;
using BooksWeb.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Resources;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace BooksWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnv;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnv)
        {
            _unitOfWork = unitOfWork;
            _webHostEnv = webHostEnv;
        }

        public IActionResult Index()
        {
            List<Product> products = _unitOfWork._productRepo.GetAll("Category").ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult GetAllInJSON()
        {
            List<Product> products = _unitOfWork._productRepo.GetAll("Category").ToList();
            return Json(new { data = products });
        }

        [HttpGet]
        [Route("Admin/Product/{action}/{actiontype}/{id?}")]  // Here action is like keyword for MVC to identify the method to be called
        public IActionResult Details(string actiontype, Guid? id)
        {
            List<Category> categoryList = _unitOfWork._categoryRepo.GetAll().ToList();
            actiontype = actiontype.ToLower();
            if (actiontype == "create")
            {
                ViewBag.ActionType = "create";
                return View("ProductDetails", new ProductVM()
                {
                    Product = new Product(),
                    CategoryList = categoryList
                });
            }
            if (actiontype == "view" || actiontype == "edit")
            {
                if (id == null)
                {
                    TempData["error"] = "Invalid Url Accessed, Returning to List Page!!";
                    return RedirectToAction("Index");
                }
                Product? product = _unitOfWork._productRepo.Get(u => u.Id == id);
                ViewBag.ActionType = actiontype;
                if (product == null)
                {
                    TempData["error"] = "Product not found !!";
                    return RedirectToAction("Index");
                }
                return View("ProductDetails", new ProductVM()
                {
                    Product = product,
                    CategoryList = categoryList
                });
            }
            else
            {
                TempData["error"] = "Invalid Url Accessed, Returning to List Page!!";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult ManageProducts(ProductVM productVM, string actionType, IFormFile? file)
       {
            try
            {
                List<Category> categoryList = _unitOfWork._categoryRepo.GetAll().ToList();
                checkAndSetModelErrors(productVM.Product);
                if (productVM.Product.ISBN.Length > 5)
                {
                    ModelState.AddModelError("", "ISBN Should atleast have 5 characters."); // Model level error 
                }
                if (ModelState.IsValid) 
                {
                    switch (actionType.ToLower())
                    {
                        case "create":
                            productVM.Product.ImageUrl = createImage(file);
                            _unitOfWork._productRepo.Add(productVM.Product);
                            _unitOfWork.Save();
                            TempData["success"] = "Successfully created the product !!";
                            break;
                        case "edit":
                            if (!string.IsNullOrEmpty(productVM.Product?.ImageUrl))
                            {
                                string oldImageUrl = Path.Combine(_webHostEnv.WebRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                                if (System.IO.File.Exists(oldImageUrl))
                                {
                                    System.IO.File.Delete(oldImageUrl);
                                }
                            }
                            productVM.Product.ImageUrl = createImage(file);
                            _unitOfWork._productRepo.Update(productVM.Product);
                            _unitOfWork.Save();
                            TempData["success"] = "Successfully updated the product !!";
                            break;
                        default:
                            TempData["error"] = "Invalid Action Type !!";
                            return RedirectToAction("Index");
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ActionType = actionType.ToLower();
                    productVM.CategoryList = categoryList;
                    return View("ProductDetails", productVM);
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error occured while processing the request !! Try again after sometime.";
                return RedirectToAction("Index");
            }
        }

        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            var removableProduct = _unitOfWork._productRepo.Get(p=>p.Id==id);
            if (removableProduct == null)
                return NotFound("Product Not Found !!");
            else
            {
                if (!string.IsNullOrEmpty(removableProduct?.ImageUrl))
                {
                    string oldImageUrl = Path.Combine(_webHostEnv.WebRootPath, removableProduct.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImageUrl))
                    {
                        System.IO.File.Delete(oldImageUrl);
                    }
                }
                _unitOfWork._productRepo.Remove(removableProduct);
                _unitOfWork.Save();
                return Ok("Delete Successfully !!");
            }
        }

        private void checkAndSetModelErrors(Product product)
        {
            var properties = product.GetType().GetProperties();

            for (int i = 0; i < properties.Length; i++)
            {
                var value = properties[i].GetValue(product);
                if (properties[i].Name == "Category") // No point od checking the Navigation Properties
                {
                    continue;
                }
                bool isRequiredProperty = Attribute.IsDefined(properties[i], typeof(RequiredAttribute));
                if (isRequiredProperty && (value is null || String.IsNullOrWhiteSpace(value.ToString())))
                {
                    ModelState.AddModelError(properties[i].Name, $"{properties[i].Name} is required");
                }
                else if (value is not null && hasInvalidCharacters(value.ToString()))
                {
                    ModelState.AddModelError("", $"{properties[i].Name} contains invalid characters");
                }
            }
        }

        private bool hasInvalidCharacters(string str)
        {
            Char[] invalidCharactersSet = new Char[] { '<', '>' };
            foreach (char c in invalidCharactersSet)
            {
                if (str.Contains(c))
                    return true;
            }
            return false;
        }

        private string? createImage(IFormFile? file)
        {
            if (file == null)
                return null;
            string fileName = DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss-fff") + Path.GetExtension(file.FileName);
            string wwwRootPath = _webHostEnv.WebRootPath;
            string productImagesPath = Path.Combine(wwwRootPath, @"images\products");
            using (var fileStream = new FileStream(Path.Combine(productImagesPath, fileName), FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return @"\images\products\" + fileName;
        }
    }
}
