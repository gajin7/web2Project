using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Linq;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using WebApp.Models;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers
{
    public class AdminController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        private IUnitOfWork _unitOfWork;
        private DbContext _context;

        public AdminController()
        {
        }

        public AdminController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;

        }

        public AdminController(IUnitOfWork unitOfWork, DbContext context)
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

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/Admin/ChangePrice")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult ChangePrice()
        {
            var req = HttpContext.Current.Request;
            int tmp;
            if (req["type"] == "undefined" || req["type"] == "")
                return BadRequest("Please select type");
            if (req["price"] == "" || req["price"] == "undefined" || !Int32.TryParse(req["price"], out tmp))
                return BadRequest("Please enter valid price value");

            Prices price = _unitOfWork.Prices.GetAll().Where((u) => u.ticketType.ToString().Equals(req["type"].Trim())).FirstOrDefault();
            var aaa = req["version"];
            if (price.Version.ToString() != req["version"].Trim())
            {
                return BadRequest("Disounts allredy changed by someone else. Reload to get new version");
            }

            price.Version += 0.1;

            price.price = tmp;

            _unitOfWork.Prices.Update(price);
             _unitOfWork.Complete();
            
           


            return Ok("Price successfully changed");
        }

        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/Admin/GetPriceListVersions")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetPriceListVersions()
        {
            List<string> versions = new List<string>();

           List<string> prices = _unitOfWork.Prices.GetAll().Select(u => u.Version.ToString()).ToList();

            return Json(prices);
        }

        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/Admin/GetDiscountsVersions")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetDiscountsVersions()
        {
            List<string> versions = new List<string>();

            List<string> disc = _unitOfWork.Discounts.GetAll().Select(u => u.Version.ToString()).ToList();

            return Json(disc);
        }

        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/Admin/ChangeDiscount")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult ChangeDiscount()
        {
            var req = HttpContext.Current.Request;
            var temp = req["type"];
            double tmp;
            if (req["type"] == "undefined" || req["type"] == "")
                return BadRequest("Please select type");
            if (req["discount"] == "" || req["discount"] == "undefined" || !Double.TryParse(req["discount"], out tmp))
                return BadRequest("Please enter valid discount value");

            Discounts discount = _unitOfWork.Discounts.GetAll().Where((u) => u.Type.ToString().Equals(req["type"].Trim())).FirstOrDefault();
            discount.Discount = tmp / 100;

            if(discount.Version.ToString() != req["version"].Trim())
            {
                return BadRequest("Disounts allredy changed by someone else. Reload to get new version");
            }

            discount.Version += 0.1;

           
                _unitOfWork.Discounts.Update(discount);
                _unitOfWork.Complete();
            
         

            return Ok("Discount successfully updated");
        }

        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/Admin/GetLines")]
        [Authorize(Roles = "Admin")]
        public List<LinesModel> GetLines()
        {
            List<LinesModel> lines = new List<LinesModel>();
            foreach (var item in _unitOfWork.Lines.GetAll())
            {
                lines.Add(new LinesModel { Line = item.Name, Version = item.Version.ToString() });
            }
          
            return lines;

        }

        [HttpPost]
       [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/Admin/GetDepatures")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetDepatures()
        {
            string mssg = "";
            var req = HttpContext.Current.Request;

            if (req["line"] == "" || req["line"] == "undefined" || req["line"] == null)
              return BadRequest("Please select line");

            if (req["type"] == "" || req["type"] == "undefined" || req["type"] == null)
               return BadRequest("Please select type");


            var lineId = _unitOfWork.Lines.GetAll().Where((u) => u.Name == req["line"].Trim()).Select((u) => u.Id).FirstOrDefault();

            var scheduleId = _unitOfWork.Schedules.GetAll().Where((u) => u.Day.ToString() == req["type"].Trim() && u.LineId == lineId).Select((u) => u.Id).FirstOrDefault();

            var version = _unitOfWork.Schedules.Get(scheduleId).Version.ToString();

            var depatures = _unitOfWork.Depatures.GetAll().Where((u) => u.ScheduleId == scheduleId).Select((u) => u.DepatureTime);

            string retVal = "";

            foreach (var item in depatures)
            {
                retVal += item + ", ";
            }

            return Json(new ScheduleModel { Depatures = retVal, Message = mssg, Version = version });

        }

        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/Admin/AddDepature")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult AddDepature()
        {

            var req = HttpContext.Current.Request;

            var r = new Regex("^(\\d\\d:\\d\\d)$");


            if (req["line"] == "" || req["line"] == "undefined")
                return BadRequest("Please select line");

            if (req["type"] == "" || req["type"] == "undefined")
                return BadRequest("Please select type");
            if (!r.IsMatch(req["depature"]))
                return BadRequest("Input in wrong format");


            var lineId = _unitOfWork.Lines.GetAll().Where((u) => u.Name == req["line"].Trim()).Select((u) => u.Id).FirstOrDefault();
            var scheduleId = _unitOfWork.Schedules.GetAll().Where((u) => u.LineId == lineId && u.Day.ToString() == req["type"].Trim()).Select((u) => u.Id).FirstOrDefault();

            var schedule = _unitOfWork.Schedules.Get(scheduleId);
            var aa = req["version"].Trim();
            if (schedule.Version.ToString() != req["version"].Trim())
            {
                return BadRequest("Schedule has changed by someone else. Please reload to get new version");
            }

            schedule.Version += 0.1;

            var depatures = _unitOfWork.Depatures.GetAll();

            if (depatures.Where((u) => u.ScheduleId == scheduleId && u.DepatureTime == req["depature"].Trim()).FirstOrDefault() != null)
            {
                return BadRequest("Defined depature allredy excist");
            }

            

            _unitOfWork.Schedules.Update(schedule);
            _unitOfWork.Complete();

            Depature dep = new Depature() { ScheduleId = scheduleId, DepatureTime = req["depature"].Trim() };

            _unitOfWork.Depatures.Add(dep);
            _unitOfWork.Complete();

            return Ok("Changes Saved");

        }

        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/Admin/RemoveDepature")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult RemoveDepature()
        {

            var req = HttpContext.Current.Request;

            var r = new Regex("^(\\d\\d:\\d\\d)$");

            if (req["line"] == "" || req["line"] == "undefined")
                return BadRequest("Please select line");

            if (req["type"] == "" || req["type"] == "undefined")
                return BadRequest("Please select type");
            if (!r.IsMatch(req["depature"]))
                return BadRequest("Input in wrong format");


            var lineId = _unitOfWork.Lines.GetAll().Where((u) => u.Name == req["line"].Trim()).Select((u) => u.Id).FirstOrDefault();
            var scheduleId = _unitOfWork.Schedules.GetAll().Where((u) => u.LineId == lineId && u.Day.ToString() == req["type"].Trim()).Select((u) => u.Id).FirstOrDefault();

            var schedule = _unitOfWork.Schedules.Get(scheduleId);
            if (schedule.Version.ToString() != req["version"].Trim())
            {
                return BadRequest("Schedule has changed by someone else. Please reload to get new version");
            }
            

            schedule.Version += 0.1;

            var depatures = _unitOfWork.Depatures.GetAll();

            var dep = depatures.Where((u) => u.ScheduleId == scheduleId && u.DepatureTime == req["depature"].Trim()).FirstOrDefault();
            if (dep == null)
            {
                return BadRequest("Defined depature not excist");
            }



            _unitOfWork.Depatures.Remove(dep);
            _unitOfWork.Complete();


            return Ok("Changes saved");

        }

        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/Admin/AddStation")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult AddStation(AddStationModel model)
        {
            var req = HttpContext.Current.Request;
            if (!ModelState.IsValid)
            {
                var mssg = ModelState.Values.Select((u) => u.Errors.Select((i) => i.ErrorMessage).FirstOrDefault()).FirstOrDefault();
                return BadRequest(mssg);
            }
            double lon, lat = new double();

            if (!Double.TryParse(model.latitude, out lat))
            {
                return BadRequest("Please check latitude");
            }

            if (!Double.TryParse(model.longitude.Trim(), out lon))
            {
                return BadRequest("Please check longitude");
            }


            Station station = new Station() { Name = model.name.Trim(), Location = new Location() { Lat = lat, Lon = lon }, Address = model.address };
            station.Version = 0;
            _unitOfWork.Stations.Add(station);
            _unitOfWork.Complete();


            return Ok("Station added");

        }

        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/Admin/DeleteStation")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteStation()
        {
            var req = HttpContext.Current.Request;
            if (req["id"] == "undefined" || req["id"] == "" || req["id"] == null)
            {
                return BadRequest("Please enter station number");
            }
            int id;
            if (!Int32.TryParse(req["id"], out id))
            {
                return BadRequest("Wrong enter, must be number");
            }


            var station = _unitOfWork.Stations.Get(id);
            
            if (station.Version.ToString() != req["version"].Trim())
            {
                return BadRequest("station changed by somebody else, please reload to get new version");
            }
            if (station == null)
            {
                return BadRequest("There is no station with entered number");
            }



            _unitOfWork.Stations.Remove(station);
            _unitOfWork.Locations.Remove(_unitOfWork.Locations.Get(station.LocationId));
            _unitOfWork.Complete();




            return Ok("Station deleted");

        }

        [HttpGet]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/Admin/GetStations")]
        [Authorize(Roles = "Admin")]
        public List<StationsModel> GetStations()
        {
            List<StationsModel> stations = new List<StationsModel>();

            foreach (var item in _unitOfWork.Stations.GetAll())
            {
                stations.Add(new StationsModel { Station = item.StationNum.ToString(), Name = item.Name, Version = item.Version.ToString() });
            }

            return stations;

        }



        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/Admin/GetStationInfo")]
        [Authorize(Roles = "Admin")]
        public StationViewModel GetStationInfo()
        {

            var req = HttpContext.Current.Request;
            if (req["id"] == "undefined" || req["id"] == "" || req["id"] == null)
            {
                return new StationViewModel
                {
                    name = "",
                    address = "",
                    longitude = "",
                    latitude = "",

                };
            }
            Station station = _unitOfWork.Stations.GetAll().Where(u => u.StationNum.ToString() == req["id"].Trim()).FirstOrDefault();


            return new StationViewModel
            {
                name = station.Name,
                address = station.Address,
                longitude = _unitOfWork.Locations.GetAll().Where((u) => u.Id == station.LocationId).Select((u) => u.Lon).FirstOrDefault().ToString(),
                latitude = _unitOfWork.Locations.GetAll().Where((u) => u.Id == station.LocationId).Select((u) => u.Lat).FirstOrDefault().ToString()

            };

        }

        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/Admin/ChangeStation")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult ChangeStation(ChangeStationModel model)
        {
            if (!ModelState.IsValid)
            {
                var mssg = ModelState.Values.Select((u) => u.Errors.Select((i) => i.ErrorMessage).FirstOrDefault()).FirstOrDefault();
                return BadRequest(mssg);
            }
            double lon, lat = new double();
            if (!Double.TryParse(model.latitude, out lat))
            {
                return BadRequest("Wrong latitude");
            }
            if (!Double.TryParse(model.longitude, out lon))
            {
                return BadRequest("Wrong longitude");
            }



            Station station = _unitOfWork.Stations.GetAll().Where(u => u.StationNum.ToString() == model.id.Trim()).FirstOrDefault();
            if (station.Version.ToString() != model.version.Trim())
            {
                return BadRequest("station changed by somebody else, please reload to get new version");
            }

            station.Name = model.name;
            station.Address = model.address;
            station.Version += 0.1;

            _unitOfWork.Stations.Update(station);
            var location = _unitOfWork.Locations.Get(station.LocationId);

            location.Lat = lat;
            location.Lon = lon;

            _unitOfWork.Locations.Update(location);
            _unitOfWork.Complete();
          

            return Ok("Changes saved");

        }

        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/Admin/AddLine")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult AddLine()
        {
            var req = HttpContext.Current.Request;

            if (req["name"] == "undefined" || req["name"] == "" || req["name"] == null)
                return BadRequest("Please enter name");
            if (req["stations"] == "undefined" || req["stations"] == "" || req["stations"] == null)
                return BadRequest("Please chose stations");




            string[] stations = req["stations"].Trim().Split(',');
            List<Station> stats = new List<Station>();
            foreach (var item in stations)
            {
                Station station = _unitOfWork.Stations.GetAll().Where((u) => u.LocationId.ToString() == item).FirstOrDefault();
                if (station != null)
                {
                    stats.Add(station);
                }
            }

            Line line = new Line() { Name = req["name"] };
            if (req["type"].Trim() == "Urban")
            {
                line.LineType = Enums.LineTypes.Urban;
            }
            else if (req["type"].Trim() == "Suburban")
            {
                line.LineType = Enums.LineTypes.Suburban;
            }

            line.Stations = stats;
            line.Version = 0;


            _unitOfWork.Lines.Add(line);
            _unitOfWork.Schedules.Add(new Schedule() { LineId = line.Id, Day = Enums.Day.WorkDay });
            _unitOfWork.Schedules.Add(new Schedule() { LineId = line.Id, Day = Enums.Day.Saturday });
            _unitOfWork.Schedules.Add(new Schedule() { LineId = line.Id, Day = Enums.Day.Sunday });
            _unitOfWork.Complete();



            return Ok("Line Added");
        }

        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/Admin/DeleteLine")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteLine()
        {
            var req = HttpContext.Current.Request;
            var temp = req["id"];
            if (req["id"] == "undefined" || req["id"] == null || req["id"] == "")
            {
                return BadRequest("Please select line");
            }

            Line line = _unitOfWork.Lines.GetAll().Where((u) => u.Name == (req["id"].Trim())).FirstOrDefault();

            if (line.Version.ToString() != req["version"].Trim())
            {
                return BadRequest("Line changed by sombody else, please reload to get new version");
            }
            _unitOfWork.Lines.Remove(line);
            _unitOfWork.Complete();

            return Ok("Line " + req["id"] + " removed");
        }


        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/Admin/RemoveLine")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult RemoveLine()
        {
            var req = HttpContext.Current.Request;

            if (req["name"] == "undefined" || req["name"] == "" || req["name"] == null)
                return BadRequest("Please enter name");
            if (req["stations"] == "undefined" || req["stations"] == "" || req["stations"] == null)
                return BadRequest("Please chose stations");




            string[] stations = req["stations"].Trim().Split(',');
            List<Station> stats = new List<Station>();
            foreach (var item in stations)
            {
                Station station = _unitOfWork.Stations.GetAll().Where((u) => u.LocationId.ToString() == item).FirstOrDefault();
                if (station != null)
                {
                    stats.Add(station);
                }
            }

            Line line = new Line() { Name = req["name"] };
            if (req["type"].Trim() == "Urban")
            {
                line.LineType = Enums.LineTypes.Urban;
            }
            else if (req["type"].Trim() == "Suburban")
            {
                line.LineType = Enums.LineTypes.Suburban;
            }

            line.Stations = stats;


            _unitOfWork.Lines.Add(line);
            _unitOfWork.Complete();



            return Ok("Line Added");
        }

        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/Admin/GetLineInfo")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetLineInfo()
        {
            var req = HttpContext.Current.Request;
            var temp = req["id"];
            if (req["id"] == "undefined" || req["id"] == null || req["id"] == "")
            {
                return BadRequest("Please select line");
            };

            Line Line = _unitOfWork.Lines.GetAll().Where((u) => u.Name == req["id"].Trim()).FirstOrDefault();
            string retVal = "";
            foreach (var item in Line.Stations)
            {
                retVal += item.StationNum + ",";
            }


            return Ok(retVal);
            
        }

        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/Admin/AddStationToLine")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult AddStationToLine()
        {
            var req = HttpContext.Current.Request;
            var temp = req["id"];
            var temp1 = req["station"];
            if (req["id"] == "undefined" || req["id"] == null || req["id"] == "")
            {
                return BadRequest("Please select line");
            };

            if (req["station"] == "undefined" || req["station"] == null || req["station"] == "")
            {
                return BadRequest("Please select line");
            };


            Line line = _unitOfWork.Lines.GetAll().Where((u) => u.Name == req["id"].Trim()).FirstOrDefault();
            if (line.Version.ToString() != req["version"].Trim())
            {
                return BadRequest("Line changed by sombody else, please reload to get new version");
            }
            
            if (line.Stations.Contains(_unitOfWork.Stations.GetAll().Where((u) => u.StationNum.ToString() == req["station"].Trim()).FirstOrDefault()))
            {
                return BadRequest("There is allredy station " + req["station"] + "in line " + req["id"]);
            }

            line.Version += 0.1;

            line.Stations.Add(_unitOfWork.Stations.GetAll().Where((u) => u.StationNum.ToString() == req["station"].Trim()).FirstOrDefault());

            try
            {
                _unitOfWork.Lines.Update(line);
                _unitOfWork.Complete();
                
            }
            catch (ChangeConflictException)
            {

                return BadRequest("You have old version of files. Please reload page.");
            }
            catch (Exception)
            {
                return BadRequest("Error.Please reload and try again.");
            }


            return Ok("Station " + req["station"] + " added");


        }


        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/Admin/DeleteStationToLine")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteStationToLine()
        {
            var req = HttpContext.Current.Request;
            var temp = req["id"];
            var temp1 = req["station"];
            if (req["id"] == "undefined" || req["id"] == null || req["id"] == "")
            {
                return BadRequest("Please select line");
            };

            if (req["station"] == "undefined" || req["station"] == null || req["station"] == "")
            {
                return BadRequest("Please select line");
            };
            


            Line line = _unitOfWork.Lines.GetAll().Where((u) => u.Name == req["id"].Trim()).FirstOrDefault();
            if (line.Version.ToString() != req["version"].Trim())
            {
                return BadRequest("Line changed by sombody else, please reload to get new version");
            }


            if (!line.Stations.Contains(_unitOfWork.Stations.GetAll().Where((u) => u.StationNum.ToString() == req["station"].Trim()).FirstOrDefault()))
            {
                return BadRequest("There is no station " + req["station"] + "in line " + req["id"]);
            }
            line.Version += 0.1;


            line.Stations.Remove(_unitOfWork.Stations.GetAll().Where((u) => u.StationNum.ToString() == req["station"].Trim()).FirstOrDefault());

            try
            {
                _unitOfWork.Lines.Update(line);
                _unitOfWork.Complete();
            }
            catch (ChangeConflictException)
            {

                return BadRequest("You have old version of files. Please reload page.");
            }
            catch (Exception)
            {
                return BadRequest("Error.Please reload and try again.");
            }




            return Ok("Station " + req["station"] + " removed");


        }








        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("api/Admin/ChangeLine")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult ChangeLine()
        {
            var req = HttpContext.Current.Request;

            if (req["name"] == "undefined" || req["name"] == "" || req["name"] == null)
                return BadRequest("Please enter name");
            if (req["stations"] == "undefined" || req["stations"] == "" || req["stations"] == null)
                return BadRequest("Please chose stations");




            string[] stations = req["stations"].Trim().Split(',');
            List<Station> stats = new List<Station>();
            foreach (var item in stations)
            {
                Station station = _unitOfWork.Stations.GetAll().Where((u) => u.LocationId.ToString() == item).FirstOrDefault();
                if (station != null)
                {
                    stats.Add(station);
                }
            }

            Line line = new Line() { Name = req["name"] };
            if (req["type"].Trim() == "Urban")
            {
                line.LineType = Enums.LineTypes.Urban;
            }
            else if (req["type"].Trim() == "Suburban")
            {
                line.LineType = Enums.LineTypes.Suburban;
            }

            line.Stations = stats;
            line.Version += 0.1;


            _unitOfWork.Lines.Add(line);
            _unitOfWork.Complete();



            return Ok("Line Added");
        }

    }
}