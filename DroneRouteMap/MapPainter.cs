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

                        waypoints.Add(point);

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

            if (overlay.Markers.Last() != marker && marker is GMapMarkerGoogleGreen)
                DelLine(overlay.Markers[index + 1].Position);

            if (marker is GMapMarkerGoogleGreen)
                waypoints.Remove(marker.Position);

            if (marker is GMapMarkerGoogleRed)
                crosses.Remove(marker);

            overlay.Markers.Remove(marker);

            int i = 0;

            foreach (GMapMarker point in overlay.Markers)
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

        void AddLine(PointLatLng point1, PointLatLng point2)
        {
            List<PointLatLng> points = new List<PointLatLng>();

            points.Add(point1); points.Add(point2);

            GMapPolygon newline = new GMapPolygon(points, "line");

            newline.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));

            newline.Stroke = new Pen(Color.Red, 1);

            overlay.Polygons.Add(newline);

            lines.Add(newline);
        }

        bool DelLine(PointLatLng endpoint)
        {
            GMapPolygon line = null;

            if (lines.Where(polygon => polygon.Points[1] == endpoint).Count() > 0) 
                line = lines.Where(polygon => polygon.Points[1] == endpoint).First();

            if (line != null)
            {
                overlay.Polygons.Remove(line);

                lines.Remove(line);

                return true;
            }

            return false;
        }
    }
}
