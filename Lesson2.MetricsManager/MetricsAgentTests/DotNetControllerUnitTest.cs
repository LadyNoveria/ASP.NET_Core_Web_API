using System;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.Extensions.Logging;
using MetricsAgent.Responses;
using MetricsAgent.Repositories;
using MetricsAgent.Requests;
using MetricsAgent.Controllers;
using MetricsAgent;

namespace MetricsAgentTests
{
    public class DotNetControllerUnitTest
    {
        private DotNetMetricsController controller;
        private Mock<IDotNetMetricsRepository> mock;

        public DotNetControllerUnitTest()
        {
            mock = new Mock<IDotNetMetricsRepository>();
            var logger = new Mock<ILogger<DotNetMetricsController>>();
            controller = new DotNetMetricsController(mock.Object, logger.Object);
        }

        [Fact]
        public void GetByTimePeriod_ResultOk()
        {
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);
            mock.Setup(repository => repository.GetAll()).Verifiable();

            var result = controller.GetByTimePeriod(fromTime, toTime);
            mock.Verify(repository => repository.GetAll(), Times.AtLeastOnce());
        }
    }
}
