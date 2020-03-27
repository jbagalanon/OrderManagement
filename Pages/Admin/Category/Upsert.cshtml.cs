﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Taste.DataAccess.Data.Repository.IRepository;

namespace Taste.Pages.Admin.Category
{
    public class UpsertModel : PageModel
    {

        private readonly IUnitOfWork _uniotOfWork;

        public UpsertModel(IUnitOfWork unitOfWork)
        {
            _uniotOfWork = unitOfWork;
        }
        public Models.Category CategoryObj { get; set; }

        public IActionResult OnGet(int? id)
        {
            CategoryObj = new Models.Category();

            if (id != null)
            {
                CategoryObj = _uniotOfWork.Category.GetFirstOrDefault(u => u.Id == id);

                if (CategoryObj == null)
                {
                    return NotFound();
                }
                
            }
            return Page();

        }
    }
}