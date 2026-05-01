using BooksWeb.DataAccess.Repository.IRepository;
using BooksWeb.Migrations.Data;
using BooksWeb.Models;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksWeb.DataAccess.Repository
{
    public class OrderDetailsRepository: Repository<OrderDetails>, IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _appDbCxt;
        public OrderDetailsRepository(ApplicationDbContext appDbCxt) : base (appDbCxt)
        {
            _appDbCxt = appDbCxt;
        }

        public void Update(OrderDetails orderDetails)
        {
            _appDbCxt.Update(orderDetails);
        }
    }
}
