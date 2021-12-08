using Backend.BLL;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace WebApp.Pages
{
    public class JobCandidatesModel : PageModel
    {
        private readonly RandomDataService _service;

        public JobCandidatesModel(RandomDataService service)
        {
            _service = service;
        }


        public List<string> Openings { get; set; } // for the drop-down
        [BindProperty]
        public string SelectedOpening { get; set; } // for the selected item in the drop-down

        // The following two List<T> will be both displayed and gathered in an HTML table
        [BindProperty]
        public List<Applicant> Applicants { get; set; }
        [BindProperty]
        public List<Candidate> Candidates { get; set; } = new();

        public void OnGet()
        {
            Applicants = _service.ListJobApplicants();
            Openings = _service.ListJobOpenings();
        }

        public void OnPost()
        {
            // This info for the drop-downs 
            Openings = _service.ListJobOpenings();
        }
    }
}
