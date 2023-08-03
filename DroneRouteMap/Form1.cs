using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET.WindowsForms;

namespace DroneRouteMap
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        GMap.NET.WindowsForms.GMapOverlay markersOverlay;

        RouteController route;

        MapPainter painter = new MapPainter();        

        string[] regimes = new string[3] {"", "point", "polygon"};

        string regime = "point";

        private void Form1_Load(object sender, EventArgs e)
        {
            //Настройки для компонента GMap.
            gMapControl1.Bearing = 0;

            //CanDragMap - Если параметр установлен в True,
            //пользователь может перетаскивать карту 
            ///с помощью правой кнопки мыши. 
            gMapControl1.CanDragMap = true;

            //Указываем, что перетаскивание карты осуществляется 
            //с использованием левой клавишей мыши.
            //По умолчанию - правая.
            gMapControl1.DragButton = MouseButtons.Left;

            gMapControl1.GrayScaleMode = true;

            //MarkersEnabled - Если параметр установлен в True,
            //любые маркеры, заданные вручную будет показаны.
            //Если нет, они не появятся.
            gMapControl1.MarkersEnabled = true;

            //Указываем значение максимального приближения.
            gMapControl1.MaxZoom = 18;

            //Указываем значение минимального приближения.
            gMapControl1.MinZoom = 2;

            //Устанавливаем центр приближения/удаления
            //курсор мыши.
            gMapControl1.MouseWheelZoomType =
                GMap.NET.MouseWheelZoomType.MousePositionAndCenter;

            //Отказываемся от негативного режима.
            gMapControl1.NegativeMode = false;

            //Разрешаем полигоны.
            gMapControl1.PolygonsEnabled = true;

            //Разрешаем маршруты
            gMapControl1.RoutesEnabled = true;

            //Скрываем внешнюю сетку карты
            //с заголовками.
            gMapControl1.ShowTileGridLines = false;

            //Указываем, что при загрузке карты будет использоваться 
            //18ти кратное приближение.
            gMapControl1.Zoom = 5;

            //Указываем что все края элемента управления
            //закрепляются у краев содержащего его элемента
            //управления(главной формы), а их размеры изменяются 
            //соответствующим образом.
            //gMapControl1.Dock = DockStyle.Fill;

            //Указываем что будем использовать карты Google.
            gMapControl1.MapProvider =
            GMap.NET.MapProviders.GMapProviders.GoogleMap;
            GMap.NET.GMaps.Instance.Mode =
                GMap.NET.AccessMode.ServerOnly;

            //Добавляем слой маркеров
            markersOverlay = new GMap.NET.WindowsForms.GMapOverlay(gMapControl1, "marker");
            gMapControl1.Overlays.Add(markersOverlay);

            route = new RouteController();

            painter.overlay = markersOverlay;

            route.painter = painter;

            ////////////////////////////////////////////////////////////
            GMap.NET.PointLatLng a = new GMap.NET.PointLatLng(1, 1), b = new GMap.NET.PointLatLng(0, 1), c = new GMap.NET.PointLatLng(0, 2);

            painter.AddMarker(a.Lat, a.Lng, "cross");

            painter.AddMarker(b.Lat, b.Lng, "cross");

            painter.AddMarker(c.Lat, c.Lng, "cross");

            Geometry g = new Geometry();

            g.basis = new GMap.NET.PointLatLng(0, 1);

            double corner = g.Corner(a, b, c);

            Console.WriteLine((corner * 180 / Math.PI/2).ToString());

            GMap.NET.PointLatLng newpoint = g.MidPoint(a,b,c);

            painter.AddMarker(newpoint.Lat, newpoint.Lng, "green");

            for(double i = 0; i < 360*Math.PI/180; i+=0.05)
                painter.AddMarker(c.Lng * Math.Sin(corner - i), c.Lng * Math.Cos(corner-i), "green");


        }

        private void gMapControl1_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            painter.DelMarker(item);
        }

        private void gMapControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //markersOverlay
        }

        private void buttonRouteDot_Click(object sender, EventArgs e)
        {
            route.Clear();

            foreach (var marker in markersOverlay.Markers)
            {
                if(marker is GMap.NET.WindowsForms.Markers.GMapMarkerGoogleGreen)
                    route.AddWayPoint(marker.Position.Lat, marker.Position.Lng);
            }

            label1.Text = route.ToJson();
        }

        private void buttonRoutePol_Click(object sender, EventArgs e)
        {
            painter.AddPolygon();
            route.RouteFromPolygon(markersOverlay.Polygons.Last(), new Drone(5,5,0.005d));
        }

        private void gMapControl1_MouseClick(object sender, MouseEventArgs e)
        {
            double lat = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lat;
            double lng = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lng;

            if (e.Button == MouseButtons.Right)
            switch (regime)
            {
                case "point":
                    {
                            painter.AddMarker(lat, lng, "green");    
                        break;
                    }
                case "polygon":
                    {
                            painter.AddMarker(lat, lng, "red");
                        break;
                    }
                default:
                    break;
            }
              
        }

        private void radioButtonDot_CheckedChanged(object sender, EventArgs e)
        {
            regime = regimes[1];
        }

        private void radioButtonPol_CheckedChanged(object sender, EventArgs e)
        {
            regime = regimes[2];
        }

        private void gMapControl1_MouseMove(object sender, MouseEventArgs e)
        {
            labelLat.Text = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lat.ToString();

            labelLng.Text = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lng.ToString();
        }
    }
}
