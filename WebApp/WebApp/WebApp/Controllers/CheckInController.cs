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
    public class CheckInController : ApiController
    {
        private IUnitOfWork _unitOfWork;
        private DbContext _context;

        public CheckInController()
        {
        }

        public CheckInController(IUnitOfWork unitOfWork, DbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }


        [HttpPost]
        [System.Web.Http.Route("api/CheckIn/CheckTicket")]
        public IHttpActionResult CheckTicekt()
        {
            var req = HttpContext.Current.Request;
            int id = -1;
            Int32.TryParse(req["ticketId"].Trim(), out id);
            if (id == -1)
                return BadRequest("Wrong ticket Id");

            if (_unitOfWork.Tickets.GetAll().Where(u => u.Id == id).First() == null)
            {
                return BadRequest("Ticket Id unkonwn");
            }

            if (_unitOfWork.Tickets.GetAll().Where(u => u.Id == id).Select(u => u.Checked).First())
            {
                return BadRequest("Ticket allredy checked");
            }

            Ticket ticket = _unitOfWork.Tickets.Get(id);
            ticket.Checked = true;
            ticket.CheckedTime = DateTime.Now.ToString();

            _unitOfWork.Tickets.Update(ticket);

            TimeSpan rTime = DateTime.Parse(_unitOfWork.Tickets.GetAll().Where(u => u.Id == id).Select(u => u.CheckedTime).First()) - DateTime.Now + TimeSpan.FromMinutes(60);

            string retVal = "Type: " + _unitOfWork.Tickets.GetAll().Where(u => u.Id == id).Select(u => u.Type).First().ToString() + System.Environment.NewLine +
                "price: " + _unitOfWork.Tickets.GetAll().Where(u => u.Id == id).Select(u => u.Price).First().ToString() + System.Environment.NewLine +
                "valid time: " + _unitOfWork.Tickets.GetAll().Where(u => u.Id == id).Select(u => u.RemainingTime).First().ToString() + System.Environment.NewLine +
                "user" + _unitOfWork.Tickets.GetAll().Where(u => u.Id == id).Select(u => u.User).FirstOrDefault().ToString() + System.Environment.NewLine +
                "Remaining time: " + rTime.ToString();


            return Ok(retVal);

        }
    }
}