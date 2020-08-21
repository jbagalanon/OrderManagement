using System;
using System.Collections.Generic;
using System.IO;
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

        [HttpGet]
        public IActionResult Get()
        {
            return Json(new
            {
                data = _unitOfWork.MenuItem.GetAll(
                    null, null, "Category,FoodType")
            });
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var objFromDb = _unitOfWork.MenuItem.GetFirstOrDefault(
                    m => m.Id == id);

                //check menu item if empty 
                if (objFromDb == null)
                {
                    return Json(new {success = false, message = "Error while deleting"});
                }

                //then check if image is exists
                var imagePath = Path.Combine
                    (_hostingEnvironment.WebRootPath, objFromDb.Image.TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                {
                    //remove the image
                    System.IO.File.Delete(imagePath);
                }

                _unitOfWork.MenuItem.Remove(objFromDb);
                _unitOfWork.Save();

               
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            return Json(new { success = true, message = "Menu Item Successfully Deleted" });
        }
    }
}
