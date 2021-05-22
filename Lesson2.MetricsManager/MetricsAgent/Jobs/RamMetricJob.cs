using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using MetricsAgent.Repositories;
using System.Diagnostics;

namespace MetricsAgent.Jobs
{
    public class RamMetricJob: IJob
    {
        private IRamMetricsRepository _repository;
        private PerformanceCounter _ramCounter;
        public RamMetricJob(IRamMetricsRepository repository)
        {
            _repository = repository;
            _ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        public Task Execute(IJobExecutionContext context)
        {
            var ramUsageInPercents = Convert.ToInt32(_ramCounter.NextValue());
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            _repository.Create(new RamMetric
            {
                Time = time,
                Value = ramUsageInPercents
            });
            return Task.CompletedTask;
        }
    }
}
