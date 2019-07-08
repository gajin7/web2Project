using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Persistence.Repository
{
    public class PayPalInfoRepository : Repository<PayPalInfo, int>, IPayPalInfoRepository
    {
        public PayPalInfoRepository(DbContext context) : base(context)
        {
        }
    
     }
}