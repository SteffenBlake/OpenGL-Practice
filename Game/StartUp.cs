using System;
using System.Diagnostics;
using System.Linq.Expressions;
using OpenGL_Practice.Services.Classes;
using OpenGL_Practice.Views;
using OpenTK;

namespace OpenGL_Practice
{
    public static class StartUp
    {

        public static void Main()
        {
            var poly1 = new Polygon();
            poly1.Vertices.Add(new Vector2(1, 6));
            poly1.Vertices.Add(new Vector2(3, 8));
            
            //poly1.Vertices.Add(new Vector2(4, 6));
            //poly1.Vertices.Add(new Vector2(6, 7));
            poly1.Vertices.Add(new Vector2(7, 4));

            poly1.Vertices.Add(new Vector2(2, 3));

            var poly2 = new Polygon();
            poly2.Vertices.Add(new Vector2(7, 4));
            poly2.Vertices.Add(new Vector2(4, 9));
            poly2.Vertices.Add(new Vector2(6, 12));
            poly2.Vertices.Add(new Vector2(10, 6));

            if (Physics.Collides(poly1, poly2))
            {
                var intersections1 = poly1.IntersectionsWith(poly2);
                var intersections2 = poly2.IntersectionsWith(poly1);
                Console.WriteLine("Polygon #1 Intersecting vert count:{0}", intersections1.Count);
                foreach (var vert in intersections1)
                {
                    Console.WriteLine("   [X:{0}, Y:{1}]", vert.X, vert.Y);
                }
                Console.WriteLine("Polygon #2 Intersecting vert count:{0}", intersections2.Count);
                foreach (var vert in intersections2)
                {
                    Console.WriteLine("   [X:{0}, Y:{1}]", vert.X, vert.Y);
                }
            }
            else
            {
                Console.WriteLine("No intersections between Polygon 1 and 2 found.");
            }

            var main = new Main();
            main.Run(60, 60);
        }
    }
}
