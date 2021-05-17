using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using MetricsAgent.Requests;
using MetricsAgent.Responses;
using Microsoft.Extensions.Logging;
using MetricsAgent.Repositories;
using AutoMapper;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private readonly ILogger<CpuMetricsController> _logger;
        private ICpuMetricsRepository _repository;

        public CpuMetricsController(
            ICpuMetricsRepository repository, 
            ILogger<CpuMetricsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CpuMetricCreateRequest request)
        {
            try
            {
                _repository.Create(new CpuMetric
                {
                    Time = request.Time,
                    Value = request.Value
                });
                _logger.LogInformation(
                    "Сохранение CPU метрики в БД с параметрами Time {0}, Value {1}", 
                    request.Time, 
                    request.Value);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(
                    "Ошибка сохранения CPU метрики с параметрами Time {0}, Value {1}. Ошибка {2}", 
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
            var config = new MapperConfiguration(cfg => cfg.CreateMap<CpuMetric, CpuMetricDto>());
            var map = config.CreateMapper();

            var metrics = _repository.GetByTimePeriod(fromTime, toTime);
            _logger.LogInformation(
                "Получен список CPU метрик из базы данных за период с {0} по {1}",
                fromTime,
                toTime);
            var response = new AllCpuMetricsResponse() { Metrics = new List<CpuMetricDto>() };
            if (metrics != null)
            {
                foreach (var metric in metrics)
                {
                    response.Metrics.Add(map.Map<CpuMetricDto>(metric));
                    _logger.LogInformation(
                        "Получены CPU метрики с параметрами Time {0}, Value {1}",
                        metric.Time,
                        metric.Value);
                }
                return Ok(response);
            }
            else
            {
                _logger.LogInformation(
                    "Cписок CPU метрик из базы данных за период с {0} по {1} пуст",
                    fromTime,
                    toTime);
                return Ok();
            }
        }
    }
}
