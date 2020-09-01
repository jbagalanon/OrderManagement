using System;
using System.Collections.Generic;
using System.Text;

namespace Taste.Models.ViewModels
{
    public class OrderDetailsCart
    {

        //list the item from shopping cart directory
        public List<ShoppingCart>listCart { get; set; }


        //add value from orderheader
        public OrderHeader OrderHeader { get; set; }
    }
}
