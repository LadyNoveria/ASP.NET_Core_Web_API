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
            _logger.LogInformation("api/metrics/hdd/Create");
            _repository.Create(new HddMetric
            {
                Time = request.Time,
                Value = request.Value
            });
            return Ok();
        }

        [HttpGet("available/from/{fromTime}/to/{toTime}")]
        public IActionResult GetFreeDiskSpace(
            [FromRoute] TimeSpan fromTime,
            [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("api/metrics/hdd/GetFreeDiskSpace");
            var metrics = _repository.GetByTimePeriod(fromTime, toTime);
            var response = new AllHddMetricsResponse() { Metrics = new List<HddMetricDto>() };
            if (metrics != null)
            {
                foreach (var metric in metrics)
                {
                    response.Metrics.Add(new HddMetricDto
                    {
                        Time = metric.Time,
                        Value = metric.Value,
                        Id = metric.Id
                    });
                }
                return Ok(response);
            }
            else
            {
                return Ok();
            }
        }

    }
}
