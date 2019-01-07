using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
   public class JsonExtract
    {
        public void extractjson(string myEventHubMessage)
        {
            myEventHubMessage = "{\"machineId\": \"2DDDBW3TP\",\"geofenceRequests\": [ { \"Latitude\": 52.516272, \"Longitude\": 13.377722, \"timestamp\": \"2018-12-12 23:34:01\"}";
           

            string[] arrayDocs = JsonConvert.DeserializeObject<string[]>(myEventHubMessage);
            String MachineId = string.Empty;

            foreach (var item in arrayDocs)
            {
                MachineId = item;
            }           

        }
    }
}

