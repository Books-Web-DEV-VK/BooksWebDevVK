using BooksWeb.DataAccess.Repository.IRepository;
using BooksWeb.Models;
using Product = BooksWeb.Models.Product;
using BooksWeb.Models.ViewModels;
using BooksWeb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using NuGet.Protocol.Resources;
using Org.BouncyCastle.Crypto.Paddings;
using Razorpay.Api;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq.Expressions;
using System.Security.Claims;

namespace BooksWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SecurityConstants _securityConstants;

        public OrderController(IUnitOfWork unitOfWork, IOptions<SecurityConstants> securityConstants)
        {
            _unitOfWork = unitOfWork;
            _securityConstants = securityConstants.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllInJSON(string orderStatus = "")
        {
            HashSet<string> statuses = new HashSet<string> { SD.StatusPending, SD.StatusApproved, SD.StatusInProgress, SD.StatusShipped, SD.StatusDelivered, SD.StatusCancelled };
            var statusFilter = statuses.Contains(orderStatus) ? orderStatus : string.Empty; // Validate the orderStatus parameter
            List<OrderHeader> orderHeaders;

            if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
                // Admin and Employee can see all orders
                Expression<Func<OrderHeader, bool>>? statusFilterExpression = string.IsNullOrEmpty(statusFilter) ? null : (o => o.OrderStatus == statusFilter);
                orderHeaders = _unitOfWork._orderHeaderRepo.GetAll(statusFilterExpression, nameof(ApplicationUser)).ToList();
            }
            else
            {
                // Customers can only see their own orders
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Expression<Func<OrderHeader, bool>> statusFilterExpression = string.IsNullOrEmpty(statusFilter) ? (o => o.ApplicationUserId == userId) : (o => o.ApplicationUserId == userId && o.OrderStatus == statusFilter);
                orderHeaders = _unitOfWork._orderHeaderRepo.GetAll(statusFilterExpression, nameof(ApplicationUser)).ToList();
            }
            return Json(new { data = orderHeaders });
        }

        [HttpGet]
        [Authorize]
        public IActionResult Details(Guid orderId)
        {
            OrderHeader orderHeader;
            List<OrderDetails> orderDetails;
            if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
                // Admin and Employee can see all orders
                orderHeader = _unitOfWork._orderHeaderRepo.Get(o => o.Id == orderId, nameof(ApplicationUser));
            }
            else
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                orderHeader = _unitOfWork._orderHeaderRepo.Get(o => o.Id == orderId && o.ApplicationUserId == userId, nameof(ApplicationUser));
            }
            orderDetails = orderHeader == null ? new List<OrderDetails>() : _unitOfWork._orderDetailsRepo.GetAll(od => od.OrderHeaderId == orderId, nameof(Product)).ToList();
            return View(new OrderVM
            {
                OrderHeader = orderHeader,
                OrderDetails = orderDetails
            });
        }

        [HttpPost]
        [Authorize(Roles = $"{SD.Role_Admin}, {SD.Role_Employee}")]
        public IActionResult UpdateOrderDetails(OrderVM orderVM)
        {
            OrderHeader orderHeader = _unitOfWork._orderHeaderRepo.Get(oh => oh.Id == orderVM.OrderHeader.Id, nameof(ApplicationUser), true);
            try
            {
                orderHeader.Name = orderVM.OrderHeader.Name;
                orderHeader.PhoneNumber = orderVM.OrderHeader.PhoneNumber;
                orderHeader.StreetAddress = orderVM.OrderHeader.StreetAddress;
                orderHeader.City = orderVM.OrderHeader.City;
                orderHeader.State = orderVM.OrderHeader.State;
                orderHeader.PostalCode = orderVM.OrderHeader.PostalCode;
                orderHeader.OrderDate = orderVM.OrderHeader.OrderDate;
                orderHeader.Carrier = orderVM.OrderHeader.Carrier;
                orderHeader.TrackingNumber = orderVM.OrderHeader.TrackingNumber;
                orderHeader.ShippedDate = orderVM.OrderHeader.ShippedDate;
                orderHeader.PaymentDate = orderVM.OrderHeader.PaymentDate;
                orderHeader.PaymentStatus = orderVM.OrderHeader.PaymentStatus;
                orderHeader.ApplicationUser.Email = orderVM.OrderHeader.ApplicationUser.Email;
                _unitOfWork.Save();
                TempData["success"] = "Order details updated successfully";
            }
            catch (Exception ex)
            {
                TempData["error"] = "Failed to Update. Try Again";
            }

            return RedirectToAction("Details", "Order", new { orderId = orderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = $"{SD.Role_Admin}, {SD.Role_Employee}")]
        public IActionResult StartProcessing(OrderVM orderVM)
        {
            OrderHeader orderHeader = _unitOfWork._orderHeaderRepo.Get(oh => oh.Id == orderVM.OrderHeader.Id, null, true);
            try
            {
                orderHeader.OrderStatus = SD.StatusInProgress; // as tracking = true no need of the Update
                _unitOfWork.Save();
                TempData["success"] = "Order status updated to In Progress";
            }
            catch (Exception ex)
            {
                TempData["error"] = "Failed to Update. Try Again";
            }
            return RedirectToAction("Details", "Order", new { orderId = orderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = $"{SD.Role_Admin}, {SD.Role_Employee}")]
        public IActionResult ShipOrder(OrderVM orderVM)
        {
            OrderHeader orderHeader = _unitOfWork._orderHeaderRepo.Get(oh => oh.Id == orderVM.OrderHeader.Id, null, true);
            try
            {
                orderHeader.OrderStatus = SD.StatusShipped;
                orderHeader.ShippedDate = DateTime.Now;
                orderHeader.Carrier = orderVM.OrderHeader.Carrier;
                orderHeader.TrackingNumber = orderVM.OrderHeader.TrackingNumber;
                orderHeader.PaymentDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30));
                _unitOfWork.Save();
                TempData["success"] = "Order status updated to Shipped";
            }
            catch (Exception ex)
            {
                TempData["error"] = "Failed to Update. Try Again";
            }
            return RedirectToAction("Details", "Order", new { orderId = orderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = $"{SD.Role_Admin}, {SD.Role_Employee}")]
        public IActionResult DeliverOrder(OrderVM orderVM)
        {
            OrderHeader orderHeader = _unitOfWork._orderHeaderRepo.Get(oh => oh.Id == orderVM.OrderHeader.Id, null, true);
            try
            {
                orderHeader.OrderStatus = SD.StatusDelivered;
                _unitOfWork.Save();
                TempData["success"] = "Order status updated to Delivered";
            }
            catch (Exception ex)
            {
                TempData["error"] = "Failed to Update. Try Again";
            }
            return RedirectToAction("Details", "Order", new { orderId = orderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = $"{SD.Role_Company},{SD.Role_Customer}")]
        public IActionResult CancelOrder(OrderVM orderVM)
        {
            OrderHeader orderHeader = _unitOfWork._orderHeaderRepo.Get(oh => oh.Id == orderVM.OrderHeader.Id, null, true);
            try
            {
                if(User.IsInRole(SD.Role_Customer) && orderHeader.PaymentStatus == SD.PaymentStatusApproved && !string.IsNullOrEmpty(orderHeader.PaymentIntentId))
                {
                    RazorpayClient client = new RazorpayClient(_securityConstants.RazorPayGateway.KeyId, _securityConstants.RazorPayGateway.KeySecret);
                    Dictionary<string, object> refundOptions = new Dictionary<string, object>
                    {
                        { "amount", orderHeader.OrderTotal * 100 }, // Amount in paise
                        { "speed", "normal" },
                        { "receipt", orderHeader.Id.ToString() }
                    };
                    Refund refund = client.Payment.Fetch(orderHeader.PaymentIntentId).Refund(refundOptions);
                    orderHeader.RefundId = refund["id"].ToString();
                    orderHeader.PaymentStatus = SD.PaymentStatusRefunded;
                    TempData["success"] = "Order status updated to Cancelled! Your amount will be refunded soon.";
                }
                if(User.IsInRole(SD.Role_Company))
                {
                    orderHeader.PaymentStatus = SD.PaymentStatusRejected;
                    TempData["success"] = "Order status updated to Cancelled! You order is cancelled successfully, and no need to pay any amount for this order";
                }
                orderHeader.OrderStatus = SD.StatusCancelled;
                _unitOfWork.Save();
                
            }
            catch (Exception ex)
            {
                TempData["error"] = "Failed to Update. Try Again";
            }
            return RedirectToAction("Details", "Order", new { orderId = orderHeader.Id });
        }
    }
}