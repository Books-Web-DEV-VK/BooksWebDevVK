using BooksWeb.DataAccess.Repository.IRepository;
using BooksWeb.Migrations.Data;
using BooksWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksWeb.DataAccess.Repository
{
    public class ProductRepository: Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _appDbCxt;
        public ProductRepository(ApplicationDbContext appDbCxt): base(appDbCxt)
        {
            _appDbCxt = appDbCxt;
        }

        public void Update(Product updatableProduct)
        {
            _appDbCxt.Update(updatableProduct);
        }
    }
}
