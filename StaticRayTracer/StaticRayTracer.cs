using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using csci342;
using OpenTK.Graphics;

namespace StaticRayTracer
{
    public partial class StaticRayTracer : Form
    {
        public StaticRayTracer()
        {
            InitializeComponent();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            GL.ClearColor(Color.Cornsilk);
            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
            DrawingTools.EnableDefaultAntiAliasing();
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!glControl1.IsHandleCreated) { return; }

            GLU.Instance.Perspective(60, 1, 1, 40);

            var eye = new Point3D(-10, 4, 10);
            var look = new Point3D(0, 0, 3);
            GLU.Instance.LookAt(eye, look);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);

            GL.Light(LightName.Light0, LightParameter.Position, new float[] { 0, 3, 8, 0 });
            GL.Light(LightName.Light0, LightParameter.Ambient, new float[] { (float)0.5, (float)0.5, (float)0.5, 1 });
            GL.Light(LightName.Light0, LightParameter.Diffuse, new float[] { (float)0.9, (float)0.9, (float)0.9, 1 });

            GL.PointSize(10);
            GL.Color3(Color.Black);
            GL.Begin(PrimitiveType.Points);
            {
                GL.Vertex3(0, 3, 8);
            }
            GL.End();

            GL.Color3(Color.Black);
            GL.Begin(PrimitiveType.Lines);
            {
                GL.Vertex3(0, 0, 1000);
                GL.Vertex3(0, 0, -1000);
            }
            GL.End();

            GL.Color3(Color.Red);
            GL.Begin(PrimitiveType.Lines);
            {
                GL.Vertex3(-1000, 0, 0);
                GL.Vertex3(1000, 0, 0);
            }
            GL.End();

            GL.Color3(Color.Green);
            GL.Begin(PrimitiveType.Lines);
            {
                GL.Vertex3(0, -10000, 0);
                GL.Vertex3(0, 10000, 0);
            }
            GL.End();

            GL.Color3(Color.Black);

            // Draw the "viewing plane"
            GL.PushMatrix();
            {
                GL.Translate(-2, -2, 3);
                GL.Begin(PrimitiveType.LineLoop);
                {
                    GL.Vertex3(0, 0, 0);
                    GL.Vertex3(4, 0, 0);
                    GL.Vertex3(4, 4, 0);
                    GL.Vertex3(0, 4, 0);
                }
                GL.End();
            }
            GL.PopMatrix();

            GL.PushMatrix();
            {
                GL.Translate(0, 0, -1);

                GL.Begin(PrimitiveType.LineLoop);
                {
                    // top
                    GL.Vertex3(1, 1, 1);
                    GL.Vertex3(1, 1, 0);
                    GL.Vertex3(-1, 1, 0);
                    GL.Vertex3(-1, 1, 1);

                    // bottom
                    GL.Vertex3(-1, 0, 1);
                    GL.Vertex3(-1, 0, 0);
                    GL.Vertex3(1, 0, 0);
                    GL.Vertex3(1, 0, 1);

                    // front side
                    GL.Vertex3(1, 1, 1);
                    GL.Vertex3(-1, 1, 1);
                    GL.Vertex3(-1, 0, 1);
                    GL.Vertex3(1, 0, 1);

                    // back side
                    GL.Vertex3(1, 0, 0);
                    GL.Vertex3(-1, 0, 0);
                    GL.Vertex3(-1, 1, 0);
                    GL.Vertex3(1, 1, 0);
                }
                GL.End();
            }
            GL.PopMatrix();

            GL.Enable(EnableCap.Lighting);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.DarkSeaGreen);

            GL.PushMatrix();
            {
                GL.Translate(0, 0, 7);
                SolidSphere(1, 100, 100);
            }
            GL.PopMatrix();

            glControl1.SwapBuffers();
        }

        public static void SolidSphere(double radius, int verticalDivisions, int horizontalDivisions)
        {
            Point3D[,] points = GetSpherePoints(radius, verticalDivisions, horizontalDivisions);

            GL.Begin(PrimitiveType.TriangleFan);
            {
                GL.Vertex3(0, radius, 0);
                for (int i = 0; i < verticalDivisions + 1; i++)
                {
                    Point3D p = points[1, i];
                    GL.Vertex3(p.X, p.Y, p.Z);
                }
            }
            GL.End();

            GL.Begin(PrimitiveType.QuadStrip);
            {
                for (int i = 1; i < (horizontalDivisions / 2); i++)
                {
                    for (int j = 0; j < verticalDivisions + 1; j++)
                    {
                        Point3D p = points[i, j];
                        GL.Normal3(p.X, p.Y, p.Z);
                        GL.Vertex3(p.X, p.Y, p.Z);

                        Point3D p2 = points[i + 1, j];
                        GL.Normal3(p2.X, p2.Y, p2.Z);
                        GL.Vertex3(p2.X, p2.Y, p2.Z);
                    }
                }
            }
            GL.End();
            GL.Begin(PrimitiveType.TriangleFan);
            {
                GL.Vertex3(0, -1 * radius, 0);
                for (int i = 0; i < verticalDivisions + 1; i++)
                {
                    Point3D p = points[1, i];
                    GL.Vertex3(p.X, -1 * p.Y, p.Z);
                }
            }
            GL.End();

            GL.Begin(PrimitiveType.QuadStrip);
            {
                for (int i = 1; i < (horizontalDivisions / 2); i++)
                {
                    for (int j = 0; j < verticalDivisions + 1; j++)
                    {
                        Point3D p = points[i, j];
                        GL.Normal3(p.X, -1 * p.Y, p.Z);
                        GL.Vertex3(p.X, -1 * p.Y, p.Z);

                        Point3D p2 = points[i + 1, j];
                        GL.Normal3(p2.X, -1 * p2.Y, p2.Z);
                        GL.Vertex3(p2.X, -1 * p2.Y, p2.Z);
                    }
                }
            }
            GL.End();
        }

        private static Point3D[,] GetSpherePoints(double radius, int verticalDivisions, int horizontalDivisions)
        {
            int divisions = (horizontalDivisions / 2);
            double deltaR = radius / divisions;
            Point3D[,] points = new Point3D[divisions + 1, verticalDivisions + 1];

            for (int i = 1; i <= divisions; i++)
            {
                for (int j = 0; j < verticalDivisions + 1; j++)
                {
                    double currentCircleRadius = Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(radius - (deltaR * i), 2));
                    double theta = ((2 * Math.PI) / verticalDivisions) * j;
                    points[i, j] = new Point3D(currentCircleRadius * Math.Cos(theta), radius - (deltaR * i), currentCircleRadius * Math.Sin(theta));
                }
            }

            return points;
        }
    }
}
