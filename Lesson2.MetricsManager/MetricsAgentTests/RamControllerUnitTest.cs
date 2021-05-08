using System;
using Xunit;
using MetricsAgent.Responses;
using MetricsAgent.Repositories;
using MetricsAgent.Requests;
using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MetricsAgent;

namespace MetricsAgentTests
{
    public class RamControllerUnitTest
    {
        private RamMetricsController controller;
        private Mock<IRamMetricsRepository> mock;

        public RamControllerUnitTest()
        {
            mock = new Mock<IRamMetricsRepository>();
            var logger = new Mock<ILogger<RamMetricsController>>();
            controller = new RamMetricsController(mock.Object, logger.Object);
        }

        [Fact]
        public void GetFreeRAMSize_ResultOk()
        {
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);
            mock.Setup(repository => repository.GetAll()).Verifiable();

            var result = controller.GetFreeRAMSize(fromTime, toTime);
            mock.Verify(repository => repository.GetAll(), Times.AtLeastOnce());
        }
    }
}
