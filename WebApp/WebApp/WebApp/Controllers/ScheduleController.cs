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
    public class ScheduleController : ApiController
    {
        private IUnitOfWork _unitOfWork;
        private DbContext _context;

        public ScheduleController()
        {
        }

        public ScheduleController(IUnitOfWork unitOfWork, DbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }


        [HttpPost]
        [System.Web.Http.Route("api/Schedule/GetSchedule")]
        public IHttpActionResult GetSchedule ()
        {
            var req = HttpContext.Current.Request;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var s = _unitOfWork.Schedules.GetAll();

            var lineId = _unitOfWork.Lines.GetAll().Where(u => u.Name == req["line"].Trim()).Select(u => u.Id).FirstOrDefault();

             List<Schedule> sch = _unitOfWork.Schedules.GetAll().Where(u => u.Day.ToString().Equals(req["day"].Trim()) && u.LineId == lineId).ToList();
             List<string> dep = new List<string>();

             foreach (var item in sch)
             {
                 dep = item.Depatures.Select(u => u.DepatureTime).ToList();
             }



            return Json(dep);

            

        }


        [HttpPost]
        [System.Web.Http.Route("api/Schedule/GetLines")]
        public IHttpActionResult GetLines()
        {
            var req = HttpContext.Current.Request;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var lines = _unitOfWork.Lines.GetAll().Where(u => u.LineType.ToString().Equals(req["type"])).Select(u => u.Name);
         

            return Json(lines);

        }

    }
}