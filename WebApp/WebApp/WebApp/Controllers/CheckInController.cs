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

            string retVal =
                   "Type: " + ticket.Type + System.Environment.NewLine +
                   "Price: " + ticket.Price + System.Environment.NewLine +
                   "Valid time: " + ticket.RemainingTime + System.Environment.NewLine;


            switch (ticket.Type)
            {
                case Enums.TicketType.TimeTicket:
                    TimeSpan rTime = DateTime.Parse(ticket.CheckedTime) - DateTime.Now + ticket.RemainingTime;
                     retVal += "Remaining time: " + rTime.ToString();
                    break;
                case Enums.TicketType.DailyTicket:
                    retVal += "Remaining time: End of day" + ticket.CheckedTime.Trim(' ')[0];
                    break;
                case Enums.TicketType.MonthlyTicket:
                    retVal += "Remaining time: End of month " + DateTime.Parse(ticket.CheckedTime).Month.ToString() + "/" + DateTime.Parse(ticket.CheckedTime).Year.ToString();
                    break;
                case Enums.TicketType.AnnualTicket:
                    retVal += "Remaining time: End of year " + DateTime.Parse(ticket.CheckedTime).Year.ToString();
                    break;
                default:
                    break;
            }


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

           
            if (!_unitOfWork.Tickets.GetAll().Where(u => u.Id == id).Select(u => u.Checked).First())
            {

                string retValue = "Ticket excist but not checked!" + System.Environment.NewLine + 
                    "Type: " +ticket.Type.ToString() + System.Environment.NewLine +
                    "Price: " + ticket.Price.ToString() + System.Environment.NewLine +
                    "Valid time: " + ticket.RemainingTime.ToString() + System.Environment.NewLine +
                    "User" + ticket.User.ToString() + System.Environment.NewLine +
                return Ok(retValue);
            }


            string retVal = "Ticket checked!" + System.Environment.NewLine + 
                     " Checked: " + ticket.CheckedTime.ToString() + System.Environment.NewLine +
                     " Type: " + ticket.Type.ToString() + System.Environment.NewLine +
                     " Price: " + ticket.Price.ToString() + System.Environment.NewLine +
                     " Valid time: " + ticket.RemainingTime.ToString() + System.Environment.NewLine +
                    

            if (ticket.User != null)
                retVal += System.Environment.NewLine +  " User" + ticket.User.ToString() ;


            switch (ticket.Type)
            {
                case Enums.TicketType.TimeTicket:
                    TimeSpan rTime = DateTime.Parse(ticket.CheckedTime) - DateTime.Now + ticket.RemainingTime;
                    retVal += " Remaining time: " + rTime.ToString();
                    if (rTime < TimeSpan.FromSeconds(0))
                    {
                        retVal += System.Environment.NewLine + "TIcket expired!!!";
                    }
                    break;
                case Enums.TicketType.DailyTicket:
                    retVal += "Remaining time: End of day" + ticket.CheckedTime.Trim(' ')[0];
                    if (DateTime.Now.Date != DateTime.Parse(ticket.CheckedTime).Date)
                    {
                        retVal += System.Environment.NewLine + "TIcket expired!!!";
                    }
                    break;
                case Enums.TicketType.MonthlyTicket:
                    retVal += "Remaining time: End of month " + DateTime.Parse(ticket.CheckedTime).Month.ToString() + "/" + DateTime.Parse(ticket.CheckedTime).Year.ToString();
                    if (DateTime.Now.Month != DateTime.Parse(ticket.CheckedTime).Month || DateTime.Now.Year != DateTime.Parse(ticket.CheckedTime).Year)
                    {
                        retVal += System.Environment.NewLine + "TIcket expired!!!";
                    }
                    break;
                case Enums.TicketType.AnnualTicket:
                    retVal += "Remaining time: End of year " + DateTime.Parse(ticket.CheckedTime).Year.ToString();
                    if (DateTime.Now.Year != DateTime.Parse(ticket.CheckedTime).Year)
                    {
                        retVal += System.Environment.NewLine + "TIcket expired!!!";
                    }
                    break;
                default:
                    break;
            }
            


            return Ok(retVal);

        }
    }
}