﻿using System;
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
    public class CategoryController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet]

        public IActionResult Get()
        {
            return Json(new {data = _unitOfWork.Category.GetAll()});
        }

        [HttpDelete("{id}")]
        public IActionResult Delete (int id)
        {
            var objFromDb = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);

            if (objFromDb == null)
            {
                return Json(new {success = false, message = "Error While Deleting"});
            }
            else
            {
                _unitOfWork.Category.Remove(objFromDb);
                _unitOfWork.Save();
                return Json(new {success = true, message = "Files Deleted Successfully"});
            }
        }
    }
}