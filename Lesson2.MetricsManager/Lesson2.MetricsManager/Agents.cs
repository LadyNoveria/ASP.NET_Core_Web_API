using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lesson2.MetricsManager
{
    public class Agents
    {
        public List<AgentInfo> listOfAgents { get; set; }
        public Agents()
        {
            listOfAgents = new List<AgentInfo>();
        }
    }
}
