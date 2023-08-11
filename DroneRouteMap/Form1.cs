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
            gMapControl1.MaxZoom = 22;

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
            gMapControl1.Zoom = 20;

            gMapControl1.Position = new GMap.NET.PointLatLng(55.2173929035307d, 82.8487533330917d);

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
            /*
            gMapControl1.Position = new GMap.NET.PointLatLng(0.5d, 1);

            gMapControl1.Zoom = 8;

            painter.AddMarker(new GMap.NET.PointLatLng(0, 2), "green");

            painter.AddMarker(new GMap.NET.PointLatLng(1, 0), "red");
            painter.AddMarker(new GMap.NET.PointLatLng(0.25, 0.25), "red");
            painter.AddMarker(new GMap.NET.PointLatLng(0, 1), "red");

            painter.UpdatePolygon();

            route.RouteFromPolygon(new Drone(5, 5, 1000d / 111319d));

            painter.AddMarker(new GMap.NET.PointLatLng(1, 0), "red");
            painter.AddMarker(new GMap.NET.PointLatLng(0.25, 0.25), "red");
            painter.AddMarker(new GMap.NET.PointLatLng(0, 1), "red");

            painter.UpdatePolygon();
            /*
            List<GMap.NET.PointLatLng> a = new List<GMap.NET.PointLatLng>
            {
                new GMap.NET.PointLatLng(1, 1), 
                new GMap.NET.PointLatLng(0, 1), 
                new GMap.NET.PointLatLng(1, 2),
                new GMap.NET.PointLatLng(1, 0)
            };

            foreach(GMap.NET.PointLatLng point in a)
            {
                painter.AddMarker(point, "green");
            }
                Console.WriteLine(route.WhereCross(a[0], a[1], a[2], a[3]));
*/
        }

        private void gMapControl1_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            painter.DelMarker(item);
            painter.UpdatePolygon();
        }

        private void gMapControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
        }

        private void buttonRouteDot_Click(object sender, EventArgs e)
        {
            label1.Text = route.ToJson();
        }

        private void buttonRoutePol_Click(object sender, EventArgs e)
        {
            if(painter.waypoints.Count > 0 && painter.polygon != null)
                route.RouteFromPolygon(new Drone(5, 5, Double.Parse(textBoxDronRadius.Text) / 111319));
        }

        private void gMapControl1_MouseClick(object sender, MouseEventArgs e)
        {
            double lat = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lat;
            double lng = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lng;

            GMap.NET.PointLatLng point = new GMap.NET.PointLatLng(lat, lng);

            if (e.Button == MouseButtons.Right)
            switch (regime)
            {
                case "point":
                    {
                            painter.AddMarker(point, "green"); 
                        break;
                    }
                case "polygon":
                    {
                            painter.AddMarker(point, "red");

                            painter.UpdatePolygon();

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
