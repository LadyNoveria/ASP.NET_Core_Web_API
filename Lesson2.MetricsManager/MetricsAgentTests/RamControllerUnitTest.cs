using System;
using Xunit;
using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgentTests
{
    public class RamControllerUnitTest
    {
        private RamMetricsController controller;
        public RamControllerUnitTest()
        {
            controller = new RamMetricsController();
        }

        [Fact]
        public void GetFreeRAMSize_ResultOk()
        {
            var result = controller.GetFreeRAMSize();

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
