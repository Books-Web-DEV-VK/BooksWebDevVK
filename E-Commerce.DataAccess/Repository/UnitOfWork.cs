using BooksWeb.DataAccess.Repository.IRepository;
using BooksWeb.Migrations.Data;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksWeb.DataAccess.Repository
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly ApplicationDbContext _appDbCxt;
        public ICategoryRepository _categoryRepo { get; private set; }
        public IProductRepository _productRepo { get; private set; }
        public ICompanyRepository _companyRepo { get; private set; }

        public UnitOfWork(ApplicationDbContext appDbCxt) { 
            _appDbCxt = appDbCxt;
            _categoryRepo = new CategoryRepository(_appDbCxt);
            _productRepo = new ProductRepository(_appDbCxt);
            _companyRepo = new CompanyRepository(_appDbCxt);
        }

        public void Save()
        {
            _appDbCxt.SaveChanges();
        }
    }
}
