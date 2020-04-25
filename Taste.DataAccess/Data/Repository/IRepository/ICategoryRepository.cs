using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Taste.Models;

namespace Taste.DataAccess.Data.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {

        //select list item is used for dropdown menu
        IEnumerable <SelectListItem> GetCategoryListForDropdown();

        void Update(Category category);

    }
}
