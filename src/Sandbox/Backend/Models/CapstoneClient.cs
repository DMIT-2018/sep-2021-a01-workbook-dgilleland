using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class CapstoneClient
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Slogan { get; set; }
        public string ContactName { get; set; }
        public bool Confirmed { get; set; }
    }
}
