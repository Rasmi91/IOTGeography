using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace GeofenceAzureFuncApp
{
    class Program
    {
        public  void ExecGeofence()
        {
            Console.WriteLine("The red points are outside, " +
                             "the blue points are inside the polygon");
            Console.ReadLine();
            var polygon = new List<Point> {
            new Point(20,30), new Point(50, 50),
            new Point(60, 45), new Point(100, 50), new Point(50, 100) };
         
            var pointsToCheck = new List<Point> {
            new Point(60, 70), new Point(80, 60), new Point(60, 20), new Point(80, 80) };
            foreach (var point in pointsToCheck)
            {
                var position = (PolyContainsPoint(polygon, point)) ? "Inside" : "Outside";
                Console.WriteLine(point.X.ToString() +" "+ point.Y.ToString()+" " + "is" +" " + position);
                Console.ReadLine();
            }

        }      
        public class Point
        {
            public Point(double x, double y)
            {
                this.X = x;
                this.Y = y;
            }

            public double X { get; set; }
            public double Y { get; set; }
        }
        public static bool PolyContainsPoint(List<Point> points, Point p)
        {
            bool inside = false;

            // An imaginary closing segment is implied,
            // so begin testing with that.
            Point v1 = points[points.Count - 1];

            foreach (Point v0 in points)
            {
                double d1 = (p.Y - v0.Y) * (v1.X - v0.X);
                double d2 = (p.X - v0.X) * (v1.Y - v0.Y);

                if (p.Y < v1.Y)
                {
                    // V1 below ray
                    if (v0.Y <= p.Y)
                    {
                        // V0 on or above ray
                        // Perform intersection test
                        if (d1 > d2)
                        {
                            inside = !inside; // Toggle state
                        }
                    }
                }
                else if (p.Y < v0.Y)
                {
                    // V1 is on or above ray, V0 is below ray
                    // Perform intersection test
                    if (d1 < d2)
                    {
                        inside = !inside; // Toggle state
                    }
                }

                v1 = v0; //Store previous endpoint as next startpoint
            }

            return inside;
        }


    }
}
