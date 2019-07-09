using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApp.Hubs;
using WebApp.Models;
using WebApp.Persistence.UnitOfWork;
using static WebApp.Models.Enums;

namespace WebApp.Controllers
{
    public class BusStationsController : ApiController
    {
        private IUnitOfWork _unitOfWork;
        private DbContext _context;
        private BusLocationHub hub;

        public BusStationsController()
        {
        }

        public BusStationsController(IUnitOfWork uw, BusLocationHub hubb)
        {
            _unitOfWork = uw;
            hub = hubb;
        }

        public BusStationsController(IUnitOfWork unitOfWork, DbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }


        [HttpGet]
        [System.Web.Http.Route("api/BusStations/GetAllStations")]
        public IHttpActionResult GetAllStations()
        {
            var stations = _unitOfWork.Stations.GetAll();
            List<StationModel> stats = new List<StationModel>();
            foreach (var item in stations)
            {
                var temp = _unitOfWork.Locations.Get(item.LocationId);
                string lines = "";
                foreach (var i in item.Lines)
                {
                    lines += i.Name + " ";
                }

                stats.Add(new StationModel { name = item.Name, latitude = temp.Lat, longitude = temp.Lon, address = item.Address, lines = lines });
            }

            return Json(stats);

        }

        [HttpGet]
        [System.Web.Http.Route("api/BusStations/GetAllLines")]
        public IHttpActionResult GetAllLines()
        {
            var AllLines = _unitOfWork.Lines.GetAll();
            List<LineModel> lines = new List<LineModel>();


            foreach (var item in AllLines)
            {
                List<Stations> stations = new List<Stations>();
                foreach (var i in item.Stations)
                {
                    var loc = _unitOfWork.Locations.Get(i.LocationId);
                    stations.Add(new Stations { Latitude = loc.Lat, Longitude = loc.Lon });
                }


                Array values = Enum.GetValues(typeof(Colors));
                Random random = new Random(DateTime.Now.Millisecond);
                Colors randomBar = (Colors)values.GetValue(random.Next(values.Length));

                lines.Add(new LineModel { LineNumber = item.Name, Stations = stations, ColorLine = randomBar.ToString() });
            }
            return Json(lines);

        }

        [HttpGet]
        [System.Web.Http.Route("api/BusStations/GetLinesForLocation")]
        public IHttpActionResult GetLinesForLocation()
        {
            var AllLines = _unitOfWork.Lines.GetAll();
            List<LineModel> lines = new List<LineModel>();


            foreach (var item in AllLines)
            {
                List<Stations> stations = new List<Stations>();
                foreach (var i in item.Stations)
                {
                    var loc = _unitOfWork.Locations.Get(i.LocationId);
                    stations.Add(new Stations { Latitude = loc.Lat, Longitude = loc.Lon });
                }

                lines.Add(new LineModel { LineNumber = item.Name, Stations = stations});
            }
            return Json(lines);

        }

        [HttpPost]
        [System.Web.Http.Route("api/BusStations/SendStationsToHub")]
        public IHttpActionResult SendStationsToHub(List<StationModel> list)
        {
            hub.AddStations(list);
            return Ok();
        }

    }
}