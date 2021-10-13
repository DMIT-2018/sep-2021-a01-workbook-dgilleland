using Backend.DAL;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.BLL
{
    public class AboutService
    {
        private readonly CapstoneContext _context;
        internal AboutService(CapstoneContext context)
        {
            _context = context;
        }

        public DatabaseVersion GetDatabaseVersion()
        {
            var versionInfo = _context.DbVersions.Single();
            return new DatabaseVersion(new Version(versionInfo.Major, versionInfo.Minor, versionInfo.Build), versionInfo.ReleaseDate);
        }
    }
}
