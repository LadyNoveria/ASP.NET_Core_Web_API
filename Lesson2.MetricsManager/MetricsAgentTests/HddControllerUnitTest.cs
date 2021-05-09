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
        public void Create_ShouldCall_Create_From_Repository()
        {
            mock.Setup(repository => repository.Create(It.IsAny<HddMetric>())).Verifiable();

            var result = controller.Create(
                new HddMetricCreateRequest
                {
                    Time = TimeSpan.FromSeconds(235),
                    Value = 42
                });
            mock.Verify(repository => repository.Create(It.IsAny<HddMetric>()), Times.AtMostOnce());
        }

        [Fact]
        public void GetFreeDiskSpace_ShouldCall_GetFreeDiskSpace_From_The_Repository()
        {
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);
            mock.Setup(repository => repository.GetByTimePeriod(fromTime, toTime)).Verifiable();

            var result = controller.GetFreeDiskSpace(fromTime, toTime);
            mock.Verify(repository => repository.GetByTimePeriod(fromTime, toTime), Times.AtLeastOnce());
        }
    }
}
