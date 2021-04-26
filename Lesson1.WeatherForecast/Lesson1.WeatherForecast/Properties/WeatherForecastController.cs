using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lesson1.WeatherForecast.Properties
{
    [Route("api/weather")]
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private readonly KeeperOfWeatherForecast _weatherForecasts;
        public WeatherForecastController(KeeperOfWeatherForecast weatherForecasts)
        {
            _weatherForecasts = weatherForecasts;
        }
        [HttpGet("readAll")]
        public IActionResult Get()
        {
            List<string> forecasts = new List<string>();
            foreach (var forecast in _weatherForecasts.WeatherForecasts)
            {
                forecasts.Add($"{forecast.Date.ToString("d")} - {forecast.TemperatureC}°C");
            }
            return Ok(forecasts);
        }

        [HttpGet("read")]
        public IActionResult Get([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateBy)
        {
            List<string> forecasts = new List<string>();
            foreach (var forecast in _weatherForecasts.WeatherForecasts)
            {
                if (forecast.Date >= dateFrom && forecast.Date <= dateBy)
                    forecasts.Add($"{forecast.Date.ToString("d")} - {forecast.TemperatureC}°C");
            }
            return Ok(forecasts);
        }

        [HttpPost("create")]
        public IActionResult AddForecast([FromQuery] DateTime date, [FromQuery] int temperature)
        {
            WeatherForecast weatherForecast = new WeatherForecast
            {
                Date = date,
                TemperatureC = temperature
            };
            _weatherForecasts.WeatherForecasts.Add(weatherForecast);
            return Ok();
        }

        [HttpPut("update")]
        public IActionResult Update([FromQuery] DateTime dateOfChange, [FromQuery] int newTemperature)
        {
            foreach (var forecast in _weatherForecasts.WeatherForecasts)
            {
                if (forecast.Date == dateOfChange)
                {
                    forecast.TemperatureC = newTemperature;
                    break;
                }
            }
            return Ok();
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateBy)
        {
            var forecasts = new List<WeatherForecast>();
            foreach (var forecast in _weatherForecasts.WeatherForecasts)
            {
                forecasts.Add(forecast);
            }

            foreach (var forecast in forecasts)
            {
                if (forecast.Date >= dateFrom && forecast.Date <= dateBy)
                {
                    _weatherForecasts.WeatherForecasts.Remove(forecast);
                }
            }

            return Ok();
        }
    }
}
