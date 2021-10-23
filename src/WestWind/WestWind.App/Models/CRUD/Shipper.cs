using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WestWind.App.Models.CRUD
{
    public record Shipper(int ID, string CompanyName, string Phone)
    {
        // Parameterless constructor
        public Shipper() : this(0, null, null) { }
    }
}
