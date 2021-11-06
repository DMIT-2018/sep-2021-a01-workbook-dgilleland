using System;
using System.Linq;

namespace Backend.Models.Queries
{
    public record StudentTeamAssignment(int StudentId, string FullName, int? ClientId, string TeamLetter)
    {
        public StudentTeamAssignment() : this(0, null, null, null) { }
    }
}
