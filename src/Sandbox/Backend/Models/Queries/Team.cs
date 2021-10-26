using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models.Queries
{
    public class Team
    {
        public string Number { get; set; }
        public string Client { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
