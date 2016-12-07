using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using csci342;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using KeyPressEventArgs = System.Windows.Forms.KeyPressEventArgs;
using Vector3 = csci342.Vector3;

namespace mygraphicslib
{
    /*
    public delegate void CameraChangedHandler(object sender, EventArgs args);

    public interface IMatrixLoader
    {
        void MatrixMode(MatrixMode newMode);
        void LoadMatrix(double[] m);
    }

    class DefaultIMatrixLoader : IMatrixLoader
    {
        public void MatrixMode(MatrixMode newMode)
        {
            GL.MatrixMode(newMode);
        }

        public void LoadMatrix(double[] m)
        {
            GL.LoadMatrix(m);
        }
    }

    public enum CameraDirection
    {
        Forward, Backward, RollClockwise, RollCounterClockwise,
        PitchUp, PitchDown, YawClockwise, YawCounterClockwise
    }

    public class Camera : ICloneable
    {
        public static Dictionary<CameraDirection, char> DefaultKeyBindings;

        private static IMatrixLoader defaultMatrixLoader;

        static Camera ()
        {
            DefaultKeyBindings = new Dictionary<CameraDirection, char>
            {
                [CameraDirection.Forward] = 'f',
                [CameraDirection.Backward] = 'F',
                [CameraDirection.PitchUp] = 'p',
                [CameraDirection.PitchDown] = 'P',
                [CameraDirection.RollClockwise] = 'r',
                [CameraDirection.RollCounterClockwise] = 'R',
                [CameraDirection.YawClockwise] = 'y',
                [CameraDirection.YawCounterClockwise] = 'Y'
            };

            defaultMatrixLoader = new DefaultIMatrixLoader();
        }

        private csci342.Vector3 u = new Vector3(0, 0, 0);
        private Vector3 v = new Vector3(0, 0, 0);
        private Vector3 n = new Vector3(0, 0, 0);

        private Vector3 up;

        private Point3D eye;
        private Point3D look;

        private double distanceFromEyeToLook;

        private KeyPressEventHandler keyHandler;
        private GLControl control;

        private Dictionary<CameraDirection, char> KeyBindings;
        
        public IMatrixLoader MatrixLoader { get; set; }
                
        private void KeyPressHandler(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == KeyBindings[CameraDirection.Forward])
            {
                Eye = Eye + -1 * n;
            }
            else if (e.KeyChar == KeyBindings[CameraDirection.Backward])
            {
                Eye = Eye + n;
            }
            else if (e.KeyChar == KeyBindings[CameraDirection.RollClockwise])
            {
                Roll(2);
            }
            else if (e.KeyChar == KeyBindings[CameraDirection.RollCounterClockwise])
            {
                Roll(-2);
            }
            else if (e.KeyChar == KeyBindings[CameraDirection.PitchUp])
            {
                Pitch(2);
            }
            else if (e.KeyChar == KeyBindings[CameraDirection.PitchDown])
            {
                Pitch(-2);
            }
            else if (e.KeyChar == KeyBindings[CameraDirection.YawClockwise])
            {
                Yaw(2);
            }
            else if (e.KeyChar == KeyBindings[CameraDirection.YawCounterClockwise])
            {
                Yaw(-2);
            }
                
            control.Invalidate();
        }

        public event CameraChangedHandler Changed;

        public Vector3 U
        {
            get
            {
                return new Vector3(u.X, u.Y, u.Z);
            }

            internal set
            {
                u.X = value.X;
                u.Y = value.Y;
                u.Z = value.Z;
                SetModelViewMatrix();
            }
        }

        public Vector3 V
        {
            get
            {
                return new Vector3(v.X, v.Y, v.Z);
            }

            internal set
            {
                v.X = value.X;
                v.Y = value.Y;
                v.Z = value.Z;
                SetModelViewMatrix();
            }
        }

        public Vector3 N
        {
            get
            {
                return new Vector3(n.X, n.Y, n.Z);
            }

            internal set
            {
                n.X = value.X;
                n.Y = value.Y;
                n.Z = value.Z;
                SetModelViewMatrix();
            }
        }

        public Point3D Eye
        {
            get
            {
                return new Point3D(eye.X, eye.Y, eye.Z);
            }

            set
            {
                eye = new Point3D(value.X, value.Y, value.Z);
                InitializeVectorsFromEyeAndLook();
                SetDistanceFromEyeToLook();
                OnChanged(EventArgs.Empty);
            }
        }

        public Point3D Look
        {
            get
            {
                return new Point3D(look.X, look.Y, look.Z);
            }

            set
            {
                look = new Point3D(value.X, value.Y, value.Z);
                InitializeVectorsFromEyeAndLook();
                SetDistanceFromEyeToLook();
                OnChanged(EventArgs.Empty);
            }
        }

        public Camera(Point3D _eye, Point3D _look, Vector3 _up, IMatrixLoader matrixLoader)
        {
            MatrixLoader = matrixLoader;

            eye = new Point3D(_eye.X, _eye.Y, _eye.Z);
            look = new Point3D(_look.X, _look.Y, _look.Z);
            up = new Vector3(_up.X, _up.Y, _up.Z);
            SetDistanceFromEyeToLook();
            InitializeVectorsFromEyeAndLook();
            keyHandler = new System.Windows.Forms.KeyPressEventHandler(this.KeyPressHandler);

            KeyBindings = new Dictionary<CameraDirection, char>(DefaultKeyBindings.Count);
            foreach (var key in DefaultKeyBindings.Keys)
            {
                KeyBindings[key] = DefaultKeyBindings[key];
            }
        }

        public Camera(Point3D _eye, Point3D _look, IMatrixLoader matrixLoader)
            : this(_eye, _look, new Vector3(0, 1, 0), matrixLoader)
        {
        }

        public Camera(Point3D _eye, Point3D _look) : this(_eye, _look, new DefaultIMatrixLoader())
        {            
        }

        public Camera(Point3D _eye, Point3D _look, Vector3 up) : this(_eye, _look, up, new DefaultIMatrixLoader())
        {
        }

        public char SetKeyBinding(CameraDirection direction, char newKey)
        {
            var oldBinding = KeyBindings[direction];
            KeyBindings[direction] = newKey;
            return oldBinding;
        }

        public char GetKeyBinding(CameraDirection direction)
        {
            return KeyBindings[direction];
        }

        public void SetModelViewMatrix()
        {            
            double[] m = new double[16];              
			
			//  Add your implementation of the SetModelViewMatrix method here
            m[0] = U.X;
            m[1] = V.X;
            m[2] = N.X;
            m[3] = 0;

            m[4] = U.Y;
            m[5] = V.Y;
            m[6] = N.Y;
            m[7] = 0;

            m[8] = U.Z;
            m[9] = V.Z;
            m[10] = N.Z;
            m[11] = 0;

            m[12] = U.Dot(new Vector3(-eye.X, -eye.Y, -eye.Z));
            m[13] = V.Dot(new Vector3(-eye.X, -eye.Y, -eye.Z));
            m[14] = N.Dot(new Vector3(-eye.X, -eye.Y, -eye.Z));
            m[15] = 1;

            MatrixLoader.MatrixMode(MatrixMode.Modelview);
            MatrixLoader.LoadMatrix(m);
        }

        public void Forward(double distance)
        {
			//  Add your implementation of the Forward method before the call to OnChanged



            OnChanged(EventArgs.Empty);
        }

        public void Backward(double distance)
        {
            Forward(-distance);
        }

        public void Left(double distance)
        {
            Right(-distance);
        }

        public void Right(double distance)
        {
			//  Add your implementation of the Right method before the call to OnChanged
            OnChanged(EventArgs.Empty);
        }

        public void Down(double distance)
        {
            Up(-distance);
        }

        public void Up(double distance)
        {
            //  Add your implementation of the Up method before the call to OnChanged
            OnChanged(EventArgs.Empty);
        }

        public void Roll(double angleInDegrees)
        {
            double angleInRadians = angleInDegrees * Math.PI / 180;

			//  Add your implementation of the Roll method before the call to SetModelViewMatrix
            SetModelViewMatrix();
            OnChanged(EventArgs.Empty);
        }

        public void Yaw(double angleInDegrees)
        {
            double angleInRadians = angleInDegrees * Math.PI / 180;
            
			//  Add your implementation of the Yaw method before the call to ResetLookPointFromN
            ResetLookPointFromN();
            SetModelViewMatrix();
            OnChanged(EventArgs.Empty);
        }

        public void Pitch(double angleInDegrees)
        {
            double angleInRadians = angleInDegrees * Math.PI / 180;
            
			//  Add your implementation of the Pitch method before the call to ResetLookPointFromN
            ResetLookPointFromN();
            SetModelViewMatrix();
            OnChanged(EventArgs.Empty);
        }

        public void ActivateCameraControls(GLControl c)
        {
            c.KeyPress += keyHandler;
            control = c;
        }

        public void DeactivateCameraControls()
        {
            control.KeyPress -= keyHandler;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(256);

            builder.AppendFormat("Location: {0}\n", eye);
            builder.AppendFormat("u: {0}\n", u);
            builder.AppendFormat("v: {0}\n", v);
            builder.AppendFormat("n: {0}", n);

            return builder.ToString();
        }

        public Object Clone()
        {
            var clone = new Camera(eye, look);
            clone.u = new Vector3(u.X, u.Y, u.Z);
            clone.v = new Vector3(v.X, v.Y, v.Z);
            clone.n = new Vector3(n.X, n.Y, n.Z);
            return clone;
        }

        protected virtual void OnChanged(EventArgs args)
        {
            Changed?.Invoke(this, args);
        }

        private void SetDistanceFromEyeToLook()
        {
            distanceFromEyeToLook = Math.Sqrt(Math.Pow(eye.X - look.X, 2) + Math.Pow(eye.Y - look.Y, 2) + Math.Pow(eye.Z - look.Z, 2));
        }

        private void ResetLookPointFromN()
        {
            look = eye + -1 * distanceFromEyeToLook * n;
        }

        private void InitializeVectorsFromEyeAndLook()
        {
			//  Add your implementation of InitializeVectorsFromEyeAndLook before the call to SetModelViewMatri

            N = new Vector3(eye.X - look.X, eye.Y - look.Y, eye.Z - look.Z);
            N = N.ToUnit();

            U = up.ToUnit().Cross(N);
            V = N.Cross(U.ToUnit());

			SetModelViewMatrix();
        }
    }

    */
}