using System;
using Xunit;
using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgentTests
{
    public class HddControllerUnitTest
    {
        private HddMetricsController controller;
        public HddControllerUnitTest()
        {
            controller = new HddMetricsController();
        }

        [Fact]
        public void GetFreeDiskSpace_ResultOk()
        {
            var result = controller.GetFreeDiskSpace();

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
