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
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {

        //initialize database
        private readonly ApplicationDbContext _db;
        public OrderDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderDetails orderDetails)
        {
           

            var orderDetailsFromDb = _db.OrderDetails.FirstOrDefault(o => o.Id == orderDetails.Id);

            _db.OrderDetails.Update(orderDetailsFromDb);

            _db.SaveChanges();
             
        }
    }
}
