using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace GeofenceAzureFuncApp
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            var content = req.Content;
            string JsonContent = content.ReadAsStringAsync().Result;
            log.Info(JsonContent);

            Customer data1 = await req.Content.ReadAsAsync<Customer>();
            log.Info(data1.Lat,data1.Long);


            var polygon = new List<Point> {
            new Point(20,30), new Point(50, 50),
            new Point(60, 45), new Point(100, 50), new Point(50, 100) };

            var pointsToCheck = new List<Point> {
            new Point(60, 70), new Point(80, 60), new Point(60, 20), new Point(80, 80) };
            foreach (var point in pointsToCheck)
            {
                var position = (PolyContainsPoint(polygon, point)) ? "Inside" : "Outside";
                log.Info(point.X.ToString() + " " + point.Y.ToString() + " " + "is" + " " + position);
            }

            // parse query parameter
            string name = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
                .Value;

            if (name == null)
            {
                // Get request body
                dynamic data = await req.Content.ReadAsAsync<object>();
                name = data?.name;
            }

            return name == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
                : req.CreateResponse(HttpStatusCode.OK, "Hello " + name);
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
    public class Customer
    {
        public string Lat { get; set; }
        public string Long { get; set; }
    }
}

