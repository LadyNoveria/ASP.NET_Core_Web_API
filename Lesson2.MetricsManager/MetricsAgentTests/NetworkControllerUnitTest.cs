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
        public void GetInfoOboutTraffic_ResultOk()
        {
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);
            mock.Setup(repository => repository.GetAll()).Verifiable();

            var result = controller.GetByTimePeriod(fromTime, toTime);
            mock.Verify(repository => repository.GetAll(), Times.AtLeastOnce());
        }
    }
}
