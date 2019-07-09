using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;
using WebApp.Models;

namespace WebApp.Hubs
{
    [HubName("notificationsBus")]
    public class BusLocationHub : Hub
    {
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<BusLocationHub>();



        private static List<StationModel> stations = new List<StationModel>();

        private static Timer timer = new Timer();
        private static int cnt = 0;

        public BusLocationHub()
        {
        }

        public void TimeServerUpdates()
        {
            
            if(timer.Interval !=4000)
            {
                timer.Interval = 4000;
                //timer.Start();
                timer.Elapsed += OnTimedEvent;

              
            }
            timer.Enabled = true;

        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
#if DEBUG 
            (source as Timer).Enabled = false;
#endif
            //GetTime();
            if (stations != null)
            {

                if (cnt >= stations.Count)
                {
                    cnt = 0;
                }
                double[] niz = { stations[cnt].latitude, stations[cnt].longitude };
                Clients.All.setRealTime(niz);
                cnt++;
            }
            else
            {
                double[] niz = { 0, 0 };
            }
         
#if DEBUG
            (source as Timer).Enabled = true;
#endif
        }

        public void GetTime()
        {
            if (stations.Count > 0)
            {
                if (cnt >= stations.Count)
                {
                    cnt = 0;
                }
                double[] niz = { stations[cnt].latitude, stations[cnt].longitude };
                //Clients.All.setRealTime(niz);
                cnt++;
            }
        }

        public void StopTimeServerUpdates()
        {
            timer.Stop();
            stations = null;
        }

        public void AddStations(List<StationModel> stationsBM)
        {
            stations = new List<StationModel>();
            stations = stationsBM;
        }
    }
}