using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models;

namespace Taste.DataAccess.Data.Repository
{
   public class FoodTypeRepository : Repository<FoodType>, IFoodTypeRepository
   {
       private readonly ApplicationDbContext _db;
       private IFoodTypeRepository _foodTypeRepositoryImplementation;

       public FoodTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetFoodTypeListForDropdown()
        {
            return _db.FoodType.Select(f => new SelectListItem()
            {
                Text = f.Name,
                Value = f.Id.ToString()
            });
        }

        public void update(FoodType foodType)
        {
            var objFromDb = _db.FoodType.FirstOrDefault(f => f.Id == foodType.Id);

            objFromDb.Name = foodType.Name;
          
        }
    }
}
