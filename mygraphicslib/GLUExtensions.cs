using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using csci342;
using OpenTK.Graphics.OpenGL;

namespace mygraphicslib
{
    public static class GLUExtensions
    {

        public static void DrawCube(this GLU glu, bool solid = false)
        {
            var vertexType = solid ? PrimitiveType.Polygon : PrimitiveType.LineLoop;

            //each side starts with the "bottom-left" vertex, relative
            //so bottom-left, top-left, top-right, bottom-right

            //front side
            GL.Begin(vertexType);
            {
                GL.Vertex3(-.5, -.5, .5);
                GL.Vertex3(-.5, .5, .5);
                GL.Vertex3(.5, .5, .5);
                GL.Vertex3(.5, -.5, .5);
            }
            GL.End();

            //back side
            GL.Begin(vertexType);
            {
                GL.Vertex3(-.5, -.5, -.5);
                GL.Vertex3(-.5, .5, -.5);
                GL.Vertex3(.5, .5, -.5);
                GL.Vertex3(.5, -.5, -.5);
            }
            GL.End();

            //top side
            GL.Begin(vertexType);
            {
                GL.Vertex3(-.5, .5, .5);
                GL.Vertex3(-.5, .5, -.5);
                GL.Vertex3(.5, .5, -.5);
                GL.Vertex3(.5, .5, .5);
            }
            GL.End();

            //bottom side
            GL.Begin(vertexType);
            {
                GL.Vertex3(-.5, -.5, .5);
                GL.Vertex3(-.5, -.5, -.5);
                GL.Vertex3(.5, -.5, -.5);
                GL.Vertex3(.5, -.5, .5);
            }
            GL.End();

            //right side
            GL.Begin(vertexType);
            {
                GL.Vertex3(.5, .5, .5);
                GL.Vertex3(.5, .5, -.5);
                GL.Vertex3(.5, -.5, -.5);
                GL.Vertex3(.5, -.5, .5);
            }
            GL.End();

            GL.Begin(vertexType);
            {
                GL.Vertex3(-.5, .5, .5);
                GL.Vertex3(-.5, .5, -.5);
                GL.Vertex3(-.5, -.5, -.5);
                GL.Vertex3(-.5, -.5, .5);
            }
            GL.End();
        }

        public static void WireSphere(this GLU glu, double radius, int rows, int columns)
        {
            Point3D[,] pts = GenerateSpherePoints(radius, rows, columns);
            GL.Begin(PrimitiveType.LineLoop);
            {
                GL.Vertex3(0, radius, 0);
                for (int i = 0; i < rows - 1; i++)
                {
                    for (int j = 0; j < columns - 1; j++)
                    {
                        //if (j == columns - 1) continue;
                        Point3D pt = pts[i, j];
                        GL.Vertex3(pt.X, pt.Y, pt.Z);
                    }
                }
            }
            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            {
                for (int i = rows - 1; i > 0; i--)
                {
                    for (int j = columns - 1; j > 0; j--)
                    {
                        //if (j == columns - 1) continue;
                        Point3D pt = pts[i, j];
                        GL.Vertex3(pt.X, -pt.Y, pt.Z);
                    }
                }
                GL.Vertex3(0, -radius, 0);
            }
            GL.End();
        }

