using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using csci342;
using OpenTK.Graphics.OpenGL;

namespace mygraphicslib
{
    public class Vector
    {
        private List<double> components;

        public double Length
        {
            get
            {
                double length = 0;
                foreach (double c in components)
                {
                    length += c*c;
                }
                return Math.Sqrt(length);
            }
        }

        public double this[int key]
        {
            get { return components[key]; }
        }

        public Vector()
        {
            components = new List<double>();
        }

        public Vector(params double[] components)
        {
            this.components = components.ToList();
        }

        public Vector(List<double> comps)
        {
            this.components = comps;
        }

        public Vector(Point2D pt) : this(pt.X, pt.Y)
        {
        }

        public Vector(Point3D pt) : this(pt.X, pt.Y, pt.Z)
        {
        }

        public Vector UnitVector()
        {
            return ScalarMultiplication(1/Length);
        }

        private void addComponent(double comp)
        {
            components.Add(comp);
        }

        public static Vector operator -(Vector lhs, Vector rhs)
        {
            if (lhs.components.Count != rhs.components.Count) throw new Exception("cannot dot with two vectors of different lengths of components");
            Vector subtractedVector = new Vector();
            for (int i = 0; i < lhs.components.Count; i++)
            {
                subtractedVector.addComponent(0);
                subtractedVector.components[i] = lhs.components[i] - rhs.components[i];
            }
            return subtractedVector;
        }

        public static Point2D operator +(Point2D lhs, Vector rhs)
        {
            if (rhs.components.Count != 2) throw new Exception("cannot add a point to a vector with too many/not enough components");
            Point2D result = new Point2D(lhs.X + rhs[0], lhs.Y + rhs[1]);
            return result;
        }

        
        public double Dot(Vector v)
        {
            if (v.components.Count != this.components.Count) throw new Exception("cannot dot with two vectors of different lengths of components");
            double product = 0;
            for (int i = 0; i < this.components.Count; i++)
            {
                product += this.components[i]*v.components[i];
            }
            return product;
        }

        public Vector ScalarMultiplication(double scalar)
        {
            List<double> newComps = new List<double>(components);


            for (int i = 0; i < components.Count; i++)
            {
                newComps[i] *= scalar;
            }
            return new Vector(newComps);
        }
    }
}
