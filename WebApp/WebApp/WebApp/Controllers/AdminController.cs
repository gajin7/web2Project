using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
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

            if (req["line"] == "" || req["line"] == "undefined")
                return BadRequest("Please select line");

            if (req["type"] == "" || req["type"] == "undefined")
                return BadRequest("Please select type");


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

        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/Admin/AddDepature")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult AddDepature()
        {

            var req = HttpContext.Current.Request;

            var r = new Regex("^(\\d\\d:\\d\\d)$");
           
            if (req["line"] == "" || req["line"] == "undefined")
                return BadRequest("Please select line");

            if (req["type"] == "" || req["type"] == "undefined")
                return BadRequest("Please select type");
            if (!r.IsMatch(req["depature"]))
                return BadRequest("Input in wrong format");


            var lineId = _unitOfWork.Lines.GetAll().Where((u) => u.Name == req["line"].Trim()).Select((u) => u.Id).FirstOrDefault();
            var scheduleId = _unitOfWork.Schedules.GetAll().Where((u) => u.LineId == lineId && u.Day.ToString() == req["type"].Trim()).Select((u) => u.Id).FirstOrDefault();

            var depatures = _unitOfWork.Depatures.GetAll();

            if(depatures.Where((u)=> u.ScheduleId == scheduleId && u.DepatureTime == req["depature"].Trim()).FirstOrDefault() != null)
            {
                return BadRequest("Defined depature allredy excist");
            }

            Depature dep = new Depature() { ScheduleId = scheduleId, DepatureTime = req["depature"].Trim() };

            _unitOfWork.Depatures.Add(dep);
            _unitOfWork.Complete();

            return Ok("Changes Saved");

        }

        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/Admin/RemoveDepature")]
         [Authorize(Roles = "Admin")]
        public IHttpActionResult RemoveDepature()
        {

            var req = HttpContext.Current.Request;

            var r = new Regex("^(\\d\\d:\\d\\d)$");

            if (req["line"] == "" || req["line"] == "undefined")
                return BadRequest("Please select line");

            if (req["type"] == "" || req["type"] == "undefined")
                return BadRequest("Please select type");
            if (!r.IsMatch(req["depature"]))
                return BadRequest("Input in wrong format");


            var lineId = _unitOfWork.Lines.GetAll().Where((u) => u.Name == req["line"].Trim()).Select((u) => u.Id).FirstOrDefault();
            var scheduleId = _unitOfWork.Schedules.GetAll().Where((u) => u.LineId == lineId && u.Day.ToString() == req["type"].Trim()).Select((u) => u.Id).FirstOrDefault();

            var depatures = _unitOfWork.Depatures.GetAll();

            var dep = depatures.Where((u) => u.ScheduleId == scheduleId && u.DepatureTime == req["depature"].Trim()).FirstOrDefault();
            if (dep == null)
            {
                return BadRequest("Defined depature not excist");
            }

            _unitOfWork.Depatures.Remove(dep);
            _unitOfWork.Complete();

            return Ok("Changes saved");

        }



    }
}