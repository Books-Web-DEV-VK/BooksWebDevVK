
namespace BooksWeb.Models
{
    public class PaymentRequestModel
    {
        /* 
         *** For Fake Payment Gateway ***
          public Guid orderId { get; set; }
          public double amount { get; set; }
          public string callBackUrl { get; set; }
          public string signature { get; set; }
        */

        /**** For Razor Pay Gateway ***/
        public string OrderReceiptId { get; set; }
        public double Amount { get; set; }
        public string KeyId { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public Guid OrderId { get; set; }
    }
}