using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using WLIrImageDll.Image;

namespace WLIrImageDll.Utility
{
    public class CoordHelper
    {
        public static double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(
                Math.Pow((p1.X - p2.X), 2)
                + Math.Pow((p1.Y - p2.Y), 2));
        }

        public static void KillMarginForPoint(ref Point p, int picWidth, int picHeight)
        {
            int margin = 10;

            int x = p.X;
            int y = p.Y;

            if (p.X > picWidth - margin)
                x = picWidth - margin;
            if (p.X < margin)
                x = margin;
            if (p.Y > picHeight - margin)
                y = picHeight - margin;
            if (p.Y < margin)
                y = margin;

            p = new Point(x, y);
        }

        private void KillMarginForPoint(ref Point p, int picWidth, int picHeight, int width, int height)
        {
            int margin = 10;

            int x = p.X;
            int y = p.Y;

            if (p.X > picWidth - width - margin)
                x = picWidth - width - margin;
            if (p.X < margin)
                x = margin;
            if (p.Y > picHeight - height - margin)
                y = picHeight - height - margin;
            if (p.Y < margin)
                y = margin;

            p = new Point(x, y);
        }    
    }
}
