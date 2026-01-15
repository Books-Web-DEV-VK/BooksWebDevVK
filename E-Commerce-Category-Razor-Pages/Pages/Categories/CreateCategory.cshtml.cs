using BooksWeb_Category_Razor_Pages.Data;
using BooksWeb_Category_Razor_Pages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BooksWeb_Category_Razor_Pages.Pages.Categories
{
    public class CreateCategoryModel : PageModel
    {
        private readonly ApplicationDbContext _appDbContext;
        [BindProperty]
        public Category category { get; set; }

        public CreateCategoryModel(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            _appDbContext.Categories.Add(category);
            _appDbContext.SaveChanges();
            return RedirectToPage("/Categories/Index");
        }
    }
}
