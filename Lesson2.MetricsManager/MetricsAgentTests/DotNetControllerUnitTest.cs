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
        public void Create_ShouldCall_Create_From_Repository()
        {
            mock.Setup(repository => repository.Create(It.IsAny<DotNetMetric>())).Verifiable();

            var result = controller.Create(
                new DotNetMetricCreateRequest
                {
                    Time = TimeSpan.FromSeconds(235),
                    Value = 42
                });
            mock.Verify(repository => repository.Create(It.IsAny<DotNetMetric>()), Times.AtMostOnce());
        }

        [Fact]
        public void GetByTimePeriod_ShouldCall_GetByTimePeriod_From_The_Repository()
        {
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);
            mock.Setup(repository => repository.GetByTimePeriod(fromTime, toTime)).Verifiable();

            var result = controller.GetByTimePeriod(fromTime, toTime);
            mock.Verify(repository => repository.GetByTimePeriod(fromTime, toTime), Times.AtLeastOnce());
        }
    }
}
