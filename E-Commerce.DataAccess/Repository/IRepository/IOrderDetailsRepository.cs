using BooksWeb.Models;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksWeb.DataAccess.Repository.IRepository
{
    public interface IOrderDetailsRepository: IRepository<OrderDetails>
    {
        void Update(OrderDetails orderDetails);
    }
}
