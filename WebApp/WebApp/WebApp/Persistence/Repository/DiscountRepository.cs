using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Persistence.Repository
{
    public class DiscountRepository : Repository<Discounts,int>, IDiscountRepository
    {
        public DiscountRepository(DbContext context) : base(context)
        {
        }
    }
}