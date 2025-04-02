// See https://aka.ms/new-console-template for more information
using Exam_1_WeatherApp;
using Newtonsoft.Json;
using System.Data;
using System.Net;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello To OpenWeather Deserialization");
        String _weathInfo = getWeather("London");
        //Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(weatherInfo, Formatting.Indented));
        //getWeather();
        
        Console.WriteLine(_weathInfo);
    }

    static string getWeather(string location)
    {   

        WeatherInfo.root weatherInfo = API.get_weather(location);
        string response = "JSON Deserialization " + "\n";
        response += "Weather: " + weatherInfo.weather[0].main + "\n";
        response += "Details: " + weatherInfo.weather[0].description + "\n";
        response += "Sunset: " + convertDateTime(weatherInfo.sys.sunset) + "\n";
        response += "Sunrise: " + convertDateTime(weatherInfo.sys.sunset) + "\n";
        response += "Windspeed: " + weatherInfo.wind.speed + "\n";
        response += "Pressure: " + weatherInfo.main.pressure + "\n";

        DataSet weatherInfoXml = API.get_weather_xml(location);
        response += "XML Deserialization" + "\n";
        foreach (DataTable table in weatherInfoXml.Tables)
        {
            foreach (var row in table.AsEnumerable())
            {
                string temp_rows = "";
                for (int i = 0; i < table.Columns.Count; ++i)
                {
                    temp_rows += table.Columns[i].ColumnName + ": " + row[i] + ", ";
                }
                response += temp_rows + "\n";
            }
        }
        return response;
    }

    static DateTime convertDateTime(long millisec)

    {
        DateTime day = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        day = day.AddSeconds(millisec).ToLocalTime();
        return day;
    }
}
