using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApp.Models;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers
{
    public class AdminController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        private IUnitOfWork _unitOfWork;
        private DbContext _context;

        public AdminController()
        {
        }

        public AdminController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;

        }

        public AdminController(IUnitOfWork unitOfWork, DbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/Admin/ChangePrice")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult ChangePrice()
        {
            var req = HttpContext.Current.Request;
            int tmp;
            if (req["type"] == "undefined" || req["type"] == "")
                return BadRequest("Please select type");
            if (req["price"] == "" || req["price"] == "undefined" || !Int32.TryParse(req["price"], out tmp))
                return BadRequest("Please enter valid price value");

            Prices price = _unitOfWork.Prices.GetAll().Where((u) => u.ticketType.ToString().Equals(req["type"].Trim())).FirstOrDefault();
            

            price.price = tmp;
            _unitOfWork.Prices.Update(price);
            _unitOfWork.Complete();


            return Ok("Price successfully changed");
        }

        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/Admin/ChangeDiscount")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult ChangeDiscount()
        {
            var req = HttpContext.Current.Request;
            var temp = req["type"];
            double tmp;
            if (req["type"] == "undefined" || req["type"] == "")
                return BadRequest("Please select type");
            if(req["discount"] == "" || req["discount"] == "undefined" || !Double.TryParse(req["discount"],out tmp))
                return BadRequest("Please enter valid discount value");

            Discounts discount = _unitOfWork.Discounts.GetAll().Where((u) => u.Type.ToString().Equals(req["type"].Trim())).FirstOrDefault();
            discount.Discount = tmp / 100;


            _unitOfWork.Discounts.Update(discount);
            _unitOfWork.Complete();


            return Ok("Discount successfully updated");
        }

        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/Admin/GetLines")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetLines()
        {
          
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var lines = _unitOfWork.Lines.GetAll().Select(u => u.Name);


            return Json(lines);

        }

        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/Admin/GetDepatures")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetDepatures()
        {

            var req = HttpContext.Current.Request;
            var temp = req["type"];
            var temp1 = req["line"];
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var lineId = _unitOfWork.Lines.GetAll().Where((u) => u.Name == req["line"].Trim()).Select((u) => u.Id).FirstOrDefault();

            var scheduleId = _unitOfWork.Schedules.GetAll().Where((u) => u.Day.ToString() == req["type"].Trim() && u.LineId == lineId).Select((u)=> u.Id).FirstOrDefault();

            var depatures = _unitOfWork.Depatures.GetAll().Where((u) => u.ScheduleId == scheduleId).Select((u) => u.DepatureTime);

            string retVal = "";

            foreach (var item in depatures)
            {
                retVal += item + ", ";
            }

           


            return Json(retVal);

        }



    }
}