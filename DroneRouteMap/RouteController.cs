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
        //List<PointLatLng> Route;

        public MapPainter painter { get; set; }

        public void RouteFromPolygon (Drone drone)
        {
            GMapPolygon polygon = painter.polygon;

            List<PointLatLng> polygon_points = polygon.Points;

            double radius = FromMeters(drone.radius);

            int count = polygon_points.Count;

            double polygon_area = Area(polygon),

            circle_area = Math.PI * Math.Pow(radius, 2d);

            if (polygon_area > circle_area * 2)
            {
                PointLatLng from_point = painter.waypoints.Last(),

                    start_point = NearestPoint(polygon_points, from_point);

                List<PointLatLng> rotated_polygon = RotatePointList(polygon_points, start_point, count - 1),

                    inner_polygon = new List<PointLatLng>(),

                    test_polygon = new List<PointLatLng>(),

                    route = new List<PointLatLng>();

                PointLatLng pre_point = rotated_polygon[count - 2],

                    next_point = new PointLatLng(),

                    test_point = new PointLatLng();

                // ставим точки у углов полигона
                foreach (PointLatLng point in rotated_polygon)
                {
                    next_point = point;

                    test_point = PointInCenterOfAngle(pre_point, start_point, next_point, radius, false);

                    PointLatLng inner_point = PointInCenterOfAngle(pre_point, start_point, next_point, radius * 2, false);

                    PointLatLng[,] lines = new PointLatLng[2, 2];

                    if (next_point == rotated_polygon.First())
                    {
                        lines = TwoParallelLinesInPolygon(pre_point, start_point, radius, polygon);

                        test_polygon.Add(lines[1, 0]); test_polygon.Add(lines[1, 1]);
                    }

                    // если угол вывернутый то ставим просто точку маршрута и внутреннюю у угла
                    if (!IsPointInPolygon(test_point, polygon))
                    {
                        test_point = PointInCenterOfAngle(pre_point, start_point, next_point, radius, true);

                        inner_point = PointInCenterOfAngle(pre_point, start_point, next_point, radius * 2, true);
                    }

                    if (IsPointInPolygon(test_point, polygon))
                    {
                        route.Add(test_point);

                        lines = TwoParallelLinesInPolygon(start_point, next_point, radius, polygon);

                        route.Add(lines[0, 0]); route.Add(lines[0, 1]);

                        test_polygon.Add(lines[1, 0]); test_polygon.Add(lines[1, 1]);

                        int test_count = test_polygon.Count;

                        PointLatLng a = test_polygon[test_count - 4],
                        b = test_polygon[test_count - 3],
                        c = test_polygon[test_count - 2],
                        d = test_polygon[test_count - 1];

                        if (cross(a, b, c, d))
                            inner_point = WhereCross(a, b, c, d);

                        if(IsPointInPolygon(inner_point, polygon))
                            inner_polygon.Add(inner_point);
                    }

                    else route.Add(start_point);

                    pre_point = start_point;

                    start_point = next_point;
                }

                foreach (PointLatLng p in route)
                    painter.AddMarker(p, "dot");

                List<PointLatLng> inner_polygon_original = new List<PointLatLng>(inner_polygon);

                /*foreach (PointLatLng p in inner_polygon_original)
                    if (!IsPointInPolygon(p, polygon))
                        inner_polygon.Remove(p);*/

                painter.DelPolygon();

                if (inner_polygon.Count < 3)
                {
                    if(!(inner_polygon.Count < 2))
                        painter.AddMarker(CenterPoint(inner_polygon), "green");
                    //else if(inner_polygon != null) painter.AddMarker(inner_polygon[0], "green");
                    return;
                }

                foreach (PointLatLng p in inner_polygon)
                {
                    painter.AddMarker(p, "red");
                }

                painter.UpdatePolygon();
                
                //RouteFromPolygon(drone);
            }
            else
            {
                painter.AddMarker(CenterPoint(polygon_points), "green");

                painter.DelPolygon();
            }
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(painter.waypoints);
        }

        public string ToText()
        {
            string text = "Points:\n{";

            int i = 1;

            foreach (PointLatLng point in painter.waypoints)
                text += "\nPoint_" + i++ + ':' + point.Lat + ',' + point.Lng;

            text += "\n}";

            return text;
        }

        List<PointLatLng> RotatePointList(List<PointLatLng> list, PointLatLng point, int index)
        {
            List<PointLatLng> rotated_polygon = new List<PointLatLng>();

            int count = list.Count, start_index = list.FindIndex(p => p == point),
                invert_index = count - index;

            for (int i = start_index + invert_index; i < count; i++)
                rotated_polygon.Add(list[i]);

            for (int i = 0; i <= start_index; i++)
                rotated_polygon.Add(list[i]);

            return rotated_polygon;
        }

        List<PointLatLng> OrginalList(List<PointLatLng> list)
        {
            List<PointLatLng> list_original = new List<PointLatLng>();

            foreach (PointLatLng check_point in list)
            {
                bool exist = false;

                foreach (PointLatLng point in list_original)
                    if (check_point == point) exist = true;

                if (!exist)
                    list_original.Add(check_point);
            }
            return list_original;
        }
    }
}
