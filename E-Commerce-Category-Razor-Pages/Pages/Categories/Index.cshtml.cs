using BooksWeb_Category_Razor_Pages.Data;
using BooksWeb_Category_Razor_Pages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BooksWeb_Category_Razor_Pages.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _appDbContext;
        public List<Category> categoriesList { get; set; }
        public IndexModel(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void OnGet()
        {
            categoriesList = _appDbContext.Categories.ToList();
        }
    }
}
