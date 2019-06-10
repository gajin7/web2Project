﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Persistence.Repository
{
    public class DepatureRepository : Repository<Depature, int>, IDepatureRepository
    {
        public DepatureRepository(DbContext context) : base(context)
        {
        }
    
    }
}