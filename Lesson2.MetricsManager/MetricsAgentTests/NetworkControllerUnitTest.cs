using System;
using Xunit;
using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgentTests
{
    public class NetworkControllerUnitTest
    {
        private NetworkMetricsController controller;
        public NetworkControllerUnitTest()
        {
            controller = new NetworkMetricsController();
        }

        [Fact]
        public void GetInfoOboutTraffic_ResultOk()
        {

            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);

            var result = controller.GetInfoOboutTraffic(fromTime, toTime);

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
