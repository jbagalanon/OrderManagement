using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models;
using Taste.Models.ViewModels;

namespace Taste.Pages.Customer.Cart
{
    public class SummaryModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public SummaryModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public OrderDetailsCart detailCart { get; set; }
        public IActionResult OnGet()
        {
            detailCart = new OrderDetailsCart()
            {
                OrderHeader = new Models.OrderHeader()
            };

            //initialialized order details

            detailCart.OrderHeader.OrderTotal = 0;

            //retrieving shopping card in database
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            //retrieve data from database
            IEnumerable<ShoppingCart> cart = _unitOfWork.ShoppingCart.GetAll(s => s.ApplicationUserId == claim.Value);

            //cart database condition retrieval
            if (cart != null)
            {
                detailCart.listCart = cart.ToList();
            }

            foreach (var cartList in detailCart.listCart)
            {
                cartList.MenuItem = _unitOfWork.MenuItem.GetFirstOrDefault(m => m.Id == cartList.MenuItemId);
                //get total multiply to item price to item count

                detailCart.OrderHeader.OrderTotal += cartList.MenuItem.Price;
            }

            //retrieve info by userid
            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(c => c.Id == claim.Value);
            detailCart.OrderHeader.PickupName = applicationUser.FullName;
            detailCart.OrderHeader.PickUpTime = DateTime.Now;
            detailCart.OrderHeader.PhoneNumber = applicationUser.PhoneNumber;
            return Page(); 
        }

        public IActionResult OnPost(string stripeToken)
        {
            var claimsIdentity = (ClaimsIdentity) User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            detailCart.listCart = _unitOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == claim.Value).ToList();
        }
    }
}
