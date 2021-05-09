using System;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using MetricsAgent;
using Microsoft.Extensions.Logging;
using Moq;
using MetricsAgent.Requests;
using MetricsAgent.Controllers;

namespace MetricsAgentTests
{
    public class CpuControllerUnitTest
    {
        private CpuMetricsController controller;
        private Mock<ICpuMetricsRepository> mock;

        public CpuControllerUnitTest()
        {
            mock = new Mock<ICpuMetricsRepository>();
            var logger = new Mock<ILogger<CpuMetricsController>>();
            controller = new CpuMetricsController(mock.Object, logger.Object);
        }

        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            mock.Setup(repository => repository.Create(It.IsAny<CpuMetric>())).Verifiable();

            var result = controller.Create(
                new CpuMetricCreateRequest {
                    Time = TimeSpan.FromSeconds(235),
                    Value = 42});
            mock.Verify(repository => repository.Create(It.IsAny<CpuMetric>()), Times.AtMostOnce());
        }
        
        [Fact]  
        public void GetByTimePeriod_Should_Get_Metrics_From_The_Repository_By_Time_Period()
        {
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(236);

            mock.Setup(repository => repository.GetByTimePeriod(fromTime, toTime)).Verifiable();
            var result = controller.GetByTimePeriod(fromTime, toTime);
            mock.Verify(repository => repository.GetByTimePeriod(fromTime, toTime), Times.AtMostOnce());
        }
    } 
}
