using Backend.BLL;
using Backend.Models.SpyAgency;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Nager.Country;
using System.Collections.Generic;
using System.Linq;

namespace WebApp.Pages.Secured
{
    public class SpyAgencyModel : PageModel
    {
        private readonly SecretService _service;
        private readonly ILogger<SpyAgencyModel> _logger;
        public SpyAgencyModel(SecretService service, ILogger<SpyAgencyModel> logger)
        {
            _service = service;
            _logger = logger;
        }

        public List<Region> WorldRegions { get; set; }
        public List<SubRegion> SubRegions { get; set; }
        public List<ICountryInfo> Countries { get; set; }
        public Dictionary<string, List<Skill>> Agents { get; set; }
        public List<Assignment> Assignments
        { 
            get
            {
                var values = System.Enum.GetValues<Assignment>();
                return new List<Assignment>(values);
            }
        }
        public string WarningMessage { get; set; }
        [TempData]
        public string FeedbackMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        public string RegionName { get; set; }
        [BindProperty(SupportsGet = true)]
        public SubRegion SubRegionName { get; set; }
        [BindProperty(SupportsGet =true)]
        public string CountryCode { get; set; }
        [BindProperty]
        public List<AgentAssignment> AgentAssignments { get; set; } = new List<AgentAssignment>(); // Ensure an empty list
        [BindProperty]
        public AgentAssignment NewlyAssignedAgent { get; set; }
        [BindProperty]
        public string AgentToRemove { get; set; }

        public void OnGet()
        {
            PopulateDropDownData();
            if(!string.IsNullOrEmpty(CountryCode))
                AgentAssignments = _service.LocateAgents(CountryCode);
        }

        private void PopulateDropDownData()
        {
            WorldRegions = _service.ListWorldRegions();
            SubRegions = _service.GetSubRegions(RegionName);
            Countries = SubRegionName == SubRegion.None ? new List<ICountryInfo>() : _service.GetCountries(SubRegionName);
            Agents = _service.AgentSkillSets;
        }

        public IActionResult OnPostChangeRegion()
        {
            return RedirectToPage(new
            {
                RegionName = RegionName,
                SubRegionName = (string)null,
                CountryCode = (string)null
            });
        }

        public IActionResult OnPostChangeSubRegion()
        {
            return RedirectToPage(new
            {
                RegionName = RegionName,
                SubRegionName = SubRegionName,
                CountryCode = (string)null
            });
        }

        public IActionResult OnPostChangeCountry()
        {
            return RedirectToPage(new
            {
                RegionName = RegionName,
                SubRegionName = SubRegionName,
                CountryCode = CountryCode
            });
        }

        public void OnPostAddAgent()
        {
            // Minimal UI validation
            // 1. Preventing duplicates
            if (AgentAssignments.Any(agent => agent.CodeName == NewlyAssignedAgent.CodeName))
            {
                WarningMessage = "You cannot add duplicate agents";
                _logger.LogInformation(WarningMessage);
            }
            else
                // UI Processing
                AgentAssignments.Add(NewlyAssignedAgent);

            PopulateDropDownData();

        }

        public void OnPostRemoveAgent()
        {
            var agent = AgentAssignments.FirstOrDefault(person => person.CodeName == AgentToRemove);
            if (agent != null)
                AgentAssignments.Remove(agent);
            else
                WarningMessage = $"Hmm. For some reason, {AgentToRemove} is already gone.";

            PopulateDropDownData();
        }

        public IActionResult OnPostAssignAgents()
        {
            try
            {
                _service.DeployAgents(CountryCode, AgentAssignments);
                FeedbackMessage = $"Agents have been deployed to {CountryCode}";
                return RedirectToPage(new { RegionName = "", SubRegionName = "", CountryCode = "" });
            }
            catch (System.Exception ex)
            {
                WarningMessage = ex.Message;
                return Page(); // so that I preserve the user's inputs so that they can correct any input
            }
        }

        public IActionResult OnPostClearForm()
        {
            return RedirectToPage(new {RegionName = "", SubRegionName = "", CountryCode = ""});
        }
    }
}
