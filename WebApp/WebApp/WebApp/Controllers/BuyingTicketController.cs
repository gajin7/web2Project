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
using static WebApp.Models.Enums;

namespace WebApp.Controllers
{
    public class BuyingTicketController : ApiController
    {
        private IUnitOfWork _unitOfWork;
        private DbContext _context;

        public BuyingTicketController()
        {
        }

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
            Ticket ticket = new Ticket() {  Checked=false, Price=0, RemainingTime=TimeSpan.FromMinutes(60),Type=Enums.TicketType.TimeTicket};
            if (!ModelState.IsValid)
            {
                  return BadRequest(ModelState);
            }

            
            _unitOfWork.Tickets.Add(ticket);
            _unitOfWork.Complete();
           
            EmailHelper.SendEmail(req.Form["email"], "TIME BUS TICKET", "You just bought your time ticket." + System.Environment.NewLine + "Ticket ID: " + ticket.Id + System.Environment.NewLine + "NOTICE: Time ticket is valid 60 minutes after checked in.");

           
           return Ok(ticket.Id);

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

            string temp = req["type"].Trim();
            int v = temp.Length;
            var temp1 = TicketType.DailyTicket.ToString();
            int v1 = temp1.Length;

            var prices = _unitOfWork.Prices.GetAll();
            //   var price = prices.Where(item => item.ticketType.ToString() == req["type"]).Select(n => n.price); ?????
            double price = -1;
            foreach (var item in prices)
            {
                if (item.ticketType.ToString().Equals(temp))
                {
                    price = item.price;
                    break;
                }
                    

            }
        
            return Ok(price);

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