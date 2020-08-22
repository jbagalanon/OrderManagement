using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models.ViewModels;

namespace Taste.Pages.Admin.MenuItem
{
    public class UpsertModel : PageModel
    {

        private readonly IUnitOfWork _unitOfWork;

        //this is use to move images in hosting from the server
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UpsertModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
        }


        //this binding prop is from mvvm menuitem property
        [BindProperty] 
        public MenuItemVM MenuItemObj { get; set; }

        public IActionResult OnGet(int? id)
        {

            //load dropdown list
            MenuItemObj = new MenuItemVM
            {
                CategoryList = _unitOfWork.Category.GetCategoryListForDropdown(),
                FoodTypeList = _unitOfWork.FoodType.GetFoodTypeListForDropdown()
            };

            if (id != null)
            {
                MenuItemObj.MenuItem= _unitOfWork.MenuItem.GetFirstOrDefault(u => u.Id == id);
                if (MenuItemObj.MenuItem == null)
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

            if (MenuItemObj.Id == 0)
            {
                _unitOfWork.Category.Add(MenuItemObj);
            }
            else
            {
                _unitOfWork.Category.Update(MenuItemObj);
            }

            _unitOfWork.Save();
            return RedirectToPage("./Index");
        }
    }

}