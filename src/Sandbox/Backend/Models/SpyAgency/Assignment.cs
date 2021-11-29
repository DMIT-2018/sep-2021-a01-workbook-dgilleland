using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models.SpyAgency
{
    public enum Assignment
    {
        None,
        Liason,
        FieldAgent,
        Analyst,
        Logistics,
        Transport
    }
    public record AgentAssignment(string CodeName, Assignment LocalAssignment, string PrimaryMission);
}
