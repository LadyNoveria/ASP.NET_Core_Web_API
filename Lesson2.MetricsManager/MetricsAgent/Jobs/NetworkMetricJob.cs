﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using MetricsAgent.Repositories;

namespace MetricsAgent.Jobs
{
    public class NetworkMetricJob: IJob
    {
        private INetworkMetricsRepository _repository;
        public NetworkMetricJob(INetworkMetricsRepository repository)
        {
            _repository = repository;
        }

        public Task Execute(IJobExecutionContext context)
        {
            return Task.CompletedTask;
        }
    }
}
