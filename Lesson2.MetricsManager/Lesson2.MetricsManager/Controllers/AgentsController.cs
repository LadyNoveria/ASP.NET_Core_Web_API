using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Lesson2.MetricsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly ILogger<AgentsController> _logger;

        public AgentsController(ILogger<AgentsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("getAgents")]
        public IActionResult GetAgents()
        {
            _logger.LogInformation("api/agents/GetAgents");
            return Ok();
        }

        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
        {
            _logger.LogInformation("api/agents/RegisterAgent");
            return Ok();
        }

        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            _logger.LogInformation("api/agents/EnableAgentById");
            return Ok();
        }

        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            _logger.LogInformation("api/agents/DisableAgentById");
            return Ok();
        }
    }
}
