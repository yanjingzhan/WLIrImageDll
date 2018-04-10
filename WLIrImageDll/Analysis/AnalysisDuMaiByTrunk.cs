using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using WLIrImageDll.Image;

namespace WLIrImageDll.Analysis
{
    public class AnalysisDuMaiByTrunk
    {
        private Rectangle _trunk;

        private List<Point[]> _dumaiPointsList = new List<Point[]>();
        private List<Point> _dumaiXueweiPointsList = new List<Point>();

        private double[] _dumaiXueWeiRatios = { 0.0, 3.2, 9.7, 18, 22, 25.7, 34, 37.2, 40.4, 46, 49.3, 56.2, 71.9, 76 };
        //督脉宽度
        private const int DumaiHalfWith = 3;

        public Rectangle Trunk { get => _trunk; }

        public AnalysisDuMaiByTrunk(Rectangle trunk)
        {
            _trunk = trunk;
        }

        public List<Point> GetDumaiXueweiPointsByTrunk()
        {
            List<Point> result = new List<Point>();
            Point point_dazhui = new Point(_trunk.X + _trunk.Width / 2, _trunk.Y);
            Point point_changqiang = new Point(_trunk.X + _trunk.Width / 2, _trunk.Bottom);

            double realdisBetweenDazhuiAndChangqiang = _dumaiXueWeiRatios[_dumaiXueWeiRatios.Length - 1];
            for (int i = 0; i < _dumaiXueWeiRatios.Length; i++)
            {
                double x_d = (double)(point_changqiang.X - point_dazhui.X) * _dumaiXueWeiRatios[i] / realdisBetweenDazhuiAndChangqiang;
                double y_d = (double)(point_changqiang.Y - point_dazhui.Y) * _dumaiXueWeiRatios[i] / realdisBetweenDazhuiAndChangqiang;

                result.Add(new Point(point_dazhui.X + (int)x_d, point_dazhui.Y + (int)y_d));
            }

            _dumaiXueweiPointsList = result;

            return result;
        }

        public List<Point[]> GetDumaiPointsByTrunk()
        {
            List<Point[]> result = new List<Point[]>();
            Point point_dazhui = new Point(_trunk.X + _trunk.Width / 2, _trunk.Y);
            Point point_changqiang = new Point(_trunk.X + _trunk.Width / 2, _trunk.Bottom);

            Point[] points_dumai1 = { new Point(point_dazhui.X - DumaiHalfWith,point_dazhui.Y), new Point(point_dazhui.X + DumaiHalfWith, point_dazhui.Y),
            new Point(point_changqiang.X + DumaiHalfWith,point_changqiang.Y),new Point(point_changqiang.X - DumaiHalfWith,point_changqiang.Y)};

            result.Add(points_dumai1);

            _dumaiPointsList = result;

            return result;
        }


        public List<float> GetDumaiXueweiTemperature(ushort[] imageData)
        {
            List<float> result = new List<float>();

            ImageData imageData_t = new ImageData();
            imageData_t.SetImageData(imageData);

            foreach (var p in _dumaiXueweiPointsList)
            {
                float tt = 0.0f;
                tt += imageData_t.GetTemperatureFromPixel(new Point(p.X - 1, p.Y - 1));
                tt += imageData_t.GetTemperatureFromPixel(new Point(p.X - 1, p.Y));
                tt += imageData_t.GetTemperatureFromPixel(new Point(p.X - 1, p.Y + 1));
                tt += imageData_t.GetTemperatureFromPixel(new Point(p.X, p.Y - 1));
                tt += imageData_t.GetTemperatureFromPixel(new Point(p.X, p.Y));
                tt += imageData_t.GetTemperatureFromPixel(new Point(p.X, p.Y + 1));
                tt += imageData_t.GetTemperatureFromPixel(new Point(p.X + 1, p.Y - 1));
                tt += imageData_t.GetTemperatureFromPixel(new Point(p.X + 1, p.Y));
                tt += imageData_t.GetTemperatureFromPixel(new Point(p.X + 1, p.Y + 1));

                result.Add(tt / 9);
            }

            return result;
        }

        public List<float> GetDumaiXueweiDeltaTemperature(ushort[] imageData)
        {
            List<float> result = new List<float>();

            ImageData imageData_t = new ImageData();
            imageData_t.SetImageData(imageData);

            float trunkTemperature = imageData_t.GetAverageTemperatureByRectArea(_trunk);
            foreach (var p in _dumaiXueweiPointsList)
            {
                float tt = 0.0f;
                tt += imageData_t.GetTemperatureFromPixel(new Point(p.X - 1, p.Y - 1));
                tt += imageData_t.GetTemperatureFromPixel(new Point(p.X - 1, p.Y));
                tt += imageData_t.GetTemperatureFromPixel(new Point(p.X - 1, p.Y + 1));
                tt += imageData_t.GetTemperatureFromPixel(new Point(p.X, p.Y - 1));
                tt += imageData_t.GetTemperatureFromPixel(new Point(p.X, p.Y));
                tt += imageData_t.GetTemperatureFromPixel(new Point(p.X, p.Y + 1));
                tt += imageData_t.GetTemperatureFromPixel(new Point(p.X + 1, p.Y - 1));
                tt += imageData_t.GetTemperatureFromPixel(new Point(p.X + 1, p.Y));
                tt += imageData_t.GetTemperatureFromPixel(new Point(p.X + 1, p.Y + 1));

                tt /= 9;

                result.Add(tt - trunkTemperature);
            }

            return result;
        }

        public float GetDumaiTemperature(ushort[] imageData)
        {
            float result = 0.0f;

            ImageData imageData_t = new ImageData();
            imageData_t.SetImageData(imageData);

            foreach (var p in _dumaiPointsList)
            {
                result += imageData_t.GetAverageTemperatureByPolygonArea(p);
            }

            return result / _dumaiPointsList.Count;
        }

        public float GetDumaiDeltaTemperature(ushort[] imageData)
        {
            float result = 0.0f;

            ImageData imageData_t = new ImageData();
            imageData_t.SetImageData(imageData);


            foreach (var p in _dumaiPointsList)
            {
                result += imageData_t.GetAverageTemperatureByPolygonArea(p);
            }

            result /= _dumaiPointsList.Count;

            float trunkTemperature = imageData_t.GetAverageTemperatureByRectArea(_trunk);
            return result - trunkTemperature;
        }
    }
}
