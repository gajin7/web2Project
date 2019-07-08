using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using WebApp.Models;
using WebApp.Persistence.UnitOfWork;
using static WebApp.Controllers.AccountController;
using static WebApp.Models.Enums;

namespace WebApp.Controllers
{
    public class BuyingTicketController : ApiController
    {
        private IUnitOfWork _unitOfWork;
        private DbContext _context;
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public BuyingTicketController()
        {
        }

        public BuyingTicketController(ApplicationUserManager userManager,
           ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;

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


        public BuyingTicketController(IUnitOfWork unitOfWork, DbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }


        [HttpPost]
        [System.Web.Http.Route("api/Ticket/SetTimeTicketForSale")]
        public TicketViewModel SetTimeTicketForSale()
        {
            var req = HttpContext.Current.Request;
            if (req.Form["email"] == "")
            {
                return new TicketViewModel
                {
                    mssg = "You must enter email!",
                    mail = "",
                    price = ""
                };
            }

            var price = _unitOfWork.Prices.GetAll().Where(u => u.ticketType == TicketType.TimeTicket).Select(u => u.price).FirstOrDefault();
          
            return new TicketViewModel
            {
                mssg = "You are just one more step from  your ticket. Please use your paypal acount to pay. Price: " + price + " USD.  When you pay, we will send details on your email: ",
                mail = req.Form["email"],
                price = price.ToString() 
            };
        }


        [HttpPost]
      
        [System.Web.Http.Route("api/Ticket/BuyTimeTicket")]
        public IHttpActionResult BuyTimeTicket()
        {
            var req = HttpContext.Current.Request;
            var price = _unitOfWork.Prices.GetAll().Where(u => u.ticketType == TicketType.TimeTicket).Select(u => u.price).FirstOrDefault();
             Ticket ticket = new Ticket() { Checked = false, Price = price, RemainingTime = TimeSpan.FromMinutes(60), Type = Enums.TicketType.TimeTicket};
             if (!ModelState.IsValid)
             {
                   return BadRequest(ModelState);
             }
           

            _unitOfWork.Tickets.Add(ticket);
            _unitOfWork.Complete();

            EmailHelper.SendEmail(req.Form["email"], "TIME BUS TICKET", "You just bought your time ticket." + System.Environment.NewLine + "Ticket ID: " + ticket.Id + System.Environment.NewLine + "Type: " + ticket.Type + System.Environment.NewLine + "Price" + ticket.Price + System.Environment.NewLine + "NOTICE: Time ticket is valid 60 minutes after checked in.");


            var id = req.Form["id"];
            var status = req.Form["status"];
            var payer_email = req.Form["payer_email"];
            var payer_id = req.Form["payer_id"];
            var create_time = req.Form["create_time"];
            var update_time = req.Form["update_time"];

            PayPalInfo payPalInfo = new PayPalInfo { CreateTime = create_time, UpdateTime = update_time, TransactionId = id, PayerEmail = payer_email, PayerId = payer_id, Status = status, TicketId = ticket.Id };
            _unitOfWork.PayPalInfos.Add(payPalInfo);

            _unitOfWork.Complete();

            return Ok(ticket.Id);
        }


        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize(Roles = "AppUser")]
        [System.Web.Http.Route("api/Ticket/SetTicketForSale")]
        public TicketViewModel SetTicketForSale()
        {


            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            var userId = User.Identity.GetUserId();

            var req = HttpContext.Current.Request;
            var ticketType = req["type"].Trim();
            bool ch = false;
            Enums.TicketType ty = new TicketType();
            TimeSpan remTime = new TimeSpan();

            switch (ticketType)
            {
                case "TimeTicket":
                    ch = false;
                    ty = TicketType.TimeTicket;
                    remTime = TimeSpan.FromMinutes(60);
                    break;
                case "DailyTicket":
                    ch = true;
                    ty = TicketType.DailyTicket;
                    break;
                case "MonthlyTicket":
                    ch = true;
                    ty = TicketType.MonthlyTicket;
                    break;
                case "AnnualTicket":
                    ch = true;
                    ty = TicketType.AnnualTicket;
                    break;
                default:
                    break;
            }

            UserType userType = _unitOfWork.Users.GetAll().Where(u => u.AppUserId == userId).Select(u => u.UserType).FirstOrDefault();

            var price = _unitOfWork.Prices.GetAll().Where(u => u.ticketType == ty).Select(u => u.price).FirstOrDefault()
                - _unitOfWork.Prices.GetAll().Where(u => u.ticketType == ty).Select(u => u.price).FirstOrDefault()
                * _unitOfWork.Discounts.GetAll().Where(u => u.Type == userType).Select(u => u.Discount).FirstOrDefault();

          
            
            return new TicketViewModel
            {
                mssg = "You are just one more step from  your ticket. Please use your paypal acount to pay. Ticket type: " + ty +  " Price: " + price + " USD.  When you pay, we will send details on your email: ",
                mail = User.Identity.GetUserName(),
                price = price.ToString()
            };
        }

        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize(Roles = "AppUser")]
        [System.Web.Http.Route("api/Ticket/BuyTicket")]
        public IHttpActionResult BuyTicket()
        {


            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            var userId = User.Identity.GetUserId();

            var req = HttpContext.Current.Request;
            var ticketType = req["type"].Trim();
            bool ch = false;
            Enums.TicketType ty = new TicketType();
            TimeSpan remTime = new TimeSpan();

            switch (ticketType)
            {
                case "TimeTicket":
                    ch = false;
                    ty = TicketType.TimeTicket;
                    remTime = TimeSpan.FromMinutes(60);
                    break;
                case "DailyTicket":
                    ch = true;
                    ty = TicketType.DailyTicket;
                    break;
                case "MonthlyTicket":
                    ch = true;
                    ty = TicketType.MonthlyTicket;
                    break;
                case "AnnualTicket":
                    ch = true;
                    ty = TicketType.AnnualTicket;
                    break;
                default:
                    break;
            }

            UserType userType = _unitOfWork.Users.GetAll().Where(u => u.AppUserId == userId).Select(u => u.UserType).FirstOrDefault();

            var price = _unitOfWork.Prices.GetAll().Where(u => u.ticketType == ty).Select(u => u.price).FirstOrDefault()
                - _unitOfWork.Prices.GetAll().Where(u => u.ticketType == ty).Select(u => u.price).FirstOrDefault()
                * _unitOfWork.Discounts.GetAll().Where(u => u.Type == userType).Select(u => u.Discount).FirstOrDefault();

              Ticket ticket = new Ticket() { Checked = ch, Price = price, CheckedTime = DateTime.Now.ToString(), RemainingTime = remTime, Type = ty, UserId = userId };


              _unitOfWork.Tickets.Add(ticket);
              _unitOfWork.Complete();


              EmailHelper.SendEmail(User.Identity.GetUserName(), " BUS TICKET", "You just bought  bus ticket." + System.Environment.NewLine + "Ticket ID: " + ticket.Id + System.Environment.NewLine + "Type: " + ticket.Type + System.Environment.NewLine + "Price" + ticket.Price + System.Environment.NewLine);

            var id = req.Form["id"];
            var status = req.Form["status"];
            var payer_email = req.Form["payer_email"];
            var payer_id = req.Form["payer_id"];
            var create_time = req.Form["create_time"];
            var update_time = req.Form["update_time"];

            PayPalInfo payPalInfo = new PayPalInfo { CreateTime = create_time, UpdateTime = update_time, TransactionId = id, PayerEmail = payer_email, PayerId = payer_id, Status = status, TicketId = ticket.Id };
            _unitOfWork.PayPalInfos.Add(payPalInfo);

            _unitOfWork.Complete();

            return Ok(ticket.Id);

        }


        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/Ticket/GetTickets")]
        public IHttpActionResult GetTickets()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            var userId = User.Identity.GetUserId();

