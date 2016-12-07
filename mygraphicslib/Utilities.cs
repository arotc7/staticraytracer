using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using csci342;

namespace mygraphicslib
{
    public class Utilities
    {
        public static void SetWindow(double left, double right, double bottom, double top)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(left, right, bottom, top, -1, 1);
        }

        public static void DrawArc(double cx, double cy, double r, double startAngle, double sweep, int numPoints = 100)
        {
            Polyline poly = GenerateArc(cx, cy, r, startAngle, sweep, numPoints);
            foreach (Point2D pt in poly.Points)
            {
                GL.Vertex2(pt.X, pt.Y);
            }
        }

        public static Polyline GenerateArc(double cx, double cy, double r, double startAngle, double sweep, int numPoints = 100)
        {
            Polyline poly = new Polyline();

            double n = numPoints;
            double increment = sweep / n;
            double currentAngle = startAngle;

            for (int i = 0; i < n; i++)
            {
                double x = r * Math.Cos(DegreesToRadians(currentAngle)) + cx;
                double y = r * Math.Sin(DegreesToRadians(currentAngle)) + cy;
                poly.AddPoints(x, y);
                currentAngle += increment;
            }

            poly.AddPoints(r * Math.Cos(DegreesToRadians(startAngle + sweep)) + cx, r * Math.Sin(DegreesToRadians(startAngle + sweep)) + cy);

            return poly;
        }

        // g is between 0 and 1
        public static void DrawRoundedRectangle(double cx, double cy, double w, double h, double g, bool filled = false)
        {
            GL.Begin(filled ? PrimitiveType.Polygon : PrimitiveType.LineLoop); // TODO: check for filled

            // radius of the circles in the corners
            double radius = g * w / 2.0;

            // Get the side dimensions given g and the height and width
            double sideHeight = h-radius*2;
            double sideWidth = w-radius*2;

            int angle = 90; // default is 90

            // left side
            GL.Vertex2(cx - w/2.0, cy + sideHeight/2.0);
            GL.Vertex2(cx - w/2.0, cy - sideHeight/2.0);

            // top left corner
            DrawArc(cx - w/2.0 + radius, cy - sideHeight/2.0, radius, -180, angle);

            // top
            GL.Vertex2(cx - sideWidth/2.0, cy - h / 2.0);
            GL.Vertex2(cx + sideWidth/2.0, cy - h / 2.0);
            
            // top right corner
            DrawArc(cx + w / 2.0 - radius, cy - sideHeight/2.0, radius, -90, angle);

            // right side
            GL.Vertex2(cx + w / 2.0, cy + sideHeight/2.0);
            GL.Vertex2(cx + w / 2.0, cy - sideHeight/2.0);

            // bottom right corner
            DrawArc(cx + sideWidth/2.0, cy + h / 2.0 - radius, radius, 0, angle);

            // bottom
            GL.Vertex2(cx + sideWidth/2.0, cy + h / 2.0);
            GL.Vertex2(cx - sideWidth/2.0, cy + h / 2.0);

            // bottom left corner
            DrawArc(cx - sideWidth/2.0, cy + h / 2.0 - radius, radius, 90, angle);
            
            GL.End();
        }

        public static double DegreesToRadians(double deg)
        {
            return deg * Math.PI / 180.0;
        }

        public static double RadiansToDegrees(double rad)
        {
            return rad * 180.0 / Math.PI;
        }
    }
}
