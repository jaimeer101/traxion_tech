﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam_1_WeatherApp
{
    class WeatherInfo
    {
        public class coord
        {
            public double lon { get; set; }
            public double lat { get; set; }
        }

        public class weather
        {
            public string main { get; set; }
            public string description { get; set; }
            public string icon { get; set; }
        }

        public class main
        {
            public double temp { get; set; }
            /*double feels_like { get; set; }
            double temp_min { get; set; }
            double temp_max { get; set; }*/
            public double pressure { get; set; }
            public double humidity { get; set; }
           /* double sea_level { get; set; }
            double grnd_level { get; set; }*/
        }
        public class wind
        {
            public  double speed { get; set; }
            public double deg { get; set; }
        }

       /* public class clouds
        {
            int all { get; set; }
        }*/

        public class sys
        {
            /*int type { get; set; }
            string country { get; set; }*/
            public long sunrise { get; set; }
            public  long sunset { get; set; }
        }

        public class root
        {
            public coord coord { get; set; }
            public List <weather> weather { get; set; }

            public main main { get; set; }

            public wind wind { get; set; }
            public sys sys { get; set; }
        }
    }
}
