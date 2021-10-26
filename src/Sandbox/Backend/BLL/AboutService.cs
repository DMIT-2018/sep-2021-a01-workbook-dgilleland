using Backend.DAL;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.BLL
{
    public class AboutService
    {
        #region Constructor and DI fields
        private readonly CapstoneContext _context;
        internal AboutService(CapstoneContext context)
        {
            _context = context;
        }
        #endregion

        public DatabaseVersion GetDatabaseVersion()
        {
            var versionInfo = _context.DbVersions.Single();
            return new DatabaseVersion(new Version(versionInfo.Major, versionInfo.Minor, versionInfo.Build), versionInfo.ReleaseDate);
        }

        public DatabaseInfo GetDatabaseInformation()
        {
            string connectionString = _context.Database.GetConnectionString();

            var result = new DatabaseInfo
            { // This is an Initializer List
                ConnectionString = connectionString
            };
            return result;
        }
    }
}
