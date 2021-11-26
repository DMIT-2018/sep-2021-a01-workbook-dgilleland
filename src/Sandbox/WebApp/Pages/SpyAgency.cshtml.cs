using Backend.BLL;
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

        [BindProperty(SupportsGet = true)]
        public string RegionName { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SubRegionName { get; set; }

        public void OnGet()
        {
            WorldRegions = _service.ListWorldRegions();
            SubRegions = _service.GetSubRegions(RegionName);
            // TODO: Resume class here....
        }

        public IActionResult OnPost()
        {
            // General handling of the navigation of
            // selecting a Region/SubRegion/Country
            return RedirectToPage(new 
            {
                RegionName = RegionName
            });
        }
    }
}
