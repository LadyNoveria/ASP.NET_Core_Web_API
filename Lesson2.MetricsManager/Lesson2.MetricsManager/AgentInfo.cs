using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lesson2.MetricsManager
{
    public class AgentInfo
    {
        public int AgentId { get; }
        public Uri AgentAddress { get; } 
        public AgentInfo(int agentId, Uri agentAddress)
        {
            AgentId = agentId;
            AgentAddress = agentAddress;
        }
    }
}
