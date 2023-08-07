using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace DroneRouteMap
{
    abstract class Geometry
    {
        public bool cross(PointLatLng a, PointLatLng b, PointLatLng c, PointLatLng d)
        {
            double k1, k2, x, y, x1 = a.Lng, y1 = a.Lat, x2 = b.Lng, y2 = b.Lat, 
                x3 = c.Lng, y3 = c.Lat, x4 = d.Lng, y4 = d.Lat;

            x = ((x1 * y2 - x2 * y1) * (x4 - x3) - (x3 * y4 - x4 * y3) * (x2 - x1)) / ((y1 - y2) * (x4 - x3) - (y3 - y4) * (x2 - x1));

            y = ((y3 - y4) * x - (x3 * y4 - x4 * y3)) / (x4 - x3);

            if (((x1 <= x) && (x2 >= x) && (x3 <= x) && (x4 >= x)) 
                || ((y1 <= y) && (y2 >= y) && (y3 <= y) && (y4 >= y)))
            {
                k1 = (x2 - x1) / (y2 - y1);
                k2 = (x4 - x3) / (y4 - y3);
                if(k1 != k2)
                return true;
            }
            return false;
        }

        public bool cross2(PointLatLng a, PointLatLng b, PointLatLng c, PointLatLng d)
        {
            PointLatLng[] points1 = new PointLatLng[2] { a, b };

            PointLatLng[] points2 = new PointLatLng[2] { c, d };

            double relativecorner = RelativeCorner(b, a),
               firstcorner = RelativeCorner(b, c),
               secondcorner = RelativeCorner(b, d);

            double firstsin = Math.Sin(firstcorner - relativecorner),
                secondsin = Math.Sin(secondcorner - relativecorner);

            if((firstsin > 0 && secondsin < 0) || (firstsin < 0 && secondsin > 0))
            {
                relativecorner = RelativeCorner(c, d);
                firstcorner = RelativeCorner(c, a); secondcorner = RelativeCorner(c, b);

                firstsin = Math.Sin(firstcorner - relativecorner);
                secondsin = Math.Sin(secondcorner - relativecorner);
                
                if ((firstsin > 0 && secondsin < 0) || (firstsin < 0 && secondsin > 0))
                    return true;
            }
            return false;
        }

        public bool IsPointInPolygon(PointLatLng a, GMapPolygon polygon)
        {
            PointLatLng testray = new PointLatLng(a.Lat + 100d, a.Lng + 100d);

            PointLatLng prepoint = polygon.Points.Last();

            int numcross = 0;

            foreach (PointLatLng point in polygon.Points)
            {
                if ((a.Lat < prepoint.Lat || a.Lat < point.Lat) && (a.Lng < prepoint.Lng || a.Lng < point.Lng))
                    if (cross2(a, testray, prepoint, point))
                    numcross++;
                prepoint = point;
            }

            if (numcross != 0 && numcross % 2 != 0)
                return true;

            return false;
        }

        public bool IsSquareInPolygon(PointLatLng a, double radius,GMapPolygon polygon)
        {
            int numin = 0;

            PointLatLng[] testpoints = new PointLatLng[4] {
                new PointLatLng(a.Lat + radius/2, a.Lng),
                new PointLatLng(a.Lat, a.Lng + radius/2),
                new PointLatLng(a.Lat, a.Lng - radius/2),
                new PointLatLng(a.Lat - radius/2, a.Lng)
            };

            foreach (PointLatLng point in testpoints)
                if (IsPointInPolygon(point, polygon))
                    numin++;

            if(numin == 4)
            return true;

            return false;
        }

        public PointLatLng PointFromDistCorner(PointLatLng a, double distance, double corner)
        {
            return new PointLatLng(a.Lat + distance * Math.Sin(corner), a.Lng + distance * Math.Cos(corner));
        }
        
        public double BaseCorner(PointLatLng a, PointLatLng b, PointLatLng c)
        {
            PointLatLng basis = new PointLatLng(b.Lat, b.Lng + 1);

            PointLatLng nextpoint = c;

            double firstcorner = RelativeCorner(b, a);
            double secondcorner = RelativeCorner(b, c);

            if (!(Math.Abs(RelativeCorner(b, a) - RelativeCorner(b, c)) > Math.PI))
            {
                if (RelativeCorner(b, a) < RelativeCorner(b, c))
                    nextpoint = a;
            }
            else if (RelativeCorner(b, a) > RelativeCorner(b, c))
                nextpoint = a;

            return RelativeCorner(b, nextpoint);
        }

        public double RelativeCorner(PointLatLng a, PointLatLng b)
        {
            PointLatLng basis = new PointLatLng(a.Lat, a.Lng + 1);

            double anticorner = 0;

                    if (basis.Lat > b.Lat)  // под базисом
                    {
                        basis.Lng -= 2;
                        anticorner = Math.PI;
                    }

            return Corner(basis, a, b) + anticorner;
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

        public double Area(GMapPolygon polygon)
        {
            var points = polygon.Points;

            PointLatLng centralpoint = CenterPoint(polygon);

            int count = points.Count(), index = 0, startindex = 0;

            double firsum = 0, secsum = 0, 
                minangle = RelativeCorner(centralpoint, points[startindex]);

            foreach (PointLatLng point in points)
            {
                if (RelativeCorner(centralpoint, point) < minangle)
                    startindex = index;
                index++;
            }

            List<PointLatLng> rotatedpoints = new List<PointLatLng>();

            for (int i = startindex; i < count; i++)
                rotatedpoints.Add(points[i]);

            for (int i = 0; i < startindex; i++)
                rotatedpoints.Add(points[i]);

            for (int i = 0; i < count - 1; i++)
            {
                firsum += points[i].Lng * points[i + 1].Lat;
                secsum += points[i].Lat * points[i + 1].Lng;
            }

            firsum += points.Last().Lng * points[0].Lat;
            secsum += points.Last().Lat * points[0].Lng;

            return Math.Abs(firsum - secsum) / 2d;
        }

        public PointLatLng CenterPoint(GMapPolygon polygon)
        {
            double lat = 0, lng = 0;

            int count = polygon.Points.Count;

            foreach(PointLatLng point in polygon.Points)
            {
                lat += point.Lat; lng += point.Lng;
            }

            return new PointLatLng(lat / count, lng / count);
        }

        double Rad(double x)
        {
            return x * Math.PI / 180;
        }

        public double getDistance(PointLatLng p1, PointLatLng p2)
        {
            var R = 6378137; // Earth’s mean radius in meter

            var dLat = Rad(p2.Lat - p1.Lat);

            var dLong = Rad(p2.Lng - p1.Lng);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(Rad(p1.Lat)) * Math.Cos(Rad(p2.Lat)) *
              Math.Sin(dLong / 2) * Math.Sin(dLong / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c; // returns the distance in meter
        }
    }
}
