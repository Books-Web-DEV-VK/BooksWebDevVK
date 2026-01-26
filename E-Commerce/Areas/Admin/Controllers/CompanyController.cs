using BooksWeb.DataAccess.Repository.IRepository;
using BooksWeb.Models;
using BooksWeb.Models.ViewModels;
using BooksWeb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BooksWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Company> companiesList = _unitOfWork._companyRepo.GetAll().ToList();
            return View(companiesList);
        }

        public IActionResult CreateCompany()
        {
            return View();
        }

        [HttpGet]
        [Route("Admin/Company/{action}/{actiontype}/{id?}")]  // Here action is like keyword for MVC to identify the method to be called
        public IActionResult Details(string actiontype, Guid? id)
        {
            List<Company> companyList = _unitOfWork._companyRepo.GetAll().ToList();
            actiontype = actiontype.ToLower();
            if (actiontype == "create")
            {
                ViewBag.ActionType = "create";
                return View("CompanyDetails", new Product());
            }
            if (actiontype == "view" || actiontype == "edit")
            {
                if (id == null)
                {
                    TempData["error"] = "Invalid Url Accessed, Returning to List Page!!";
                    return RedirectToAction("Index");
                }
                Company? company = _unitOfWork._companyRepo.Get(u => u.Id == id);
                ViewBag.ActionType = actiontype;
                if (company == null)
                {
                    TempData["error"] = "Company not found !!";
                    return RedirectToAction("Index");
                }
                return View("CompanyDetails", company);
            }
            else
            {
                TempData["error"] = "Invalid Url Accessed, Returning to List Page!!";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult ManageCompanies(Company company, string actionType, IFormFile? file)
        {
            try
            {
                List<Category> categoryList = _unitOfWork._categoryRepo.GetAll().ToList();
                checkAndSetModelErrors(company);
                if (company.PostalCode.Length > 6)
                {
                    ModelState.AddModelError("", "Company Postal Code should not have more than 5 characters."); // Model level error 
                }
                if (ModelState.IsValid)
                {
                    switch (actionType.ToLower())
                    {
                        case "create":
                            _unitOfWork._companyRepo.Add(company);
                            _unitOfWork.Save();
                            TempData["success"] = "Successfully created the Company !!";
                            break;
                        case "edit":
                            _unitOfWork._companyRepo.Update(company);
                            _unitOfWork.Save();
                            TempData["success"] = "Successfully updated the Company !!";
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
                    return View("CompanyDetails", company);
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
            var removableCompany = _unitOfWork._companyRepo.Get(c => c.Id == id);
            if (removableCompany == null)
                return NotFound("Company Not Found !!");
            else
            {
                _unitOfWork._companyRepo.Remove(removableCompany);
                _unitOfWork.Save();
                return Ok("Deleted Company Successfully !!");
            }
        }

        private void checkAndSetModelErrors(Company Company)
        {
            var properties = Company.GetType().GetProperties();

            for (int i = 0; i < properties.Length; i++)
            {
                var value = properties[i].GetValue(Company);
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
    }
}
