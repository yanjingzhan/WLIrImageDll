using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using WLIrImageDll.Image;
using WLIrImageDll.Utility;

namespace WLIrImageDll.Analysis
{
    public class AnalysisRenMaiByTrunkAndShenQue
    {
        private Rectangle _trunk;
        private Rectangle _shenQue;

        private List<Rectangle> _sanJiaoList = new List<Rectangle>();
        private List<Point[]> _renmaiPointsList = new List<Point[]>();
        private List<Point> _renmaiXueweiPointsList = new List<Point>();

        //private string[] _renmaiXueweiName = { "大椎", "陶道", "身柱", "神道", "灵台", "至阳", "筋缩", "中枢", "脊中", "悬枢", "命门", "腰阳关", "腰俞", "长强" };
        private string[] _renmaiXueweiName = { "天突", "璇玑", "华盖", "紫宫", "玉堂", "膻中", "中庭", "鸠尾", "巨阙", "上脘", "中脘", "建里", "下脘", "水分", "神阙", "阴交", "气海", "石门", "关元", "中极", "曲骨" };
        //神阙以上，距离天突的长度
        private double[] _renmaiXueWeiRatios1 = { 0.0, 1.7, 4.2, 7.0, 9.7, 12.3, 14.9, 17.0, 19.2, 21.6, 23.9, 26.0, 28.2, 30.5, 33 };
        //下焦，距离神阙的长度
        private double[] _renmaiXueWeiRatios2 = { 0.0, 2.4, 3.7, 5.0, 7.3, 10.0, 12.0 };
        //任脉宽度
        private const int RenmaiHalfWith = 3;

        public Rectangle ShenQue { get => _shenQue; }
        public Rectangle Trunk { get => _trunk; }

        public AnalysisRenMaiByTrunkAndShenQue(Rectangle trunk, Rectangle sheQue)
        {
            _trunk = trunk;
            _shenQue = sheQue;
        }

        public List<Point[]> GetRemmaiPointsByTrunkAndShenQue()
        {
            List<Point[]> result = new List<Point[]>();

            Point point_tiantu = new Point(_trunk.X + _trunk.Width / 2, _trunk.Y);
            Point point_shenque = new Point(_shenQue.X + _shenQue.Width / 2, _shenQue.Y + _shenQue.Height / 2);
            Point point_qugu = new Point(_trunk.X + _trunk.Width / 2, _trunk.Bottom);

            Point[] points_renmai1 = { new Point(point_tiantu.X - RenmaiHalfWith,point_tiantu.Y), new Point(point_tiantu.X + RenmaiHalfWith, point_tiantu.Y),
            new Point(point_shenque.X + RenmaiHalfWith,point_shenque.Y),new Point(point_shenque.X - RenmaiHalfWith,point_shenque.Y)};

            Point[] points_renmai2 = { new Point(point_shenque.X - RenmaiHalfWith,point_shenque.Y), new Point(point_shenque.X + RenmaiHalfWith,point_shenque.Y),
            new Point(point_qugu.X + RenmaiHalfWith,point_qugu.Y),new Point(point_qugu.X - RenmaiHalfWith,point_qugu.Y)};

            result.Add(points_renmai1);
            result.Add(points_renmai2);

            _renmaiPointsList = result;

            return _renmaiPointsList;
        }

        public List<Point> GetRenMaiXueweiPointsByTrunkAndShenQue()
        {
            List<Point> result = new List<Point>();

            Point point_tiantu = new Point(_trunk.X + _trunk.Width / 2, _trunk.Y);
            Point point_shenque = new Point(_shenQue.X + _shenQue.Width / 2, _shenQue.Y + _shenQue.Height / 2);
            Point point_qugu = new Point(_trunk.X + _trunk.Width / 2, _trunk.Bottom);

            double realdisBetweenTiantuAndShenque = _renmaiXueWeiRatios1[_renmaiXueWeiRatios1.Length - 1];
            double realdisBetweenShenqueAndQugu = _renmaiXueWeiRatios2[_renmaiXueWeiRatios2.Length - 1];


            for (int i = 0; i < _renmaiXueWeiRatios1.Length; i++)
            {
                double x_d = (double)(point_shenque.X - point_tiantu.X) * _renmaiXueWeiRatios1[i] / realdisBetweenTiantuAndShenque;
                double y_d = (double)(point_shenque.Y - point_tiantu.Y) * _renmaiXueWeiRatios1[i] / realdisBetweenTiantuAndShenque;

                result.Add(new Point(point_tiantu.X + (int)x_d, point_tiantu.Y + (int)y_d));
            }

            //去掉神阙
            result.RemoveAt(result.Count - 1);

            double disBetweenShenqueAndQugu = CoordHelper.GetDistance(point_shenque, point_qugu);
            for (int i = 0; i < _renmaiXueWeiRatios2.Length; i++)
            {
                double x_d = (double)(point_qugu.X - point_shenque.X) * _renmaiXueWeiRatios2[i] / realdisBetweenShenqueAndQugu;
                double y_d = (double)(point_qugu.Y - point_shenque.Y) * _renmaiXueWeiRatios2[i] / realdisBetweenShenqueAndQugu;

                result.Add(new Point(point_shenque.X + (int)x_d, point_shenque.Y + (int)y_d));
            }

            for (int i = 0; i < result.Count; i++)
            {
                Debug.WriteLine(string.Format("{0}:{1},{2}", _renmaiXueweiName[i], result[i].X, result[i].Y));
            }

            _renmaiXueweiPointsList = result;
            return result;
        }

