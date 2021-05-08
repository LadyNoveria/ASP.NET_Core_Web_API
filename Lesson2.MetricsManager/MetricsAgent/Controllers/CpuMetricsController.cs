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

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private readonly ILogger<CpuMetricsController> _logger;
        private ICpuMetricsRepository _repository;
        public CpuMetricsController(ICpuMetricsRepository repository)
        {
            _repository = repository;
        }
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
            _logger.LogInformation("api/metrics/cpu/Create");
            _repository.Create(new CpuMetric
            {
                Time = request.Time,
                Value = request.Value
            }) ;
            return Ok();
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetByTimePeriod(
            [FromRoute] TimeSpan fromTime, 
            [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("api/metrics/cpu/GetByTimePeriod");
            var metrics = _repository.GetAll();
            var response = new AllCpuMetricsResponse() { Metrics = new List<CpuMetricDto>()};
            foreach (var metric in metrics)
            {
                response.Metrics.Add(new CpuMetricDto { 
                    Time = metric.Time, 
                    Value = metric.Value, 
                    Id = metric.Id}); 
            }
            return Ok(response);
        }
    }
}
