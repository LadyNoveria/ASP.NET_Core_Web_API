using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MetricsAgent.Responses;
using MetricsAgent.Requests;
using MetricsAgent.Repositories;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        private readonly ILogger<RamMetricsController> _logger;
        private IRamMetricsRepository _repository;
        public RamMetricsController(
            IRamMetricsRepository repository,
            ILogger<RamMetricsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] RamMetricCreateRequest request)
        {
            try
            {
                _repository.Create(new RamMetric
                {
                    Time = request.Time,
                    Value = request.Value
                });
                _logger.LogInformation(
                    "Сохранение Ram метрики в БД с параметрами Time {0}, Value {1}",
                    request.Time,
                    request.Value);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(
                    "Ошибка сохранения Ram метрики с параметрами Time {0}, Value {1}. Ошибка {2}",
                    request.Time,
                    request.Value,
                    ex.Message);
                return Ok(ex.Message);
            }
            return Ok();
        }

        [HttpGet("available/{fromTime}/to/{toTime}")]
        public IActionResult GetFreeRAMSize(
            [FromRoute] TimeSpan fromTime,
            [FromRoute] TimeSpan toTime)
        {
            var metrics = _repository.GetByTimePeriod(fromTime, toTime);
            _logger.LogInformation(
                "Получено количество свободной оперативной памяти из базы данных за период с {0} по {1}",
                fromTime,
                toTime);
            var response = new AllRamMetricsResponse() { Metrics = new List<RamMetricDto>() };
            if (metrics != null)
            {
                foreach (var metric in metrics)
                {
                    response.Metrics.Add(new RamMetricDto
                    {
                        Time = metric.Time,
                        Value = metric.Value
                    });
                    _logger.LogInformation(
                        "Получены Ram метрики с параметрами Time {0}, Value {1}",
                        metric.Time,
                        metric.Value);
                }
                return Ok(response);
            }
            else
            {
                _logger.LogInformation(
                    "Cписок Ram метрик из базы данных за период с {0} по {1} пуст",
                    fromTime,
                    toTime);
                return Ok();
            }
        }
    }
}
