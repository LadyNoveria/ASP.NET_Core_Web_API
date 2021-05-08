using System;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MetricsAgent.Responses;
using MetricsAgent.Repositories;
using MetricsAgent.Requests;
using MetricsAgent.Controllers;
using MetricsAgent;

namespace MetricsAgentTests
{
    public class HddControllerUnitTest
    {
        private HddMetricsController controller;
        private Mock<IHddMetricsRepository> mock;

        public HddControllerUnitTest()
        {
            mock = new Mock<IHddMetricsRepository>();
            var logger = new Mock<ILogger<HddMetricsController>>();
            controller = new HddMetricsController(mock.Object, logger.Object);
        }

        [Fact]
        public void GetFreeDiskSpace_ResultOk()
        {
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);
            mock.Setup(repository => repository.GetAll()).Verifiable();

            var result = controller.GetFreeDiskSpace(fromTime, toTime);
            mock.Verify(repository => repository.GetAll(), Times.AtLeastOnce());
        }
    }
}
