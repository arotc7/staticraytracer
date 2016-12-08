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
using csci342.GlutDrawingReplacements;
using OpenTK.Graphics;
using mygraphicslib;

namespace StaticRayTracer
{
    public partial class StaticRayTracer : Form
    {
        private Point3D _lightingPoint;

        public StaticRayTracer()
        {
            _lightingPoint = new Point3D(3, 2, 6);
            InitializeComponent();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            GL.ClearColor(Color.White);
            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
            DrawingTools.EnableDefaultAntiAliasing();

            // enable lighting
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);

            GL.ShadeModel(ShadingModel.Smooth);

            GL.Enable(EnableCap.DepthTest);

            // setup lighting parameters
            GL.Light(LightName.Light0, LightParameter.Position, new float[] { (float)_lightingPoint.X, (float)_lightingPoint.Y, (float)_lightingPoint.Z, 0 });
            GL.Light(LightName.Light0, LightParameter.Ambient, new float[] { (float)0.5, (float)0.5, (float)0.5, 1 });
            GL.Light(LightName.Light0, LightParameter.Diffuse, new float[] { (float)0.9, (float)0.9, (float)0.9, 1 });

            // set large point size
            GL.PointSize(10);
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!glControl1.IsHandleCreated) { return; }

            GLU.Instance.Perspective(60, 1, 1, 40);

            // what the values should be
            //var eye = new Point3D(-8, 4, 8);
            //var look = new Point3D(0, 0, 4);

            // testing values
            var eye = new Point3D(-7, 2, 7);
            var look = new Point3D(0, 0, 7);

            GLU.Instance.LookAt(eye, look);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);

            // draw the x, y, and z axises
            GL.Disable(EnableCap.Lighting);
            GL.Color3(Color.Yellow);
            GL.Begin(PrimitiveType.Points);
            {
                GL.Vertex3(_lightingPoint.X, _lightingPoint.Y, _lightingPoint.Z);
            }
            GL.End();
            // z axis
            GL.Color3(Color.Black);
            GL.Begin(PrimitiveType.Lines);
            {
                GL.Vertex3(0, 0, 1000);
                GL.Vertex3(0, 0, -1000);
            }
            GL.End();
            // x axis
            GL.Color3(Color.Red);
            GL.Begin(PrimitiveType.Lines);
            {
                GL.Vertex3(-1000, 0, 0);
                GL.Vertex3(1000, 0, 0);
            }
            GL.End();
            // y axis
            GL.Color3(Color.Green);
            GL.Begin(PrimitiveType.Lines);
            {
                GL.Vertex3(0, -10000, 0);
                GL.Vertex3(0, 10000, 0);
            }
            GL.End();

            // Draw the "viewing plane"
            GL.PushMatrix();
            {
                GL.Color3(Color.Black);
                // draw the plane
                GL.Translate(-2, -2, 3);
                GL.Begin(PrimitiveType.LineLoop);
                {
                    GL.Vertex3(0, 0, 0);
                    GL.Vertex3(4, 0, 0);
                    GL.Vertex3(4, 4, 0);
                    GL.Vertex3(0, 4, 0);
                }
                GL.End();

                GL.Color3(Color.Gray);
                // draw the grid inside the plane
                GL.Begin(PrimitiveType.Lines);
                {
                    var delta = 4 / 8.0;

                    // horizontal lines
                    for (var i = 0; i < 8; i++)
                    {
                        GL.Vertex3(0, i * delta, 0);
                        GL.Vertex3(4, i * delta, 0);
                    }

                    // vertical lines
                    for (var i = 0; i < 8; i++)
                    {
                        GL.Vertex3(i * delta, 0, 0);
                        GL.Vertex3(i * delta, 4, 0);
                    }
                }
                GL.End();
            }
            GL.PopMatrix();

            // draw the camera shape
            GL.PushMatrix();
            {
                GL.Translate(0, -0.5, -1);

                GL.Color3(Color.Black);
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

                GL.Translate(0, 0.5, 1);

                // draw the circle/cylinder on the front side
                GL.Color3(Color.Black);
                // GLU.Instance.SolidCylinder(0.5, 0.5, 0.75, 30, 2); // too weird looking
                GL.Begin(PrimitiveType.LineLoop);
                {
                    mygraphicslib.Utilities.DrawArc(0, 0, 0.4, 0, 360);
                }
                GL.End();
            }
            GL.PopMatrix();

            // enable the lighting and set the material
            GL.Enable(EnableCap.Lighting);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.SeaGreen);

            // draw the sphere
            GL.PushMatrix();
            {
                GL.Translate(0, 0, 7);
                SolidSphere(1, 100, 100);
            }
            GL.PopMatrix();

            // draw the rays
            GL.Disable(EnableCap.Lighting);
            GL.Color3(Color.Blue);
            GL.Begin(PrimitiveType.Lines);
            {
                // draw the ray from the eye to the top of the sphere
                GL.Vertex3(0.0, 0.0, 0.0);
                GL.Vertex3(0, 1, 7);
            }
            GL.End();

            GL.Disable(EnableCap.Lighting);
            GL.Color3(Color.LightGray);

            GL.PushMatrix();
            {
                Vector3 lightToSphere = new Vector3(_lightingPoint, new Point3D(0, 0, 7));
                lightToSphere *= (1 / lightToSphere.Length);

                double lightToPlane = projection(new Vector3(0, 1, 0), lightToSphere).Length;

                lightToSphere *= lightToPlane;

                GL.Translate(-1 * (_lightingPoint.X + lightToSphere.X), -1 * (_lightingPoint.Y + lightToSphere.Y), _lightingPoint.Z + lightToSphere.Z);
                GL.Rotate(90, 1, 0, 0);

                GL.Begin(PrimitiveType.Polygon);
                {
                    Utilities.DrawArc(0, 0, 2, 0, 360);
                }
                GL.End();
            }
            GL.PopMatrix();

            glControl1.SwapBuffers();
        }

        public Vector3 projection(Vector3 projectedOn, Vector3 vector)
        {
            return (projectedOn.Dot(vector) / projectedOn.Dot(projectedOn)) * projectedOn;
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
