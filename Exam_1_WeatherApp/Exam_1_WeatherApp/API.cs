using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Data;
using System.Xml;

namespace Exam_1_WeatherApp
{

    class API
    {
        private static string main_url = "https://api.openweathermap.org/data/2.5/weather";
        private static string APP_ID = "";

        public static WeatherInfo.root get_weather(string location)
        {
            using (WebClient web = new())
             {
                string url = string.Format("{0}?APPID={1}&q={2}", main_url, APP_ID, location);
                Console.WriteLine(url);
                 var json = web.DownloadString(url);
                WeatherInfo.root Info = JsonConvert.DeserializeObject<WeatherInfo.root>(json);

                return Info;
            }
         }

        public static DataSet get_weather_xml(string location)
        {
            string url = string.Format("{0}?APPID={1}&q={2}&mode=xml", main_url, APP_ID, location);

            XmlReader xmlFile;
            xmlFile = XmlReader.Create(url);
            DataSet ds = new DataSet();
            ds.ReadXml(xmlFile);


            return ds;

        }
    }
}
