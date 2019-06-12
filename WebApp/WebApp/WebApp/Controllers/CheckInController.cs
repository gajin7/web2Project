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
            Int32.TryParse(req["Id"].Trim(), out id);
            if (id == -1)
                return Ok("Wrong ticket Id");

            if (_unitOfWork.Tickets.GetAll().Where(u => u.Id == id).FirstOrDefault() == null)
            {
                return Ok("Ticket Id unkonwn");
            }

            if (_unitOfWork.Tickets.GetAll().Where(u => u.Id == id).Select(u => u.Checked).First())
            {
                return Ok("Ticket allredy checked");
            }

            Ticket ticket = _unitOfWork.Tickets.Get(id);
            ticket.Checked = true;
            ticket.CheckedTime = DateTime.Now.ToString();

            _unitOfWork.Tickets.Update(ticket);
            _unitOfWork.Complete();

            TimeSpan rTime = DateTime.Parse(ticket.CheckedTime) - DateTime.Now + TimeSpan.FromMinutes(60);

            string retVal = 
                "Type: " + ticket.Type + System.Environment.NewLine +
                "Price: " +ticket.Price + System.Environment.NewLine +
                "Valid time: " + ticket.RemainingTime + System.Environment.NewLine +
                "Remaining time: " + rTime.ToString();

            return Ok(retVal);

        }


        [HttpPost]
        [System.Web.Http.Route("api/CheckIn/ControlTicket")]
        public IHttpActionResult ControlTicket()
        {
            var req = HttpContext.Current.Request;
            int id = -1;
            Int32.TryParse(req["Id"].Trim(), out id);
            Ticket ticket = _unitOfWork.Tickets.Get(id);
            if (id == -1)
                return Ok("Ticket Id doens't excist in database. Please check your enter.");

            if (ticket == null)
            {
                return Ok("Ticket Id unkonwn");
            }

            TimeSpan rTime = DateTime.Parse(ticket.CheckedTime) - DateTime.Now + TimeSpan.FromMinutes(60);
            if (!_unitOfWork.Tickets.GetAll().Where(u => u.Id == id).Select(u => u.Checked).First())
            {

                string retValue = "Ticket excist but not checked!" + System.Environment.NewLine + 
                    "Type: " +ticket.Type.ToString() + System.Environment.NewLine +
                    "Price: " + ticket.Price.ToString() + System.Environment.NewLine +
                    "Valid time: " + ticket.RemainingTime.ToString() + System.Environment.NewLine +
                    "User" + ticket.User.ToString() + System.Environment.NewLine +
                    "Remaining time: " + rTime.ToString();
                return Ok(retValue);
            }


            string retVal = "Ticket checked!" + System.Environment.NewLine + 
                     " Checked: " + ticket.CheckedTime.ToString() + System.Environment.NewLine +
                     " Type: " + ticket.Type.ToString() + System.Environment.NewLine +
                     " Price: " + ticket.Price.ToString() + System.Environment.NewLine +
                     " Valid time: " + ticket.RemainingTime.ToString() + System.Environment.NewLine +
                     " Remaining time: " + rTime.ToString();

            if (ticket.User != null)
                retVal += System.Environment.NewLine +  " User" + ticket.User.ToString() ;

            if(rTime < TimeSpan.FromSeconds(0))
            {

                retVal += System.Environment.NewLine + "TIcket expired!!!";
            }


            return Ok(retVal);

        }
    }
}