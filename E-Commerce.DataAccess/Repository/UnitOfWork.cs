using BooksWeb.DataAccess.Repository.IRepository;
using BooksWeb.Migrations.Data;
using Microsoft.AspNetCore.Identity;
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
        public IShoppingCartRepository _shoppingCartRepo { get; private set; }
        public IApplicationUserRepository _applicationUserRepo { get; private set; }
        public IOrderHeaderRepository _orderHeaderRepo { get; private set; }
        public IOrderDetailsRepository _orderDetailsRepo { get; private set; }
        public IApplicationUserRepository _applicationUserRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext appDbCxt) { 
            _appDbCxt = appDbCxt;
            _categoryRepo = new CategoryRepository(_appDbCxt);
            _productRepo = new ProductRepository(_appDbCxt);
            _companyRepo = new CompanyRepository(_appDbCxt);
            _shoppingCartRepo = new ShoppingCartRepository(_appDbCxt);
            _applicationUserRepo = new ApplicationUserRepository(_appDbCxt);
            _orderHeaderRepo = new OrderHeaderRepository(_appDbCxt);
            _orderDetailsRepo = new OrderDetailsRepository(_appDbCxt);
            _applicationUserRepository = new ApplicationUserRepository(_appDbCxt);
        }

        public void Save()
        {
            _appDbCxt.SaveChanges();
        }
    }
}
