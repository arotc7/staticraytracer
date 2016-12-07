using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using csci342;
using OpenTK.Graphics.OpenGL;

namespace mygraphicslib
{
    public class Polyline : IEnumerable<Point2D>, ICloneable
    {
        protected List<Point2D> points;

        public List<Point2D> Points
        {
            get { return points; }
        }

        public Polyline()
        {
            points = new List<Point2D>();
        }

        public Polyline(Point2D pt) : this()
        {
            AddPoint(pt);
        }

        public Polyline(params double[] pts) : this()
        {
            AddPoints(pts);
        }

        public void AddPoint(Point2D pt)
        {
            points.Add(pt);
        }

        public void RemoveLast()
        {
            points.RemoveAt(points.Count() - 1);
        }

        public void AddPoints(params double[] pts)
        {
            // if the length of the points is odd, throw exception 
            if (pts.Length % 2 == 1)
            {
                throw new ArgumentException();
            }
            for (int i = 0; i < pts.Length; i += 2)
            {
                points.Add(new Point2D(pts[i], pts[i+1]));
            }
        }

        public virtual void Draw(PrimitiveType type)
        {
            GL.Begin(type);
            foreach (Point2D pt in points)
            {
                GL.Vertex2(pt.X, pt.Y);
            }
            GL.End();
        }

        public IEnumerator<Point2D> GetEnumerator()
        {
            return points.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) points).GetEnumerator();
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
