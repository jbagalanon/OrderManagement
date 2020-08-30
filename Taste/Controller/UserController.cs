using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taste.DataAccess.Data.Repository.IRepository;

namespace Taste.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        public IActionResult Get()
        {
            return Json(new {data = _unitOfWork.ApplicationUser.GetAll()});
        }


        //this is for lock and unlocking account
        [HttpPost]
        public IActionResult LockUnlock ([FromBody] string id)
        {
            var objFromDb = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == id);

            if (objFromDb == null)
            {
                return Json(new {success = false, message = "Error While Deleting"});
            }
            else
            {
                _unitOfWork.ApplicationUser.Remove(objFromDb);
                _unitOfWork.Save();
                return Json(new {success = true, message = "Files Deleted Successfully"});
            }

        }
    }
}