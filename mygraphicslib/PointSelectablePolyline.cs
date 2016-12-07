using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using csci342;
using System.Drawing;

namespace mygraphicslib
{
    public class PointSelectablePolyline : Polyline
    {
        private Point2D _selectedPoint;
        public Point2D selectedPoint
        {
            get
            {
                return _selectedPoint;
            }

            set
            {
                if (value == null)
                {
                    _selectedPoint = null;
                    return;
                }

                for (var i = 0; i < points.Count; i++)
                {
                    if (points[i].X == value.X && points[i].Y == value.Y)
                    {
                        _selectedPoint = points[i];
                        return;
                    }
                }
                Console.WriteLine("Error!");
            }
        }
        public Color selectedPointColor = Color.Yellow;
        public Color nonSelectedPointColor = Color.Green;

        public PointSelectablePolyline() : base()
        {
        }

        public PointSelectablePolyline(Polyline p)
        {
            this.points = p.Points;
        }

        override public void Draw(PrimitiveType type)
        {
            if (selectedPoint != null)
            {
                Console.WriteLine("drawing where the selected point is good!");
            }
            GL.Begin(type);
            foreach (Point2D pt in points)
            {
                if (selectedPoint != null && pt.X == selectedPoint.X && pt.Y == selectedPoint.Y && type == PrimitiveType.Points)
                {
                    GL.Color3(selectedPointColor);
                }
                else if (selectedPoint != null && (pt.X != selectedPoint.X || pt.Y != selectedPoint.Y) && type == PrimitiveType.Points)
                {
                    GL.Color3(nonSelectedPointColor);
                }
                GL.Vertex2(pt.X, pt.Y);
            }
            GL.End();
        }

        public void DeletePoint(Point2D pt)
        {
            points.Remove(pt);
        }
    }
}
