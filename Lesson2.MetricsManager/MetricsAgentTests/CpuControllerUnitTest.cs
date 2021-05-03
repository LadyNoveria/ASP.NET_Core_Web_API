using System;
using Xunit;
using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgentTests
{
    public class CpuControllerUnitTest
    {
        private CpuMetricsController controller;
        public CpuControllerUnitTest()
        {
            controller = new CpuMetricsController();
        }

        [Fact]
        public void GetInfo_ResultOk()
        {
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);

            var result = controller.GetInfo(fromTime, toTime);

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    } 
}
