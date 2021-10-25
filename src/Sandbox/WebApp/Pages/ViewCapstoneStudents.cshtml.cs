using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.BLL;
using Backend.Models.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class ViewCapstoneStudentsModel : PageModel
    {
        #region Constructor and Dependencies
        private readonly CapstoneService _service;
        public ViewCapstoneStudentsModel(CapstoneService service)
        {
            _service = service;
        }
        #endregion

        public IEnumerable<StudentAssignment> StudentAssignments { get; set; }

        public void OnGet()
        {
            StudentAssignments = _service.ListStudentAssignments();
        }
    }
}
