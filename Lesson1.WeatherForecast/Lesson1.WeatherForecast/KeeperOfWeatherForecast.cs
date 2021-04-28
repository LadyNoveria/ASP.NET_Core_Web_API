using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lesson1.WeatherForecast
{
    public class KeeperOfWeatherForecast
    {
        public List<WeatherForecast> WeatherForecasts { get; set; }
        public KeeperOfWeatherForecast()
        {
            WeatherForecasts = new List<WeatherForecast>();
        }
    }
}
