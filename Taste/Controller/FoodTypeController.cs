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
    public class FoodTypeController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public FoodTypeController(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;

        }

        [HttpGet]
        public IActionResult Get()
        {

            return Json(new {data = _unitOfWork.FoodType.GetAll()});
        }


        [HttpDelete ("{id}")]
        public IActionResult Delete(int  id)
        {
            var objFromDb = _unitOfWork.FoodType.GetFirstOrDefault(f => f.Id == id);

            if (objFromDb == null)
            {
                return Json(new {success = false, message = "File Not Found"});

            }
            else
            {
                _unitOfWork.FoodType.Remove(objFromDb);
                _unitOfWork.Save();
                return Json(new {success=true, message ="File Deleted Successfully"});

            }
        }


    }
}
