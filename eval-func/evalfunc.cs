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
                      
            string name = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
                .Value;
            string machineId = string.Empty;
            List<GeographyPoints> pointList = new List<GeographyPoints>();
             GeographyPoints pointItem = new GeographyPoints();

            if (name == null)
            {              
                dynamic data = await req.Content.ReadAsAsync<object>();
                name = data?.name;
                machineId = data?.machineid;
               var geography = data?.geofenceRequests;
              var pointList1 = JsonConvert.DeserializeObject<List<GeographyPoints>>(geography.ToString());
                foreach (var item in pointList1)
                {
                    foreach (var pt in item)
                    {
                        pointItem.Latitude = pt;
                        pointItem.Longitude = pt;
                        pointItem.Timestamp = pt;
                        pointList.Add(pt);
                    }
                }
            }           

            if (name == "rasmi")
            {
                string anotherFunctionSecret = "NFDGHECTq7qUzPQRdSpWdMTRRZfaHvf0OkACVfK5WQQs3udmVja6cw==";
                Uri anotherFunctionUri = new Uri(req.RequestUri.AbsoluteUri.Replace(
                    req.RequestUri.PathAndQuery,
                    $"/api/geography-eval?code={anotherFunctionSecret}&lat=10&long=20"));
                HttpClient client = new HttpClient();
                var output = await client.GetAsync(anotherFunctionUri);
                return output;
            }
            return name == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
                : req.CreateResponse(HttpStatusCode.OK, "Hello " + name + machineId);
        }
    }
   
}

public class GeographyPoints
{
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public string Timestamp { get; set; }
}
