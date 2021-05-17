using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MetricsAgent.Repositories;
using MetricsAgent.Requests;
using MetricsAgent.Responses;
using AutoMapper;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/dotnet")]
    [ApiController]
    public class DotNetMetricsController : ControllerBase
    {
        private readonly ILogger<DotNetMetricsController> _logger;
        private IDotNetMetricsRepository _repository;
        public DotNetMetricsController(
            IDotNetMetricsRepository repository,
            ILogger<DotNetMetricsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] DotNetMetricCreateRequest request)
        {
            try
            {
                _repository.Create(new DotNetMetric
                {
                    Time = request.Time,
                    Value = request.Value
                });
                _logger.LogInformation(
                    "Сохранение DotNet метрики в БД с параметрами Time {0}, Value {1}", 
                    request.Time, 
                    request.Value);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(
                    "Ошибка сохранения DotNet метрики с параметрами Time {0}, Value {1}. Ошибка {2}", 
                    request.Time,
                    request.Value,
                    ex.Message);
                return Ok(ex.Message);
            }
            return Ok();
        }
        [HttpGet("errors-count/from/{fromTime}/to/{toTime}")]
        public IActionResult GetByTimePeriod(
            [FromRoute] TimeSpan fromTime, 
            [FromRoute] TimeSpan toTime)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<DotNetMetric, DotNetMetricDto>());
            var map = config.CreateMapper();

            var metrics = _repository.GetByTimePeriod(fromTime, toTime);
            _logger.LogInformation(
                "Получен список DotNet метрик из базы данных за период с {0} по {1}",
                fromTime,
                toTime);
            var response = new AllDotNetMetricsResponse() { Metrics = new List<DotNetMetricDto>() };
            if (metrics != null)
            {
                foreach (var metric in metrics)
                {
                    response.Metrics.Add(map.Map<DotNetMetricDto>(metric));
                    _logger.LogInformation(
                        "Получены DotNet метрики с параметрами Time {0}, Value {1}",
                        metric.Time,
                        metric.Value);
                }
                return Ok(response);
            }
            else
            {
                _logger.LogInformation(
                    "Cписок DotNet метрик из базы данных за период с {0} по {1} пуст",
                    fromTime,
                    toTime);
                return Ok();
            }
        }
    }
}
