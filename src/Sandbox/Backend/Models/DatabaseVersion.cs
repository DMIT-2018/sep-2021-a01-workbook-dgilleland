using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models
{
    public record DatabaseVersion(Version Version, DateTime ReleaseDate);
}
