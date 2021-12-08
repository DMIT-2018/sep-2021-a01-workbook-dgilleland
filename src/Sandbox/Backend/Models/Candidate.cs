using System;
using System.Linq;

namespace Backend.Models
{
    public record Candidate(int Id, bool ShortList, DateTime? InterviewDate, string Notes);
}
