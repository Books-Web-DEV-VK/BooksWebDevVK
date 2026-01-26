using BooksWeb.DataAccess.Repository.IRepository;
using BooksWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BooksWeb.Utility;

namespace BooksWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork  unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Category> categories = _unitOfWork._categoryRepo.GetAll().ToList();
            return View(categories);
        }

        public IActionResult CreateCategory()
        {
            return View(); // nothing is passed to the view CreateCategory.cshtml - empty Category model eill be created and sebt to view
        }

        [HttpPost]
        public IActionResult CreateCategory(Category newCategory)
        {
            try
            {
                if (!String.IsNullOrEmpty(newCategory.Name) && hasInvalidCharacters(newCategory.Name))
                {
                    ModelState.AddModelError("Name", "Cannot contain invalid characters");  // Field level error - for property "Name"
                }
                if (newCategory.Name == newCategory.DisplayOrder.ToString())
                {
                    ModelState.AddModelError("", "The Name and Display order Value should not be same."); // Model level error 
                }
                if (ModelState.IsValid)
                {
                    _unitOfWork._categoryRepo.Add(newCategory);
                    _unitOfWork.Save();
                    TempData["success"] = "Successfully created the category !!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("CreateCategory", newCategory);
                }
            } catch(Exception ex)
            {
                TempData["error"] = "Error occured while creating the category !! Try again after sometime.";
                return RedirectToAction("Index");
            }            
        }

        [HttpGet]
        public IActionResult ViewCategory(Guid id, string actionType="get")
        {
            Category? category = _unitOfWork._categoryRepo.Get(u=> u.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            if(actionType.ToLower() == "get")
                return View("ViewCategory",category);
            else if(actionType.ToLower() == "edit")
                return View("EditCategory",category);
            else if (actionType.ToLower() == "delete")
                return View("DeleteConfirmation", category);
            else
                return View("Error");
        }

        [HttpPost]
        public IActionResult UpdateCategory(Category updatedCategory)
        {
            try
            {
                if (!String.IsNullOrEmpty(updatedCategory.Name) && hasInvalidCharacters(updatedCategory.Name))
                {
                    ModelState.AddModelError("Name", "Cannot contain invalid characters");  // Field level error - for property "Name"
                }
                if (updatedCategory.Name == updatedCategory.DisplayOrder.ToString())
                {
                    ModelState.AddModelError("", "The Name and Display order Value should not be same."); // Model level error 
                }
                if (ModelState.IsValid)
                {
                    TempData["success"] = "Successfully updated the category !!";
                    _unitOfWork._categoryRepo.Update(updatedCategory);
                    _unitOfWork.Save();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("EditCategory", updatedCategory);
                }
            }
            catch(Exception ex)
            {
                TempData["error"] = "Error occured while updating the category !! Try again after sometime.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult DeleteCategory(Category removingCategory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["success"] = "Successfully deleted the Category !!";
                    _unitOfWork._categoryRepo.Remove(removingCategory);
                    _unitOfWork.Save();
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorr"] = "Error occured while deleting the category !! Try again after sometime.";
                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                TempData["errorr"] = "Error occured while deleting the category !! Try again after sometime.";
                return RedirectToAction("Index");
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
