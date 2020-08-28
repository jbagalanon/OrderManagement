using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models;

namespace Taste.DataAccess.Data.Repository
{
    //implement the repo
    public class MenuItemRepository : Repository<MenuItem>, IMenuItemRepository
    {

        //initialize database
        private readonly ApplicationDbContext _db;
        public MenuItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(MenuItem menuItem)
        {
            var menuItemFromDb = _db.MenuItem.FirstOrDefault(m => m.Id == menuItem.Id);

            menuItemFromDb.Name = menuItem.Name;
            menuItemFromDb.Description = menuItem.Description;
            menuItemFromDb.CategoryId = menuItem.CategoryId;
            menuItemFromDb.FoodTypeId = menuItem.FoodTypeId;
           menuItemFromDb.Price = menuItem.Price;

            //update only if image is uploaded
            if (menuItemFromDb.Image != null)
            {
                menuItemFromDb.Image = menuItem.Image;

            }
            

            _db.SaveChanges();
        }
    }
}
