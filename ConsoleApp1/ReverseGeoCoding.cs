using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GeofenceAzureFuncApp
{
  
    class ReverseGeoCoding
    {
      
        public  void GeoCoding()
            {
            XmlDocument xDoc = new XmlDocument();          
            xDoc.Load("https://maps.googleapis.com/maps/api/geocode/json?address=Chennai&key=AIzaSyDwEsdtIIzk29v8DvxctDmbgbhHCkj9iNs");
            XmlNodeList xNodelst = xDoc.GetElementsByTagName("results");
            XmlNode xNode = xNodelst.Item(0);
            string adress = xNode.SelectSingleNode("formatted_address").InnerText;
            string mahalle = xNode.SelectSingleNode("geometry[2]/Location").InnerText;          
        }

        
    }
}