        //vertical divisions are rows, horizontal divisions are columns
        public static void SolidSphere(this GLU glu, double radius, int rows, int columns)
        {
            Point3D[,] pts = GenerateSpherePoints(radius, rows, columns);
            GL.Begin(PrimitiveType.TriangleFan);
            {
                GL.Normal3(0, radius, 0);
                GL.Vertex3(0, radius, 0);
                for (int i = 0; i < rows - 1; i++)
                {
                    for (int j = 0; j < columns - 1; j++)
                    {
                        //if (j == columns - 1) continue;
                        Point3D pt = pts[i, j];
                        GL.Normal3(pt.X, pt.Y, pt.Z);
                        GL.Vertex3(pt.X, pt.Y, pt.Z);
                    }
                }
            }
            GL.End();

            GL.Begin(PrimitiveType.QuadStrip);
            {
                for (int i = 0; i < rows - 1; i++)
                {
                    for (int j = 0; j < columns - 1; j++)
                    {
                        Point3D pt = pts[i, j];
                        GL.Normal3(pt.X, pt.Y, pt.Z);
                        GL.Vertex3(pt.X, pt.Y, pt.Z);
                    }
                }
            }
            GL.End();

            GL.Begin(PrimitiveType.QuadStrip);
            {
                for (int i = rows - 1; i > 0; i--)
                {
                    for (int j = columns - 1; j > 0; j--)
                    {
                        //if (j == columns - 1) continue;
                        Point3D pt = pts[i, j];
                        GL.Normal3(pt.X, -pt.Y, pt.Z);
                        GL.Vertex3(pt.X, -pt.Y, pt.Z);
                    }
                }
            }
            GL.End();

            GL.Begin(PrimitiveType.TriangleFan);
            {
                for (int i = rows - 1; i > 0; i--)
                {
                    for (int j = columns - 1; j > 0; j--)
                    {
                        //if (j == columns - 1) continue;
                        Point3D pt = pts[i, j];
                        GL.Normal3(pt.X, -pt.Y, pt.Z);
                        GL.Vertex3(pt.X, -pt.Y, pt.Z);
                    }
                }
                GL.Normal3(0, -radius, 0);
                GL.Vertex3(0, -radius, 0);
            }
            GL.End();
        }

        public static Point3D[,] GenerateSpherePoints(double radius, int rows, int columns)
        {
            Point3D[,] pts = new Point3D[rows, columns];
            double deltaY = radius / rows;
            double deltaTheta = (2 * Math.PI) / columns;
            double y;
            double theta;
            for (int i = 0; i < rows; i++)
            {
                y = radius - (deltaY * (i + 1));
                theta = 0;
                for (int j = 0; j < columns; j += 2)
                {
                    if (j == columns - 1) continue;
                    double x = Math.Sqrt(radius * radius - y * y);
                    pts[i, j] = new Point3D(x * Math.Cos(theta), y, x * Math.Sin(theta));
                    pts[i, j + 1] = new Point3D(x * Math.Cos(theta), y - deltaY, x * Math.Sin(theta));
                    theta += deltaTheta;
                }
            }
            return pts;

        }

        public static void WireCylinder(this GLU glu, double baseRadius = 1, double topRadius = 1, double height = 1,
            int rows = 30, int columns = 30)
        {
            Point3D[,] pts = GenerateCylinderPoints(baseRadius, topRadius, height, rows, columns);
            GL.Begin(PrimitiveType.LineLoop);
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        Point3D pt = pts[i, j];
                        GL.Normal3(pt.X, pt.Y, pt.Z);
                        GL.Vertex3(pt.X, pt.Y, pt.Z);
                    }
                }
            }
            GL.End();
        }


        public static Point3D[,] GenerateCylinderPoints(double bR, double tR, double height, int rows, int columns)
        {
            Point3D[,] pts = new Point3D[rows, columns];
            double deltaY = height / rows;
            double deltaR = (tR - bR) / rows;
            double deltaTheta = (2 * Math.PI) / columns;
            double y;
            double theta;
            for (int i = 0; i < rows; i++)
            {
                y = deltaY * i;
                double radius = bR + (deltaR * i);
                theta = 0;
                for (int j = 0; j < columns; j += 2)
                {
                    if (j == columns - 1) continue;
                    double x = Math.Sqrt(radius * radius - y * y);
                    pts[i, j] = new Point3D(x * Math.Cos(theta), y, x * Math.Sin(theta));
                    pts[i, j + 1] = new Point3D(x * Math.Cos(theta), y - deltaY, x * Math.Sin(theta));
                    theta += deltaTheta;
                }
            }
            return pts;
        }
    }
}