        public List<Rectangle> GetSanJiaoRectanglesByTrunkAndShenQue()
        {
            List<Rectangle> result = new List<Rectangle>();

            int shenQuePointX = ShenQue.X + ShenQue.Width / 2;
            int shenQuePointY = ShenQue.Y + ShenQue.Height / 2;
            Rectangle rectangle_xiajiao = new Rectangle(Trunk.X, shenQuePointY, Trunk.Width, Trunk.Bottom - shenQuePointY);
            Rectangle rectangle_shangjiao = new Rectangle(Trunk.X, Trunk.Y, Trunk.Width, (shenQuePointY - Trunk.Y) / 2);
            Rectangle rectangle_zhongjiao = new Rectangle(Trunk.X, Trunk.Y + (shenQuePointY - Trunk.Y) / 2, Trunk.Width, (shenQuePointY - Trunk.Y) / 2);

            result.Add(rectangle_shangjiao);
            result.Add(rectangle_zhongjiao);
            result.Add(rectangle_xiajiao);

            _sanJiaoList = result;
            return result;
        }

        public List<float> GetSanJiaoTemperature(ushort[] imageData)
        {
            List<float> result = new List<float>();

            ImageData imageData_t = new ImageData();
            imageData_t.SetImageData(imageData);

            foreach (var r in _sanJiaoList)
            {
                result.Add(imageData_t.GetAverageTemperatureByRectArea(r));
            }

            return result;
        }

        public List<float> GetSanJiaoDeltaTemperature(ushort[] imageData)
        {
            List<float> result = new List<float>();

            ImageData imageData_t = new ImageData();
            imageData_t.SetImageData(imageData);

            float trunkTemperature = imageData_t.GetAverageTemperatureByRectArea(_trunk);
            foreach (var r in _sanJiaoList)
            {
                result.Add(imageData_t.GetAverageTemperatureByRectArea(r) - trunkTemperature);
            }

            return result;
        }


        public List<float> GetRenmaiXueweiTemperature(ushort[] imageData)
        {
            List<float> result = new List<float>();

            ImageData imageData_t = new ImageData();
            imageData_t.SetImageData(imageData);

            foreach (var p in _renmaiXueweiPointsList)
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

        public List<float> GetRenmaiXueweiDeltaTemperature(ushort[] imageData)
        {
            List<float> result = new List<float>();

            ImageData imageData_t = new ImageData();
            imageData_t.SetImageData(imageData);

            float trunkTemperature = imageData_t.GetAverageTemperatureByRectArea(_trunk);
            foreach (var p in _renmaiXueweiPointsList)
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

        public float GetRenmaiTemperature(ushort[] imageData)
        {
            float result = 0.0f;

            ImageData imageData_t = new ImageData();
            imageData_t.SetImageData(imageData);


            foreach (var p in _renmaiPointsList)
            {
                result += imageData_t.GetAverageTemperatureByPolygonArea(p);
            }

            return result / _renmaiPointsList.Count;
        }

        public float GetRenmaiDeltaTemperature(ushort[] imageData)
        {
            float result = 0.0f;

            ImageData imageData_t = new ImageData();
            imageData_t.SetImageData(imageData);

            foreach (var p in _renmaiPointsList)
            {
                result += imageData_t.GetAverageTemperatureByPolygonArea(p);
            }

            result /= _renmaiPointsList.Count;

            float trunkTemperature = imageData_t.GetAverageTemperatureByRectArea(_trunk);
            return result - trunkTemperature;
        }

        public float GetTrunkTemperature(ushort[] imageData)
        {
            float result = 0.0f;

            ImageData imageData_t = new ImageData();
            imageData_t.SetImageData(imageData);

            result = imageData_t.GetAverageTemperatureByRectArea(_trunk);
            return result;
        }
    }
}
