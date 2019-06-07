using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers
{
    public class PriceListController : ApiController
    {
        private IUnitOfWork _unitOfWork;
        private DbContext _context;

        public PriceListController()
        {
        }

        public PriceListController(IUnitOfWork unitOfWork, DbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }


        [HttpGet]
        [System.Web.Http.Route("api/Pricelist/GetActualPricelist")]
        public IHttpActionResult BuyTimeTicket()
        {
            var req = HttpContext.Current.Request;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            //_unitOfWork.Tickets.Add(ticket);
           // _unitOfWork.Complete();

         //   EmailHelper.SendEmail(req.Form["email"], "TIME BUS TICKET", "You just bought your time ticket." + System.Environment.NewLine + "Ticket ID: " + ticket.Id + System.Environment.NewLine + "NOTICE: Time ticket is valid 60 minutes after checked in.");


            return Ok();

        }
    }
}