 using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace evalfunc
{
    public static class evalfunc
    {
        [FunctionName("eval-func")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");
                                
            string machineId = string.Empty;
            List<GeographyPoints> pointList = new List<GeographyPoints>();
            GeographyPoints pointItem = new GeographyPoints();
                          
            dynamic data = await req.Content.ReadAsAsync<object>();                         
            var geography = data?.geofenceRequest;
            //Json Syntax1
            machineId = data?.machineid;
            var pointList1 = JsonConvert.DeserializeObject<List<GeographyPoints>>(geography.ToString());
            foreach (var item in pointList1)
            {
                foreach (var pt in item)
                {
                    pointItem.latitude = pt;
                    pointItem.longitude = pt;
                    pointItem.timestamp = pt;
                    pointList.Add(pt);
                }
            }
            //Json Syntax2
            //var pointList2 = JsonConvert.DeserializeObject<List<GeographyCoordinates>>(geography.ToString());
            //    foreach (var item in pointList2)
            //    {
            //        foreach (var pt in item)
            //        {
            //            pointItem.latitude = pt;
            //            pointItem.longitude = pt;
            //            pointItem.timestamp = pt;
            //            pointList.Add(pt);
            //        }
            //    }

            string toCheck = "rasmi";
            if (toCheck == "rasmi")
            {
                string anotherFunctionSecret = "NFDGHECTq7qUzPQRdSpWdMTRRZfaHvf0OkACVfK5WQQs3udmVja6cw==";
                Uri anotherFunctionUri = new Uri(req.RequestUri.AbsoluteUri.Replace(
                    req.RequestUri.PathAndQuery,
                    $"/api/geography-eval?code={anotherFunctionSecret}&lat=10&long=20"));
                HttpClient client = new HttpClient();
                var output = await client.GetAsync(anotherFunctionUri);
                return output;
            }
            return toCheck == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
                : req.CreateResponse(HttpStatusCode.OK, "Hello " + toCheck + machineId);
        }
    }
   
}

public class GeographyPoints
{
    public string latitude { get; set; }
    public string longitude { get; set; }
    public string timestamp { get; set; }
}

public class GeographyCoordinates
{
    public  List<GeographyPoints> coordinates { get; set; }
    public string machineId { get; set; }

}
