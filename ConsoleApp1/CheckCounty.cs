using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class CheckCounty
    {
        public void checkgeofence(string Lat ,string Longi)
        {
            var _Devicepostalcode = "600017";//County Postal Code,It will come from db 
            var json = new WebClient().DownloadString("https://maps.googleapis.com/maps/api/geocode/json?latlng="+Lat+","+Longi+"&key=AIzaSyDwEsdtIIzk29v8DvxctDmbgbhHCkj9iNs");         
            var VehiclespostalCode = "";
            var data = (JObject)JsonConvert.DeserializeObject(json);
            JToken memberName = data["results"];          
            foreach (var val in memberName)
            {                         
                if (val["types"].FirstOrDefault().ToString()=="postal_code") 
                {
                    VehiclespostalCode = val["address_components"][0]["long_name"].ToString();
                }
            }
            Console.WriteLine("Geofence Location Postal Code is " + _Devicepostalcode);
            Console.ReadLine();
            Console.WriteLine("Vehicle's Location Postal Code is " + VehiclespostalCode);
            Console.ReadLine();

            if (string.Equals(VehiclespostalCode, _Devicepostalcode))
            {              
                Console.WriteLine("Your device is inside of the location");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Your device is outside of the location");
                Console.ReadLine();
            }
                       
        }
       
    }
}
