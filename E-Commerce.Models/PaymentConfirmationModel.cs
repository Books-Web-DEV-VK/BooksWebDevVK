using Microsoft.AspNetCore.Routing.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksWeb.Models
{
    public class OrderConfirmationModel
    {
        public string? OrderId { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? PaymentId { get; set; }
        public double? Amount { get; set; }
    }
}
