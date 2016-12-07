using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using csci342;
using OpenTK.Graphics.OpenGL;

namespace mygraphicslib
{
    public class PolylineCollection : IEnumerable<Polyline>
    {
        private List<Polyline> polylines;

        public PolylineCollection()
        {
            polylines = new List<Polyline>();
        }

        public PolylineCollection(params Polyline[] pls) : this()
        {
            AddPolylines(pls);
        }

        public void AddPolyline(Polyline line)
        {
            polylines.Add(line);
        }

        public void AddPolylines(params Polyline[] lines)
        {
            polylines.AddRange(lines);
        }

        public Polyline RemoveLast()
        {
            // TODO: throw
            if (polylines.Count == 0) return null;
            
            Polyline line = polylines[polylines.Count - 1];
            polylines.RemoveAt(polylines.Count-1);
            return line;
        }

        public void Draw(PrimitiveType type)
        {
            foreach (Polyline line in polylines)
            {
                line.Draw(type);
            }
        }

        public IEnumerator<Polyline> GetEnumerator()
        {
            return polylines.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) polylines).GetEnumerator();
        }
    }
}
