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
using System.Text;
using System.Threading.Tasks;

namespace evalfunc
{
    public static class evalfunc
    {
        [FunctionName("evaluateBatch")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");
            string response=string.Empty; 
            
            //1.1 Receiving a Request                                      
            dynamic data = await req.Content.ReadAsAsync<object>();          
            var tenantId = data?.tenantId;
            var metadata= data?.metadata;
            var geography = data?.geofenceRequest;

            //1.2 Validate Batch Reuqest   
            if(data!=null)
            {
                string anotherFunctionSecret = "NFDGHECTq7qUzPQRdSpWdMTRRZfaHvf0OkACVfK5WQQs3udmVja6cw==";
                Uri anotherFunctionUri = new Uri(req.RequestUri.AbsoluteUri.Replace(
                    req.RequestUri.PathAndQuery,
                    $"/api/validateBatchRequest?code={anotherFunctionSecret}"));
                var content = new StringContent(data.ToString(), Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient();
                var validation = await client.PostAsync(anotherFunctionUri, content);
                response = validation.ToString();
                if (response == "success")
                {
                    //1.3 Send Request to Queue
                    string anotherFunctionSecret1 = "NFDGHECTq7qUzPQRdSpWdMTRRZfaHvf0OkACVfK5WQQs3udmVja6cw==";
                    Uri anotherFunctionUri1 = new Uri(req.RequestUri.AbsoluteUri.Replace(
                        req.RequestUri.PathAndQuery,
                        $"/api/validateBatchRequest?code={anotherFunctionSecret1}"));
                    var content1 = new StringContent(data.ToString(), Encoding.UTF8, "application/json");
                    HttpClient client1 = new HttpClient();
                    var validation1 = await client.PostAsync(anotherFunctionUri, content);
                }
               
            }
            else
            {
                response = "Please send a json input";
            }
            
                     
            //1.4 Send Synchronous Response
            return response != null
              ? req.CreateResponse(HttpStatusCode.BadRequest, response)
              : req.CreateResponse(HttpStatusCode.OK, "Your request has been processed");

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
