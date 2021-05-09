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
            _logger.LogInformation("api/metrics/network/Create");
            _repository.Create(new NetworkMetric
            {
                Time = request.Time,
                Value = request.Value
            });
            return Ok();
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetByTimePeriod(
            [FromRoute] TimeSpan fromTime, 
            [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("api/metrics/network/GetByTimePeriod");
            var metrics = _repository.GetByTimePeriod(fromTime, toTime);
            var response = new AllNetworkMetricsResponse() { Metrics = new List<NetworkMetricDto>() };
            if (metrics != null)
            {
                foreach (var metric in metrics)
                {
                    response.Metrics.Add(new NetworkMetricDto
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
