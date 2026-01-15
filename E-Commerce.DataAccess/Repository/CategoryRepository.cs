using BooksWeb.DataAccess.Repository.IRepository;
using BooksWeb.Migrations.Data;
using BooksWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BooksWeb.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _appDb;

        public CategoryRepository(ApplicationDbContext appDb) : base(appDb)
        {
            _appDb = appDb;
        }

        public void Update(Category updatableCategory)
        {
            _appDb.Update(updatableCategory);
        }
    }
}
