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
            return Ok(_weatherForecasts.WeatherForecasts);
        }

        [HttpGet("read")]
        public IActionResult Get([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateBy)
        {
            var forecasts = new List<WeatherForecast>();
            foreach (var forecast in _weatherForecasts.WeatherForecasts)
            {
                if (forecast.Date >= dateFrom && forecast.Date <= dateBy)
                {
                    forecasts.Add(forecast);
                }
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
                if (forecast.Date >= dateFrom && forecast.Date <= dateBy)
                {
                    forecasts.Add(forecast);
                }
            }
            foreach (var forecast in forecasts)
            {
                _weatherForecasts.WeatherForecasts = 
                    _weatherForecasts.WeatherForecasts.Where(w => w != forecast).ToList();
            }
            return Ok();
        }
    }
}
