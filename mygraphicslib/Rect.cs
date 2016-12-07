using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using csci342;

namespace mygraphicslib
{
    public class Rect
    {
        public double X;
        public double Y;
        public double Width;
        public double Height;

        public double CenterX
        {
            get
            {
                return X + Width / 2.0;
            }

            set
            {
                X = value - Width / 2.0;
            }
        }

        public double CenterY
        {
            get
            {
                return Y + Width / 2.0;
            }

            set
            {
                Y = value - Width / 2.0;
            }
        }

        public Rect(double x, double y, double width, double height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public bool PointWithin(Point2D pt)
        {
            return pt.X >= X && pt.X <= X + Width && pt.Y >= Y && pt.Y <= Y + Height;
        }

        public bool PointWithin(Point pt)
        {
            return pt.X >= X && pt.X <= X + Width && pt.Y >= Y && pt.Y <= Y + Height;
        }
    }
}
