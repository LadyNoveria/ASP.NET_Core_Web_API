using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HW_1
{
    public class KeeperOfWeatherForecasts
    {
        public List<WeatherForecast> WeatherForecasts { get; set; }
        public KeeperOfWeatherForecasts()
        {
            WeatherForecasts = new List<WeatherForecast>();
        }
    }
}
