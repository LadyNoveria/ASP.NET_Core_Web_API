using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using MetricsAgent.Repositories;
using System.Diagnostics;

namespace MetricsAgent.Jobs
{
    public class HddMetricJob: IJob
    {
        private IHddMetricsRepository _repository;
        private PerformanceCounter _hddCounter;
        public HddMetricJob(IHddMetricsRepository repository)
        {
            _repository = repository;
            _hddCounter = new PerformanceCounter("Logical Disk", "% Free Space", "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            var hddUsageInPercents = Convert.ToInt32(_hddCounter.NextValue());
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            _repository.Create(new HddMetric
            {
                Time = time,
                Value = hddUsageInPercents
            });
            return Task.CompletedTask;
        }
    }
}
