using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Taste.DataAccess.Data.Repository.IRepository;

namespace Taste.Pages.Admin.FoodType
{
    public class UpsertModel : PageModel
    {

        private readonly IUnitOfWork  _unitOfWork;

        public UpsertModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [BindProperty]
        //get models 
        public Models.FoodType FoodTypeObj { get; set; }

        public IActionResult OnGet(int ? id)
        {
            //initialize obj
            FoodTypeObj = new Models.FoodType();

            //check obj validdation
            if (id != null)
            {
                FoodTypeObj = _unitOfWork.FoodType.GetFirstOrDefault(f => f.Id == id);

                if (FoodTypeObj == null)
                {
                    return NotFound();
                }
                
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            //if food object is equal to  0 add object
            else if (FoodTypeObj.Id == 0)
            {
                _unitOfWork.FoodType.Add(FoodTypeObj);
            }
            else
            {
               _unitOfWork.FoodType.update(FoodTypeObj); 
            }
            _unitOfWork.Save();
            return RedirectToPage("./Index");

        }



    }
}
