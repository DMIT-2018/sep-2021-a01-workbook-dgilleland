using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.BLL;
using Backend.Models.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Admin
{
    public class AssignTeamsModel : PageModel
    {
        private readonly CapstoneService _service;
        public AssignTeamsModel(CapstoneService service)
        {
            _service = service;
        }

        public List<ClientInfo> Clients { get; set; }

        [BindProperty] // Model Binding that will happen on the POST request
        public List<StudentTeamAssignment> Students { get; set; }

        [TempData] // Is stored in a temporary "cookie" on the page as part of the HTTPResponse
        public string Feedback { get; set; }
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            Clients = _service.ListConfirmedClients();
            Students = _service.ListTeamAssignments();
        }

        public IActionResult OnPost()
        {
            try
            {
                // Send data to BLL for processing
                _service.ModifyTeamAssignments(Students);
                // TODO: Do a Post-Redirect-Get
                Feedback = "Students assigned to teams.";
                return RedirectToPage(); // Redirect to the current page
            }
            catch (Exception ex)
            {
                // TODO: Show the error message(s)
                // TODO: Stay on the current page
                Clients = _service.ListConfirmedClients(); // for the drop-downs
                ErrorMessage = ex.Message;
                return Page(); // Return the POST rendering of the page - we keep the data the user entered
            }
        }
    }
}
