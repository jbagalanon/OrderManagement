using System;
using System.Collections.Generic;
using System.Text;
using Taste.Models;

namespace Taste.DataAccess.Data.Repository.IRepository
{

    //inherit to menuitem Irepo
    public interface IMenuItemRepository : IRepository<MenuItem>
    {
        void Update(MenuItem menuItem);
    }
}
