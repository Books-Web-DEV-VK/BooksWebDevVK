using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksWeb.Utility
{
    public class FakePaymentGateway
    {
        public string BaseUrl { get; set; }
        public string SecretKey { get; set; }
    }

    public class RazorPayGateway
    {
        public string KeyId { get; set; }
        public string KeySecret { get; set; }
    }

    public class SecurityConstants
    {
        public FakePaymentGateway FakePaymentGateway { get; set; }
        public RazorPayGateway RazorPayGateway { get; set; }
    }
}
