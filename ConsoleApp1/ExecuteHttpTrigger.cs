using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class ExecuteHttpTrigger
    {
       public static void main(string[] args)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://geofenceazurefuncapp20181221030202.azurewebsites.net/api/Function1?code=Lsk1CbbyMK0ewYzeeFTfVB9q6MkL2li0RNdkcIVyK4bIygaxGbav3g==");
            req.Method = "POST";
            req.ContentType = "application/json";
            Stream stream = req.GetRequestStream();
            string json = "{latitude: 13.0845962813354 ,longitude: 80.1545903898268}";            
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            stream.Write(buffer, 0, buffer.Length);
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            Console.WriteLine(res.ToString());
            Console.ReadLine();
        }
    }
}
