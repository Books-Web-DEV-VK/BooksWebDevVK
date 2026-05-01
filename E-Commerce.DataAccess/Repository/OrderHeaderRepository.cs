using BooksWeb.DataAccess.Repository.IRepository;
using BooksWeb.Migrations.Data;
using BooksWeb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksWeb.DataAccess.Repository
{
    public class OrderHeaderRepository: Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _appDbCxt;
        public OrderHeaderRepository(ApplicationDbContext appDbCxt) : base (appDbCxt)
        {
            _appDbCxt = appDbCxt;
        }

        public void Update(OrderHeader orderHeader)
        {
            _appDbCxt.Update(orderHeader);
        }
    }
}
