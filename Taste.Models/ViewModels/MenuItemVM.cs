using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Taste.Models.ViewModels
{
    class MenuItemVM
    {

        //MenuItem is is the main or primary while cat and food is fk
        public MenuItem MenuItem { get; set; }

        //Selectlistitem is an mvc rendering, this is use fo dropdown selection
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> FoodTypeList { get; set; }
    }
}
