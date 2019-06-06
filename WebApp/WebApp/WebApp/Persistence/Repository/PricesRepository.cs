using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Persistence.Repository
{
    public class PricesRepository : Repository<Prices, int>, IPricesRepository
    {
        public PricesRepository(DbContext context) : base(context)
        {
        }
    }
}