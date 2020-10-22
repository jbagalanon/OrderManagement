using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models;
using Taste.Models.ViewModels;
using Taste.Utility;

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
                OrderHeader = new Models.OrderHeader(),
                //if claim is null, it must ne a new empty shopping cart to avoid the error
                listCart = new List<ShoppingCart>()
            };

            //initialialized order details

            OrderDetailsCartVM.OrderHeader.OrderTotal = 0;

            //retrieving shopping card in database
            var claimsIdentity = (ClaimsIdentity) User.Identity;

            // this claim directly go to database it must be null unless login
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                //retrieve data from database
                IEnumerable<ShoppingCart> cart = _unitOfWork.ShoppingCart.GetAll
                    (s => s.ApplicationUserId == claim.Value);

                //cart database condition retrieval
                if (cart != null)
                {
                    OrderDetailsCartVM.listCart = cart.ToList()
                    
                }

                foreach (var cartList in OrderDetailsCartVM.listCart)
                {
                    cartList.MenuItem = _unitOfWork.MenuItem.GetFirstOrDefault(m => m.Id == cartList.MenuItemId);
                    //get total multiply to item price to item count
                    //?     OrderDetailsCartVM.OrderHeader.OrderTotal += cartList.MenuItem.Price * cartList.Count;
                    OrderDetailsCartVM.OrderHeader.OrderTotal += cartList.MenuItem.Price;
                }
            }

           
        }


        //this is for post handler plus minus and delete
        public IActionResult OnPostPlus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId);
            _unitOfWork.ShoppingCart.IncrementCount(cart, 1);


            _unitOfWork.Save();
            return RedirectToPage("/Customer/Cart/Index");
        }

        //post handler for minus

        public IActionResult OnPostMinus(int cartId)
        {
            
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId);

            //check if value to one so that it can be remove

            if (cart.Count == 1)
            {
                _unitOfWork.ShoppingCart.Remove(cart);
                _unitOfWork.Save();

                //update session
                var cnt = _unitOfWork.ShoppingCart.GetAll(
                    u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
                //setting session and load it automatically
                HttpContext.Session.SetInt32(SD.ShoppingCart, cnt);
            }
            else
            {
                _unitOfWork.ShoppingCart.DecrementCount(cart, 1);
                _unitOfWork.Save();
            }

            return RedirectToPage("/Customer/Cart/Index");
        }

        public IActionResult OnPostRemove(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(cart);

            //then reset session
            var cnt = _unitOfWork.ShoppingCart.GetAll(
                u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
            //setting session and load it automatically
            HttpContext.Session.SetInt32(SD.ShoppingCart, cnt);


            _unitOfWork.Save();
            return RedirectToPage("/Customer/Cart/Index");
        }


    }
}
