﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models;
using Taste.Models.ViewModels;
using Taste.Utility;


namespace Taste.Controller
{
    [Route("api/[controller]")]
    [ApiController]

    //note no json if it is controller base
    public class OrderController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<OrderDetailsViewModel> orderListVM = new List<OrderDetailsViewModel>();

            IEnumerable<OrderHeader> OrderHeaderList;


            //if role is customer
            if (User.IsInRole(SD.CustomerRole))
            {
                //retrieve customer orders

                //need the identity for that customer
                var claimsIdentity = (ClaimsIdentity)User.Identity;

                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                OrderHeaderList = _unitOfWork.OrderHeader.GetAll(u => u.UserId == claim.Value, null, "ApplicationUser");

            }
            else
            {
                OrderHeaderList = _unitOfWork.OrderHeader.GetAll(null, null, "ApplicationUser");
            }
            //then populate orders
            foreach (OrderHeader item in OrderHeaderList)
            {
                OrderDetailsViewModel individual = new OrderDetailsViewModel
                {
                    OrderHeader = item,
                    //get the item from database
                    OrderDetails = _unitOfWork.OrderDetails.GetAll(o => o.OrderId == item.Id).ToList()
                };
                //then add the list to viewmodel
                orderListVM.Add(individual);
            }
            //return as json response
            return Json(new {data = orderListVM});
        }

    }

}
}
