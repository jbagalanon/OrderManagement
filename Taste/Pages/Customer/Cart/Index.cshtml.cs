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
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public OrderDetailsCart OrderDetailsCartVM { get; set; }
        public void OnGet()
        {
            OrderDetailsCartVM = new OrderDetailsCart()
            {
                OrderHeader = new Models.OrderHeader()
            };

            //initialialized order details

            OrderDetailsCartVM.OrderHeader.OrderTotal = 0;

            //retrieving shopping card in database
            var claimsIdentity = (ClaimsIdentity) User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            

            //retrieve data from database
            IEnumerable<ShoppingCart> cart = _unitOfWork.ShoppingCart.GetAll(s => s.ApplicationUserId == claim.Value);

            //cart database condition retrieval
            if (cart != null)
            {
                OrderDetailsCartVM.listCart = cart.ToList();
            }

            foreach (var cartList in OrderDetailsCartVM.listCart)
            {
                cartList.MenuItem = _unitOfWork.MenuItem.GetFirstOrDefault(m => m.Id == cartList.MenuItemId);
                //get total multiply to item price to item count

                OrderDetailsCartVM.OrderHeader.OrderTotal += cartList.MenuItem.Price;
            }
        }
    }
}
