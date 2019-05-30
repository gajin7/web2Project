using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Persistence.Repository
{
    public class ProductRepository : Repository<Product,int>, IProductRepository
    {
        
        public ProductRepository(DbContext context) : base(context)
        {
        }
    }
}