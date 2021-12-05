using Backend.Models.SpyAgency;
using Nager.Country;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.BLL
{
    public class SecretService
    {
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

        private static Dictionary<string, List<Skill>> _AgentSkillSets;

        public Dictionary<string, List<Skill>> AgentSkillSets
        {
            get
            {
                // "Lazy-loading" of data
                if (_AgentSkillSets == null)
                {
                    _AgentSkillSets = new Dictionary<string, List<Skill>>
                    {
                        { "Bjorne", new() { Skill.Field_Training, Skill.Weapons_Specialist, Skill.Infiltration } },
                        { "Not Bourne", new() { Skill.Mechanics_Expert, Skill.Extraction } },
                        { "Frying Dutchman", new() { Skill.Explosives, Skill.Mechanics_Expert } },
                        { "Mr. MoneyDollar", new() { Skill.Operations_Specialist } },
                        { "Guido", new() { Skill.Multiple_Languages, Skill.Ventriloquism } },
                        { "DanG-IT", new() { Skill.IT_Specialist } }
                    };
                    foreach (var skillset in _AgentSkillSets.Values)
                        skillset.Add(Skill.Basic_Training);
                }
                return _AgentSkillSets;
            }
        }

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

    }
}
