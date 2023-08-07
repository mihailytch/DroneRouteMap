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
    class RouteController : Geometry
    {
        public MapPainter painter { get; set; }

        public void RouteFromPolygon (Drone drone)
        {
            var polygon = painter.polygon;

            double radius = drone.radius;//getDistance(new PointLatLng(0, 0), new PointLatLng(0, drone.radius));

            double polygonarea = Area(painter.polygon),

            circlearea = Math.PI * Math.Pow(radius, 2d) // 111319;

            if (polygonarea > circlearea)
            {
                PointLatLng frompoint = painter.waypoints.Last(),
                    
                    startpoint = polygon.Points.First(),
                    
                    nextpoint = new PointLatLng(),
                   
                    prepoint = new PointLatLng(),
                    
                    testpoint = new PointLatLng();

                List<PointLatLng> rotatedpolygon = new List<PointLatLng>(),
                    
                    innerpolygon = new List<PointLatLng>();

                int i = 0, startindex = 0;

                foreach (PointLatLng point in polygon.Points)
                {
                    double distance = Distance(point, frompoint);

                    double startdistance = Distance(startpoint, frompoint);

                    if (distance < startdistance)
                    {
                        startpoint = point;
                        startindex = i;
                    }
                    i++;
                }

                for (i = startindex + 1; i < polygon.Points.Count; i++)
                    rotatedpolygon.Add(polygon.Points[i]);

                for (i = 0; i <= startindex; i++)
                    rotatedpolygon.Add(polygon.Points[i]);

                int PointCount = rotatedpolygon.Count;

                prepoint = rotatedpolygon[PointCount - 2];
                startpoint = rotatedpolygon[PointCount - 1];

                for (i = 0; i <= PointCount; i++)
                {
                    if (i < PointCount)
                        nextpoint = rotatedpolygon[i];
                    else nextpoint = rotatedpolygon[0];

                    double corner = Corner(prepoint, startpoint, nextpoint);

                    double basecorner = BaseCorner(prepoint, startpoint, nextpoint);

                    double sumcorner = corner / 2d + basecorner;

                    testpoint = PointFromDistCorner(startpoint, drone.radius, sumcorner);

                    PointLatLng innerpoint = PointFromDistCorner(testpoint, drone.radius * 2, sumcorner);

                    if (!IsPointInPolygon(testpoint, polygon))
                    {
                        testpoint = PointFromDistCorner(startpoint, drone.radius, sumcorner + Math.PI);

                        innerpoint = PointFromDistCorner(testpoint, drone.radius * 2, sumcorner + Math.PI);
                    }

                    //if (IsSquareInPolygon(testpoint, drone.radius, polygon))
                        painter.AddMarker(testpoint, "green");
                    //else return;

                    if (i < PointCount)
                        innerpolygon.Add(innerpoint);

                    prepoint = startpoint;

                    startpoint = nextpoint;
                }

                painter.DelPolygon();

                painter.AddPolygon(innerpolygon);

                Console.WriteLine(Area(painter.polygon));

                Console.WriteLine(Math.PI * Math.Pow(drone.radius, 2));

                RouteFromPolygon(drone);

            }
            else
                painter.AddMarker(CenterPoint(polygon), "green");
            painter.DelPolygon();
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(painter.waypoints);
        }
    }
}
