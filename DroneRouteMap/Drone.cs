using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneRouteMap
{
    class Drone
    {
        public double length { get; set; }

        public double width { get; set; }

        public double radius { get; set; }

        public Drone (double l, double w, double r)
        {
            length = l;
            width = w;
            radius = r;
        }

        public double GetRadiusFromMeters ()
        {
            return radius / 111319;
        }
    }
}
