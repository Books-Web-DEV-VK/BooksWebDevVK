using BooksWeb.DataAccess.Repository.IRepository;
using BooksWeb.Migrations.Data;
using BooksWeb.Models;
using System.Linq.Expressions;

namespace BooksWeb.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _dbCxt;

        public CategoryRepository(ApplicationDbContext dbCxt) : base(dbCxt)
        {
            _dbCxt = dbCxt;
        }

        public void Update(Category updatableCategory)
        {
            _dbCxt.Update(updatableCategory);
        }
    }
}
