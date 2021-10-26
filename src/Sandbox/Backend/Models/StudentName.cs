using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models
{
    public record StudentName(int StudentId, string FormalName)
    { public StudentName() : this(0, null) { } }
}
