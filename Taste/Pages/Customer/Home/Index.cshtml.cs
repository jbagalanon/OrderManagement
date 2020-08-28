using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models;

namespace Taste.Pages.Customer.Home
{
    public class IndexModel : PageModel
    {
        
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<MenuItem> MenuItemlist { get; set; }
        public IEnumerable<Category> CategoryList { get; set; }


        public void OnGet()
        {

            MenuItemlist = _unitOfWork.MenuItem.GetAll(null, null,"Category,FoodType");
            CategoryList = _unitOfWork.Category.GetAll(null, o => o.OrderBy(
                            o=>o.DisplayOrder),null); 

        }

    }
}
