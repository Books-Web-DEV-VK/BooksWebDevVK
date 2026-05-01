using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooksWeb.DataAccess.Repository;

namespace BooksWeb.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository _categoryRepo { get; }
        IProductRepository _productRepo { get; }
        ICompanyRepository _companyRepo { get; }
        IShoppingCartRepository _shoppingCartRepo { get; }
        IApplicationUserRepository _applicationUserRepo { get; }
        IOrderHeaderRepository _orderHeaderRepo { get; }
        IOrderDetailsRepository _orderDetailsRepo { get; }

        void Save();
    }
}
