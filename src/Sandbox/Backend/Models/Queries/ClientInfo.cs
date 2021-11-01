using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models.Queries
{
    public record ClientInfo(int ClientId, string Company)
    {
        public ClientInfo() : this(0, null) { }
    }
}
