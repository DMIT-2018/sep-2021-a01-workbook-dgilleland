using Backend.DAL;
using Backend.Models;
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

        #region World Information
        // I've implemented a "poor-man's cache"
        private static List<Region> _WorldRegions;
        public List<Region> ListWorldRegions()
        {
            // TODO: Find out why this seems to be taking so long
            List<Region> result = _WorldRegions;
            if (result == null)
            {
                ICountryProvider countryProvider = new CountryProvider();
                result =
                    countryProvider
                        .GetCountries()
                        .Select(country => country.Region)
                        .Distinct()
                        .ToList();
                _WorldRegions = result;
            }
            return result;
        }

        public List<SubRegion> GetSubRegions(string regionName)
        {
            Region region;
            Enum.TryParse(regionName, out region);
            ICountryProvider provider = new CountryProvider();
            var result = provider
                .GetCountries()
                .Where(country => 
                       country.Region.Equals(region))
                .Select(country => country.SubRegion)
                .Distinct()
                .ToList();
            return result;
        }

        public List<ICountryInfo> GetCountries(SubRegion area)
        {
            ICountryProvider provider = new CountryProvider();
            var result = provider
                .GetCountries()
                .Where(country => 
                       country.SubRegion.Equals(area))
                .ToList();
            return result;
        }
        #endregion
    }
}
