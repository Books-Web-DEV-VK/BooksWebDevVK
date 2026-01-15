using BooksWeb_Category_Razor_Pages.Data;
using BooksWeb_Category_Razor_Pages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BooksWeb_Category_Razor_Pages.Pages.Categories
{
    public class EditCategoryModel : PageModel
    {
        private readonly ApplicationDbContext _appDbContext;
        [BindProperty]
        public Category? category { get; set; }
        public Guid? Id { get; set; }
        public EditCategoryModel(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult OnGet(Guid? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            this.Id = Id;
            category = _appDbContext.Categories.FirstOrDefault<Category>(c => c.Id == Id);
            if (category == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost() { 
            if(category == null)
            {
                TempData["error"] = "Something wrong happened!! PLease try again !!";
                return RedirectToPage("/Categories/Index");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _appDbContext.Categories.Update(category);
            _appDbContext.SaveChanges();
            TempData["success"] = "Successfully edited the category !!";
            return RedirectToPage("/Categories/Index");
        }
    }
}
