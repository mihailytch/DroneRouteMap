using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneRouteMap
{
    class WayPoint
    {
        public double latitude;
        public double longitude;
        
        public WayPoint(double lat, double lng)
        {
            latitude = lat;
            longitude = lng;
        }
    }
}
