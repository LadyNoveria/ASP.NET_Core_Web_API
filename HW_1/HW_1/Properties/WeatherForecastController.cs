using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HW_1.Properties
{
    [Route("api/weather")]
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private readonly KeeperOfWeatherForecasts _weatherForecasts;
        public WeatherForecastController(KeeperOfWeatherForecasts weatherForecasts)
        {
            _weatherForecasts = weatherForecasts;
        }
        [HttpGet("read")]
        public IActionResult Get()
        {
            List<string> forecasts = new List<string>();
            foreach (var forecast in _weatherForecasts.WeatherForecasts)
            {
                forecasts.Add($"{forecast.GetDate()} \t {forecast.GetTemperature()}");
            }
            return Ok(forecasts);
        }

        [HttpPost("create")]
        public IActionResult AddForecast([FromQuery] DateTime date, [FromQuery] int temperature)
        {
            WeatherForecast weatherForecast = new WeatherForecast(date, temperature);
            _weatherForecasts.WeatherForecasts.Add(weatherForecast);
            return Ok();
        }

        [HttpPut("update")]
        public IActionResult Update([FromQuery] DateTime dateOfChange, [FromQuery] int newTemperature)
        {
            foreach (var forecast in _weatherForecasts.WeatherForecasts)
            {
                if (forecast.GetDate() == dateOfChange)
                {
                    forecast.SetTemperatureC(newTemperature);
                    break;
                }
            }
            return Ok();
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateBy)
        {
            var forecasts = _weatherForecasts.WeatherForecasts;
            foreach (var forecast in forecasts)
            {
                if (forecast.GetDate() >= dateFrom && forecast.GetDate() <= dateBy)
                {
                    _weatherForecasts.WeatherForecasts.Remove(forecast);
                }
            }
            return Ok();
        }
    }
}
