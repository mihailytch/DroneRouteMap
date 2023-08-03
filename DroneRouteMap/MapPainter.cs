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

        List<GMapPolygon> polygons = new List<GMapPolygon>();

        //GMapMarker forpoints = new GMapMarkerGoogleGreen(); 

        int numpoints = 0;
        int numpolygons = 0;


        public void AddMarker(double lat, double lng, string color)
        {
            PointLatLng point = new PointLatLng(lat, lng);

            switch (color)
            {
                case "red":
                    {
                        overlay.Markers.Add(new GMapMarkerGoogleRed(point));
                        break;
                    }
                case "green":
                    {
                        overlay.Markers.Add(new GMapMarkerGoogleGreen(point));
                        
                        var newmarker = overlay.Markers.Last();

                        newmarker.ToolTip = new GMap.NET.WindowsForms.ToolTips.GMapRoundedToolTip(newmarker);

                        newmarker.ToolTipText = numpoints.ToString();

                        newmarker.ToolTipMode = MarkerTooltipMode.Always;

                        if (overlay.Markers.Count() > 1)
                        {
                            var premarker = overlay.Markers[overlay.Markers.Count() - 2];

                            PointLatLng prepoint = new PointLatLng(premarker.Position.Lat, premarker.Position.Lng);

                            AddLine(prepoint, point);
                        }
                        numpoints++;
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

            if (DelLine(marker.Position) && overlay.Markers.Last() != marker && overlay.Markers.First() != marker)
                AddLine(overlay.Markers[index-1].Position, overlay.Markers[index+1].Position);

            if (overlay.Markers.Last() != marker)
                DelLine(overlay.Markers[index + 1].Position);

            if (marker is GMapMarkerGoogleGreen)
                numpoints--;

            overlay.Markers.Remove(marker);

            int i = 0;

            foreach (GMapMarker point in overlay.Markers)
            {
                point.ToolTipText = i.ToString();
                i++;
            }
        }

        public void AddPolygon()
        {
            List<PointLatLng> points = new List<PointLatLng>();

            List<GMapMarker> crosses = new List<GMapMarker>();

            int i = 0;

            foreach (var point in overlay.Markers)
            {
                if (point is GMapMarkerGoogleRed)
                {
                    points.Add(point.Position);

                    crosses.Add(point);
                }
                i++;
            }

            foreach (GMapMarker cross in crosses)
                overlay.Markers.Remove(cross);

            overlay.Polygons.Add(new GMapPolygon(points, "polygon"));
            
            numpolygons++;

            /*
            centralpoint = new PointLatLng();

            foreach(PointLatLng point in points)
            {
                centralpoint.Lat += point.Lat;

                centralpoint.Lng += point.Lng;
            }

            centralpoint.Lat /= points.Count();
            centralpoint.Lng /= points.Count();

            AddMarker(centralpoint.Lat, centralpoint.Lng, "green");
            */
        }

        void AddLine(PointLatLng point1, PointLatLng point2)
        {
            List<PointLatLng> points = new List<PointLatLng>();

            points.Add(point1); points.Add(point2);

            GMapPolygon newline = new GMapPolygon(points, "line");

            newline.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));

            newline.Stroke = new Pen(Color.Red, 1);

            polygons.Add(newline);

            overlay.Polygons.Add(newline);
        }

        bool DelLine(PointLatLng endpoint)
        {
            GMapPolygon line = null;

            if (overlay.Polygons.Where(polygon => polygon.Points[1] == endpoint).Count() > 0) 
                line = overlay.Polygons.Where(polygon => polygon.Points[1] == endpoint).First();

            if (line != null)
            {
                overlay.Polygons.Remove(line);
                return true;
            }

            return false;
        }
    }
}
