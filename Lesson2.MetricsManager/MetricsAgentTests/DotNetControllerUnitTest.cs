using System;
using Xunit;
using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgentTests
{
    public class DotNetControllerUnitTest
    {
        private DotNetMetricsController controller;
        public DotNetControllerUnitTest()
        {
            controller = new DotNetMetricsController();
        }

        [Fact]
        public void GetInfoAboutDotNet_ResultOk()
        {
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);

            var result = controller.GetInfoAboutDotNet(fromTime, toTime);

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
