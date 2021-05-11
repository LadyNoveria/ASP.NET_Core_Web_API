using System;
using Xunit;
using Lesson2.MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using Lesson2.MetricsManager;

namespace MetricsManagerTests
{
    public class AgentsControllerUnitTest
    {
        private AgentsController controller;
        public AgentsControllerUnitTest()
        {
            controller = new AgentsController(new Agents());
        }

        [Fact]
        public void RegisterAgent_ResultOk()
        {
            var agentInfo = new AgentInfo();
            var uri = new Uri("http://www.contoso.com/");

            agentInfo.AgentId = 1;
            agentInfo.AgentAddress = uri;

            var result = controller.RegisterAgent(agentInfo);

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void DisableAgentById_ResultOk()
        {
            var agentId = 1;

            var result = controller.DisableAgentById(agentId);
            
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void EnableAgentById_ResultOk()
        {
            var agentId = 1;

            var result = controller.EnableAgentById(agentId);

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void GetAgents_ResultOk()
        {
            var result = controller.GetAgents();
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
