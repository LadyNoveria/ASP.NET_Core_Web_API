using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MetricsAgent.Requests;
using MetricsAgent.Responses;
using MetricsAgent.Repositories;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/network")]
    [ApiController]
    public class NetworkMetricsController : ControllerBase
    {
        private readonly ILogger<NetworkMetricsController> _logger;
        private INetworkMetricsRepository _repository;
        public NetworkMetricsController(
            INetworkMetricsRepository repository,
            ILogger<NetworkMetricsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] NetworkMetricCreateRequest request)
        {
            try
            {
                _repository.Create(new NetworkMetric
                {
                    Time = request.Time,
                    Value = request.Value
                });
                _logger.LogInformation(
                    "Сохранение Network метрики в БД с параметрами Time {0}, Value {1}",
                    request.Time,
                    request.Value);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(
                    "Ошибка сохранения Network метрики с параметрами Time {0}, Value {1}. Ошибка {2}",
                    request.Time,
                    request.Value,
                    ex.Message);
                return Ok(ex.Message);
            }
            return Ok();
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetByTimePeriod(
            [FromRoute] TimeSpan fromTime, 
            [FromRoute] TimeSpan toTime)
        {
            var metrics = _repository.GetByTimePeriod(fromTime, toTime);
            _logger.LogInformation(
                "Получен список Network метрик из базы данных за период с {0} по {1}",
                fromTime,
                toTime);
            var response = new AllNetworkMetricsResponse() { Metrics = new List<NetworkMetricDto>() };
            if (metrics != null)
            {
                foreach (var metric in metrics)
                {
                    response.Metrics.Add(new NetworkMetricDto
                    {
                        Time = metric.Time,
                        Value = metric.Value
                    });
                    _logger.LogInformation(
                        "Получены Network метрики с параметрами Time {0}, Value {1}",
                        metric.Time,
                        metric.Value);
                }
                return Ok(response);
            }
            else
            {
                _logger.LogInformation(
                    "Cписок Network метрик из базы данных за период с {0} по {1} пуст",
                    fromTime,
                    toTime);
                return Ok();
            }
        }
    }
}
