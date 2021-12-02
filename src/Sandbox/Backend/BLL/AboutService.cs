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

        #region SpyAgency Work
        // "Fake" database
        private static Dictionary<string, List<AgentAssignment>> AgentDeployments
            = new();

        public void DeployAgents(string countryCode, List<AgentAssignment> agentAssignments)
        {
            if (AgentDeployments.ContainsKey(countryCode))
                AgentDeployments[countryCode] = agentAssignments;
            else
                AgentDeployments.Add(countryCode, agentAssignments);
        }
        public List<AgentAssignment> LocateAgents(string countryCode)
        {
            List<AgentAssignment> result = new();
            if (AgentDeployments.ContainsKey(countryCode))
                result = AgentDeployments[countryCode];
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
