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
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {

        //initialize database
        private readonly ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader orderHeader)
        {
            var orderHeaderFromDb = _db.OrderHeader.FirstOrDefault(o=>o.Id == orderHeader.Id);

            _db.OrderHeader.Update(orderHeaderFromDb);

            _db.SaveChanges();
        }
    }
}
