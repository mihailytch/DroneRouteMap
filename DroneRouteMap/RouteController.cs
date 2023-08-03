using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace DroneRouteMap
{
    class RouteController
    {

        public MapPainter painter { get; set; }

        List<PointLatLng> route = new List<PointLatLng>();

        Geometry geometry = new Geometry();
        
        public void AddWayPoint (double lat, double lng)
        {
            route.Add(new PointLatLng(lat, lng));
        }

        public void DelWayPoint (int index)
        {
            route.RemoveAt(index);
        }

        public void Clear()
        {
            route.Clear();
        }

        public void ChgPointPosition(int oldindex, int newindex)
        {

        }

        public void RouteFromPolygon (GMapPolygon polygon, Drone drone)
        {
            PointLatLng frompoint = route.Last();

            PointLatLng startpoint = polygon.Points.First();

            int i = 0, startindex = 0;

            foreach (PointLatLng point in polygon.Points)
            {
                double distance = geometry.Distance(point, frompoint);

                double startdistance = geometry.Distance(startpoint, frompoint);

                if (distance < startdistance)
                {
                    startpoint = point;
                    startindex = i;
                }
                i++;
            }

            PointLatLng nextpoint = new PointLatLng(), prepoint = new PointLatLng();

            if (startpoint == polygon.Points.Last())
            {
                nextpoint = polygon.Points.First();

                prepoint = polygon.Points[startindex - 1];
            }
            else if (startpoint == polygon.Points.First())
            {
                prepoint = polygon.Points.Last();

                nextpoint = polygon.Points[startindex + 1];
            }
            else
            {
                prepoint = polygon.Points[startindex - 1];

                nextpoint = polygon.Points[startindex + 1];
            }

            double corner = geometry.Corner(prepoint, startpoint, nextpoint);

            Console.WriteLine(corner / Math.PI * 180);

            geometry.basecorner = geometry.CornerToBase(startpoint, nextpoint);

            Console.WriteLine(geometry.basecorner / Math.PI * 180);

            startpoint.Lat = startpoint.Lat + drone.radius * Math.Cos(corner);

            startpoint.Lng = startpoint.Lng + drone.radius * Math.Sin(corner);

            painter.AddMarker(startpoint.Lat, startpoint.Lng, "cross");

            startpoint = geometry.PointToBase(startpoint);

            painter.AddMarker(startpoint.Lat, startpoint.Lng, "green");

            AddWayPoint(startpoint.Lat, startpoint.Lng);
        }

        public GMapPolygon GetPolygon()
        {
            return new GMapPolygon(route, "lox");
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(route);
        }

    }
}
