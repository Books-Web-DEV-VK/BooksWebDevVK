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
    public class ProductRepository: Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _dbCxt;
        public ProductRepository(ApplicationDbContext dbCxt): base(dbCxt)
        {
            _dbCxt = dbCxt;
        }

        public void Update(Product updatableProduct)
        {
            if (updatableProduct != null)
            {
                var existingProduct = _dbCxt.Products.FirstOrDefault(p => p.Id == updatableProduct.Id);
                if (existingProduct != null)
                {
                    existingProduct.Title = updatableProduct.Title;
                    existingProduct.Description = updatableProduct.Description;
                    existingProduct.ISBN = updatableProduct.ISBN;
                    existingProduct.Author = updatableProduct.Author;
                    existingProduct.ListPrice = updatableProduct.ListPrice;
                    existingProduct.Price = updatableProduct.Price;
                    existingProduct.Price50 = updatableProduct.Price50;
                    existingProduct.Price100 = updatableProduct.Price100;
                    existingProduct.CategoryId = updatableProduct.CategoryId;
                    if (updatableProduct.ImageUrl != null)
                    {
                        existingProduct.ImageUrl = updatableProduct.ImageUrl;
                    }
                }
            }
        }
    }
}
