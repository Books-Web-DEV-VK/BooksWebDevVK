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
    public class ShoppingCartRepository: Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext _db;
        public ShoppingCartRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
        public void Update(ShoppingCart shoppingCart)
        {
            var objFromDb = _db.ShoppingCarts.FirstOrDefault(s => s.Id == shoppingCart.Id);
            if (objFromDb != null)
            {
                objFromDb.Count = shoppingCart.Count;
                // You can update other properties if needed
            }
        }
    }
}
