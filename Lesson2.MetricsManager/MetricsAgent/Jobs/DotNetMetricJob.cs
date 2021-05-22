using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using MetricsAgent.Repositories;
using System.Diagnostics;

namespace MetricsAgent.Jobs
{
    public class DotNetMetricJob: IJob
    {
        private IDotNetMetricsRepository _repository;
        private PerformanceCounter _dotNetCounter;
        public DotNetMetricJob(IDotNetMetricsRepository repository, string processName)
        {
            _repository = repository;
            _dotNetCounter = new PerformanceCounter("Process", "% Processor Time", processName);
        }

        public Task Execute(IJobExecutionContext context)
        {
            var dotNetUsageInPercents = Convert.ToInt32(_dotNetCounter.NextValue());
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            _repository.Create(new DotNetMetric
            {
                Time = time,
                Value = dotNetUsageInPercents
            });
            return Task.CompletedTask;
        }
    }
}
