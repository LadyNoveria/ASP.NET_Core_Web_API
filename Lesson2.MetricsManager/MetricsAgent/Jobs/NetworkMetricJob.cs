using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using MetricsAgent.Repositories;
using System.Diagnostics;

namespace MetricsAgent.Jobs
{
    public class NetworkMetricJob: IJob
    {
        private INetworkMetricsRepository _repository;
        private PerformanceCounter _networkCounter;
        public NetworkMetricJob(INetworkMetricsRepository repository, string networkCard)
        {
            _repository = repository;
            _networkCounter = new PerformanceCounter("Network Interface", "Current Bandwidth", networkCard);
        }

        public Task Execute(IJobExecutionContext context)
        {
            var networkUsageInPercents = Convert.ToInt32(_networkCounter.NextValue());
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            _repository.Create(new NetworkMetric
            {
                Time = time,
                Value = networkUsageInPercents
            });
            return Task.CompletedTask;
        }
    }
}
