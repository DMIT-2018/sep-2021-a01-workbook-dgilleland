using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.BLL;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Admin
{
    public class ManageCapstoneStudentsModel : PageModel
    {
        #region Constructor and Depedencies
        private readonly CapstoneService _service;
        public ManageCapstoneStudentsModel(CapstoneService service)
        {
            _service = service;
        }
        #endregion

        public List<StudentName> AllStudents { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SelectedStudent { get; set; }

        public void OnGet()
        {
            AllStudents = _service.ListCapstoneStudents();
        }

        public IActionResult OnPost()
        {
            return RedirectToPage(new { SelectedStudent });
        }
    }
}
