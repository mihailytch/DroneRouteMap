using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

namespace DroneRouteMap
{
    class MapPainter
    {
        public GMapOverlay overlay { get; set; }

        public GMapPolygon polygon = null;

        public List<GMapPolygon> lines = new List<GMapPolygon>();

        public List<PointLatLng> waypoints = new List<PointLatLng>();

        List<PointLatLng> dots = new List<PointLatLng>();

        List<GMapMarker> markers = new List<GMapMarker>();

        public List<GMapMarker> crosses = new List<GMapMarker>();

        public void AddMarker(PointLatLng point, string color)
        {
            switch (color)
            {
                case "red":
                    {
                        overlay.Markers.Add(new GMapMarkerGoogleRed(point));

                        crosses.Add(overlay.Markers.Last());

                        break;
                    }
                case "green":
                    {
                        overlay.Markers.Add(new GMapMarkerGoogleGreen(point));
                        
                        var newmarker = overlay.Markers.Last();

                        newmarker.ToolTip = new GMap.NET.WindowsForms.ToolTips.GMapRoundedToolTip(newmarker);

                        newmarker.ToolTipText = (waypoints.Count).ToString();

                        newmarker.ToolTipMode = MarkerTooltipMode.OnMouseOver;

                        if (waypoints.Count() > 0)
                        {
                            PointLatLng prepoint = waypoints.Last();

                            AddLine(prepoint, point);
                        }

                        markers.Add(newmarker);

                        waypoints.Add(point);

                        break;
                    }
                case "dot":
                    {
                        double mindouble = 0.000001d;

                        PointLatLng a = new PointLatLng(point.Lat + mindouble, point.Lng + mindouble),
                            b = new PointLatLng(point.Lat + mindouble, point.Lng - mindouble),
                            c = new PointLatLng(point.Lat - mindouble, point.Lng - mindouble),
                            d = new PointLatLng(point.Lat - mindouble, point.Lng + mindouble);

                        List<PointLatLng> shape = new List<PointLatLng>() {a, b, c, d};

                        GMapPolygon dot = new GMapPolygon(shape, "dot");

                        dot.Fill = new SolidBrush(Color.FromArgb(100, Color.Green));

                        dot.Stroke = new Pen(Color.Green, 1);

                        overlay.Polygons.Add(dot);

                        if (waypoints.Count() > 0)
                        {
                            PointLatLng prepoint = waypoints.Last();

                            AddLine(prepoint, point);
                        }
                        waypoints.Add(point);

                        dots.Add(point);

                        break;
                    }
                default:
                    {
                        overlay.Markers.Add(new GMapMarkerCross(point));

                        break;
                    }
            }
        }

        public void DelMarker(GMapMarker marker)
        {
            int index = overlay.Markers.IndexOf(marker);

            if (DelLine(marker) && overlay.Markers.Last() != marker && overlay.Markers.First() != marker)
                AddLine(overlay.Markers[index-1].Position, overlay.Markers[index+1].Position);

            if (overlay.Markers.Last() != marker && marker is GMapMarkerGoogleGreen)
                DelLine(overlay.Markers[index + 1]);

            if (marker is GMapMarkerGoogleGreen)
                waypoints.Remove(marker.Position);

            if (marker is GMapMarkerGoogleRed)
                crosses.Remove(marker);

            overlay.Markers.Remove(marker);

            int i = 0;

            foreach (GMapMarker point in markers)
            {
                point.ToolTipText = i.ToString();

                i++;
            }
        }

        public void AddPolygon(List<PointLatLng> points)
        {
            polygon = new GMapPolygon(points, "polygon");

            overlay.Polygons.Add(polygon);
        }

        public void DelPolygon()
        {
            overlay.Polygons.Remove(polygon);

            foreach (GMapMarker cross in crosses)
                overlay.Markers.Remove(cross);

            crosses.Clear();

            polygon = null;
        }

        public void UpdatePolygon()
        {
            if (overlay.Markers.Where(
                                marker => marker is GMapMarkerGoogleRed).Count() > 2)
            {
                if (polygon != null)
                {
                    overlay.Polygons.Remove(polygon);
                    polygon = null;
                }

                List<PointLatLng> points = new List<PointLatLng>();

                foreach (GMapMarkerGoogleRed cross in crosses)
                {
                    points.Add(cross.Position);
                }
                AddPolygon(points);

            }
            else if (polygon != null)
            {
                overlay.Polygons.Remove(polygon);
                polygon = null;
            }
        }

        public void AddLine(PointLatLng point1, PointLatLng point2)
        {
            List<PointLatLng> points = new List<PointLatLng>();

            points.Add(point1); points.Add(point2);

            GMapPolygon newline = new GMapPolygon(points, "line");

            newline.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));

            newline.Stroke = new Pen(Color.Red, 1);

            overlay.Polygons.Add(newline);

            lines.Add(newline);
        }

        bool DelLine(GMapMarker end_marker)
        {
            PointLatLng end_point = end_marker.Position;

            GMapPolygon line = lines.Where(polygon => polygon.Points[1] == end_point).Count() > 0

                ? lines.Where(polygon => polygon.Points[1] == end_point).First() : null;

            if (line != null)
            {
                overlay.Polygons.Remove(line);

                lines.Remove(line);

                return true;
            }
            return false;
        }

        public void AddCircle(PointLatLng a, double radius)
        {
            List<PointLatLng> points = new List<PointLatLng>();

            double seg = Math.PI * 2 / 10;

            for (int i = 0; i < 10; i++)
            {
                double theta = seg * i,
                x = a.Lng + Math.Cos(theta) * radius,
                y = a.Lat + Math.Sin(theta) * radius;

                PointLatLng point = new PointLatLng(y, x);

                points.Add(point);
            }
            GMapPolygon polygon = new GMapPolygon(points, "circle");

            overlay.Polygons.Add(polygon);
        }

        bool DelLine2(GMapMarker end_marker)
        {
            PointLatLng end_point = end_marker.Position;

            GMapPolygon line = lines.Where(polygon => polygon.Points[1] == end_point).Count() > 0

                ? lines.Where(polygon => polygon.Points[1] == end_point).First() : null;

            if (line != null)
            {
                PointLatLng start_point = line.Points[1];

                GMapMarker start_marker = markers.Where(marker => marker.Position == start_point).Count() > 0

                    ? markers.Where(marker => marker.Position == start_point).First() : null;

                int index = waypoints.IndexOf(start_point);

                PointLatLng pre_point = markers[markers.IndexOf(start_marker) - 1].Position;

                int pre_index = waypoints.IndexOf(pre_point);

                if (waypoints[index - 1] != pre_point)
                {
                    for (int i = index; i > pre_index; i--)
                    {
                        PointLatLng point = waypoints[i];

                        GMapPolygon this_line = lines.Where(polygon => polygon.Points[1] == point).Count() > 0

                    ? lines.Where(polygon => polygon.Points[1] == point).First() : null;

                        overlay.Polygons.Remove(this_line);

                        lines.Remove(this_line);
                    }
                }
                else
                {
                    overlay.Polygons.Remove(line);

                    lines.Remove(line);
                }
                return true;
            }

            return false;
        }
    }
}
