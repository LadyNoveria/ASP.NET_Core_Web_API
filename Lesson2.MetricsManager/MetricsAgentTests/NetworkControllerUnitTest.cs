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
    public class NetworkControllerUnitTest
    {
        private NetworkMetricsController controller;
        private Mock<INetworkMetricsRepository> mock;

        public NetworkControllerUnitTest()
        {
            mock = new Mock<INetworkMetricsRepository>();
            var logger = new Mock<ILogger<NetworkMetricsController>>();
            controller = new NetworkMetricsController(mock.Object, logger.Object);
        }

        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            mock.Setup(repository => repository.Create(It.IsAny<NetworkMetric>())).Verifiable();

            var result = controller.Create(
                new NetworkMetricCreateRequest
                {
                    Time = TimeSpan.FromSeconds(235),
                    Value = 42
                });
            mock.Verify(repository => repository.Create(It.IsAny<NetworkMetric>()), Times.AtMostOnce());
        }

        [Fact]
        public void GetInfoOboutTraffic_Should_Get_Info_About_Traffic_From_The_Repository_By_Time_Period()
        {
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);
            mock.Setup(repository => repository.GetByTimePeriod(fromTime, toTime)).Verifiable();

            var result = controller.GetByTimePeriod(fromTime, toTime);
            mock.Verify(repository => repository.GetByTimePeriod(fromTime, toTime), Times.AtLeastOnce());
        }
    }
}
