namespace Fake_PaymentGateway.Models
{
    public class TestCard
    {
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string Expiry { get; set; }   // MM/YY format
        public string CVV { get; set; }
        public string Brand { get; set; }
        public string Result { get; set; }
    }

}
