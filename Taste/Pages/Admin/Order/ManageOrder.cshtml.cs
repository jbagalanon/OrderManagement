using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models;
using Taste.Models.ViewModels;
using Taste.Utility;

namespace Taste.Pages.Admin.Order
{
    public class ManageOrderModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public ManageOrderModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public List<OrderDetailsViewModel> orderDetailsVM { get; set; }


        public void OnGet()
        {
            orderDetailsVM = new List<OrderDetailsViewModel>();

            List<OrderHeader> OrderHeaderList = _unitOfWork.OrderHeader.GetAll
                    (o => o.Status == SD.StatusSubmitted || o.Status == SD.StatusInProcess)
                .OrderByDescending(u => u.PickupName).ToList();

            foreach (OrderHeader item in OrderHeaderList)
            {
                OrderDetailsViewModel individual = new OrderDetailsViewModel
                {
                    OrderHeader = item,
                    OrderDetails = _unitOfWork.OrderDetails.GetAll(o => o.OrderId == item.Id).ToList()
                };

                orderDetailsVM.Add(individual);
            }

        }

        public IActionResult OnPostPrepare(int orderId)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == orderId);

            orderHeader.Status = SD.StatusInProcess;
            _unitOfWork.Save();

            return RedirectToPage("ManageOrder");
        }

        public IActionResult OnPostOrderReady(int orderId)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == orderId);

            orderHeader.Status = SD.StatusReady;
            _unitOfWork.Save();

            return RedirectToPage("ManageOrder");
        }

        public IActionResult OnPostOrderCancel(int orderId)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == orderId);

            orderHeader.Status = SD.StatusCancelled;
            _unitOfWork.Save();

            return RedirectToPage("ManageOrder");
        }

        public IActionResult OnPostOrderRefund(int orderId)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == orderId);

            //refund the amount
            //go to stripe and find the documentation in refund
            //copy the post call in refunds documentation
            // this is the code of refund
            var options = new RefundCreateOptions
            {
                Amount = Convert.ToInt32(orderHeader.OrderTotal*100),
                Reason = RefundReasons.RequestedByCustomer,
                //something wrong with charge. I think that is ChargeId
                Charge= orderHeader.TransactionId
            };
            var service = new RefundService();

            //Refund refund = service.Create("myAPI coedes", refundOptions);
            Refund refund = service.Create(options);


            orderHeader.Status = SD.StatusRefunded;
            _unitOfWork.Save();

            return RedirectToPage("ManageOrder");
        }
    }

}
