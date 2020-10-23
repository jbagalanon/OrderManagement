using System;
using System.Collections.Generic;
using System.Text;

namespace Taste.Models.ViewModels
{
   public class OrderDetailsViewModel
    {

        //this is for sigle order
        public OrderHeader OrderHeader { get; set; }

        //List because for one order there are more than one details
        public List< OrderDetails> OrderDetails { get; set; }


    }
}
