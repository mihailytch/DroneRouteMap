using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;

namespace DroneRouteMap
{
    class Geometry
    {
        public double basecorner { get; set; }

        public PointLatLng basis = new PointLatLng(0, 0);

        public double CornerToBase(PointLatLng a, PointLatLng b)
        {
            return Corner(new PointLatLng(0, 1), a, b);
        }

        public PointLatLng PointToBase(PointLatLng a)
        {
            return new PointLatLng(a.Lng * Math.Sin(basecorner) + a.Lat * Math.Cos(basecorner), 
                a.Lng * Math.Cos(basecorner) - a.Lat * Math.Sin(basecorner));
        }

        public double Distance(PointLatLng a, PointLatLng b)
        {
            return Math.Pow(Math.Pow(b.Lng - a.Lng, 2d) + Math.Pow(b.Lat - a.Lat, 2d), 1d / 2d);
        }

        public double Distance(PointLatLng ab)
        {
            return Math.Pow(Math.Pow(ab.Lng, 2d) + Math.Pow(ab.Lat, 2d), 1d / 2d);
        }

        public PointLatLng Vector(PointLatLng a, PointLatLng b)
        {
            return new PointLatLng(b.Lng - a.Lng, b.Lat - a.Lat);
        }

        public double Corner(PointLatLng a, PointLatLng b, PointLatLng c)
        {
            PointLatLng ba = Vector(b, a);

            PointLatLng bc = Vector(b, c);

            double scalyar = ba.Lng * bc.Lng + ba.Lat * bc.Lat;

            return Math.Acos(scalyar / (Distance(ba) * Distance(bc)));
        }

        public PointLatLng MidPoint(PointLatLng a, PointLatLng b, PointLatLng c)
        {
            return new PointLatLng ((a.Lat + b.Lat + c.Lat) / 3, (a.Lng + b.Lng + c.Lng) / 3);
        }

        public PointLatLng RotatePoint(PointLatLng a, double corner)
        {
            return new PointLatLng(a.Lng * Math.Sin(corner), a.Lng * Math.Cos(corner));
        }
    }
}