            var retVal = _unitOfWork.Tickets.GetAll().Where(u => u.UserId == userId).ToList();

            return Ok(retVal);

        }


        [HttpPost]
        [System.Web.Http.Route("api/Ticket/GetTicketPrice")]
        public IHttpActionResult GetTicetPrice()
        {
            var req = HttpContext.Current.Request;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            double price = -1;
            var prices = _unitOfWork.Prices.GetAll();
            price = prices.Where(item => item.ticketType.ToString() == req["type"].Trim()).Select(n => n.price).FirstOrDefault();


            return Ok(price);

        }

        [System.Web.Http.Route("api/Ticket/GetPriceList")]
        public IHttpActionResult GetPriceList()
        {
            var pricesTemp = _unitOfWork.Prices.GetAll();

            var discounts = _unitOfWork.Discounts.GetAll();

            double[] retVals = new double[14];


            //urban ticket prices

            //time ticket
            retVals[0] = pricesTemp.Where((a) => a.ticketType == TicketType.TimeTicket).Select(n => n.price).FirstOrDefault();
            retVals[0] = retVals[0] - retVals[0] * discounts.Where((a) => a.Type == UserType.regular).Select(n => n.Discount).FirstOrDefault();
            retVals[1] = pricesTemp.Where((a) => a.ticketType == TicketType.TimeTicket).Select(n => n.price).FirstOrDefault();
            retVals[1] = retVals[1] - retVals[1] * discounts.Where((a) => a.Type == UserType.student).Select(n => n.Discount).FirstOrDefault();
            retVals[2] = pricesTemp.Where((a) => a.ticketType == TicketType.TimeTicket).Select(n => n.price).FirstOrDefault();
            retVals[2] = retVals[2] - retVals[2] * discounts.Where((a) => a.Type == UserType.retiree).Select(n => n.Discount).FirstOrDefault();

            //day ticket
            retVals[3] = pricesTemp.Where((a) => a.ticketType == TicketType.DailyTicket).Select(n => n.price).FirstOrDefault();
            retVals[3] = retVals[3] - retVals[3] * discounts.Where((a) => a.Type == UserType.regular).Select(n => n.Discount).FirstOrDefault();
            retVals[4] = pricesTemp.Where((a) => a.ticketType == TicketType.DailyTicket).Select(n => n.price).FirstOrDefault();
            retVals[4] = retVals[4] - retVals[4] * discounts.Where((a) => a.Type == UserType.student).Select(n => n.Discount).FirstOrDefault();
            retVals[5] = pricesTemp.Where((a) => a.ticketType == TicketType.DailyTicket).Select(n => n.price).FirstOrDefault();
            retVals[5] = retVals[4] - retVals[4] * discounts.Where((a) => a.Type == UserType.retiree).Select(n => n.Discount).FirstOrDefault();

            //month ticket
            retVals[6] = pricesTemp.Where((a) => a.ticketType == TicketType.MonthlyTicket).Select(n => n.price).FirstOrDefault();
            retVals[6] = retVals[6] - retVals[6] * discounts.Where((a) => a.Type == UserType.regular).Select(n => n.Discount).FirstOrDefault();
            retVals[7] = pricesTemp.Where((a) => a.ticketType == TicketType.MonthlyTicket).Select(n => n.price).FirstOrDefault();
            retVals[7] = retVals[7] - retVals[7] * discounts.Where((a) => a.Type == UserType.student).Select(n => n.Discount).FirstOrDefault();
            retVals[8] = pricesTemp.Where((a) => a.ticketType == TicketType.MonthlyTicket).Select(n => n.price).FirstOrDefault();
            retVals[8] = retVals[8] - retVals[8] * discounts.Where((a) => a.Type == UserType.retiree).Select(n => n.Discount).FirstOrDefault();

            //year ticekt
            retVals[9] = pricesTemp.Where((a) => a.ticketType == TicketType.AnnualTicket).Select(n => n.price).FirstOrDefault();
            retVals[9] = retVals[9] - retVals[9] * discounts.Where((a) => a.Type == UserType.regular).Select(n => n.Discount).FirstOrDefault();
            retVals[10] = pricesTemp.Where((a) => a.ticketType == TicketType.AnnualTicket).Select(n => n.price).FirstOrDefault();
            retVals[10] = retVals[10] - retVals[10] * discounts.Where((a) => a.Type == UserType.student).Select(n => n.Discount).FirstOrDefault();
            retVals[11] = pricesTemp.Where((a) => a.ticketType == TicketType.AnnualTicket).Select(n => n.price).FirstOrDefault();
            retVals[11] = retVals[11] - retVals[11] * discounts.Where((a) => a.Type == UserType.retiree).Select(n => n.Discount).FirstOrDefault();

            //suburban ticket




            retVals[12] = Convert.ToInt32(discounts.Where((a) => a.Type == UserType.student).Select(n => n.Discount).FirstOrDefault() * 100);
            retVals[13] = Convert.ToInt32(discounts.Where((a) => a.Type == UserType.retiree).Select(n => n.Discount).FirstOrDefault() * 100);

            return Json(retVals);



        }



        public Int32 GetUniqueId()
        {
            var now = DateTime.Now;
            var zeroDate = DateTime.MinValue.AddHours(now.Hour).AddMinutes(now.Minute).AddSeconds(now.Second).AddMilliseconds(now.Millisecond);
            int uniqueId = (int)(zeroDate.Ticks / 10000);

            return uniqueId;
        }


    }
}