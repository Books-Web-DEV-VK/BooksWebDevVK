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
        void Save();
    }
}
