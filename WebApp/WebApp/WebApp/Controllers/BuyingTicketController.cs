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
        [System.Web.Http.Route("api/Ticket/ByTimeTicket")]
        public IHttpActionResult BuyTimeTicket()
        {
            var req = HttpContext.Current.Request;
            var temp = req["lineType"];
            var price = _unitOfWork.Prices.GetAll().Where(u => u.ticketType == TicketType.TimeTicket && u.LineType.ToString() == req["lineType"].Trim()).Select(u => u.price).FirstOrDefault();
            Ticket ticket = new Ticket() { Checked = false, Price = price, RemainingTime = TimeSpan.FromMinutes(60), Type = Enums.TicketType.TimeTicket};
            if (!ModelState.IsValid)
            {
                  return BadRequest(ModelState);
            }

            
            _unitOfWork.Tickets.Add(ticket);
            _unitOfWork.Complete();
           
            EmailHelper.SendEmail(req.Form["email"], "TIME BUS TICKET", "You just bought your time ticket." + System.Environment.NewLine + "Ticket ID: " + ticket.Id + System.Environment.NewLine + "Type: " + ticket.Type + System.Environment.NewLine + "Price" + ticket.Price + System.Environment.NewLine + "NOTICE: Time ticket is valid 60 minutes after checked in.");

          

           return Ok(ticket.Id);

        }

        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
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
                    ch = false;
                    ty = TicketType.DailyTicket;
                    break;
                 case "MonthlyTicket":
                    ch = false;
                    ty = TicketType.MonthlyTicket;
                    break;
                case "AnnualTicket":
                    ch = false;
                    ty = TicketType.AnnualTicket;
                    break;
                default:
                    break;
            }


            Ticket ticket = new Ticket() { Checked = ch, Price = 0,   CheckedTime = DateTime.Now.ToString(), RemainingTime = TimeSpan.FromMinutes(60), Type = ty, UserId = userId };
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            _unitOfWork.Tickets.Add(ticket);
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
            price = prices.Where(item => item.ticketType.ToString() == req["type"].Trim() && item.LineType.ToString() == req["linetype"].Trim()).Select(n => n.price).FirstOrDefault(); 
         
 
            return Ok(price);

        }

        [System.Web.Http.Route("api/Ticket/GetPriceList")]
        public IHttpActionResult GetPriceList()
        {
            var pricesTemp = _unitOfWork.Prices.GetAll();
        
            var discounts = _unitOfWork.Discounts.GetAll();

            double[] retVals = new double[26];


            //urban ticket prices

            //time ticket
            retVals[0] = pricesTemp.Where((a) => a.LineType == LineTypes.Urban && a.ticketType == TicketType.TimeTicket).Select(n => n.price).FirstOrDefault();
            retVals[0] = retVals[0] - retVals[0] * discounts.Where((a) => a.Type == UserType.regular).Select(n => n.Discount).FirstOrDefault();
            retVals[1] = pricesTemp.Where((a) => a.LineType == LineTypes.Urban && a.ticketType == TicketType.TimeTicket).Select(n => n.price).FirstOrDefault();
            retVals[1] = retVals[1] - retVals[1] * discounts.Where((a) => a.Type == UserType.student).Select(n => n.Discount).FirstOrDefault();
            retVals[2] = pricesTemp.Where((a) => a.LineType == LineTypes.Urban && a.ticketType == TicketType.TimeTicket).Select(n => n.price).FirstOrDefault();
            retVals[2] = retVals[2] - retVals[2] * discounts.Where((a) => a.Type == UserType.retiree).Select(n => n.Discount).FirstOrDefault();

            //day ticket
            retVals[3] = pricesTemp.Where((a) => a.LineType == LineTypes.Urban && a.ticketType == TicketType.DailyTicket).Select(n => n.price).FirstOrDefault();
            retVals[3] = retVals[3] - retVals[3] * discounts.Where((a) => a.Type == UserType.regular).Select(n => n.Discount).FirstOrDefault();
            retVals[4] = pricesTemp.Where((a) => a.LineType == LineTypes.Urban && a.ticketType == TicketType.DailyTicket).Select(n => n.price).FirstOrDefault();
            retVals[4] = retVals[4] - retVals[4] * discounts.Where((a) => a.Type == UserType.student).Select(n => n.Discount).FirstOrDefault();
            retVals[5] = pricesTemp.Where((a) => a.LineType == LineTypes.Urban && a.ticketType == TicketType.DailyTicket).Select(n => n.price).FirstOrDefault();
            retVals[5] = retVals[4] - retVals[4] * discounts.Where((a) => a.Type == UserType.retiree).Select(n => n.Discount).FirstOrDefault();

            //month ticket
            retVals[6] = pricesTemp.Where((a) => a.LineType == LineTypes.Urban && a.ticketType == TicketType.MonthlyTicket).Select(n => n.price).FirstOrDefault();
            retVals[6] = retVals[6] - retVals[6] * discounts.Where((a) => a.Type == UserType.regular).Select(n => n.Discount).FirstOrDefault();
            retVals[7] = pricesTemp.Where((a) => a.LineType == LineTypes.Urban && a.ticketType == TicketType.MonthlyTicket).Select(n => n.price).FirstOrDefault();
            retVals[7] = retVals[7] - retVals[7] * discounts.Where((a) => a.Type == UserType.student).Select(n => n.Discount).FirstOrDefault();
            retVals[8] = pricesTemp.Where((a) => a.LineType == LineTypes.Urban && a.ticketType == TicketType.MonthlyTicket).Select(n => n.price).FirstOrDefault();
            retVals[8] = retVals[8] - retVals[8] * discounts.Where((a) => a.Type == UserType.retiree).Select(n => n.Discount).FirstOrDefault();

            //year ticekt
            retVals[9] = pricesTemp.Where((a) => a.LineType == LineTypes.Urban && a.ticketType == TicketType.AnnualTicket).Select(n => n.price).FirstOrDefault();
            retVals[9] = retVals[9] - retVals[9] * discounts.Where((a) => a.Type == UserType.regular).Select(n => n.Discount).FirstOrDefault();
            retVals[10] = pricesTemp.Where((a) => a.LineType == LineTypes.Urban && a.ticketType == TicketType.AnnualTicket).Select(n => n.price).FirstOrDefault();
            retVals[10] = retVals[10] - retVals[10] * discounts.Where((a) => a.Type == UserType.student).Select(n => n.Discount).FirstOrDefault();
            retVals[11] = pricesTemp.Where((a) => a.LineType == LineTypes.Urban && a.ticketType == TicketType.AnnualTicket).Select(n => n.price).FirstOrDefault();
            retVals[11] = retVals[11] - retVals[11] * discounts.Where((a) => a.Type == UserType.retiree).Select(n => n.Discount).FirstOrDefault();

            //suburban ticket

            //time ticket
            retVals[12] = pricesTemp.Where((a) => a.LineType == LineTypes.Suburban && a.ticketType == TicketType.TimeTicket).Select(n => n.price).FirstOrDefault();
            retVals[12]= retVals[12] - retVals[12] * discounts.Where((a) => a.Type == UserType.regular).Select(n => n.Discount).FirstOrDefault();
            retVals[13] = pricesTemp.Where((a) => a.LineType == LineTypes.Suburban && a.ticketType == TicketType.TimeTicket).Select(n => n.price).FirstOrDefault();
            retVals[13] = retVals[13] - retVals[13] * discounts.Where((a) => a.Type == UserType.student).Select(n => n.Discount).FirstOrDefault();
            retVals[14] = pricesTemp.Where((a) => a.LineType == LineTypes.Suburban && a.ticketType == TicketType.TimeTicket).Select(n => n.price).FirstOrDefault();
            retVals[14] = retVals[14] - retVals[14] * discounts.Where((a) => a.Type == UserType.retiree).Select(n => n.Discount).FirstOrDefault();

            //day ticket
            retVals[15] = pricesTemp.Where((a) => a.LineType == LineTypes.Suburban && a.ticketType == TicketType.DailyTicket).Select(n => n.price).FirstOrDefault();
            retVals[15] = retVals[15] - retVals[15] * discounts.Where((a) => a.Type == UserType.regular).Select(n => n.Discount).FirstOrDefault();
            retVals[16] = pricesTemp.Where((a) => a.LineType == LineTypes.Suburban && a.ticketType == TicketType.DailyTicket).Select(n => n.price).FirstOrDefault();
            retVals[16] = retVals[16] - retVals[16] * discounts.Where((a) => a.Type == UserType.student).Select(n => n.Discount).FirstOrDefault();
            retVals[17] = pricesTemp.Where((a) => a.LineType == LineTypes.Suburban && a.ticketType == TicketType.DailyTicket).Select(n => n.price).FirstOrDefault();
            retVals[17] = retVals[17] - retVals[17] * discounts.Where((a) => a.Type == UserType.retiree).Select(n => n.Discount).FirstOrDefault();

            //month ticket
            retVals[18] = pricesTemp.Where((a) => a.LineType == LineTypes.Suburban && a.ticketType == TicketType.MonthlyTicket).Select(n => n.price).FirstOrDefault();
            retVals[18] = retVals[18] - retVals[18] * discounts.Where((a) => a.Type == UserType.regular).Select(n => n.Discount).FirstOrDefault();
            retVals[19] = pricesTemp.Where((a) => a.LineType == LineTypes.Suburban && a.ticketType == TicketType.MonthlyTicket).Select(n => n.price).FirstOrDefault();
            retVals[19] = retVals[19] - retVals[19] * discounts.Where((a) => a.Type == UserType.student).Select(n => n.Discount).FirstOrDefault();
            retVals[20] = pricesTemp.Where((a) => a.LineType == LineTypes.Suburban && a.ticketType == TicketType.MonthlyTicket).Select(n => n.price).FirstOrDefault();
            retVals[20] = retVals[20] - retVals[20] * discounts.Where((a) => a.Type == UserType.retiree).Select(n => n.Discount).FirstOrDefault();

            //year ticekt
            retVals[21] = pricesTemp.Where((a) => a.LineType == LineTypes.Suburban && a.ticketType == TicketType.AnnualTicket).Select(n => n.price).FirstOrDefault();
            retVals[21] = retVals[21] - retVals[21] * discounts.Where((a) => a.Type == UserType.regular).Select(n => n.Discount).FirstOrDefault();
            retVals[22] = pricesTemp.Where((a) => a.LineType == LineTypes.Suburban && a.ticketType == TicketType.AnnualTicket).Select(n => n.price).FirstOrDefault();
            retVals[22] = retVals[22] - retVals[22] * discounts.Where((a) => a.Type == UserType.student).Select(n => n.Discount).FirstOrDefault();
            retVals[23] = pricesTemp.Where((a) => a.LineType == LineTypes.Suburban && a.ticketType == TicketType.AnnualTicket).Select(n => n.price).FirstOrDefault();
            retVals[23] = retVals[23] - retVals[23] * discounts.Where((a) => a.Type == UserType.retiree).Select(n => n.Discount).FirstOrDefault();


            retVals[24] = discounts.Where((a) => a.Type == UserType.student).Select(n => n.Discount).FirstOrDefault() * 100;
            retVals[25] = discounts.Where((a) => a.Type == UserType.retiree).Select(n => n.Discount).FirstOrDefault() * 100;

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