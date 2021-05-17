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
using AutoMapper;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        private readonly ILogger<HddMetricsController> _logger;
        private IHddMetricsRepository _repository;
        public HddMetricsController(
            IHddMetricsRepository repository, 
            ILogger<HddMetricsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] HddMetricCreateRequest request)
        {
            try
            {
                _repository.Create(new HddMetric
                {
                    Time = request.Time,
                    Value = request.Value
                });
                _logger.LogInformation(
                    "Сохранение Hdd метрики в БД с параметрами Time {0}, Value {1}",
                    request.Time,
                    request.Value);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(
                    "Ошибка сохранения Hdd метрики с параметрами Time {0}, Value {1}. Ошибка {2}",
                    request.Time,
                    request.Value,
                    ex.Message);
                return Ok(ex.Message);
            }
            return Ok();
        }

        [HttpGet("available/from/{fromTime}/to/{toTime}")]
        public IActionResult GetFreeDiskSpace(
            [FromRoute] TimeSpan fromTime,
            [FromRoute] TimeSpan toTime)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<HddMetric, HddMetricDto>());
            var map = config.CreateMapper();

            var metrics = _repository.GetByTimePeriod(fromTime, toTime);
            _logger.LogInformation(
                "Получено количество свободного пространства на жестком диске из базы данных за период с {0} по {1}",
                fromTime,
                toTime);
            var response = new AllHddMetricsResponse() { Metrics = new List<HddMetricDto>() };
            if (metrics != null)
            {
                foreach (var metric in metrics)
                {
                    response.Metrics.Add(new HddMetricDto
                    {
                        Time = metric.Time,
                        Value = metric.Value
                    });
                    response.Metrics.Add(map.Map<HddMetricDto>(metric));
                    _logger.LogInformation(
                        "Получены Hdd метрики с параметрами Time {0}, Value {1}",
                        metric.Time,
                        metric.Value);
                }
                return Ok(response);
            }
            else
            {
                _logger.LogInformation(
                    "Cписок Hdd метрик из базы данных за период с {0} по {1} пуст",
                    fromTime,
                    toTime);
                return Ok();
            }
        }

    }
}
