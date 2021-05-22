using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using MetricsAgent.Repositories;
using System.Diagnostics;

namespace MetricsAgent.Jobs
{
    public class CpuMetricJob : IJob
    {
        private ICpuMetricsRepository _repository;
        private PerformanceCounter _cpuCounter;

        public CpuMetricJob(ICpuMetricsRepository repository)
        {
            _repository = repository;
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            var cpuUsageInPercents = Convert.ToInt32(_cpuCounter.NextValue());
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            _repository.Create(new CpuMetric { 
                Time = time,
                Value = cpuUsageInPercents
            });
            return Task.CompletedTask;
        }
    }
}
