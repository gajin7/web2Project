using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
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
        private ApplicationUserManager _userManager;

        public CheckInController()
        {
        }

        public CheckInController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
         
        }


        public CheckInController(IUnitOfWork unitOfWork, DbContext context)
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
            string retVal =
                   "Type: " + ticket.Type + System.Environment.NewLine +
                   "Price: " + ticket.Price + System.Environment.NewLine;


            switch (ticket.Type)
            {
                case Enums.TicketType.TimeTicket:
                    ticket.Checked = true;
                    ticket.CheckedTime = DateTime.Now.ToString();

                    _unitOfWork.Tickets.Update(ticket);
                    _unitOfWork.Complete();
                    TimeSpan rTime = DateTime.Parse(ticket.CheckedTime) - DateTime.Now + ticket.RemainingTime;
                     retVal += "Remaining time: " + rTime.ToString();
                    break;
                case Enums.TicketType.DailyTicket:
                    retVal += "Remaining time: End of day" + DateTime.Parse(ticket.CheckedTime).Date;
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


        [Authorize(Roles = "Controller")]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
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
                    "Type: " + ticket.Type.ToString() + System.Environment.NewLine +
                    "Price: " + ticket.Price.ToString() + System.Environment.NewLine;
                return Ok(retValue);
            }


            string retVal = "Ticket checked!" + System.Environment.NewLine +
                     " Checked: " + ticket.CheckedTime.ToString() + System.Environment.NewLine +
                     " Type: " + ticket.Type.ToString() + System.Environment.NewLine +
                     " Price: " + ticket.Price.ToString() + System.Environment.NewLine;
                    

            if (ticket.User != null)
                retVal += System.Environment.NewLine +  " User" + ticket.User.ToString() ;

            string userName = "";
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
                     userName = _unitOfWork.Users.GetAll().Where(u => u.AppUserId == ticket.UserId).Select(u => u.FirstName).FirstOrDefault() + " " +
                        _unitOfWork.Users.GetAll().Where(u => u.AppUserId == ticket.UserId).Select(u => u.LastName).FirstOrDefault();
                    retVal += "User:" + userName + System.Environment.NewLine;
                    retVal += "Remaining time: End of day" + ticket.CheckedTime.Trim(' ')[0];
                    if (DateTime.Now.Date != DateTime.Parse(ticket.CheckedTime).Date)
                    {
                        retVal += System.Environment.NewLine + "TIcket expired!!!";
                    }
                    break;
                case Enums.TicketType.MonthlyTicket:
                     userName = _unitOfWork.Users.GetAll().Where(u => u.AppUserId == ticket.UserId).Select(u => u.FirstName).FirstOrDefault() + " " +
                        _unitOfWork.Users.GetAll().Where(u => u.AppUserId == ticket.UserId).Select(u => u.LastName).FirstOrDefault();
                    retVal += "User:" + userName + System.Environment.NewLine;
                    retVal += "Remaining time: End of month " + DateTime.Parse(ticket.CheckedTime).Month.ToString() + "/" + DateTime.Parse(ticket.CheckedTime).Year.ToString();
                    if (DateTime.Now.Month != DateTime.Parse(ticket.CheckedTime).Month || DateTime.Now.Year != DateTime.Parse(ticket.CheckedTime).Year)
                    {
                        retVal += System.Environment.NewLine + "TIcket expired!!!";
                    }
                    break;
                case Enums.TicketType.AnnualTicket:
                     userName = _unitOfWork.Users.GetAll().Where(u => u.AppUserId == ticket.UserId).Select(u => u.FirstName).FirstOrDefault() + " " +
                        _unitOfWork.Users.GetAll().Where(u => u.AppUserId == ticket.UserId).Select(u => u.LastName).FirstOrDefault();
                    retVal += "User:" + userName + System.Environment.NewLine;
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

        [HttpPost]
        [Authorize(Roles = "Controller")]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/CheckIn/GetUsersToApprove")]
        public List<ControlInfoModel> GetUsersToApprove()
        {

            var retVal = new List<ControlInfoModel>();

            var users = _unitOfWork.Users.GetAll().Where((u) => u.UserType != Enums.UserType.regular && u.Approved == false && u.postedImage == true && u.Checked == false);

            foreach (var item in users)
            {
                var email = UserManager.Users.Where((u) => u.Id == item.AppUserId).Select((u) => u.Email).FirstOrDefault();
                var pic = _unitOfWork.Pictures.GetAll().Where((u) => u.AppUserId == item.AppUserId).FirstOrDefault();
               
                string imgSource = System.Convert.ToBase64String(pic.ImageSource);
                retVal.Add(new ControlInfoModel { Email = email, FirstName = item.FirstName, LastName = item.LastName, Type = item.UserType.ToString(), DateOfBirth = item.DateOfBirth.Date.ToShortDateString(), Image = imgSource });
            }

            return retVal;

        }


        [HttpPost]
        [Authorize(Roles = "Controller")]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/CheckIn/ApproveUser")]
        public IHttpActionResult ApproveUser()
        {
            var req = HttpContext.Current.Request;
            var email = req["email"].Trim();

            var appUsrID = UserManager.Users.Where((u) => u.Email == email).Select((u) => u.Id).FirstOrDefault();

            User user = _unitOfWork.Users.GetAll().Where((u) => u.AppUserId == appUsrID).FirstOrDefault();
            user.Approved = true;
            user.Checked = true;

            _unitOfWork.Users.Update(user);
            _unitOfWork.Complete();

            EmailHelper.SendEmail(req.Form["email"], "ACCOUNT APORVEDD", "Your account just have been aprroved by controler");

            return Ok();

        }

        [HttpPost]
        [Authorize(Roles = "Controller")]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/CheckIn/DeclineUser")]
        public IHttpActionResult DeclineUser()
        {
            var req = HttpContext.Current.Request;
            var email = req["email"].Trim();

            var appUsrID = UserManager.Users.Where((u) => u.Email == email).Select((u) => u.Id).FirstOrDefault();

            User user = _unitOfWork.Users.GetAll().Where((u) => u.AppUserId == appUsrID).FirstOrDefault();
            user.Approved = false;
            user.Checked = true;

            _unitOfWork.Users.Update(user);
            _unitOfWork.Complete();

            EmailHelper.SendEmail(req.Form["email"], "ACCOUNT DECLINED", "Your account just have been declined by controler");

            return Ok();

        }
    }
}