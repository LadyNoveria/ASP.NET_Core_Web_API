using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HW_1
{
    public class WeatherForecast
    {
        private DateTime _date;
        private int _temperatureC;

        public WeatherForecast(DateTime date, int temperature)
        {
            _date = date;
            _temperatureC = temperature;
        }

        public DateTime GetDate()
        {
            return _date;
        }
        public void SetTemperatureC(int temperature)
        {
            _temperatureC = temperature;
        }
        public int GetTemperature()
        {
            return _temperatureC;
        }

    }
}
