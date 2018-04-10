using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace WLIrImageDll.Image
{
    public class ImageData
    {
        //红外图像的图像格式属性
        public const int imageWidth = 288;
        public const int imageHeight = 384;
        public const int imageDataShortLength = 110592;
        public const int imageDataByteLength = 221184;

        //public const int imageWidth = 240;
        //public const int imageHeight = 320;
        //public const int imageDataShortLength = 76800;
        //public const int imageDataByteLength = 153600;

        public const int imageLeftRemain = 1;
        public const int imageTopRemain = 1;
        public const int imageRightRemain = 1;
        public const int imageBottomRemain = 1;
        public const int imageHorStart = imageLeftRemain;
        public const int imageHorEnd = imageWidth - imageLeftRemain - imageRightRemain;
        public const int imageVerStart = imageTopRemain;
        public const int imageVerEnd = imageHeight - imageTopRemain - imageRightRemain;
        public const int imageShowWidth = imageWidth - imageLeftRemain - imageRightRemain;
        public const int imageShowHeight = imageHeight - imageTopRemain - imageBottomRemain;

        private ushort[] _imageData = new ushort[imageDataShortLength];

        public void SetImageData(ushort[] imageData)
        {
            Array.Copy(imageData, _imageData, imageDataShortLength);
        }

        //通过位置得到温度值
        public float GetTemperatureFromPixel(Point pt)
        {
            ushort value = _imageData[pt.Y * ImageData.imageWidth + pt.X];
            return GetTemperatureFromGrayValue(value);
        }

        //通过原始灰度值得到温度值
        public float GetTemperatureFromGrayValue(ushort sValue)
        {
            return mmmd.GetTemperatureByGrayValue(sValue, _imageData);
        }

        //得到矩形范围内的平均温度
        public float GetAverageTemperatureByRectArea(Rectangle rc)
        {
            float sum = 0.0f;
            int iCount = 0;

            for (int row = rc.Top; row <= rc.Bottom; row++)
            {
                for (int col = rc.Left; col <= rc.Right; col++)
                {
                    sum += GetTemperatureFromGrayValue(_imageData[row * ImageData.imageWidth + col]);
                    iCount++;
                }
            }

            float sValue = 0.0f;
            if (iCount > 0)
            {
                sValue = (sum / (float)iCount);
            }

            return sValue;
        }

        //得到椭圆范围内的平均温度
        public float GetAverageTemperatureByEllipseArea(Rectangle rc)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(rc);
            Region region = new Region(path);

            Point point = new Point();

            float sum = 0.0f;
            int iCount = 0;

            for (int row = rc.Top; row <= rc.Bottom; row++)
            {
                for (int col = rc.Left; col <= rc.Right; col++)
                {
                    point.X = col;
                    point.Y = row;
                    if (region.IsVisible(point))
                    {
                        sum += GetTemperatureFromGrayValue(_imageData[row * ImageData.imageWidth + col]);
                        iCount++;
                    }
                }
            }

            float sValue = 0.0f;
            if (iCount > 0)
            {
                sValue = (sum / (float)iCount);
            }

            return sValue;
        }

        //得到矩形范围内的平均温度
        public float GetAverageTemperatureByPolygonArea(Point[] pointArray)
        {
            if (0 == pointArray.Length) return 0;  //后加返回项

            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(pointArray);
            Region region = new Region(path);

            Point point = new Point();
            float sum = 0.0f;
            int iCount = 0;

            Rectangle rc = Rectangle.Ceiling(path.GetBounds());
            for (int row = rc.Top; row <= rc.Bottom; row++)
            {
                for (int col = rc.Left; col <= rc.Right; col++)
                {
                    point.X = col;
                    point.Y = row;
                    if (true == region.IsVisible(point))
                    {
                        sum += GetTemperatureFromGrayValue(_imageData[row * ImageData.imageWidth + col]);
                        iCount++;
                    }
                }
            }

            float sValue = 0;
            if (iCount > 0)
            {
                sValue = (sum / (float)iCount);
            }

            return sValue;
        }
    }
}
