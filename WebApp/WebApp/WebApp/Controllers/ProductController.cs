using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using WebApp.Models;
using WebApp.Persistence;
using WebApp.Persistence.Repository;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers
{
    public class ProductController : ApiController
    {
        private IUnitOfWork _unitOfWork;
        private DbContext _context;

        public ProductController(IUnitOfWork unitOfWork, DbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

       // private IProductRepository repo = new ProductRepository(new ApplicationDbContext());
        // GET: Product
        
        public IEnumerable<Product> GetProducts()
        {
            return _unitOfWork.Products.GetAll();
        }

        [System.Web.Http.Authorize(Roles = "Admin")]
        [System.Web.Http.Route("api/Product/PostProudct")]
        public IHttpActionResult PostProduct()
        {
            var req = HttpContext.Current.Request;
            var product = new Product() { name = req.Form["Name"] };
            if( !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _unitOfWork.Products.Add(product);
            _unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = product.id }, product);
        }
    }
}