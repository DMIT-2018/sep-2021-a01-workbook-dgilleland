using Backend.DAL;
using Backend.Models;
using Backend.Models.Colors;
using Backend.Models.SpyAgency;
using Microsoft.EntityFrameworkCore;
using Nager.Country;
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

        #region Get Database Information
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
        #endregion

        #region Named HTML Colors
        public List<NamedColor> ListHTMLColors()
        {
            List<NamedColor> colors = new List<NamedColor> {
                new NamedColor("rgb(255, 0, 0)", "#FF0000", "RED"),
                new NamedColor("rgb(255, 192, 203)", "#FFC0CB", "PINK"),
                new NamedColor("rgb(255, 165, 0)", "#FFA500", "ORANGE"),
                new NamedColor("rgb(255, 255, 0)", "#FFFF00", "YELLOW"),
                new NamedColor("rgb(128, 0, 128)", "#800080", "PURPLE"),
                new NamedColor("rgb(0, 128, 0)", "#008000", "GREEN"),
                new NamedColor("rgb(0, 0, 255)", "#0000FF", "BLUE"),
                new NamedColor("rgb(165, 42, 42)", "#A52A2A", "BROWN"),
                new NamedColor("rgb(255, 255, 255)", "#FFFFFF", "WHITE"),
                new NamedColor("rgb(128, 128, 128)", "#808080", "GRAY")
                };
            return colors;
        }
        #endregion
    }
}
