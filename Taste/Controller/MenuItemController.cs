using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models;

namespace Taste.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //deleting item means deleting image in the server
        private readonly IWebHostEnvironment _hostingEnvironment;

        public MenuItemController(IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
        }


        //public MenuItem MenuItem { get; set; }

        //[HttpGet]
        //public IActionResult Get()
        //{
        //    //return Json(new { data = _unitOfWork.})
        //}
    }
}
