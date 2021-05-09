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
            _logger.LogInformation("api/metrics/dotnet/Create");
            _repository.Create(new DotNetMetric
            {
                Time = request.Time,
                Value = request.Value
            });
            return Ok();
        }
        [HttpGet("errors-count/from/{fromTime}/to/{toTime}")]
        public IActionResult GetByTimePeriod(
            [FromRoute] TimeSpan fromTime, 
            [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("api/metrics/dotnet/GetByTimePeriod");
            var metrics = _repository.GetByTimePeriod(fromTime, toTime);
            var response = new AllDotNetMetricsResponse() { Metrics = new List<DotNetMetricDto>() };
            if (metrics != null)
            {
                foreach (var metric in metrics)
                {
                    response.Metrics.Add(new DotNetMetricDto
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
