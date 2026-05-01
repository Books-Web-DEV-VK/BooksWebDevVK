using BooksWeb.DataAccess.Repository;
using BooksWeb.DataAccess.Repository.IRepository;
using BooksWeb.Models;
using Product = BooksWeb.Models.Product;
using BooksWeb.Models.ViewModels;
using BooksWeb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Razorpay.Api;
using System.Security.Claims;
using BooksWeb.Utility.EmailSender;

namespace BooksWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SecurityConstants _securityConstants;
        private readonly IEmailSender _emailSender;

        public ShoppingCartController(IUnitOfWork unitOfWork, IOptions<SecurityConstants> securityConstants, IEmailSender emailSender) 
        {
            _unitOfWork = unitOfWork;
            _securityConstants = securityConstants.Value;
            _emailSender = emailSender;
        }

        [Authorize(Roles = $"{SD.Role_Company},{SD.Role_Customer}")]
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var itemsList = _unitOfWork._shoppingCartRepo.GetAll(sc => sc.ApplicationUserId == userId, "Product");
            double orderTotal = 0;
            foreach (var item in itemsList)
            {
                item.Price = GetQuantityBasedPrice(item.Count, item.Product);
                orderTotal += item.Count * item.Price;
            }
            return View(new ShoppingCartVM
            {
                ItemsList = itemsList,
                OrderHeader = new()
                {
                    OrderTotal = orderTotal
                }
            });
        }

        [Authorize(Roles = $"{SD.Role_Company},{SD.Role_Customer}")]
        public IActionResult Plus(Guid cartId)
        {
            var cartObj = _unitOfWork._shoppingCartRepo.Get(c => c.Id == cartId, null, true);
            cartObj.Count++;
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = $"{SD.Role_Company},{SD.Role_Customer}")]
        public IActionResult Minus(Guid cartId)
        {
            var cartObj = _unitOfWork._shoppingCartRepo.Get(c => c.Id == cartId, null, true);
            cartObj.Count--;
            if (cartObj.Count == 0)
            {
                _unitOfWork._shoppingCartRepo.Remove(cartObj);
                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork._shoppingCartRepo.GetAll(c => c.ApplicationUserId == cartObj.ApplicationUserId).Count() - 1); // Doing -1 as current poduct is going to be removed from the cart
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = $"{SD.Role_Company},{SD.Role_Customer}")]
        public IActionResult Remove(Guid cartId)
        {
            var cartObj = _unitOfWork._shoppingCartRepo.Get(c => c.Id == cartId);
            _unitOfWork._shoppingCartRepo.Remove(cartObj);
            _unitOfWork.Save();
            HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork._shoppingCartRepo.GetAll(c => c.ApplicationUserId == cartObj.ApplicationUserId).Count()); // Updating the session cart count after removing the product from cart
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = $"{SD.Role_Company},{SD.Role_Customer}")]
        public double GetQuantityBasedPrice(int quantity, Product productPriceDetails)
        {
            double price = 0;
            if (quantity <= 50)
            {
                price = productPriceDetails.Price;
            }
            else if (quantity <= 100)
            {
                price = productPriceDetails.Price50;
            }
            else
            {
                price = productPriceDetails.Price100;
            }
            return price;
        }

        [Authorize(Roles = $"{SD.Role_Company},{SD.Role_Customer}")]
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            double orderTotal = 0;
            var itemsList = _unitOfWork._shoppingCartRepo.GetAll(s => s.ApplicationUserId == userId, nameof(Product));
            foreach (var item in itemsList)
            {
                item.Price = GetQuantityBasedPrice(item.Count, item.Product);
                orderTotal += item.Count * item.Price;
            }
            var userDetails = _unitOfWork._applicationUserRepo.Get(u => u.Id == userId);
            return View(new ShoppingCartVM()
            {
                OrderHeader = new OrderHeader
                {
                    Name = userDetails.UserName,
                    PhoneNumber = userDetails.PhoneNumber,
                    StreetAddress = userDetails.StreetAddress,
                    City = userDetails.City,
                    State = userDetails.State,
                    PostalCode = userDetails.PostalCode,
                    OrderTotal = orderTotal
                },
                ItemsList = itemsList
            });
        }

        [HttpPost]
        [Authorize(Roles = $"{SD.Role_Company},{SD.Role_Customer}")]
        public IActionResult PlaceOrder(ShoppingCartVM shoppingCartVm)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            double orderTotal = 0;

            shoppingCartVm.OrderHeader.OrderDate = DateTime.Now;
            shoppingCartVm.OrderHeader.ApplicationUserId = userId;
            var user = _unitOfWork._applicationUserRepo.Get(u => u.Id == userId);
            if (user.CompanyId.GetValueOrDefault() == Guid.Empty) // Customer is ordering not company admins
            {
                shoppingCartVm.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                shoppingCartVm.OrderHeader.OrderStatus = SD.StatusPending;
            }
            else
            {
                shoppingCartVm.OrderHeader.PaymentDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30));
                shoppingCartVm.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment; 
                shoppingCartVm.OrderHeader.OrderStatus = SD.StatusApproved;
            }
            _unitOfWork._orderHeaderRepo.Add(shoppingCartVm.OrderHeader); // Adding Order Header into the Record
            _unitOfWork.Save();

            var itemsList = _unitOfWork._shoppingCartRepo.GetAll(s => s.ApplicationUserId == userId, nameof(Product));
            foreach (var item in itemsList)
            {
                item.Price = GetQuantityBasedPrice(item.Count, item.Product);
                orderTotal += item.Count * item.Price;
                OrderDetails orderDetails = new()
                {
                    ProductId = item.ProductId,
                    OrderHeaderId = shoppingCartVm.OrderHeader.Id,
                    Count = item.Count,
                    Price = item.Price
                };
                _unitOfWork._orderDetailsRepo.Add(orderDetails); // Addng Order Details into the Record
            }
            shoppingCartVm.OrderHeader.OrderTotal = orderTotal;
            _unitOfWork._orderHeaderRepo.Update(shoppingCartVm.OrderHeader); // Updating Order Header into the Record
            _unitOfWork.Save(); // Saving all the Order Header Updations and Order Details into the Record

            if (User.IsInRole(SD.Role_Company)) // Companies are having 30 days payment waiver so directly showing the order confirmation page with delayed payment message
            {
                _unitOfWork._shoppingCartRepo.RemoveRange(_unitOfWork._shoppingCartRepo.GetAll(c => c.ApplicationUserId == userId)); // Removing the existing products from the shopping cart
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart, 0); // As order is placed - removing everything form the cart
                return View("OrderConfirmation", new OrderConfirmationModel
                {
                    OrderId = shoppingCartVm.OrderHeader.Id.ToString(),
                    OrderStatus = shoppingCartVm.OrderHeader.OrderStatus,
                    PaymentStatus = shoppingCartVm.OrderHeader.PaymentStatus
                });
            }
            // Create Razor Pay - receipt id and add that in transaction Number and pass details to view
            var keyId  = _securityConstants.RazorPayGateway.KeyId;
            var keySecret = _securityConstants.RazorPayGateway.KeySecret;
            try
            {
                RazorpayClient razorpayClient = new(keyId, keySecret);
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", orderTotal * 100); // Razorpay works with paise so multiplying by 100
                options.Add("currency", "INR");
                options.Add("receipt", shoppingCartVm.OrderHeader.Id.ToString());
                Razorpay.Api.Order order = razorpayClient.Order.Create(options); // makes the API call to Razorpay to create order and get the order details in response
                return View("ProcessPayment", new PaymentRequestModel
                {
                    OrderReceiptId = order["id"],
                    Amount = orderTotal,
                    KeyId = keyId,
                    UserEmail = user.Email,
                    UserName = user.UserName,
                    OrderId = shoppingCartVm.OrderHeader.Id
                });
            }
            catch (Exception ex)
            {
                return View("Error");
            }
            /*
             *** FAKE PAYMENT GATEWAY INTEGRATION - STARTS HERE - FOR DEMO PURPOSES ONLY - NOT PRODUCTION READY CODE - DO NOT USE IN PRODUCTION ENVIRONMENT WITHOUT PROPER SECURITY MEASURES AND ENCRYPTION IN PLACE
            string fakePaymentBaseUrl = _securityConstants.FakePaymentBaseUrl;
            var callBackUrl = "https://localhost:7076/Customer/ShoppingCart/PaymentStatus";
            return View("ProcessPayment", new PaymentRequestModel
            {
                orderId = shoppingCartVm.OrderHeader.Id,
                amount = orderTotal,
                callBackUrl = callBackUrl,
                signature = GetSecureSignature(shoppingCartVm.OrderHeader.Id, orderTotal)
            });
            */
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Company)]
        public IActionResult SettleDelayedPayment(OrderVM orderVM)
        {
            OrderHeader orderHeader = _unitOfWork._orderHeaderRepo.Get(oh => oh.Id == orderVM.OrderHeader.Id);
            RazorpayClient razorpayClient = new RazorpayClient(_securityConstants.RazorPayGateway.KeyId, _securityConstants.RazorPayGateway.KeySecret);

            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", (orderHeader.OrderTotal * 100));
            options.Add("currency", "INR");
            options.Add("receipt", orderHeader.Id.ToString());

            Razorpay.Api.Order razorPayOrder = razorpayClient.Order.Create(options);

            var user = _unitOfWork._applicationUserRepo.Get(u => u.Id == orderHeader.ApplicationUserId);
            return View("ProcessPayment", new PaymentRequestModel
            {
                OrderReceiptId = razorPayOrder["id"],
                Amount = orderHeader.OrderTotal,
                KeyId = _securityConstants.RazorPayGateway.KeyId,
                UserEmail = user.Email,
                UserName = user.UserName,
                OrderId = orderHeader.Id
            });
        }

        [HttpPost]
        [Authorize(Roles = $"{SD.Role_Company},{SD.Role_Customer}")]
        public IActionResult VerifyPaymentStatus(string razorpayPaymentIntentId, string razorpayOrderId, string razorpaySignature, string orderHeaderId)
        {
            try
            {
                RazorpayClient razorpayClient = new(_securityConstants.RazorPayGateway.KeyId, _securityConstants.RazorPayGateway.KeySecret);
                Dictionary<string, string> options = new Dictionary<string, string>();
                options.Add("razorpay_payment_id", razorpayPaymentIntentId);
                options.Add("razorpay_order_id", razorpayOrderId);
                options.Add("razorpay_signature", razorpaySignature);

                Utils.verifyPaymentSignature(options); // Verifies the signature sent by Razorpay to ensure that the payment details are not tampered with

                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                var orderHeader = _unitOfWork._orderHeaderRepo.Get(o => o.Id.ToString() == orderHeaderId, includeProperties: "ApplicationUser");
                orderHeader.PaymentStatus = SD.PaymentStatusApproved;
                orderHeader.PaymentDate = DateTime.Now;
                orderHeader.PaymentIntentId = razorpayPaymentIntentId;
                if(User.IsInRole(SD.Role_Customer))
                {
                    orderHeader.OrderStatus = SD.StatusApproved;
                    _unitOfWork._shoppingCartRepo.RemoveRange(_unitOfWork._shoppingCartRepo.GetAll(c => c.ApplicationUserId == userId));
                    HttpContext.Session.SetInt32(SD.SessionCart, 0);
                }
                if(User.IsInRole(SD.Role_Company))
                {
                    orderHeader.OrderStatus = SD.StatusShipped;
                }
                _unitOfWork._orderHeaderRepo.Update(orderHeader);
                _unitOfWork.Save();

                // Send email notification
                _emailSender.SendEmailAsync(orderHeader.ApplicationUser.Email , "Books Web Order Confirmation", $"Your order with ID {orderHeader.Id} has been successfully processed.");

                return View("OrderConfirmation", new OrderConfirmationModel
                {
                    OrderId = orderHeaderId,
                    OrderStatus = orderHeader.OrderStatus,
                    PaymentStatus = orderHeader.PaymentStatus,
                    PaymentId = razorpayPaymentIntentId
                });
            }
            catch(Exception ex)
            {
                // If any other exception occurs, update the order status to Rejected and show the message to user
                var orderHeader = _unitOfWork._orderHeaderRepo.Get(o => o.Id.ToString() == orderHeaderId);
                if (User.IsInRole(SD.Role_Customer))
                {
                    orderHeader.PaymentStatus = SD.PaymentStatusPending;
                    orderHeader.OrderStatus = SD.StatusPending;
                }
                if (User.IsInRole(SD.Role_Company))
                {
                    orderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                    orderHeader.OrderStatus = SD.StatusShipped;
                }
                _unitOfWork._orderHeaderRepo.Update(orderHeader);
                _unitOfWork.Save();
                return View("OrderConfirmation", new OrderConfirmationModel
                {
                    OrderId = orderHeaderId,
                    OrderStatus = orderHeader.OrderStatus,
                    PaymentStatus = orderHeader.PaymentStatus
                });                                                                                                                                
            }
        }
        /* 
         *** FAKE PAYMENT GATEWAY INTEGRATION - STARTS HERE - FOR DEMO PURPOSES ONLY - NOT PRODUCTION READY CODE - DO NOT USE IN PRODUCTION ENVIRONMENT WITHOUT PROPER SECURITY MEASURES AND ENCRYPTION IN PLACE
        [HttpGet]
        public IActionResult PaymentStatus(bool success = false, string msg = "", string orderId = "", string taxationId = "", string signature = "")
        {
            var fetchedRawData = $"{success}|{msg}|{orderId}|{taxationId}";
            var expectedSignature = SecurityHelper.GenerateSignature(fetchedRawData, _securityConstants.FakePaymentGateway.SecretKey);
            if (expectedSignature != signature)
            {
                return View("OrderConfirmation", new OrderConfirmationModel
                {
                    OrderId = orderId,
                    PaymentStatus = SD.PaymentStatusPending,
                    Message = msg
                });
            }
            else if(!success)
            {
                return View("OrderConfirmation", new OrderConfirmationModel
                {
                    OrderId = orderId,
                    PaymentStatus = SD.PaymentStatusRejected,
                    Message = msg
                });
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                var orderHeader = _unitOfWork._orderHeaderRepo.Get(o => o.Id.ToString() == orderId);
                orderHeader.PaymentStatus = SD.PaymentStatusApproved;
                orderHeader.OrderStatus = SD.StatusApproved;
                orderHeader.PaymentDate = DateTime.Now;
                _unitOfWork._orderHeaderRepo.Update(orderHeader);

                _unitOfWork._shoppingCartRepo.RemoveRange(_unitOfWork._shoppingCartRepo.GetAll(c => c.ApplicationUserId == userId));
                _unitOfWork.Save();
                return View("OrderConfirmation", new OrderConfirmationModel
                {
                    OrderId = orderHeader.Id.ToString(),
                    TaxationId = taxationId,
                    PaymentStatus = orderHeader.PaymentStatus,
                    Message = msg
                });
            }
        }

        private string GetSecureSignature(Guid orderId, double amount)
        {
            string rawData = $"{orderId}|{amount}";
            string key = _securityConstants.FakePaymentGateway.SecretKey;
            return SecurityHelper.GenerateSignature(rawData, key);
        }
        */
    }
}
