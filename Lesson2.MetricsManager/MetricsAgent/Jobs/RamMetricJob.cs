﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using MetricsAgent.Repositories;

namespace MetricsAgent.Jobs
{
    public class RamMetricJob: IJob
    {
        private IRamMetricsRepository _repository;
        public RamMetricJob(IRamMetricsRepository repository)
        {
            _repository = repository;
        }

        public Task Execute(IJobExecutionContext context)
        {
            return Task.CompletedTask;
        }
    }
}
