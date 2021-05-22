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
using AutoMapper;

namespace MetricsAgentTests
{
    public class RamControllerUnitTest
    {
        private RamMetricsController controller;
        private Mock<IRamMetricsRepository> mock;
        private readonly IMapper _mapper;

        public RamControllerUnitTest()
        {
            mock = new Mock<IRamMetricsRepository>();
            var logger = new Mock<ILogger<RamMetricsController>>();

            var mapperConfiguration = new MapperConfiguration(
                mp => mp.AddProfile(new MapperProfile()));
            _mapper = mapperConfiguration.CreateMapper();

            controller = new RamMetricsController(mock.Object, logger.Object, _mapper);
        }

        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            mock.Setup(repository => repository.Create(It.IsAny<RamMetric>())).Verifiable();

            var result = controller.Create(
                new RamMetricCreateRequest
                {
                    Time = TimeSpan.FromSeconds(235),
                    Value = 42
                });
            mock.Verify(repository => repository.Create(It.IsAny<RamMetric>()), Times.AtMostOnce());
        }

        [Fact]
        public void GetFreeRAMSize_ShouldCall_GetFreeRAMSize_From_The_Repository()
        {
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);
            mock.Setup(repository => repository.GetByTimePeriod(fromTime, toTime)).Verifiable();

            var result = controller.GetFreeRAMSize(fromTime, toTime);
            mock.Verify(repository => repository.GetByTimePeriod(fromTime, toTime), Times.AtLeastOnce());
        }
    }
}
