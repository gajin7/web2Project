using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Persistence.Repository
{
    public class PictureRepository : Repository<Picture, int>, IPictureRepository
    {
        public PictureRepository(DbContext context) : base(context)
        {
        }
    }
}