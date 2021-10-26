using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models.Queries
{
    public class StudentAssignment
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public Team Assignment { get; set; }
    }
}
