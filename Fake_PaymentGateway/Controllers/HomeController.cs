using Fake_PaymentGateway.Helpers;
using Fake_PaymentGateway.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Fake_PaymentGateway.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;

        public HomeController(IConfiguration config , ILogger<HomeController> logger)
        {
            _config = config;
            _logger = logger;
        }

        public static List<TestCard> TestCards = new List<TestCard>
        {
            new TestCard
            {
                CardNumber = "4111111111111111",
                CardHolderName = "JOHN DOE",
                Expiry = "12/28",
                CVV = "123",
                Brand = "Visa",
                Result = "Success"
            },
            new TestCard
            {
                CardNumber = "5555555555554444",
                CardHolderName = "JANE SMITH",
                Expiry = "11/27",
                CVV = "456",
                Brand = "Mastercard",
                Result = "Success"
            },
            new TestCard
            {
                CardNumber = "378282246310005",
                CardHolderName = "ROBERT BROWN",
                Expiry = "10/29",
                CVV = "1234",
                Brand = "Amex",
                Result = "Success"
            },
            new TestCard
            {
                CardNumber = "4000000000000002",
                CardHolderName = "DECLINE USER",
                Expiry = "09/26",
                CVV = "321",
                Brand = "Visa",
                Result = "Declined"
            },
            new TestCard
            {
                CardNumber = "4000000000009995",
                CardHolderName = "LOW BALANCE",
                Expiry = "08/26",
                CVV = "999",
                Brand = "Visa",
                Result = "InsufficientFunds"
            }
        };


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Pay(PaymentRequestModel payRequest) 
        {
            return View("PaymentRequest", payRequest);
        }

        [HttpPost]
        public IActionResult ProcessPayment(PaymentRequestModel payRequest, string cardHolder, string cardNumber, string cardExpiry, string cardCvv) 
        {
            var fetchedRawData = $"{payRequest.orderId}|{payRequest.amount}";
            var signature = SecurityHelper.GenerateSignature(fetchedRawData, _config["SecretKey"]);
            var errorMsg = string.Empty;
            var successMsg = string.Empty;
            bool isSuccess = false;
            var msg = string.Empty;
            var taxationId = string.Empty;
            if (signature != payRequest.signature)
            {

                errorMsg = "Request Tampered. Please try again.";
            }
            else
            {
                TestCard? card = TestCards.FirstOrDefault(c => 
                    c.CardNumber == cardNumber.Trim() && 
                    c.CardHolderName.Equals(cardHolder?.Trim(), StringComparison.OrdinalIgnoreCase) && 
                    c.Expiry == cardExpiry.Trim() && 
                    c.CVV == cardCvv.Trim());

                if (card != null) {
                    if (card.Result == "Declined")
                    {
                        errorMsg = "Card Declined. Please use another card.";
                    }
                    else if (card.Result == "InsufficientFunds")
                    {
                        errorMsg = "Insufficient Funds. Please use another card.";
                    }
                    else if (card.Result == "Success")  // Explicitly check for success
                    {
                        taxationId = Guid.NewGuid().ToString();
                        successMsg = "Payment Processed Successfully.";
                    }
                }
                else
                {
                    errorMsg = "Invalid Card Details. Please check and try again.";
                }

                isSuccess = errorMsg == string.Empty;
                msg = isSuccess ? successMsg : errorMsg;
            }
            var sendingRawData = $"{isSuccess}|{msg}|{payRequest.orderId}|{taxationId}";
            signature = SecurityHelper.GenerateSignature(sendingRawData, _config["SecretKey"]);
            var targetUrl = $"{payRequest.callBackUrl}?success={isSuccess}&msg={msg}&orderId={payRequest.orderId}&taxationId={taxationId}&signature={signature}";
            return Redirect(targetUrl);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
