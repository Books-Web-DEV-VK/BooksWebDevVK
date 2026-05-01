namespace Fake_PaymentGateway.Models
{
    public class PaymentRequestModel
    {
        public Guid orderId { get; set; }
        public double amount { get; set; }
        public string callBackUrl { get; set; }
        public string signature { get; set; }
    }
}
