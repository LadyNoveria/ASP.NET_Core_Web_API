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
            mock.Verify(repository => repository.Create(It.IsAny<CpuMetric>()));
        }
        
        [Fact]  
        public void GetByTimePeriod_ResultOk()
        {
            mock.Setup(repository => repository.GetAll()).Verifiable();
            var result = controller.GetByTimePeriod(
                TimeSpan.FromSeconds(0), 
                TimeSpan.FromSeconds(236));
            mock.Verify(repository => repository.GetAll());
        }
    } 
}
