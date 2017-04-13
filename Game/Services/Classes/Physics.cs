using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenGL_Practice.Services.Classes
{
    public static class Physics
    {
        private const double Tolerance = 0.0001;
        private const double Infinity = double.MaxValue;

        public static double Length(Vector2 vert1, Vector2 vert2)
        {
            return (vert1 - vert2).Length;
        }

        public static double Length(Line line)
        {
            return Length(line.Vert1, line.Vert2);
        }

        public static double Slope(Vector2 vector)
        {
            return Math.Abs(vector.X) > Tolerance ? vector.Y / vector.X : Infinity;
        }

        public static double Slope(Vector2 vert1, Vector2 vert2)
        {
            return Slope(vert2 - vert1);
        }

        public static double Slope(Line line)
        {
            return Slope(line.Vert1, line.Vert2);
        }

        public static double Angle(Vector2 vector)
        {
            return Math.Tan(Slope(vector));
        }

        public static double Angle(Vector2 vert1, Vector2 vert2)
        {
            return Angle(vert2 - vert1);
        }

        public static double Angle(Line line)
        {
            return Angle(line.Vert1, line.Vert2);
        }

        public static Enums.VectorDirection Direction(Vector2 vector)
        {
            if (Math.Abs(vector.Length) < Tolerance) return Enums.VectorDirection.Point;
            if (Math.Abs(vector.X) < Tolerance) return Enums.VectorDirection.Vertical;
            return vector.X > 0 ? Enums.VectorDirection.Right : Enums.VectorDirection.Left;
        }

        public static Enums.VectorDirection Direction(Vector2 vert1, Vector2 vert2)
        {
            return Direction(vert2 - vert1);
        }

        public static Enums.VectorDirection Direction(Line line)
        {
            return Direction(line.Vert1, line.Vert2);
        }

        public static bool Collides(Polygon poly1, Polygon poly2)
        {
            return HasIntersections(poly1, poly2) || HasIntersections(poly2, poly1);
        }

        public static bool HasIntersections(Polygon intersector, Polygon reciever)
        {
            return intersector.Vertices.Any(vert => Intersects(vert, reciever));
        }

        public static bool Intersects(Vector2 vertex, Polygon reciever)
        {
            if (reciever.Vertices.Any(v => v == vertex)) return true;

            return reciever.Lines.Count(line => 
                RangeBound(line, vertex) 
                && VertDirectionFromLine(line, vertex) == Enums.VectorDirection.Right) 
                % 2 == 1;
        }

        public static List<Vector2> IntersectingVerts(Polygon intersector, Polygon reciever)
        {
            return intersector.Vertices.Where(vert => Intersects(vert, reciever)).ToList();
        }

        /// <summary>
        /// Returns true if vertex lies withing the range of the line (height)
        /// </summary>
        /// <param name="line"></param>
        /// <param name="vert"></param>
        /// <returns></returns>
        public static bool RangeBound(Line line, Vector2 vert)
        {
            return line.LowerPoint.Y <= vert.Y && line.HigherPoint.Y > vert.Y;
        }

        /// <summary>
        /// returns right if the vertice is to the right of the line,
        /// left if the vertice is left of the line.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="vert"></param>
        /// <returns></returns>
        public static Enums.VectorDirection VertDirectionFromLine(Line line, Vector2 vert)
        {
            var lowerVertLine = new Line(line.LowerPoint, vert);
            var upperVertLine = new Line(line.HigherPoint, vert);

            if (lowerVertLine.Direction == upperVertLine.Direction)
                return lowerVertLine.Direction == Enums.VectorDirection.Left
                    ? Enums.VectorDirection.Left
                    : Enums.VectorDirection.Right;

            return lowerVertLine.Slope < line.Slope ? Enums.VectorDirection.Right : Enums.VectorDirection.Left;
        }
    }

    public class Polygon
    {
        public List<Vector2> Vertices { get; set; }

        public Polygon()
        {
            Vertices = new List<Vector2>();
        }

        public List<Line> Lines
            => Vertices.Select((v, n) =>
                new Line(Vertices[n], Vertices[n == Vertices.Count - 1 ? 0 : n+1])).ToList();

        public List<double> Angles => Lines.Select(Physics.Angle).ToList();

        public List<double> Lengths => Lines.Select(Physics.Length).ToList();

        public bool Intersects(Polygon targetPoly) => Physics.HasIntersections(this, targetPoly);

        public bool Collides(Polygon targetPoly) => Physics.Collides(this, targetPoly);

        public List<Vector2> IntersectionsWith(Polygon targetPoly) => Physics.IntersectingVerts(this, targetPoly);
    }

    public class Line
    {
        public Line(Vector2? vert1 = null, Vector2? vert2 = null)
        {
            Vert1 = vert1 ?? new Vector2();
            Vert2 = vert2 ?? new Vector2();
        }

        public Vector2 Vert1 { get; set; }
        public Vector2 Vert2 { get; set; }

        public double Length => Physics.Length(this);
        public double Slope => Physics.Slope(this);
        public double Angle => Physics.Angle(this);
        public Enums.VectorDirection Direction => Physics.Direction(this);

        public Vector2 LowerPoint => Vert2.Y > Vert1.Y ? Vert1 : Vert2;
        public Vector2 HigherPoint => Vert2.Y > Vert1.Y ? Vert2 : Vert1;

        public Vector2 LeftPoint => Vert2.X > Vert1.X ? Vert1 : Vert2;
        public Vector2 RightPoint => Vert2.X > Vert1.X ? Vert2 : Vert1;
    }
}