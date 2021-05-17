using System;
using Xunit;
using Lesson2.MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.Extensions.Logging;

namespace MetricsManagerTests
{
    public class NetworkControllerUnitTest
    {
        private NetworkMetricsController controller;

        public NetworkControllerUnitTest()
        {
            var logger = new Mock<ILogger<NetworkMetricsController>>();
            controller = new NetworkMetricsController(logger.Object);
        }

        [Fact]
        public void GetMetricsFromAgent_ResultOk()
        {
            var agentId = 1;
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);

            var result = controller.GetMetricsFromAgent(agentId, fromTime, toTime);

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void GetMetricsFromAllCluster_ResultOk()
        {
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);

            var result = controller.GetMetricsFromAllCluster(fromTime, toTime);

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
