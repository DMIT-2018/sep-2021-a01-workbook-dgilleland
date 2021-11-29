using Backend.BLL;
using Backend.Models.SpyAgency;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nager.Country;
using System.Collections.Generic;

namespace WebApp.Pages
{
    public class SpyAgencyModel : PageModel
    {
        private readonly AboutService _service;
        public SpyAgencyModel(AboutService service)
        {
            _service = service;
        }

        public List<Region> WorldRegions { get; set; }
        public List<SubRegion> SubRegions { get; set; }
        public List<ICountryInfo> Countries { get; set; }
        public List<Assignment> Assignments
        { 
            get
            {
                var values = System.Enum.GetValues<Assignment>();
                return new List<Assignment>(values);
            }
        }


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

        public void OnGet()
        {
            PopulateDropDownData();
        }

        private void PopulateDropDownData()
        {
            WorldRegions = _service.ListWorldRegions();
            SubRegions = _service.GetSubRegions(RegionName);
            Countries = _service.GetCountries(SubRegionName);
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
            PopulateDropDownData();
            AgentAssignments.Add(NewlyAssignedAgent);
            NewlyAssignedAgent = new(null, Assignment.None, null); // TODO: Fix this, as it is not working....
        }
    }
}
