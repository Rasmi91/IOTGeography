using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace geographyeval
{
    public static class geographyeval
    {
        [FunctionName("geography-eval")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string Lat = "13.0405";
            string Longi = "80.2337";

            var _Devicepostalcode = "600017";//County Postal Code,It will come from db 
            var json = new WebClient().DownloadString("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + Lat + "," + Longi + "&key=AIzaSyDwEsdtIIzk29v8DvxctDmbgbhHCkj9iNs");
            var VehiclespostalCode = "";
            var Jdata = (JObject)JsonConvert.DeserializeObject(json);
            JToken memberName = Jdata["results"];
            foreach (var val in memberName)
            {
                if (val["types"].FirstOrDefault().ToString() == "postal_code")
                {
                    VehiclespostalCode = val["address_components"][0]["long_name"].ToString();
                }
            }
            log.Info("Geofence's location postal code is " + _Devicepostalcode);
           
            log.Info("Vehicle's location postal code is " + VehiclespostalCode);
           

            if (string.Equals(VehiclespostalCode, _Devicepostalcode))
            {
                log.Info("Your device is inside of the location");
              
            }
            else
            {
                log.Info("Your device is outside of the location");
               
            }

            // parse query parameter
            string latitude = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "lat", true) == 0)
                .Value;

            string longitude = req.GetQueryNameValuePairs()
               .FirstOrDefault(q => string.Compare(q.Key, "long", true) == 0)
               .Value;


            if (latitude == null)
            {
                // Get request body
                dynamic data = await req.Content.ReadAsAsync<object>();
                latitude = data?.latitude;
            }
            if (longitude == null)
            {
                // Get request body
                dynamic data = await req.Content.ReadAsAsync<object>();
                longitude = data?.longitude;
            }
            log.Info("Latitude " + latitude+"Longitude " + longitude);


            return latitude == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
                : req.CreateResponse(HttpStatusCode.OK, "Latitude  '"+ latitude+"' Longitude '" + longitude+"'");
        }

    }
}
